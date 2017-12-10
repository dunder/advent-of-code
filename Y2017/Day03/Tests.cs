using Xunit;

namespace Y2017.Day03 {
    public class Tests {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(12, 3)]
        [InlineData(23, 2)]
        public void Problem1_Example1(int address, int expectedDistance) {
            var distance = SpiralMemory.Distance(address);
            Assert.Equal(expectedDistance, distance);
        }

        [Theory]
        [InlineData(4, 5)]
        [InlineData(5, 10)]
        [InlineData(330, 351)]
        public void Problem2_Example1(int value, int expected) {
            var written = SpiralMemory.FirstWrittenLargerThan(value);
            Assert.Equal(expected, written);
        }
    }
}
