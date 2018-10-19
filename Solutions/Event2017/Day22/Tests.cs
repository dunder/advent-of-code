using Xunit;

namespace Solutions.Event2017.Day22 {
    public class Tests {
        [Fact]
        public static void FirstStarExample() {

            var input = new [] {
                "..#",
                "#..",
                "..."
            };

            var result = SporificaVirus.BurstsCausingInfection(input);

            Assert.Equal(5587, result);
        }
        [Fact]
        public static void SecondStarExample() {

            var input = new [] {
                "..#",
                "#..",
                "..."
            };

            var result = SporificaVirus.BurstsCausingInfectionV2(input);

            Assert.Equal(2511944, result);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("5433", actual);
        }
        
        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("2512599", actual);
        }
    }
}
