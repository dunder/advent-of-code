using Xunit;

namespace Solutions.Event2016.Day10 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {
            string[] input = {
                "value 5 goes to bot 2                           ",
                "bot 2 gives low to bot 1 and high to bot 0      ",
                "value 3 goes to bot 1                           ",
                "bot 1 gives low to output 1 and high to bot 0   ",
                "bot 0 gives low to output 2 and high to output 0",
                "value 2 goes to bot 2                           "
            };

            var bot = Robots.FindBot(input, 2, 5);

            Assert.Equal(2, bot);
        }

        [Fact]
        public void Day10_Problem1_Solution()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("31", actual);
        }

        [Fact]
        public void Day10_Problem2_Solution()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("55", actual);
        }
    }
}
