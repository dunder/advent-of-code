using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day09 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string input = File.ReadAllText(@".\Day09\input.txt");

            var result = GroupParser.CountGroupScore(input); 

            Assert.Equal(7616, result.GroupScore);
            _output.WriteLine($"Day 9 problem 1: {result.GroupScore}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day09\input.txt");

            var result = GroupParser.CountGroupScore(input);

            Assert.Equal(3838, result.CanceldGarbage);
            _output.WriteLine($"Day 9 problem 2: {result.CanceldGarbage}");
        }
    }
}
