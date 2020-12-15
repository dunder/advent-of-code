using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;


namespace Solutions.Event2020
{
    // --- Day 15: Rambunctious Recitation ---
    public class Day15
    {
        private readonly ITestOutputHelper output;

        private static readonly List<int> Numbers = new List<int> { 16, 11, 15, 0, 1, 7 };

        public Day15(ITestOutputHelper output)
        {
            this.output = output;
        }

        private class SpokenWhen
        {
            public int LastSpokenRound { get; private set;  }
            public int? PreviouslySpokenRound { get; set; }

            public SpokenWhen(int lastSpoken)
            {
                LastSpokenRound = lastSpoken;
            }

            public void AddLastSpoken(int lastSpokenRound)
            {
                PreviouslySpokenRound = LastSpokenRound;
                LastSpokenRound = lastSpokenRound;
            }

            public int WhenSpoken()
            {
                if (PreviouslySpokenRound.HasValue)
                {
                    return PreviouslySpokenRound.Value;
                }
                return LastSpokenRound;
            }

            public override string ToString()
            {
                var previously = PreviouslySpokenRound.HasValue ? PreviouslySpokenRound.Value.ToString() : "N/A";
                return $"Last = '{LastSpokenRound}' Previosly: '{previously}'";
            }
        }

        private class SpokenHistory
        {
            private int? lastSpoken = null;
            private int rounds = 0;
            private readonly Dictionary<int, SpokenWhen> spokenWhen = new Dictionary<int, SpokenWhen>();

            public int LastSpoken()
            {
                if (!lastSpoken.HasValue)
                {
                    throw new InvalidOperationException("No words spoken yet!");
                }

                return lastSpoken.Value;
            }

            public void AddSpoken(int spoken)
            {
                lastSpoken = spoken;
                var round = rounds;
                if (spokenWhen.ContainsKey(spoken))
                {
                    spokenWhen[spoken].AddLastSpoken(round);
                }
                else
                {
                    spokenWhen.Add(spoken, new SpokenWhen(round));
                }
                rounds += 1;
            }

            public bool IsSpokenBefore(int spoken)
            {
                return spokenWhen.ContainsKey(spoken);
            }

            public int PreviouslySpokenRound(int lastSpoken)
            {
                return spokenWhen[lastSpoken].WhenSpoken();
            }
        }

        private int Run(List<int> numbers, int to)
        {
            var spokenHistory = new SpokenHistory();

            foreach (var turn in Enumerable.Range(0, numbers.Count))
            {
                var spokenNumber = numbers[turn];
                spokenHistory.AddSpoken(spokenNumber);
            }

            foreach (var turn in Enumerable.Range(numbers.Count, to - numbers.Count))
            {
                var lastSpoken = spokenHistory.LastSpoken();

                if (!spokenHistory.IsSpokenBefore(lastSpoken))
                {
                    spokenHistory.AddSpoken(0);
                }
                else
                {
                    var nextSpoken = turn - 1 - spokenHistory.PreviouslySpokenRound(lastSpoken);
                    spokenHistory.AddSpoken(nextSpoken);
                }
            }

            return spokenHistory.LastSpoken();
        }

        public int FirstStar()
        {
            return Run(Numbers, 2020);
        }

        public int SecondStar()
        {
            return Run(Numbers, 30000000);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(662, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(37312, SecondStar());
        }

        [Theory]
        [InlineData(0,3,6,436)]
        [InlineData(1,3,2,1)]
        [InlineData(2, 1, 3, 10)]
        [InlineData(1, 2, 3, 27)]
        [InlineData(2, 3, 1, 78)]
        [InlineData(3, 2, 1, 438)]
        [InlineData(3, 1, 2, 1836)]
        public void Examples(int v1, int v2, int v3, int expected)
        {
            var lastSpoken = Run(new List<int> { v1, v2, v3 }, 2020);

            Assert.Equal(expected, lastSpoken);
        }
    }
}
