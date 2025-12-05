using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 5: Cafeteria ---
    public class Day05
    {
        private readonly ITestOutputHelper output;

        public Day05(ITestOutputHelper output)
        {
            this.output = output;
        }

        public record Range(long Start, long End)
        {
            public bool IsWithin(long id) => id >= Start && id <= End;
        }

        private static long Problem1(IList<string> input)
        {
            input = input.Where(line => !string.IsNullOrEmpty(line)).ToList();

            var ranges = input.Where(line => line.Contains("-")).Select(line => {

                var parts = line.Split("-");
                return new Range(long.Parse(parts[0]), long.Parse(parts[1]));
            });
            var ids = input.Where(line => !line.Contains("-")).Select(line => long.Parse(line)).ToList();

            return ids.Count(id => ranges.Any(range => range.IsWithin(id)));
        }

        private static long Problem2(IList<string> input)
        {
            var ranges = input.Where(line => line.Contains("-")).Select(line =>
            {
                var parts = line.Split("-");
                return new Range(long.Parse(parts[0]), long.Parse(parts[1]));
            });

            var sortedRanges = ranges.OrderBy(range => range.Start).ThenBy(range => range.End).ToList();

            Range previous = sortedRanges[0];
            List<Range> trimmedRanges = [previous];

            for (int i = 1; i < sortedRanges.Count; i++)
            {
                Range range = sortedRanges[i];

                if (range.Start > previous.End)
                {
                    trimmedRanges.Add(range);
                    previous = range;
                    continue;
                }

                if(range.End > previous.End)
                {
                    range = range with { Start = previous.End + 1 };
                    trimmedRanges.Add(range);
                    previous = range;
                    continue;
                }
            }

            return trimmedRanges.Sum(line => line.End - line.Start + 1);
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(770, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(357674099117260, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(3, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(14, Problem2(exampleInput));
        }
    }
}
