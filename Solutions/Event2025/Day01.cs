using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // Day 1: Secret Entrance
    public class Day01
    {
        private readonly ITestOutputHelper output;

        public Day01(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static int Problem1(IList<string> input)
        {
            List<(char turn, int count)> instructions = input.Select(row => (row[0], int.Parse(row.Substring(1)))).ToList();

            int count = 0;
            int dial = 50;

            foreach (var instruction in instructions)
            {
                var move = instruction.turn == 'R' ? instruction.count : -instruction.count;

                dial = (dial + move) % 100;

                if (dial == 0)
                {
                    count++;
                }
            }

            return count;
        }

        private static int Problem2(IList<string> input)
        {
            List<(char turn, int count)> instructions = input.Select(row => (row[0], int.Parse(row.Substring(1)))).ToList();

            int count = 0;
            int dial = 50;

            foreach (var instruction in instructions)
            {
                var move = instruction.turn == 'R' ? 1 : -1;

                foreach (var _ in Enumerable.Repeat(1, instruction.count))
                {
                    dial = (dial + move) % 100;

                    if (dial == 0)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1092, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(6616, Problem2(input));
        }
    }
}
