using Xunit;

namespace Solutions.Event2016.Day14
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            var salt = "abc";

            int result = Problem.FirstStarSolution(salt);

            Assert.Equal(22728, result);
        }

        [Fact]
        public void StretchedHash()
        {
            string hash = new Problem.StretchedHash().Hash("abc0");
            Assert.StartsWith("a107ff", hash);
        }

        [Theory]
        [InlineData("b000", '0')]
        [InlineData("fffb", 'f')]
        [InlineData("bfff", 'f')]
        [InlineData("bfffcaaa", 'f')]
        [InlineData("b0000", null)]
        [InlineData("ffffb", null)]
        [InlineData("bffff", null)]
        [InlineData("bffffcaaa", 'a')]
        [InlineData("b", null)]
        public void Find_Match(string input, char? expected)
        {
            var c = Problem.FirstRepeatedCharacter(input, 3);

            Assert.Equal(expected, c);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void SecondStarExample()
        {
            var salt = "abc";

            int result = Problem.SecondStarSolution(salt);

            Assert.Equal(22551, result); // 4 min 47 s
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("15035", actual);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("19968", actual); // 4 min 10 s
        }
    }
}
