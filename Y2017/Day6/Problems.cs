using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            string input = File.ReadAllText(@".\Day6\input.txt");
            int[] slots = Regex.Replace(input, @"\s+", " ").Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            var result = DebuggerMemory.CountRedistsToSame(slots);

            _output.WriteLine($"Day 6 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day6\input.txt");
            int[] slots = Regex.Replace(input, @"\s+", " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            var result = DebuggerMemory.CountRedistsToSame(slots);

            _output.WriteLine($"Day 6 problem 1: {result.Item2}");
        }
    }
}
