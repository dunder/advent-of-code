using Xunit;

namespace Solutions.Event2017.Day19 {
    public class Tests {
        [Fact]
        public static void FirstStarExample() {

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
        public static void SecondStarExample() {

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

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("SXPZDFJNRL", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("18126", actual);
        }
    }
}
