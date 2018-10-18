using Xunit;

namespace Solutions.Event2017.Day15 {
    public class Tests {
        [Trait("Category", "LongRunning")]
        [Fact]
        public static void FirstStarExample() {

            var count = Generator.Judge(65, 8921);

            Assert.Equal(588, count);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public static void SecondStarExample() {

            var count = Generator.Judge2(65, 8921);

            Assert.Equal(309, count);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("567", actual);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("323", actual);
        }
    }
}
