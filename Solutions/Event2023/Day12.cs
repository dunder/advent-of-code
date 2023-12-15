using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 12: Hot Springs ---
    public class Day12
    {
        private readonly ITestOutputHelper output;

        private Dictionary<string, long> countArrangementsCache = new();

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

        private bool TryInsert(string spring, int left, int size, int totalSize)
        {
            if (left + size > spring.Length)
            {
                return false;
            }

            string positioned = spring.Substring(left, size);

            if (positioned.Contains(".")) { return false; }

            int after = left + size;

            if (after < spring.Length)
            {
                var afterSpring = spring[after..];
                if (afterSpring[0] == '#' || size + afterSpring.Count(c => c == '#') > totalSize)
                {
                    return false;
                }
            }

            string before = spring.Substring(0, left);

            if (before.Contains("#")) { return false; }

            return true;
        }

        private long SumOfCounts(IList<string> input, int unfold = 1)
        {
            return Parse(input, unfold).Select(CountArrangements).Sum();
        }

        private long CountArrangements((string, List<int>) row)
        {
            (string springs, List<int> sizes) = row;

            var key = springs + string.Join(",", sizes);

            if (countArrangementsCache.ContainsKey(key))
            {
                return countArrangementsCache[key];
            }

            var totalSize = sizes.Sum();
            var availableSpace = springs.Length - totalSize;

            var size = sizes.First();

            long total = 0;

            for (int i = 0; i <= availableSpace; i++)
            {
                if (TryInsert(springs, i, size, totalSize))
                {
                    if (sizes.Count == 1)
                    {
                        total += 1;
                        continue;
                    }
                    var from = i + size + 1;
                    
                    var replaced = springs.Substring(from);

                    total += CountArrangements((replaced, sizes.Skip(1).ToList()));
                }
            }

            countArrangementsCache.Add(key, total);

            return total;
        }

        public long FirstStar()
        {
            var input = ReadLineInput();
            return SumOfCounts(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();
            return SumOfCounts(input, 5);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(8419, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(160500973317706, SecondStar());
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
        public void CountArrangementsRecurseTests()
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
