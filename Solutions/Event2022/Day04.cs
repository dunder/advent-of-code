using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;

namespace Solutions.Event2022
{
    // --- Day 4: Camp Cleanup ---
    public class Day04
    {
        private readonly ITestOutputHelper output;

        public Day04(ITestOutputHelper output)
        {
            this.output = output;
        }

        public record Range(int Start, int End);

        private static List<(Range range1, Range range2)> Parse(IList<string> input)
        {
            return input.Select(row =>
            {
                var parts = row.Split(",");
                var r1 = parts[0].Split("-");
                var r2 = parts[1].Split("-");

                Range range1 = new (int.Parse(r1[0]), int.Parse(r1[1]));
                Range range2 = new (int.Parse(r2[0]), int.Parse(r2[1]));

                return (range1, range2);
            }).ToList();
        }

        private static bool Contains(Range range1, Range range2)
        {
            bool contains1 = range1.Start >= range2.Start && range1.End <= range2.End;
            bool contains2 = range2.Start >= range1.Start && range2.End <= range1.End;

            return contains1 || contains2;
        }

        private static bool Overlaps(Range range1, Range range2)
        {
            if (Contains(range1, range2))
            {
                return true;
            }

            bool overlap1 = range1.Start >= range2.Start && range1.Start <= range2.End;
            bool overlap2 = range1.End >= range2.Start && range1.End <= range2.End;

            return overlap1 || overlap2;
        }

        private static int Problem1(IList<string> input)
        {
            return Parse(input).Count(x => Contains(x.range1, x.range2));
        }

        private static int Problem2(IList<string> input)
        {
            return Parse(input).Count(x => Overlaps(x.range1, x.range2));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(490, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(921, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(2, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(4, Problem2(exampleInput));
        }
    }
}
