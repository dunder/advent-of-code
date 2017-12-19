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
            string[] input = File.ReadAllLines(@".\Day19\input.txt");

            var result = Tubes.LettersOnWayOut(input);

            Assert.Equal("SXPZDFJNRL", result);
            _output.WriteLine($"Day 19 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day19\input.txt");

            var result = Tubes.CountSteps(input);

            Assert.Equal(18126, result);
            _output.WriteLine($"Day 19 problem 2: {result}");
        }
    }
}
