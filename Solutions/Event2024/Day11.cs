using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 11: Plutonian Pebbles ---
    public class Day11
    {
        private readonly ITestOutputHelper output;

        public Day11(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static long Blink(Dictionary<(long n, long t), long> precomputed, long number, int blinks)
        {
            if (blinks == 0)
            {
                return 1;
            }

            if (precomputed.ContainsKey((number, blinks)))
            {
                return precomputed[(number, blinks)];
            }

            if (number == 0)
            {
                var result = Blink(precomputed, 1, blinks - 1);

                precomputed.Add((number, blinks), result);

                return result;
            }
            else if (number.ToString().Length % 2 == 0)
            {
                var value = number.ToString();
                var left = value.Substring(0, value.Length / 2);
                var right = value.Substring(left.Length);

                var result = Blink(precomputed, long.Parse(left), blinks - 1) + Blink(precomputed, long.Parse(right), blinks - 1);

                precomputed.Add((number, blinks), result);

                return result;
            }
            else
            {
                var result = Blink(precomputed, number * 2024, blinks - 1);

                precomputed.Add((number, blinks), result);

                return result;
            }
        }

        private static long Solve(string input, int blinks)
        {
            Dictionary<(long n, long t), long> precomputed = new();

            return input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .Select(stone => Blink(precomputed, stone, blinks))
                .Sum();
        }

        private static long Problem1(string input)
        {
            return Solve(input, 25);
        }

        private static long Problem2(string input)
        {
            return Solve(input, 75);
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadInput();

            Assert.Equal(204022, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadInput();

            Assert.Equal(241651071960597, Problem2(input));
        }

        private string exampleText = "0 1 10 99 999";
        private string exampleText2 = "125 17";

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(7, Solve(exampleText, 1));
        }
        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample2()
        {
            Assert.Equal(55312, Problem1(exampleText2));
        }
    }
}
