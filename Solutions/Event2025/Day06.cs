using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 6: Trash Compactor ---
    public class Day06
    {
        private readonly ITestOutputHelper output;

        public Day06(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static long Problem1(IList<string> input)
        {
            var operations = input.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            return input
                .Take(input.Count - 1)
                .Select(line => line
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToList())
                .Aggregate((line1, line2) => line1
                    .Zip(line2)
                    .Select((p, i) =>
                    {
                        var operation = operations[i];
                        return operation == "+" ? p.First + p.Second : p.First * p.Second;
                    })
                    .ToList())
                .Sum();
        }

        private static long Problem2(IList<string> input)
        {
            var operations = input.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            return input
                .Take(input.Count - 1)
                .Select(line => line.ToCharArray().Select(c => c.ToString()).ToList())
                .ToList()
                .Aggregate((line1, line2) => line1.Zip(line2).Select(z => "" + z.First + z.Second).ToList())
                .Select(s => s.Trim())
                .Split("")
                .Select(columnStrings => columnStrings.Select(int.Parse))
                .Select((columnValues, column) =>
                {
                    string operation = operations[column];
                    if (operation == "+")
                    {
                        return columnValues.Aggregate(0L, (value, acc) => acc + value);
                    }
                    else
                    {
                        return columnValues.Aggregate(1L, (value, acc) => acc * value);
                    }
                })
                .Sum();
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(5524274308182, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(8843673199391, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(4277556, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(3263827, Problem2(exampleInput));
        }
    }
}
