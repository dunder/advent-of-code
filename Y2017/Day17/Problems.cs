using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day17 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            int input = 301;

            var result = CircularBuffer.ValueAfter(input, 2017);

            _output.WriteLine($"Day 17 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            int input = 301;

            var result = CircularBuffer.ValueAfterZero(input, 50_000_000);

            _output.WriteLine($"Day 17 problem 2: {result}");
        }
    }

    public class CircularBuffer {
        internal static int ValueAfter(int stepLength, int insertions) {

            List<int> circularBuffer = new List<int> {0};

            int currentPosition = 0;

            for (int i = 1; i <= insertions; i++) {
                currentPosition += stepLength;
                currentPosition = currentPosition > circularBuffer.Count - 1 ? currentPosition % circularBuffer.Count : currentPosition;
                currentPosition += 1;
                circularBuffer.Insert(currentPosition, i);
            }

            return circularBuffer.ElementAt(currentPosition + 1);
        }

        public static object ValueAfterZero(int stepLength, int insertions) {

            int currentPosition = 0;
            int lastInsertedAt0 = 0;
            int currentLength = 1;

            for (int i = 1; i <= insertions; i++) {
                currentPosition += stepLength;
                currentPosition = currentPosition > currentLength - 1 ? currentPosition % currentLength : currentPosition;
                if (currentPosition == 0) {
                    lastInsertedAt0 = i;
                }
                currentPosition += 1;
                currentLength += 1;
            }

            return lastInsertedAt0;
        }
    }
}
