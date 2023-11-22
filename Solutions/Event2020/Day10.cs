using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 10: Adapter Array ---
    public class Day10
    {
        private readonly ITestOutputHelper output;

        public Day10(ITestOutputHelper output)
        {
            this.output = output;
        }

        private IList<int> ParseAdapters(IList<string> input) 
        {
            return input.Select(int.Parse).ToList();
        }

        private int Solve1(IList<string> input)
        {
            var adapters = ParseAdapters(input);

            var sorted = adapters.OrderBy(x => x).ToList();

            int previous = 0;
            var diffs = new List<int> { 3 };

            for (int i = 0; i < sorted.Count; i++)
            {
                var adapter = sorted[i];
                diffs.Add(adapter - previous);
                previous = adapter;
            }

            var grouped = diffs
                .GroupBy(x => x)
                .Select(group => new { 
                    Diff = group.Key,
                    Count = group.Count()
                })
                .ToDictionary(x => x.Diff, x => x.Count);

            return grouped[1] * grouped[3];
        }

        
        private long Solve2(IList<string> input)
        {
            var adapters = ParseAdapters(input);

            adapters.Add(0);

            var sorted = adapters.OrderBy(x => x).ToList();

            var connectable = new Dictionary<int, List<int>>();

            int outlet = 0;
            
            // deal with last separately
            for (int i = 0; i < sorted.Count-1; i++)
            {
                var adapter = sorted[i];

                var compatible = sorted.Skip(i + 1).TakeWhile(a => a <= adapter + 3).ToList();

                connectable.Add(adapter, compatible);

                outlet = adapter;
            }

            var output = sorted.Last() + 3;

            // diff with last adapter to built in adapter is always 3
            connectable.Add(sorted.Last(), new List<int> { output });

            var counters = new Dictionary<int, long> { { output, 1 } };

            for (int i = sorted.Count-1; i >= 0; i--)
            {
                var adapter = sorted[i];

                var compatible = connectable[adapter];

                counters.Add(adapter, compatible.Sum(a => counters[a]));
            }

            return counters[0];
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return Solve1(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();

            return Solve2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(2470, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1973822685184, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "16",
                "10",
                "15",
                "5",
                "1",
                "11",
                "7",
                "19",
                "6",
                "12",
                "4"
            };

            Assert.Equal(35, Solve1(example));
        }

        [Fact]
        public void FirstStarExample2()
        {
            var example = new List<string>
            {
                "28",
                "33",
                "18",
                "42",
                "31",
                "14",
                "46",
                "20",
                "48",
                "47",
                "24",
                "23",
                "49",
                "45",
                "19",
                "38",
                "39",
                "11",
                "1",
                "32",
                "25",
                "35",
                "8",
                "17",
                "7",
                "9",
                "4",
                "2",
                "34",
                "10",
                "3"
            };

            Assert.Equal(220, Solve1(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "16",
                "10",
                "15",
                "5",
                "1",
                "11",
                "7",
                "19",
                "6",
                "12",
                "4"
            };

            Assert.Equal(8, Solve2(example));
        }

        [Fact]
        public void SecondStarExample2()
        {
            var example = new List<string>
            {
                "28",
                "33",
                "18",
                "42",
                "31",
                "14",
                "46",
                "20",
                "48",
                "47",
                "24",
                "23",
                "49",
                "45",
                "19",
                "38",
                "39",
                "11",
                "1",
                "32",
                "25",
                "35",
                "8",
                "17",
                "7",
                "9",
                "4",
                "2",
                "34",
                "10",
                "3"
            };

            Assert.Equal(19208, Solve2(example));
        }
    }
}
