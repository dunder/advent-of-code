using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 5: Print Queue ---
    public class Day05
    {
        private readonly ITestOutputHelper output;

        public Day05(ITestOutputHelper output)
        {
            this.output = output;
        }

        public (List<(int, int)> rules, List<List<int>> updates) Parse(IList<string> input)
        {
            List<(int, int)> rules = new List<(int, int)>();
            List<List<int>> updates = new List<List<int>>();

            foreach (var line in input.Where(line => !string.IsNullOrEmpty(line)))
            {
                if (line.Contains("|"))
                {
                    var parts = line.Split('|');
                    var first = int.Parse(parts[0]);
                    var second = int.Parse(parts[1]);

                    rules.Add((first, second));
                }
                else
                {
                    updates.Add(line.SplitOnCommaSpaceSeparated().Select(int.Parse).ToList());
                }
            }

            return (rules, updates);
        }

        private List<List<int>> Sorted(List<(int first, int second)> rules, List<List<int>> updates)
        {
            return updates.Where(update => rules.All(rule =>
                {
                    int f = update.IndexOf(rule.first);
                    int s = update.IndexOf(rule.second);

                return f < 0 || s < 0 || f < s;
                })).ToList();
        }

        private List<List<int>> Unsorted(List<(int first, int second)> rules, List<List<int>> updates)
        {
            return updates.Where(update => rules.Any(rule =>
            {
                int f = update.IndexOf(rule.first);
                int s = update.IndexOf(rule.second);

                return f >= 0 && s >= 0 && f > s;
            })).ToList();
        }

        private List<List<int>> Sort(List<(int, int)> rules, List<List<int>> updates)
        {
            foreach (var update in updates)
            {
                update.Sort(delegate (int x, int y)
                {
                    foreach (var rule in rules)
                    {
                        if ((x, y) ==  rule || (y, x) == rule)
                        {
                            var (first, second) = rule;
                            return (first - second) * (x - y);
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

        private int Problem1(IList<string> input)
        {
            var printerData = Parse(input);

            return Sorted(printerData.rules, printerData.updates).Select(Middle).Sum();
        }

        private int Problem2(IList<string> input)
        {
            var printerData = Parse(input);

            var unsortedUpdates = Unsorted(printerData.rules, printerData.updates);
            var unsortedUpdatesSorted = Sort(printerData.rules, unsortedUpdates);

            return unsortedUpdatesSorted.Select(Middle).Sum();
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(6034, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(6305, Problem2(input));
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
            Assert.Equal(143, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(123, Problem2(exampleInput));
        }
    }
}
