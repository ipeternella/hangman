using Xunit;

namespace Tests.Hangman.Unit
{
    public class UnitTest1
    {
        [Fact(DisplayName = "Should add 1 + 1 and get 2")]
        public void TestShouldAdd1plus1AndGet2()
        {
            const int x = 1;
            const int y = 1;
            
            Assert.Equal(2, x + y);
        }
    }
}