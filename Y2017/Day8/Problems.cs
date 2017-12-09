using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day8 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day8\input.txt");

            (var highestFinal, _) = Interpreter.LargestRegisterCount(input);

            _output.WriteLine($"Day 8 problem 1: {highestFinal}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day8\input.txt");

            (_, var highestStored) = Interpreter.LargestRegisterCount(input);

            _output.WriteLine($"Day 8 problem 2: {highestStored}");
        }

        
    }
}
