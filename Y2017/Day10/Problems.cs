using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day10 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            const string input = "225,171,131,2,35,5,0,13,1,246,54,97,255,98,254,110";

            var result = StringHash.Hash(256, input);

            Assert.Equal(23874, result);
            _output.WriteLine($"Day 10 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            const string input = "225,171,131,2,35,5,0,13,1,246,54,97,255,98,254,110";

            var result = StringHash.HashAscii(256, input);

            Assert.Equal("e1a65bfb5a5ce396025fab5528c25a87", result);
            _output.WriteLine($"Day 10 problem 2: {result}");
        }
    }
}
