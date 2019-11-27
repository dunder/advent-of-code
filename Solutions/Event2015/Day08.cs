using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 8: Matchsticks ---
    public class Day08
    {
        public static int FirstStar()
        {
            var input = ReadLineInput();

            return 0;
        }

        public static int SecondStar()
        {
            var input = ReadLineInput();

            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(1350, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(2085, result);
        }
    }
}
