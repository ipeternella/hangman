using System.Net.Http;
using System.Threading.Tasks;
using HangmanProject = Hangman;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Tests.Hangman.Integration
{
    public class GameRoomControllerTest : IClassFixture<WebApplicationFactory<HangmanProject.Startup>>
    {
        private readonly WebApplicationFactory<HangmanProject.Startup> _factory;
        private readonly HttpClient _httpClient;

        /**
         * xUnit.net creates a new instance for every test. Construct is always run before.
         */
        public GameRoomControllerTest(WebApplicationFactory<HangmanProject.Startup> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Theory(DisplayName = "Should make a request to /api/v1/gameroom and get HTTP 200")]
        [InlineData("/api/v1/gameroom")]
        public async Task TestShouldMakeRequestToGameRoomAndGetHttp200(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}