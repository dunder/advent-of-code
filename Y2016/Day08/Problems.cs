using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day08 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day08\input.txt");

            int result = new Display(6, 50).CountPixelsLit(input);

            Assert.Equal(123, result);
            _output.WriteLine($"Day 8 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day08\input.txt");

            var display = new Display(6, 50);
            display.SendInstructions(input);
            var result = display.Print();

            _output.WriteLine($"Day 8 problem 2: {Environment.NewLine}{result}");  // AFBUPZBJPS
        }
    }
}
