using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            string[] input = File.ReadAllLines(@".\Day5\input.txt").Select(x => Regex.Replace(x, @"\s+", " ")).ToArray();

            var result = "Not implemented yet";

            _output.WriteLine($"Day 4 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day5\input.txt").Select(x => Regex.Replace(x, @"\s+", " ")).ToArray();

            var result = "Not implemented yet";

            _output.WriteLine($"Day 4 problem 2: {result}");
        }
    }
}
