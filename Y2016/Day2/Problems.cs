using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day2 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day2\input.txt");

            var result = KeyPad.Crack(input.ToList());

            _output.WriteLine($"Day 2 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day2\input.txt");

            var result = AlphaNumericKeyPad.Crack(input);

            _output.WriteLine($"Day 2 problem 2: {result}");
        }
    }
}
