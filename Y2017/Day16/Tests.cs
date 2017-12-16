using Xunit;
using Y2017.Day15;

namespace Y2017.Day16 {
    public class Tests {
        [Theory]
        [InlineData("s1", "eabcd")]
        [InlineData("s1,x3/4", "eabdc")]
        [InlineData("s1,x3/4,pe/b", "baedc")]
        public static void Problem1_Example1(string instructions, string expected) {

            var positions = Dancing.Read("abcde", instructions);

            Assert.Equal(expected, positions);
        }

        [Fact]
        public static void Problem1_Example2() {
            string instructions = "s3";
            var order = Dancing.Read("abcde", instructions);

            Assert.Equal("cdeab", order);
        }
    }
}
