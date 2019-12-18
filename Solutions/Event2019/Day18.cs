using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // 
    public class Day18
    {

        public int FirstStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(3401852, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(5099916, SecondStar());
        }
    }
}
