using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day01 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            string input = File.ReadAllText(@".\Day01\input.txt");

            int result = Captcha.Read(input);

            Assert.Equal(1216, result);
            _output.WriteLine($"Day 1 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day01\input.txt");

            int result = Captcha.ReadHalfway(input);

            Assert.Equal(1072, result);
            _output.WriteLine($"Day 1 problem 2: {result}");
        }
    }

 
}
