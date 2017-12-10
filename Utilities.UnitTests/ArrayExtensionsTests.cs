using Xunit;

namespace Utilities.UnitTests
{
    public class ArrayExtensionsTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 1)]
        [InlineData(-1, 3)]
        public void Test(int index, int expectedValue) {
            int[] array = {1, 2, 3};

            int value = array.GetWithWrappedIndex(index);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData(new[] {1, 2, 3, 4}, 2, 2, new[] {3,4})]
        [InlineData(new[] {1, 2, 3, 4}, 3, 2, new[] {4,1})]
        public void SubArrayWithWrap(int[] data, int index, int length, int[] expected) {

            var result = data.SubArrayWithWrap(index, length);

            Assert.Equal(expected, result);
        }
    }
}
