using System;
using System.Net.Http;
using Hangman.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Hangman.Controllers.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Tests.Hangman.Support
{
    public class Fixture
    {
        /**
         * Static constructor: run only once throughout the application, before the first instance
         * is created.
         */
        static Fixture()
        {
            CreateDatabaseAndMigrate();
        }
    
        private static void CreateDatabaseAndMigrate()
        {
            var dbConnectionString = "Host=localhost;Port=5432;Username=hangman;Password=hangman;Database=hangman;";
            var options = new DbContextOptionsBuilder<HangmanDbContext>()
                .UseNpgsql(dbConnectionString)
                .Options;
    
            new HangmanDbContext(options).Database.Migrate();
        }
    }
    
    public class TestingCaseFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer _server;
        private readonly IServiceProvider _services;
        protected IDbContextTransaction Transaction { get; }

        protected readonly HttpClient Client;
        protected HangmanDbContext DbContext { get; set; }

        public TestingCaseFixture()
        {
            var builder = WebHost.CreateDefaultBuilder()
                .UseStartup<TStartup>()
                .UseSerilog();

            // construct the test server and client we'll use to send requests
            _server = new TestServer(builder);
            _services = _server.Host.Services;  // services provider of the host (after startup)

            // resolve a DbContext instance from the container and begin a transaction on the context.
            // DbContext = GetRequiredService<HangmanDbContext>();
            Client = _server.CreateClient();
            DbContext = GetService<HangmanDbContext>();
            Transaction = DbContext.Database.BeginTransaction();
        }

        protected T GetService<T>() => (T) _services.GetService(typeof(T));

        public void Dispose()
        {
            if (Transaction == null) return;

            Transaction.Rollback();
            Transaction.Dispose();
        }
    }
}