using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day5 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            const string input = "reyedfim";

            var result = PasswordGenerator.Generate(input, 8);

            _output.WriteLine($"Day 2 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            const string input = "reyedfim";

            var result = "Not implemented yet";

            _output.WriteLine($"Day 2 problem 2: {result}");
        }
    }
}
