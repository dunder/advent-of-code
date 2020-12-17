using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // 

    public class Day18
    {
        private readonly ITestOutputHelper output;

        public Day18(ITestOutputHelper output)
        {
            this.output = output;
        }

        public int FirstStar()
        {
            var input = ReadLineInput().ToList();

            return 0;
        }

        public long SecondStar()
        {
            var input = ReadLineInput().ToList();

            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }
    }
}
