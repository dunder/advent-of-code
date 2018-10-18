using Xunit;

namespace Solutions.Event2016.Day09 {
    public class Tests {
        [Theory]
        [InlineData("ADVENT", "ADVENT")]
        [InlineData("A(1x5)BC", "ABBBBBC")]
        [InlineData("(3x3)XYZ", "XYZXYZXYZ")]
        [InlineData("(6x1)(1x3)A", "(1x3)A")]
        [InlineData("X(8x2)(3x3)ABCY", "X(3x3)ABC(3x3)ABCY")]
        public void FirstStarExample(string input, string expected) {

            var result = Compresser.Decompress(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("(3x3)XYZ", 9)]
        [InlineData("X(8x2)(3x3)ABCY", 20)]
        public void SecondStarExample(string input, int expected) {

            var result = Compresser.DecompressV2(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("(27x12)(20x12)(13x14)(7x10)(1x12)A", 241920)]
        [InlineData("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN", 445)]
        public void SecondStarExample2(string input, int expected) {

            var result = Compresser.DecompressV2(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("138735", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("11125026826", actual);
        }
    }
}
