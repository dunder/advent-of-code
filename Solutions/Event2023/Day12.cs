using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 12: Hot Springs ---
    public class Day12
    {
        private readonly ITestOutputHelper output;

        public Day12(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<(string springs, List<int> conditions)> Parse(IList<string> input, int unfold)
        {
            List<(string springs, List<int> conditions)> result = input.Select(line =>
            {
                var parts = line.Split(" ");
                var springs = string.Join("?", Enumerable.Repeat(parts[0], unfold));
                var conditions = string.Join(",", Enumerable.Repeat(parts[1], unfold)).Split(",").Select(int.Parse).ToList();
                return (springs, conditions);
            }).ToList();

            return result;
        }
        private static string ReplaceSubstring(string target, int index, string replacement)
        {
            var stringBuilder = new StringBuilder(target);

            if (index + replacement.Length > stringBuilder.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < replacement.Length; ++i)
            {
                stringBuilder[index + i] = replacement[i];
            }

            return stringBuilder.ToString();
        }

        private int CountArrangements((string, List<int>) row)
        {
            (string springs, List<int> sizes) = row;

            var totalSize = sizes.Sum();
            var arrangements = new List<(string arrangement, int left)> { (springs, 0) };
            var availableSpace = springs.Length - totalSize;

            for (int s = 0; s < sizes.Count; s++)
            {
                var size = sizes[s];

                List<(string, int)> newArrangements = new();

                foreach ((string arrangement, int left) a in arrangements) 
                {
                    for (int x = a.left; x <= a.left + availableSpace; x++)
                    {
                        if (IsValid(a.arrangement, x, size))
                        {
                            newArrangements.Add((ReplaceSubstring(a.arrangement, x, new string('#', size)), x + size + 1));
                        }
                    }
                }

                arrangements = newArrangements;
            }

            return arrangements.Where(a => IsValidCount(a.Item1, totalSize)).Count();
        }

        private bool IsValidCount(string arrangment, int totalSize)
        {
            return arrangment.Count(c => c == '#') == totalSize;
        }

        private bool IsValid(string spring, int left, int size)
        {
            if (left + size > spring.Length)
            {
                return false;
            }

            string positioned = spring.Substring(left, size);

            if (positioned.Contains(".")) { return false; }

            int after = left + size;

            if (after < spring.Length && spring[after] == '#') { return false; }

            int before = left - 1;

            if (before > 0 && spring[before] == '#') { return false; }

            return true;
        }

        private int SumOfCounts(IList<string> input, int unfold = 1)
        {
            return Parse(input, unfold).Select(CountArrangements).Sum();
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return SumOfCounts(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(8419, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "???.### 1,1,3",
                ".??..??...?##. 1,1,3",
                "?#?#?#?#?#?#?#? 1,3,1,6",
                "????.#...#... 4,1,1",
                "????.######..#####. 1,6,5",
                "?###???????? 3,2,1",
            };

            Assert.Equal(21, SumOfCounts(example));
        }

        [Fact]
        public void CountArrangementsTests()
        {
            Assert.Equal(1, CountArrangements(("???.###", new List<int> { 1, 1, 3 })));
            Assert.Equal(4, CountArrangements((".??..??...?##.", new List<int> { 1, 1, 3 })));
            Assert.Equal(1, CountArrangements(("?#?#?#?#?#?#?#?", new List<int> { 1, 3, 1, 6 })));
            Assert.Equal(1, CountArrangements(("????.#...#...", new List<int> { 4, 1, 1 })));
            Assert.Equal(4, CountArrangements(("????.######..#####.", new List<int> { 1, 6, 5 })));
            Assert.Equal(10, CountArrangements(("?###????????", new List<int> { 3, 2, 1 })));
            Assert.Equal(4, CountArrangements(("?#??????##?", new List<int> { 1, 1, 2 })));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "???.### 1,1,3",
                ".??..??...?##. 1,1,3",
                "?#?#?#?#?#?#?#? 1,3,1,6",
                "????.#...#... 4,1,1",
                "????.######..#####. 1,6,5",
                "?###???????? 3,2,1",
            };

            Assert.Equal(525152, SumOfCounts(example, 5));
        }
    }
}
