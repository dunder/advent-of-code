using Xunit;

namespace Solutions.Event2018.Day01
{
    public class Tests
    {
        [Fact]
        public void Problem1_Example()
        {

        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("513", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("287", actual);
        }
    }
}
