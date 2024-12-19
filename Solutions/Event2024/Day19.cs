using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 19: Phrase ---
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

            var examine = towels.Where(pattern.StartsWith).ToList();

            bool found = false;

            foreach (var towel in examine)
            {
                found = found || PickTowels(towels, pattern.Substring(pattern.Length - (pattern.Length - towel.Length)));
            }

            return found;
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

            var examine = towels.Where(pattern.StartsWith).ToList();

            long count = 0;

            foreach (var towel in examine)
            {
                string next = pattern.Substring(pattern.Length - (pattern.Length - towel.Length));
                var tcount = CountTowels(towels, next, countCache);

                countCache[next] = tcount;

                count += tcount;
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

            Assert.Equal(-1, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input));  // 2634497 That's not the right answer
        }

        private string exampleText = "";
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
            Assert.Equal(-1, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(-1, Problem2(exampleInput));
        }
    }
}
