using Xunit;

namespace Y2017.Day24 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {

            string[] input = {
                "0/2",
                "2/2",
                "2/3",
                "3/4",
                "3/5",
                "0/1",
                "10/1",
                "9/10"
            };

            var result = ElectromagneticMoat.StrongestBridge(input);

            Assert.Equal(31, result);
        }

        [Fact]
        public static void Problem2_Example() {

            string[] input = {
                "0/2",
                "2/2",
                "2/3",
                "3/4",
                "3/5",
                "0/1",
                "10/1",
                "9/10"
            };

            var result = ElectromagneticMoat.StrongestLongestBridge(input);

            Assert.Equal(19, result);
        }
    }
}
