using Xunit;

namespace Solutions.Event2016.Day03
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            string[] input = {"5 4 3"};

            var count = Triangle.CountPossibleTriangles(input);

            Assert.Equal(1, count);
        }

        [Fact]
        public void FirstStarInvalidTriangle()
        {
            string[] input = {"5 10 25"};

            var count = Triangle.CountPossibleTriangles(input);

            Assert.Equal(0, count);
        }

        [Fact]
        public void SecondStarExample()
        {
            string[] input =
            {
                "101 301 501",
                "102 302 502",
                "103 303 503",
                "201 401 601",
                "202 402 602",
                "203 403 603"
            };

            var count = Triangle.CountPossibleTrianglesVertically(input);

            Assert.Equal(6, count);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("1050", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("1921", actual);
        }
    }
}