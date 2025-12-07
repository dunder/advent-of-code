using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2022
{
    // --- Day 5: Supply Stacks ---
    public class Day05
    {
        private readonly ITestOutputHelper output;

        public Day05(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static Dictionary<int, Stack<char>> CreateStacks(List<string> stackInput)
        {
            int stackCount = stackInput
                .Last()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .Max();

            Dictionary<int, Stack<char>> stacks = Enumerable.Range(1, stackCount)
                .Select(i => (i, new Stack<char>()))
                .ToDictionary();

            foreach (var column in Enumerable.Range(1, stackCount))
            {
                int stackIndex = stackInput.Last().IndexOf(column.ToString());

                for (int i = stackInput.Count - 2; i >= 0; i--)
                {
                    var crates = stackInput[i];
                    var crate = crates[stackIndex];

                    if (char.IsLetter(crate))
                    {
                        stacks[column].Push(crate);
                    }
                }
            }

            return stacks;
        }

        private static string Problem1(IList<string> input)
        {
            List<string> stackInput = input.Split("").First().ToList();
            List<string> moves = input.Split("").Last().ToList();

            Dictionary<int, Stack<char>> stacks = CreateStacks(stackInput);

            foreach (var move in moves)
            {
                var values = move.Split(" ").Where(w => int.TryParse(w, out _)).Select(int.Parse).ToList();
                var times = values[0];
                var from = values[1];
                var to = values[2];

                foreach (var _ in Enumerable.Range(0, times))
                {
                    stacks[to].Push(stacks[from].Pop());
                }
            }

            return string.Join("", stacks.Keys.Select(i => stacks[i].Pop()));
        }

        private static string Problem2(IList<string> input)
        {
            List<string> stackInput = input.Split("").First().ToList();
            List<string> moves = input.Split("").Last().ToList();

            Dictionary<int, List<char>> stacks = CreateStacks(stackInput)
                .Select(kvp => (kvp.Key, new List<char>(kvp.Value)))
                .ToDictionary();

            foreach (var move in moves)
            {
                var values = move.Split(" ").Where(w => int.TryParse(w, out _)).Select(int.Parse).ToList();
                var times = values[0];
                var from = values[1];
                var to = values[2];

                var moved = stacks[from].Take(times).ToList();
                stacks[from] = stacks[from].Skip(times).ToList();
                moved.AddRange(stacks[to]);
                stacks[to] = moved;
            }

            return string.Join("", stacks.Keys.Select(i => stacks[i][0]));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal("QNNTGTPFN", Problem1(input));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal("GGNPJBTTR", Problem2(input));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal("CMZ", Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal("MCD", Problem2(exampleInput));
        }
    }
}