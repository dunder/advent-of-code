using Xunit;

namespace Solutions.Event2017.Day02
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            var input = new[]
            {
                "5 1 9 5",
                "7 5 3",
                "2 4 6 8"
            };

            var checksum = new SpreadSheet(input).Checksum;

            Assert.Equal(8 + 4 + 6, checksum);
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new[]
            {
                "5 9 2 8",
                "9 4 7 3",
                "3 8 6 5"
            };

            var sumOfEvenDivisable = new SpreadSheet(input).SumEvenDivisable;

            Assert.Equal(8 / 2 + 9 / 3 + 6 / 3, sumOfEvenDivisable);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("32020", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("236", actual);
        }
    }
}