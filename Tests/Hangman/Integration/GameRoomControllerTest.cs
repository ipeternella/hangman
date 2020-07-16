using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hangman.Repository;
using HangmanProject = Hangman;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Tests.Hangman.Support;
using Xunit;
using Newtonsoft.Json.Linq;

namespace Tests.Hangman.Integration
{
    public class GameRoomControllerTest : IClassFixture<WebApplicationFactory<HangmanProject.Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;

        /**
         * xUnit.net creates a new instance for every test. Construct is always run before.
         */
        public GameRoomControllerTest(WebApplicationFactory<HangmanProject.Startup> webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
            _serviceProvider = webApplicationFactory.Server.Services;  // Server.Host.Services for non-generic host;
        }

        [Theory(DisplayName = "Should make a request to /api/v1/gameroom and get an HTTP 200")]
        [InlineData("/api/v1/gameroom")]
        public async Task TestShouldMakeRequestToGameRoomAndGetHttp200(string url)
        {
            // Arrange -- service provider allows one to obtain a DB context which is disposed on the scope's end (cleans DB)
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetService<HangmanDbContext>();
            var testingScenarioBuilder = new TestingScenarioBuilder(db);
            
            testingScenarioBuilder.RefreshDatabaseState();
            testingScenarioBuilder.BuildScenarioWithThreeRooms();

            // Act
            var response = await _httpClient.GetAsync(url);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseAsJson = JArray.Parse(responseAsString);  // JObject if response is not an array

            // Assert
            Assert.Equal(3, responseAsJson.Count);
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}