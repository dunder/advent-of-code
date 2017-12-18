using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day18 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day18\input.txt");

            var result = DuetAssembly.RecoveredFrequency(input);

            Assert.Equal(4601, result);
            _output.WriteLine($"Day 18 problem 1: {result}");  // not: -3989
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day18\input.txt");

            var result = DuetAssembly.DualCount2(input);

            Assert.Equal(6858, result);
            _output.WriteLine($"Day 18 problem 2: {result}");
        }
    }
}
