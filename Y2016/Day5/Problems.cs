using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day5 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Problem1() {

            const string input = "reyedfim";

            var result = PasswordGenerator.GenerateParallel(input, 8);

            _output.WriteLine($"Day 2 problem 1: {result}");
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Problem2() {
            const string input = "reyedfim";

            var result = PasswordGenerator.GenerateNew(input, 8);

            _output.WriteLine($"Day 2 problem 2: {result}");
        }
    }
}
