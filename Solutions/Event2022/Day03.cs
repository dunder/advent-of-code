using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2022
{
    // --- Day 3: Rucksack Reorganization ---
    public class Day03
    {
        private readonly ITestOutputHelper output;

        public Day03(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static int Priority(char c) => c switch
        {
            >= 'a' and <= 'z' => c - 'a' + 1,
            >= 'A' and <= 'Z' => c - 'A' + 27,
            _ => throw new ArgumentOutOfRangeException(nameof(c), $"Bad character: {c}"),
        };

        private static int Problem1(IList<string> input)
        {
            int sum = 0;

            foreach (var rucksack in input)
            {
                var compartment1 = rucksack.Substring(0, rucksack.Length / 2).ToHashSet();
                var compartment2 = rucksack.Substring(rucksack.Length / 2).ToHashSet();

                var error = compartment1.Intersect(compartment2).Single();

                sum += Priority(error);
            }

            return sum;
        }

        private static int Problem2(IList<string> input)
        {
            List<string[]> groups = input.Chunk(3).ToList();

            int sum = 0;

            foreach (var group in groups)
            {
                var x = group.Select(g => g.ToHashSet()).ToList();

                var intersection = x[0];

                intersection.IntersectWith(x[1]);
                intersection.IntersectWith(x[2]);

                sum += Priority(intersection.Single());
            }

            return sum;
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(7908, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(2838, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(157, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(70, Problem2(exampleInput));
        }
    }
}
