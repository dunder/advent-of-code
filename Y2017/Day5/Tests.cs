using Xunit;

namespace Y2017.Day5 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {
            int[] input = {0, 3, 0, 1, -3};
            var result = JumpInterrupting.StepsToExit(input, 0);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Problem2_Example1() {
            int[] input = { 0, 3, 0, 1, -3 };
            var result = JumpInterrupting.StepsToExitNew(input);
            Assert.Equal(10, result);
        }
        
    }
}
