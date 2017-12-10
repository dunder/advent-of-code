using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day03 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            var result = SpiralMemory.Distance(289326);

            _output.WriteLine($"Day 3 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            var result = SpiralMemory.FirstWrittenLargerThan(289326);

            _output.WriteLine($"Day 3 problem 2: {result}");
        }
    }

 
}
