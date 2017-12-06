using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day6 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string input = File.ReadAllText(@".\Day6\input.txt");
            int[] slots = Regex.Replace(input, @"\s+", " ").Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            var result = DebuggerMemory.CountRedistsToSame(slots);

            _output.WriteLine($"Day 6 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day6\input.txt");
            int[] slots = Regex.Replace(input, @"\s+", " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            var result = DebuggerMemory.CountRedistsToSame(slots);

            _output.WriteLine($"Day 6 problem 1: {result.Item2}");
        }
    }

    public class DebuggerMemory {
        public static (int, int) CountRedistsToSame(int[] slots) {
            int count = 0;
            int firstCycleCount = 0;
            bool cycle = true;
            HashSet<string> targets = new HashSet<string>();
            targets.Add(string.Join(" ", slots));
            while (true) {

                int maxIndex = 0;
                int max = 0;
                for (int i = 0; i < slots.Length; i++) {
                    if (slots[i] > max) {
                        max = slots[i];
                        maxIndex = i;
                    }
                }
                int distribute = slots[maxIndex];
                slots[maxIndex] = 0;
                int index = maxIndex;
                while (true) {

                    if (++index == slots.Length) {
                        index = 0;
                    }

                    slots[index] += 1;
                    distribute--;

                    
                    if (distribute == 0) {
                        count++;

                        var slotCheck = string.Join(" ", slots);
                        if (!targets.Add(slotCheck)) {
                            if (cycle) {
                                firstCycleCount = count;
                                count = 0;
                                cycle = false;
                                targets.Clear();
                                targets.Add(slotCheck);
                            }
                            else {
                                return (firstCycleCount, count);
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
