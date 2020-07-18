using System;
using System.Net.Http;
using Hangman.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace Tests.Hangman.Support
{
    public class TestingCaseFixture<TStartup> : IDisposable where TStartup : class
    {
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
            var server = new TestServer(builder);
            _services = server.Host.Services;  // services provider of the host (after startup)

            // resolve a DbContext instance from the container and begin a transaction on the context.
            // DbContext = GetRequiredService<HangmanDbContext>();
            Client = server.CreateClient();
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