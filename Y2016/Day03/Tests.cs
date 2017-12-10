using Xunit;

namespace Y2016.Day03 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {
            string[] input = { "5 4 3" };

            var count = Triangle.CountPossibleTriangles(input);

            Assert.Equal(1, count);
        }

        [Fact]
        public void Problem1_Invalid() {
            string[] input = { "5 10 25"};

            var count = Triangle.CountPossibleTriangles(input);

            Assert.Equal(0, count);
        }

        [Fact]
        public void Problem2_Example1() {
            string[] input = {
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
    }
}
