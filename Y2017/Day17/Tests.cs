using Xunit;

namespace Y2017.Day17 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {

            var result = CircularBuffer.ValueAfter(3, 2017);

            Assert.Equal(638, result);
        }
    }
}
