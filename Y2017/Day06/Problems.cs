using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day06 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string input = File.ReadAllText(@".\Day06\input.txt");
            int[] slots = Regex.Replace(input, @"\s+", " ").Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            (int firstCycleCount, _) = DebuggerMemory.CountRedistsToSame(slots);

            Assert.Equal(7864, firstCycleCount);
            _output.WriteLine($"Day 6 problem 1: {firstCycleCount}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day06\input.txt");
            int[] slots = Regex.Replace(input, @"\s+", " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            (_, int count) = DebuggerMemory.CountRedistsToSame(slots);

            Assert.Equal(1695, count);
            _output.WriteLine($"Day 6 problem 1: {count}");
        }
    }
}
