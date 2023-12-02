using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day X: Phrase ---
    public class Day02
    {
        private readonly ITestOutputHelper output;

        public Day02(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record SetInfo(int red, int green, int blue);
        private record GameRecord(int id, List<SetInfo> sets);

        private List<GameRecord> ParseGames(IList<string> input)
        {
            var colorRegexes = new[] { "red", "green", "blue"}.Select(color => new Regex(@$"(\d+) {color}"));

            return input.Select(line =>
            {
                var idSplit = line.Split(": ");
                var id = int.Parse(idSplit[0][4..]);
                var setSplit = idSplit[1].Split("; ");
                var sets = setSplit.Select(set => {

                    var counts = colorRegexes.Select(regex =>
                    {
                        var match = regex.Match(set);
                        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
                    }).ToArray();
                

                    return new SetInfo(counts[0], counts[1], counts[2]);
                }).ToList();

                return new GameRecord(id, sets);
            }).ToList();
        }

        private int CountPossible(IList<string> input, int maxRed, int maxGreen, int maxBlue)
        {
            var games = ParseGames(input);

            var possible = games.Where(game => game.sets.All(set => set.red <= maxRed && set.green <= maxGreen && set.blue <= maxBlue));

            return possible.Sum(game => game.id);
        }

        private int Power(IList<string> input)
        {
            var games = ParseGames(input);

            var powers = games.Select(game => {

                var minRed = game.sets.Max(set => set.red);
                var minGreen = game.sets.Max(set => set.green);
                var minBlue = game.sets.Max(set => set.blue);

                return minRed * minGreen * minBlue;
            });

            return powers.Sum();
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return CountPossible(input, 12, 13, 14);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return Power(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(1853, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(72706, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
            };

            Assert.Equal(8, CountPossible(example, 12, 13   , 14));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
            };

            Assert.Equal(2286, Power(example));
        }
    }
}
