using System;
using System.Net.Http;
using Hangman.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Tests.Hangman.Support
{
    public class TestingCaseFixture<TStartup> : IDisposable where TStartup : class
    {
        // private testing properties
        private readonly IServiceProvider _services;
        private readonly IDbContextTransaction _transaction;
     
        // properties used by testing classes
        protected readonly HttpClient Client;
        protected HangmanDbContext DbContext { get; set; }

        public TestingCaseFixture()
        {
            var builder = WebHost.CreateDefaultBuilder()
                .UseStartup<TStartup>()
                .UseSerilog();

            // construct the test server and client we'll use to send requests
            var server = new TestServer(builder);
            _services = server.Host.Services;  // services provider of the host (after startup)

            // resolve a DbContext instance from the container and begin a transaction on the context.
            // DbContext = GetRequiredService<HangmanDbContext>();
            Client = server.CreateClient();
            DbContext = GetService<HangmanDbContext>();
            _transaction = DbContext.Database.BeginTransaction();
        }

        private T GetService<T>() => (T) _services.GetService(typeof(T));

        public void Dispose()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
            _transaction.Dispose();
        }
    }
}