using Xunit;

namespace Solutions.Event2017.Day17
{
    public class Tests
    {
        [Fact]
        public static void FirstStarExample()
        {
            var result = CircularBuffer.ValueAfter(3, 2017);

            Assert.Equal(638, result);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("1642", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("33601318", actual);
        }
    }
}