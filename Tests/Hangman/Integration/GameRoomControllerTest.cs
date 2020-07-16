using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hangman.Repository;
using HangmanProject = Hangman;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Tests.Hangman.Support;
using Xunit;
using Newtonsoft.Json.Linq;

namespace Tests.Hangman.Integration
{
    public class GameRoomControllerTest : IClassFixture<WebApplicationFactory<HangmanProject.Startup>>
    {
        private readonly WebApplicationFactory<HangmanProject.Startup> _webApplicationFactory;
        private readonly HttpClient _httpClient;
        private readonly HangmanDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        /**
         * xUnit.net creates a new instance for every test. Construct is always run before.
         */
        public GameRoomControllerTest(WebApplicationFactory<HangmanProject.Startup> factory)
        {
            _webApplicationFactory = factory;
            _httpClient = _webApplicationFactory.CreateClient();
            _serviceProvider = _webApplicationFactory.Server.Services;  // Server.Host.Services for non-generic host;
        }

        [Theory(DisplayName = "Should make a request to /api/v1/gameroom and get an HTTP 200")]
        [InlineData("/api/v1/gameroom")]
        public async Task TestShouldMakeRequestToGameRoomAndGetHttp200(string url)
        {
            // Arrange -- service provider allows one to obtain a DB context which is disposed on the scope's end (cleans DB)
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetService<HangmanDbContext>();
            new TestingScenarioBuilder(db).BuildScenarioWithThreeRooms();

            // Act
            var response = await _httpClient.GetAsync(url);
            // dynamic jsonObject = JObject.Parse(response.Content.ToString());

            // Assert
            // Assert.Equal(3, (int) jsonObject.Count);
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}