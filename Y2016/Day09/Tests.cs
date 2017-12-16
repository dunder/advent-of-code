using System.Linq;
using Xunit;

namespace Y2016.Day09 {
    public class Tests {
        [Theory]
        [InlineData("ADVENT", "ADVENT")]
        [InlineData("A(1x5)BC", "ABBBBBC")]
        [InlineData("(3x3)XYZ", "XYZXYZXYZ")]
        [InlineData("(6x1)(1x3)A", "(1x3)A")]
        [InlineData("X(8x2)(3x3)ABCY", "X(3x3)ABC(3x3)ABCY")]
        public void Problem1_Examples(string input, string expected) {

            var result = Compresser.Compress(input);

            Assert.Equal(expected, result);
        }
    }
}
