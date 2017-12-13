using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day13 {
    public partial class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day13\input.txt");

            var result = Firewall.CountSeverity(input);

            Assert.Equal(2160, result);
            _output.WriteLine($"Day 13 problem 1: {result}");
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day13\input.txt");

            var result = Firewall.DelayToSafe(input);

            Assert.Equal(3907470, result);
            _output.WriteLine($"Day 13 problem 2: {result}");  // 303966 och 303870 är för lågt
        }
    }
}
