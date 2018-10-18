using Xunit;

namespace Solutions.Event2017.Day05
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            int[] input = {0, 3, 0, 1, -3};
            var result = JumpInterrupting.StepsToExit(input, 0);
            Assert.Equal(5, result);
        }

        [Fact]
        public void SecondStarExample()
        {
            int[] input = {0, 3, 0, 1, -3};
            var result = JumpInterrupting.StepsToExitNew(input);
            Assert.Equal(10, result);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("326618", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("21841249", actual);
        }
    }
}