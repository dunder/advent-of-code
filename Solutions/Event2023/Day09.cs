using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day X: Phrase ---
    public class Day09
    {
        private readonly ITestOutputHelper output;

        public Day09(ITestOutputHelper output)
        {
            this.output = output;
        }

        private delegate int Extrapolation(int acc, List<int> history);

        private int FindNextValue(IList<string> input, Extrapolation extrapolation)
        {
            var values = input.Select(line => line.Split(" ").Select(int.Parse).ToList());

            return values.Select(history => FindNextValue(history, extrapolation)).Sum();
        }
        private int FindNextValue(List<int> history, Extrapolation extrapolation)
        {
            var histories = new Stack<List<int>>();
            histories.Push(history);

            List<int> Loop(List<int> h)
            {
                if (h.All(x => x == 0)) return h;
                var next = h.Pairwise((a, b) => b - a).ToList();
                histories.Push(next);
                return Loop(next);
            }

            Loop(history);

            return histories.Aggregate(0, (a, h) => extrapolation(a, h));
        }

        private int NextValueExtrapolation(int acc, List<int> history)
        {
            return history[^1] + acc;
        }

        private int PreviousValueExtrapolation(int acc, List<int> history)
        {
            return history[0] - acc;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return FindNextValue(input, NextValueExtrapolation);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return FindNextValue(input, PreviousValueExtrapolation);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(2043183816, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1118, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "0 3 6 9 12 15",
                "1 3 6 10 15 21",
                "10 13 16 21 30 45",
            };

            Assert.Equal(114, FindNextValue(example, NextValueExtrapolation));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "0 3 6 9 12 15",
                "1 3 6 10 15 21",
                "10 13 16 21 30 45",
            };

            Assert.Equal(2, FindNextValue(example, PreviousValueExtrapolation));
        }
    }
}
