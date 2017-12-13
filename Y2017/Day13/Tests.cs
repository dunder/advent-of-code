using Xunit;

namespace Y2017.Day13 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {

            string[] input = {
                "0: 3",
                "1: 2",
                "4: 4",
                "6: 4"
            };
            var severity = Problems.Firewall.CountSeverity(input);

            Assert.Equal(24, severity);
        }

        [Fact]
        public static void Problem2_Example() {

            string[] input = {
                "0: 3",
                "1: 2",
                "4: 4",
                "6: 4"
            };
            var delay = Problems.Firewall.DelayToSafe(input);

            Assert.Equal(10, delay);
        }
    }
}
