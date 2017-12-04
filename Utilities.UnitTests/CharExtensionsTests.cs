using Xunit;

namespace Utilities.UnitTests {
    public class CharExtensionsTests {

        [Theory]
        [InlineData('a', 0, 'a')]
        [InlineData('a', 1, 'b')]
        [InlineData('a', 8, 'i')]
        [InlineData('a', 25, 'z')]
        [InlineData('z', 1, 'a')]
        [InlineData('a', 26, 'a')]
        [InlineData('a', 27, 'b')]
        [InlineData('x', 343, 'c')]
        public void Shift(char input, int shift, char expectedResult) {
            var result = input.Shift(shift);
            Assert.Equal(expectedResult, result);
        }
    }
}
