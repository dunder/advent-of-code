using Xunit;

namespace Y2017.Day14 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {
            const string input = "flqrgnkx";

            var lit = HashGrid.CountLit(input);

            Assert.Equal(8108, lit);
        }
        [Fact]
        public static void Problem2_Example() {
            const string input = "flqrgnkx";

            var groups = HashGrid.ContinousRegions(input);

            Assert.Equal(1242, groups);
        }

        // 1242
    }
}
