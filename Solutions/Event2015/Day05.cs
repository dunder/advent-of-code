using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 5: Doesn't He Have Intern-Elves For This? ---
    public class Day05
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

            Assert.Equal(238, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(69, result);
        }

    }
}
