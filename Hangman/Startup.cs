using System.Collections.Generic;
using System.Linq;
using Hangman.Application;
using Hangman.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangman.Repository;
using Hangman.Repository.Interfaces;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Hangman
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // injection of health-checking services (database, cache, etc. goes here)
            services.AddHealthChecks()
                .AddNpgSql(Configuration.GetConnectionString("DBConnection"));

            // injection of the friendly health-check UI service (requires DB for health history persistence)
            // configuration of endpoints for the UI client on the 'HealthChecksUI' of appsettings.json
            services.AddHealthChecksUI()
                .AddPostgreSqlStorage(Configuration.GetConnectionString("DBConnection"));
            
            // injection os other services
            services.AddHttpContextAccessor()
                .AddDbContext<HangmanDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DBConnection")))
                .AddScoped(typeof(IHangmanRepositoryAsync<>), typeof(HangmanRepositoryAsync<>)) // generic repository
                .AddScoped<IGameRoomServiceAsync, GameRoomServiceAsync>()
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configuring start up with environment: {EnvironmentName}", env.EnvironmentName);

            if (env.IsDevelopment() || env.IsEnvironment("Local"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // middleware for condensing many access log lines into a SINGLE useful one
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            // middleware for activating the healthcheck UI
            app.UseHealthChecksUI(options => options.UIPath = "/healthcheck-dashboard");
            
            app.UseEndpoints(endpoints =>
            {
                // changes health-check endpoint to return a JSON rather than plain/text 'Healthy'
                // this endpoint must match the endpoint of 'HealthChecksUI' URI
                endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapControllers();
            });

            // Migrations and seed db (when in development/compose ONLY)
            Migrate(app, logger,
                executeSeedDb: env.IsDevelopment() || env.IsEnvironment("Local") || env.IsEnvironment("DockerCompose"));
        }

        /**
         * Applies possible missing migrations from the database.
         */
        public static void Migrate(IApplicationBuilder app, ILogger<Startup> logger, bool executeSeedDb = false)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<HangmanDbContext>();

            // always execute possible missing migrations
            if (context.Database.GetPendingMigrations().ToList().Any())
            {
                logger.LogInformation("Applying migrations...");
                context.Database.Migrate();
            }

            // seeding DB only when asked
            if (!executeSeedDb) return;

            logger.LogInformation("Seeding the database...");
            SeedDb(context, logger);
        }

        /**
         * Seeds DB with pre-defined entities/models.
         */
        private static void SeedDb(HangmanDbContext context, ILogger<Startup> logger)
        {
            if (context.GameRooms.Any())
            {
                logger.LogInformation("Database has already been seeded. Skipping it...");
                return;
            }

            logger.LogInformation("Saving entities...");
            var gameRooms = new List<GameRoom>
            {
                new GameRoom {Name = "Game Room 1"},
                new GameRoom {Name = "Game Room 2"},
                new GameRoom {Name = "Game Room 3"}
            };
            context.AddRange(gameRooms);

            logger.LogInformation("Database has been seeded successfully.");
            context.SaveChanges();
        }
    }
}