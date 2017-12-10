using System.Linq;
using Xunit;
using Y2017.Day10;

namespace Y2017.Day11 {
    public class Tests {
        [Fact]
        public void Problem1_ArrayReplace() {
            var newArray = StringHash.ArrayReverseReplace(new[] {1, 2, 3, 4}, 3, 2);
            Assert.Equal(new [] {4, 2, 3, 1}, newArray);
        }

        [Fact]
        public void Problem1_Example1() {
            var hash = StringHash.Hash(5, "3,4,1,5");
            Assert.Equal(12, hash);
        }
       
        [Fact]
        public void Problem2_Test1() {

            int[] numbers = {65, 27, 9, 1, 4, 3, 40, 50, 91, 7, 6, 0, 2, 5, 68, 22};
            int condenced = numbers.Aggregate((a, b) => a ^ b);
            Assert.Equal(64, condenced);
        }

        [Theory]
        [InlineData("", "a2582a3a0e66e6e86e3812dcb672a272")]
        [InlineData("AoC 2017", "33efeb34ea91902bb2f59c9920caa6cd")]
        [InlineData("1,2,3", "3efbe78a8d82f29979031a4aa0b16a9d")]
        [InlineData("1,2,4", "63960835bcdc130f0b66d7ff4f6a5a8e")]
        public void Problem2_TestFull(string input, string expectedHash) {

            var hash = StringHash.HashAscii(256, input);

            Assert.Equal(expectedHash, hash);
        }

    }
}
