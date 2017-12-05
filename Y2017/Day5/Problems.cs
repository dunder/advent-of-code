using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day5 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            int[] input = File.ReadAllLines(@".\Day5\input.txt").Select(int.Parse).ToArray();

            var result = JumpInterrupting.StepsToExit(input);

            _output.WriteLine($"Day 5 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            int[] input = File.ReadAllLines(@".\Day5\input.txt").Select(int.Parse).ToArray();

            var result = JumpInterrupting.StepsToExitNew(input);

            _output.WriteLine($"Day 5 problem 2: {result}");
        }
    }

    public class JumpInterrupting {
        public static int StepsToExit(int[] input) {
            return StepsToExit(input, 0);
        }

        internal static int StepsToExit(int[] input, int startPosition) {
            int steps = 0;
            int currentIndex = startPosition;
            while (currentIndex < input.Length && currentIndex >= 0) {
                var instruction = input[currentIndex];
                input[currentIndex] += 1;
                currentIndex += instruction;
                steps++;
            }
            return steps;
        }

        public static int StepsToExitNew(int[] input) {
            int steps = 0;
            int currentIndex = 0;
            while (currentIndex < input.Length && currentIndex >= 0) {
                var instruction = input[currentIndex];
                if (instruction >= 3) {
                    input[currentIndex] -= 1;
                }
                else {
                    input[currentIndex] += 1;
                }
                currentIndex += instruction;
                steps++;
            }
            return steps;
        }
    }
}
