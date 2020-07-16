using System;
using System.Linq;
using Hangman.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.Hangman.Infrastructure
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        // before WebApplicationFactory creates an in-memory testing application
        // this method is called with an IWebHostBuilder (initializer abstraction)
        // to configure the testing application server
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                               typeof(DbContextOptions<HangmanDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using var scope = sp.CreateScope(); // end of the scope: disposes of services
                var scopedServices = scope.ServiceProvider; // provides services such as injected ones

                var db = scopedServices.GetRequiredService<HangmanDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                // Ensure the database is created.
                db.Database.EnsureCreated();
            });
        }
    }
}