using Xunit;

namespace Tests.Unit
{
    public class UnitTest1
    {
        [Fact(DisplayName = "Should add 1 + 1 and get 2")]
        public void TestShouldAdd1plus1AndGet2()
        {
            var x = 1;
            var y = 1;
            
            Assert.Equal(2, x + y);
        }
    }
}