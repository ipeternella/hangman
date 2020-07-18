using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tests.Hangman.Support;
using HangmanProject = Hangman;
using Xunit;


namespace Tests.Hangman.Integration
{
    public class GameRoomsTests : TestingCaseFixture<HangmanProject.TestStartUp>
    {
        [Theory(DisplayName = "Should make a request to /api/v1/gameroom and get an HTTP 200")]
        [InlineData("/api/v1/gameroom")]
        public async Task TestShouldMakeRequestToGameRoomAndGetHttp200(string url)
        {
            // arrange
            var testingScenarioBuilder = new TestingScenarioBuilder(DbContext);
            await testingScenarioBuilder.BuildScenarioWithThreeRooms();

            // act
            var response = await Client.GetAsync(url);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseAsJson = JArray.Parse(responseAsString);  // JObject if response is not an array

            // assert
            Assert.Equal(3, responseAsJson.Count);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}