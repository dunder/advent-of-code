using Xunit;

namespace Solutions.Event2018.Day05
{
    public class Tests
    {
        [Theory]
        [InlineData("dabAcCaCBAcCcaDA", "dabAaCBAcaDA")]
        [InlineData("dabAaCBAcCcaDA", "dabCBAcaDA")]
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
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("10132", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("4572", actual);
        }
    }
}
