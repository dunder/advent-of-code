using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day1 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            string input = File.ReadAllText(@".\Day1\input.txt");

            int result = Captcha.Read(input);

            _output.WriteLine($"Day 2 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day1\input.txt");

            int result = Captcha.ReadHalfway(input);

            _output.WriteLine($"Day 2 problem 2: {result}");
        }
    }

 
}
