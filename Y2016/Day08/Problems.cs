using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day08 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day08\input.txt");
            int result = 0;

            Assert.Equal(123, result);
            _output.WriteLine($"Day 7 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day08\input.txt");

            var result = "";

            Assert.Equal("AFBUPZBJPS", result);
            _output.WriteLine($"Day 7 problem 2: {result}");
        }
    }
}
