using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day25 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day25\input.txt");

            var result = TheHaltingProblem.DiagnosticChecksum(input);

            _output.WriteLine($"Day 25 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day25\input.txt");

            var result = "";

            _output.WriteLine($"Day 25 problem 2: {result}");
        }
    }

    public class TheHaltingProblem {
        public static int DiagnosticChecksum(string[] input) {
            throw new System.NotImplementedException();
        }
    }
}
