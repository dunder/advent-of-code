using Xunit;

namespace Solutions.Event2018.Day05
{
    public class Tests
    {
        [Theory]
        [InlineData("dabAcCaCBAcCcaDA", "dabAaCBAcCcaDA")]
        [InlineData("dabAaCBAcCcaDA", "dabCBAcCcaDA")]
        public void ReactReduce(string polymer, string expected)
        {
            var reduced = Problem.ReactReduce(polymer);

            Assert.Equal(expected, reduced);
        }

        [Fact]
        public void FirstStarExample()
        {
            var polymer = "dabAcCaCBAcCcaDA";
            var reduced = Problem.ReduceAll(polymer);

            Assert.Equal(10, reduced);
        }
        [Fact]
        public void SecondStarExample()
        {
            var polymer = "dabAcCaCBAcCcaDA";
            var reduced = Problem.ReduceEnhanced(polymer);

            Assert.Equal(4, reduced);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("10132", actual);
        }

        [Trait("Category", "LongRunning")] // 3 min 18 s
        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("4572", actual);
        }
    }
}
