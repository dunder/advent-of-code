using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 3: Lobby ---
    public class Day03
    {
        private readonly ITestOutputHelper output;

        public Day03(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static int Problem1(IList<string> input)
        {
            var banks = input.Select(row => row.Select(c => c - '0').ToList());

            var joltage = 0;

            foreach (var bank in banks)
            {
                var first = bank.Take(bank.Count - 1);

                var max = first.Max();
                var right = bank[(bank.IndexOf(max)+1)..];

                var next = right.Max();

                joltage += 10 * max + next;
            }

            return joltage;
        }

        private static long Problem2(IList<string> input)
        {
            var banks = input.Select(row => row.Select(c => c - '0').ToList());

            long joltage = 0;

            foreach (var bank in banks)
            {
                List<int> result = [];

                var i = 0;

                while (result.Count < 12)
                {
                    var left = bank.Skip(i).ToList();
                    var check = left.Take(left.Count - (12 - result.Count - 1)).ToList();
                    var max = check.Max();
                    var imax = check.IndexOf(max);
                    result.Add(bank[i + imax]);
                    i = i + imax + 1;
                }

                joltage += long.Parse(string.Join("", result));
            }

            return joltage;
        }

        [Fact]
        [Trait("Event", "2025")]
        [Trait("Event", "2025-03")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(17408, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        [Trait("Event", "2025-03")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(172740584266849, Problem2(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        [Trait("Event", "2025-03")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(357, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2025")]
        [Trait("Event", "2025-03")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(3121910778619, Problem2(exampleInput));
        }
    }
}
