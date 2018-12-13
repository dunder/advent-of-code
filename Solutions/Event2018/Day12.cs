using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day12 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day12;
        public string Name => "Subterranean Sustainability";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = SumPotsWithPlants(input, 20);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = SumPotsWithPlants(input, 50_000_000_000);
            return result.ToString();
        }

        public static (string initialState, Dictionary<string, bool> rules) ParseInput(IList<string> input)
        {
            string initialState = input.First().Substring(15);
            var ruleNotes = input.Skip(2).ToList();

            var rules = new Dictionary<string, bool>();
            foreach (var ruleNote in ruleNotes)
            {
                var pattern = ruleNote.Substring(0, 5);
                var result = ruleNote.Substring(9, 1);

                if (result == "#")
                {
                    rules.Add(pattern, result == "#");
                }
            }

            return (initialState, rules);
        }

        public static (string state, int indexOfCenter) Extend(string state, int indexOfCenter)
        {
            var firstPlant = state.IndexOf("#");
            var prefix = "";
            var postfix = "";

            if (firstPlant < 4)
            {
                var fillCount = 4 - firstPlant;
                indexOfCenter += fillCount;
                prefix = string.Join("", Enumerable.Repeat(".", fillCount));
            }

            var lastPlant = state.LastIndexOf("#");
            if (lastPlant > state.Length - 6)
            {
                var fillCount = 4 - (state.Length - 1 - lastPlant);
                postfix = string.Join("", Enumerable.Repeat(".", fillCount));
            }

            return ($"{prefix}{state}{postfix}", indexOfCenter);
        }

        public static long SumPotsWithPlants(IList<string> input, long generations)
        {
            var (initialState, rules) = ParseInput(input);
            var state = $"{initialState}";
            var sum = 0L;
            var diff = 0L;
            var stabilityCounter = 0;
            var indexOfCenter = 0;
            (state, indexOfCenter) = Extend(state, indexOfCenter);

            for (int g = 1; g <= generations; g++)
            {
                var newGeneration = new StringBuilder(state);

                for (int i = 2; i < state.Length - 2; i++)
                {
                    var pattern = state.Substring(i - 2, 5);
                    newGeneration[i] = rules.ContainsKey(pattern) ? '#' : '.';
                }

                (state, indexOfCenter) = Extend(newGeneration.ToString(), indexOfCenter);

                var newSum = SumPotValues(state, indexOfCenter);
                var newDiff = newSum - sum;

                stabilityCounter = diff == newDiff ? stabilityCounter + 1 : 0;

                diff = newDiff;
                sum = newSum;

                if (stabilityCounter > 5)
                {
                    return (generations - g)*diff + sum;
                }
            }

            return sum;
        }

        private static long SumPotValues(string state, int indexOfCenter)
        {
            long sum = 0;

            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] == '#')
                {
                    if (i < indexOfCenter)
                    {
                        sum -= indexOfCenter - i;
                    }
                    else
                    {
                        sum += i - indexOfCenter;
                    }
                }
            }

            return sum;
        }

        [Theory]
        [InlineData("#....", "....#....", 4)]
        [InlineData(".##....", "....##....", 3)]
        [InlineData("..##..", "....##....", 2)]
        [InlineData("...##..", "....##....", 1)]
        [InlineData("....##....", "....##....", 0)]
        [InlineData("..##", "....##....", 2)]
        [InlineData("...##.", "....##....", 1)]
        [InlineData("....##..", "....##....", 0)]
        [InlineData(".....##...", ".....##....", 0)]
        [InlineData(".....##....", ".....##....", 0)]
        [InlineData("......##.....", "......##.....", 0)]
        public void FillTests(string input, string expectetState, int expectedCenter)
        {
            var(state, indexOfCenter) = Extend(input, 0);

            Assert.Equal(expectetState, state);
            Assert.Equal(expectedCenter, indexOfCenter);
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "initial state: #..#.#..##......###...###",
                "",
                "...## => #",
                "..#.. => #",
                ".#... => #",
                ".#.#. => #",
                ".#.## => #",
                ".##.. => #",
                ".#### => #",
                "#.#.# => #",
                "#.### => #",
                "##.#. => #",
                "##.## => #",
                "###.. => #",
                "###.# => #",
                "####. => #",
            };

            var result = SumPotsWithPlants(input, 20);

            Assert.Equal(325, result);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("2040", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("1700000000011", actual);
        }
    }
}