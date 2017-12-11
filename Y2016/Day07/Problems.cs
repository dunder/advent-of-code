using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day07 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day07\input.txt");
            int result = Tls.CountAbba(input);

            Assert.Equal(110, result);
            _output.WriteLine($"Day 7 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day07\input.txt");

            var result = Tls.CountAba(input);

            Assert.Equal(242, result);
            _output.WriteLine($"Day 7 problem 2: {result}");
        }
    }
}
