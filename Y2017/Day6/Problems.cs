using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day6 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            int[] input = File.ReadAllLines(@".\Day6\input.txt").Select(int.Parse).ToArray();

            var result = "Not implemented";

            _output.WriteLine($"Day 6 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            int[] input = File.ReadAllLines(@".\Day6\input.txt").Select(int.Parse).ToArray();

            var result = "Not implemented";

            _output.WriteLine($"Day 6 problem 2: {result}");
        }
    }
}
