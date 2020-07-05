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
using Microsoft.EntityFrameworkCore;

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
            services.AddDbContext<HangmanDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DBConnection")));
            services.AddScoped(typeof(IHangmanRepositoryAsync<>), typeof(HangmanRepositoryAsync<>)); // generic repository
            services.AddScoped<IGameRoomServiceAsync, GameRoomServiceAsync>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            // Migrations and seed db (when in development ONLY)
            Migrate(app, executeSeedDb: env.IsDevelopment());
        }
        
        /**
         * Applies possible missing migrations from the database.
         */
        public static void Migrate(IApplicationBuilder app, bool executeSeedDb = false)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<HangmanDbContext>();

            // always execute possible missing migrations
            if (context.Database.GetPendingMigrations().ToList().Any())
                context.Database.Migrate();
            
            // seeding DB only when asked
            if (!executeSeedDb) return;
            SeedDb(context);
        }

        /**
         * Seeds DB with pre-defined entities/models.
         */
        private static void SeedDb(HangmanDbContext context)
        {
            if (context.GameRooms.Any()) return;  // no seeding again
            
            // Entities/models seeding...
            var gameRooms = new List<GameRoom>
            {
                new GameRoom {Name = "Game Room 1"},
                new GameRoom {Name = "Game Room 2"},
                new GameRoom {Name = "Game Room 3"}
            };
            context.AddRange(gameRooms);
            
            // final db saving
            context.SaveChanges();
        }
    }
}
