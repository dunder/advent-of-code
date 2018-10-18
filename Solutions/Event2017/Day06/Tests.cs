using Xunit;

namespace Solutions.Event2017.Day06 {
    public class Tests {
        [Fact]
        public void FirstStarExample() {

            (int firstCycleCount, int count) = DebuggerMemory.CountRedistsToSame(new[] {0, 2, 7, 0});

            Assert.Equal(5, firstCycleCount);
            Assert.Equal(4, count);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("7864", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("1695", actual);
        }
    }
}
