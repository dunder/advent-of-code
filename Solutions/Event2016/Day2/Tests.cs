using Xunit;

namespace Solutions.Event2016.Day2 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {
            var input = new [] {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            };

            var result = KeyPad.NumericKeyPad.Sequence(input);

            Assert.Equal("1985", result);
        }

        [Fact]
        public void Problem2_Example1() {
            var input = new [] {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            };

            var result = KeyPad.AlphaNumericKeyPad.Sequence(input);

            Assert.Equal("5DB3", result);
        }

        [Fact]
        public void Day2_Problem1_Solution()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("74921", actual);
        }

        [Fact]
        public void Day2_Problem2_Solution()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("A6B35", actual);
        }
    }
}
