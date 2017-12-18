using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day16 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            string input = File.ReadAllText(@".\Day16\input.txt");

            var result = Dancing.Read("abcdefghijklmnop", input);

            Assert.Equal("kgdchlfniambejop", result);
            _output.WriteLine($"Day 16 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day16\input.txt");

            var result = Dancing.Read("abcdefghijklmnop", input, 1_000_000_000);

            Assert.Equal("fjpmholcibdgeakn", result);
            _output.WriteLine($"Day 16 problem 2: {result}");
        }
    }
}
