using Xunit;

namespace Y2017.Day6 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {

            (int firstCycleCount, int count) = DebuggerMemory.CountRedistsToSame(new[] {0, 2, 7, 0});

            Assert.Equal(5, firstCycleCount);
            Assert.Equal(4, count);
        }

        [Fact]
        public void Problem2_Example1() {
            
        }
        
    }
}
