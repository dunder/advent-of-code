using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day19 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string input = File.ReadAllText(@".\Day18\input.txt");

            var result = "";

            _output.WriteLine($"Day 18 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day18\input.txt");

            var result = "";

            _output.WriteLine($"Day 18 problem 2: {result}");
        }
    }
}
