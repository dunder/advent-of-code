using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day09 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string input = File.ReadAllText(@".\Day09\input.txt");
            int result = Compresser.Decompress(input).Length;

            Assert.Equal(138735, result);
            _output.WriteLine($"Day 9 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day09\input.txt");

            var result = Compresser.DecompressV2(input);

            Assert.Equal(11125026826, result);
            _output.WriteLine($"Day 9 problem 2: {result}");
        }
    }
}
