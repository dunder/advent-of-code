using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day15 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            var result = Generator.Judge(512, 191);

            _output.WriteLine($"Day 15 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {

            var result = Generator.Judge2(512, 191);

            _output.WriteLine($"Day 15 problem 2: {result}");
        }
    }

}
