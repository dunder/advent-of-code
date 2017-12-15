using Xunit;
using Y2017.Day14;

namespace Y2017.Day15 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {

            var count = Generator.Judge(65, 8921);

            Assert.Equal(588, count);
        }

        [Fact]
        public static void Problem2_Example() {

            var count = Generator.Judge2(65, 8921);

            Assert.Equal(309, count);
        }
    }
}
