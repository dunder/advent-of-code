using Xunit;

namespace Y2017.Day19 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {

            string[] input = {
                "    |         ",
                "    |  +--+   ",
                "    A  |  C   ",
                "F---|----E|--+",
                "    |  |  |  D",
                "    +B-+  +--+"
            };

            string letters = RoutingDiagram.LettersInRoute(input);

            Assert.Equal("ABCDEF", letters);
        }
        [Fact]
        public static void Problem2_Example() {

            string[] input = {
                "    |         ",
                "    |  +--+   ",
                "    A  |  C   ",
                "F---|----E|--+",
                "    |  |  |  D",
                "    +B-+  +--+"
            };

            long count = RoutingDiagram.CountSteps(input);

            Assert.Equal(38, count);
        }
    }
}
