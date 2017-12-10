using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day03 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            string[] input = File.ReadAllLines(@".\Day03\input.txt");

            var result = Triangle.CountPossibleTriangles(input);

            Assert.Equal(1050, result);
            _output.WriteLine($"Day 3 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day03\input.txt");

            var result = Triangle.CountPossibleTrianglesVertically(input);

            Assert.Equal(1921, result);
            _output.WriteLine($"Day 3 problem 2: {result}");
        }
    }
}
