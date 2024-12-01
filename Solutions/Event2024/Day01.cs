using MoreLinq;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 1: Historian Hysteria ---
    public class Day01
    {
        private readonly ITestOutputHelper output;

        public Day01(ITestOutputHelper output)
        {
            this.output = output;
        }

        public static List<(int Left, int Right)> ParseLocationIds(IList<string> input)
        {
            return input
                .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(s => (int.Parse(s[0]), int.Parse(s[1])))
                .ToList();
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            int Execute()
            {
                var locationIds = ParseLocationIds(input);

                var (leftList, rightList) = (new List<int>(), new List<int>());

                var (left, right) = locationIds.Aggregate((leftList, rightList), (state, idPair) =>
                {
                    state.leftList.Add(idPair.Left);
                    state.rightList.Add(idPair.Right);

                    return state;
                });

                left.Sort();
                right.Sort();

                return left.Zip(right, (l, r) => Math.Abs(l - r)).Sum();
            }

            Assert.Equal(1660292, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            int Execute()
            {
                var locationIds = ParseLocationIds(input);

                var initialCounts = new Dictionary<int, int>();

                var counts = locationIds.Aggregate(initialCounts, (currentCounts, idPair) =>
                {
                    if (currentCounts.TryGetValue(idPair.Right, out int count))
                    {
                        currentCounts[idPair.Right] = count + 1;
                    }
                    else
                    {
                        currentCounts.Add(idPair.Right, 1);
                    }

                    return currentCounts;
                });

                return locationIds
                    .Select(idPair => counts.TryGetValue(idPair.Left, out int count) ? idPair.Left * count : 0)
                    .Sum();
            }

            Assert.Equal(22776016, Execute());
        }
    }
}
