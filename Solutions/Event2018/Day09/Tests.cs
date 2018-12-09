using Xunit;

namespace Solutions.Event2018.Day09
{
    public class Tests
    {
        [Theory]
        [InlineData(9, 25, 32)]
        [InlineData(10, 1618, 8317)]
        [InlineData(13, 7999, 146373)]
        [InlineData(17, 1104, 2764)]
        [InlineData(21, 6111, 54718)]
        [InlineData(30, 5807, 37305)]
        public void FirstStar_Example(int players, int scoreLastMarble, int expectedScore)
        {
            var score = Problem.WinnerScore(players, scoreLastMarble);

            Assert.Equal(expectedScore, score);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("371284", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("3038972494", actual);
        }
    }
}
