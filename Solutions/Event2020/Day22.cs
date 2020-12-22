using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 22: Crab Combat ---

    public class Day22
    {
        private readonly ITestOutputHelper output;

        public Day22(ITestOutputHelper output)
        {
            this.output = output;
        }

        private (List<int>, List<int>) Parse(List<string> lines)
        {
            List<int> player1 = new List<int>();
            List<int> player2 = new List<int>();

            var currentPlayer = player1;

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.Contains("Player 2"))
                {
                    currentPlayer = player2;
                    continue;
                }

                currentPlayer.Add(int.Parse(line));
            }

            return (player1, player2);
        }

        private int Play(List<int> player1, List<int> player2)
        {
            var players = new List<List<int>> { player1, player2 };

            while (player1.Any() && player2.Any())
            {
                var p1Card = player1[0];
                var p2Card = player2[0];

                player1.RemoveAt(0);
                player2.RemoveAt(0);

                if (p1Card > p2Card)
                {
                    player1.Add(p1Card);
                    player1.Add(p2Card);
                }
                else
                {
                    player2.Add(p2Card);
                    player2.Add(p1Card);
                }
            }

            var winner = players.Single(player => player.Any());

            winner.Reverse();

            return winner.Select((card, i) => card*(i+1)).Sum();
        }

        private int PlayRecursive(List<int> player1, List<int> player2)
        {
            var players = new List<List<int>> { player1, player2 };

            while (player1.Any() && player2.Any())
            {
                var p1Card = player1[0];
                var p2Card = player2[0];

                player1.RemoveAt(0);
                player2.RemoveAt(0);

                if (player1.Count >= p1Card && player2.Count >= p2Card)
                {
                    var (sub1, sub2) = PlaySubGame(new List<int>(player1.Take(p1Card)), new List<int>(player2.Take(p2Card)));

                    if (sub1.Any() && sub2.Any())
                    {
                        player1.Add(p1Card);
                        player1.Add(p2Card);
                    }
                    else if (sub1.Any())
                    {
                        player1.Add(p1Card);
                        player1.Add(p2Card);
                    }
                    else
                    {
                        player2.Add(p2Card);
                        player2.Add(p1Card);
                    }
                    continue;
                }

                if (p1Card > p2Card)
                {
                    player1.Add(p1Card);
                    player1.Add(p2Card);
                }
                else
                {
                    player2.Add(p2Card);
                    player2.Add(p1Card);
                }
            }

            var winner = players.Single(player => player.Any());

            winner.Reverse();

            return winner.Select((card, i) => card * (i + 1)).Sum();
        }

        private (List<int>, List<int>) PlaySubGame(List<int> player1, List<int> player2)
        {
            var previousRounds = new List<List<List<int>>> 
                {
                    new List<List<int>> { new List<int>(player1), new List<int>(player2) }
                };

            while (player1.Any() && player2.Any())
            {
                var p1Card = player1[0];
                var p2Card = player2[0];

                player1.RemoveAt(0);
                player2.RemoveAt(0);

                if (player1.Count >= p1Card && player2.Count >= p2Card)
                {
                    var (sub1, sub2) = PlaySubGame(new List<int>(player1.Take(p1Card)), new List<int>(player2.Take(p2Card)));

                    if (sub1.Any() && sub2.Any())
                    {
                        player1.Add(p1Card);
                        player1.Add(p2Card);
                    }
                    else if (sub1.Any()) 
                    {
                        player1.Add(p1Card);
                        player1.Add(p2Card);
                    }
                    else
                    {
                        player2.Add(p2Card);
                        player2.Add(p1Card);
                    }

                    continue;
                }

                if (p1Card > p2Card)
                {
                    player1.Add(p1Card);
                    player1.Add(p2Card);
                }
                else
                {
                    player2.Add(p2Card);
                    player2.Add(p1Card);
                }

                var loop = previousRounds.Any(round => round[0].SequenceEqual(player1) && round[1].SequenceEqual(player2));

                if (loop)
                {
                    return (player1, player2);
                }

                previousRounds.Add(new List<List<int>> { new List<int>(player1), new List<int>(player2) });
            }

            return (player1, player2);
        }

        public int FirstStar()
        {
            var input = ReadLineInput().ToList();
            var (player1, player2) = Parse(input);

            var result = Play(player1, player2);
            return result;
        }

        public int SecondStar()
        {
            var input = ReadLineInput().ToList();
            var (player1, player2) = Parse(input);

            var result = PlayRecursive(player1, player2);
            return result;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(31629, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();
            Assert.Equal(35196, result);
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "Player 1:",
                "9",
                "2",
                "6",
                "3",
                "1",
                "",
                "Player 2:",
                "5",
                "8",
                "4",
                "7",
                "10"
            };

            var (player1, player2) = Parse(input);

            var result = Play(player1, player2);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void SecondStarLoopExample()
        {
            var input = new List<string>
            {
                "Player 1:",
                "43",
                "19",
                "",
                "Player 2:",
                "2",
                "29",
                "14",
            };

            var (player1, player2) = Parse(input);

            var result = PlaySubGame(player1, player2);

            // dummy assert, this was just to test that the code does not get stuck in an infinite loop
            Assert.NotNull(result.Item1);
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string>
            {
                "Player 1:",
                "9",
                "2",
                "6",
                "3",
                "1",
                "",
                "Player 2:",
                "5",
                "8",
                "4",
                "7",
                "10"
            };

            var (player1, player2) = Parse(input);

            var result = PlayRecursive(player1, player2);

            Assert.Equal(291, result);
        }
    }
}
