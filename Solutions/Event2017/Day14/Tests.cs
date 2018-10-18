using Xunit;

namespace Solutions.Event2017.Day14 {
    public class Tests {
        [Fact]
        public static void FirstStarExample() {
            const string input = "flqrgnkx";

            var lit = HashGrid.CountLit(input);

            Assert.Equal(8108, lit);
        }
        [Fact]
        public static void SecondStarExample() {
            const string input = "flqrgnkx";

            var groups = HashGrid.ContinousRegions(input);

            Assert.Equal(1242, groups);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("8216", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("1139", actual);
        }
    }
}
