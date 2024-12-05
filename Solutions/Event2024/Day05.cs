using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.Event2018.Day13;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 5: Phrase ---
    public class Day05
    {
        private readonly ITestOutputHelper output;

        public Day05(ITestOutputHelper output)
        {
            this.output = output;
        }

        public record Ordering(int First, int Second);

        public (List<Ordering> orderings, List<List<int>> updates) Parse(IList<string> input)
        {
            List<Ordering> orderings = new List<Ordering>();
            List<List<int>> updates = new List<List<int>>();

            foreach (var line in input.Where(line => !string.IsNullOrEmpty(line)))
            {
                if (line.Contains("|"))
                {
                    var parts = line.Split('|');
                    var first = int.Parse(parts[0]);
                    var second = int.Parse(parts[1]);

                    orderings.Add(new Ordering(first, second));
                }
                else
                {
                    updates.Add(line.SplitOnCommaSpaceSeparated().Select(int.Parse).ToList());
                }
            }

            return (orderings, updates);
        }

        private List<List<int>> Sorted(List<Ordering> orderings, List<List<int>> updates)
        {
            var sorted = new List<List<int>>();

            foreach (var update in updates)
            {
                bool isSorted = true;
                foreach (var ordering in orderings)
                {
                    int f = update.IndexOf(ordering.First);
                    int s = update.IndexOf(ordering.Second);

                    if (f < 0 || s  < 0)
                    {
                        continue;
                    }

                    isSorted = f < s;

                    if (!isSorted)
                    {
                        break;
                    }
                }
                if (isSorted)
                {
                    sorted.Add(update);
                }
            }

            return sorted;
        }

        private List<List<int>> Sort(List<Ordering> orderings, List<List<int>> updates)
        {
            var sorted = new List<List<int>>();

            foreach (var update in updates)
            {
                update.Sort(delegate (int x, int y)
                {
                    foreach (var ordering in orderings)
                    {
                        if (ordering.First == x && ordering.Second == y || ordering.First == y && ordering.Second == x)
                        {
                            // 6|7 - 1
                            // 6,7 - 1

                            var diff = x - y;
                            var xdiff = ordering.First - ordering.Second;

                            return xdiff * diff;
                        }
                    }

                    return 0;
                });
            }
            return updates;
        }

        private int Middle(List<int> updates)
        {
            return updates[updates.Count / 2];
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            int Execute()
            {
                var x = Parse(input);

                var result = Sorted(x.orderings, x.updates);
                var sum = result.Select(r => Middle(r)).Sum();
                return sum;
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            int Execute()
            {
                var x = Parse(input);

                var result = Sorted(x.orderings, x.updates);

                var unsorted = x.updates.Where(x => !result.Contains(x)).ToList();

                var sorted = Sort(x.orderings, unsorted);
                var sum = sorted.Select(r => Middle(r)).Sum();
                return sum;
            }

            Assert.Equal(-1, Execute());
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "47|53",
                "97|13",
                "97|61",
                "97|47",
                "75|29",
                "61|13",
                "75|53",
                "29|13",
                "97|29",
                "53|29",
                "61|53",
                "97|53",
                "61|29",
                "47|13",
                "75|47",
                "97|75",
                "47|61",
                "75|61",
                "47|29",
                "75|13",
                "53|13",
                "",
                "75,47,61,53,29",
                "97,61,53,29,13",
                "75,29,13",
                "75,97,47,61,53",
                "61,13,29",
                "97,13,75,29,47",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }
    }
}
