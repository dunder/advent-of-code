using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day5 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            int[] input = File.ReadAllLines(@".\Day5\input.txt").Select(int.Parse).ToArray();

            var result = JumpInterrupting.StepsToExit(input);

            _output.WriteLine($"Day 5 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            int[] input = File.ReadAllLines(@".\Day5\input.txt").Select(int.Parse).ToArray();

            var result = JumpInterrupting.StepsToExitNew(input);

            _output.WriteLine($"Day 5 problem 2: {result}");
        }
    }
}
