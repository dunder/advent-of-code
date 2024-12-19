using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 19: Linen Layout ---
    public class Day19
    {
        private readonly ITestOutputHelper output;

        public Day19(ITestOutputHelper output)
        {
            this.output = output;
        }


        private static (HashSet<string> towels, List<string> patterns) Parse(IList<string> input)
        {
            var towels = input.First().Split(", ", StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            var patterns = input.Skip(2).ToList();

            return (towels, patterns);
        }

        private static bool PickTowels(HashSet<string> towels, string pattern)
        {
            if (!pattern.Any())
            {
                return true;
            }

            var matchingTowels = towels.Where(pattern.StartsWith).ToList();

            bool isMatch = false;

            foreach (var towel in matchingTowels)
            {
                string patternWithoutTowel = pattern.Substring(pattern.Length - (pattern.Length - towel.Length));

                isMatch = isMatch || PickTowels(towels, patternWithoutTowel);
            }

            return isMatch;
        }

        private static long CountTowels(HashSet<string> towels, string pattern, Dictionary<string, long> countCache)
        {
            if (countCache.ContainsKey(pattern))
            {
                return countCache[pattern];
            }

            if (!pattern.Any())
            {
                return 1;
            }

            var matchingTowels = towels.Where(pattern.StartsWith).ToList();

            long count = 0;

            foreach (var towel in matchingTowels)
            {
                string patternWithoutTowel = pattern.Substring(pattern.Length - (pattern.Length - towel.Length));
                
                var towelCount = CountTowels(towels, patternWithoutTowel, countCache);

                countCache[patternWithoutTowel] = towelCount;

                count += towelCount;
            }

            return count;
        }

        private static int Problem1(IList<string> input)
        {
            (HashSet<string> towels, List<string> patterns) = Parse(input);

            int count = 0;

            foreach (var pattern in patterns)
            {
                if (PickTowels(towels, pattern))
                {
                    count++;
                }
            }

            return count;
        }

        private static long Problem2(IList<string> input)
        {
            (HashSet<string> towels, List<string> patterns) = Parse(input);

            long count = 0;
            
            foreach (var pattern in patterns)
            {
                count += CountTowels(towels, pattern, []);
            }

            return count;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(236, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(643685981770598, Problem2(input));
        }

        private List<string> exampleInput =
            [
                "r, wr, b, g, bwu, rb, gb, br",
                "",
                "brwrr",
                "bggr",
                "gbbr",
                "rrbgbr",
                "ubwu",
                "bwurrg",
                "brgr",
                "bbrgwb",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(6, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(16, Problem2(exampleInput));
        }
    }
}
