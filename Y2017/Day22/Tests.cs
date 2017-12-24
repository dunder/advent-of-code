using Xunit;

namespace Y2017.Day22 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {

            var input = new [] {
                "..#",
                "#..",
                "..."
            };

            var result = SporificaVirus.BurstsCausingInfection(input);

            Assert.Equal(5587, result);
        }
        [Fact]
        public static void Problem2_Example() {

            var input = new [] {
                "..#",
                "#..",
                "..."
            };

            var result = SporificaVirus.BurstsCausingInfectionV2(input);

            Assert.Equal(2511944, result);
        }
    }
}
