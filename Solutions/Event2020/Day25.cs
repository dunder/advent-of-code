using Xunit;
using Xunit.Abstractions;


namespace Solutions.Event2020
{
    // 

    public class Day25
    {
        private readonly ITestOutputHelper output;


        public Day25(ITestOutputHelper output)
        {
            this.output = output;
        }

        public int FirstStar()
        {
            return 0;
        }

        public int SecondStar()
        {
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
            var result = SecondStar();
            Assert.Equal(-1, result);
        }
    }
}
