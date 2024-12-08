using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 7: Bridge Repair ---
    public class Day07
    {
        private readonly ITestOutputHelper output;

        public Day07(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static List<(long result, List<int> numbers)> Parse(IList<string> lines)
        {
            return lines.Select(line =>
            {
                string[] parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                long result = long.Parse(parts[0]);
                List<int> numbers = parts[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                return (result, numbers);

            }).ToList();
        }

        public enum Operator
        {
            Addition,
            Multiplication,
            Merge
        }

        private static long Compute(Dictionary<int, Func<long, int, long>> methods, IList<string> input)
        {
            var calibrations = Parse(input);

            var valid = new List<long>();

            List<long> Compute(List<int> numbers, List<long> totals, long result)
            {
                if (!numbers.Any())
                {
                    return totals;
                }

                var second = numbers.First();

                // optimization
                if (second > result)
                {
                    return totals;
                }

                var newNumbers = numbers.Skip(1).ToList();
                var newTotals = methods.Values
                    .Select(method => totals.Select(total => method(total, second)).ToList())
                    .SelectMany(x => x)
                    .ToList();

                return Compute(newNumbers, newTotals, result);
            }

            foreach (var calibration in calibrations)
            {
                var totals = Compute(calibration.numbers.Skip(1).ToList(), [calibration.numbers.First()], calibration.result);

                if (totals.Any(total => total == calibration.result))
                {
                    valid.Add(calibration.result);
                }
            }

            return valid.Sum();
        }

        private static long Problem1(IList<string> input)
        {
            var methods = new Dictionary<int, Func<long, int, long>>
                {
                    { 1, (x, y) => x + y },
                    { 2, (x, y) => x * y },
                };

            return Compute(methods, input);
        }

        private static long Problem2(IList<string> input)
        {
            var methods = new Dictionary<int, Func<long, int, long>>
                {
                    { 1, (x, y) => x + y },
                    { 2, (x, y) => long.Parse(x.ToString() + y) },
                    { 3, (x, y) => x * y },
                };

            return Compute(methods, input);
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1620690235709, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(145397611075341, Problem2(input));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "190: 10 19",
                "3267: 81 40 27",
                "83: 17 5",
                "156: 15 6",
                "7290: 6 8 6 15",
                "161011: 16 10 13",
                "192: 17 8 14",
                "21037: 9 7 18 13",
                "292: 11 6 16 20",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(3749, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(11387, Problem2(exampleInput));
        }
    }
}
