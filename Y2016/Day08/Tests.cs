using Xunit;

namespace Y2016.Day08 {
    public class Tests {
        [Fact]
        public void Problem1_Exmple1() {

            var input = new[] {
                "rect 3x2",
                "rotate column x=1 by 1",
                "rotate row y=0 by 4"
            };
            var display = new Display(3,7);
            var count = display.CountPixelsLit(input);

            Assert.Equal(6, count);
        }
    }
}
