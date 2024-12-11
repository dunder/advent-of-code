using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Shared.Tree;
using static Solutions.InputReader;
using Shared.Extensions;


namespace Solutions.Event2024
{
    // --- Day 10: Hoof It ---
    public class Day10
    {
        private readonly ITestOutputHelper output;

        public Day10(ITestOutputHelper output)
        {
            this.output = output;
        }

        public static (int[,], List<(int row, int column)>) Parse(IList<string> input)
        {
            var trailHeads = new List<(int row, int column)>();
            var map = new int[input.Count, input.First().Length];

            for (var row = 0; row < input.Count; row++)
            {
                var line = input[row];
                for (var column = 0; column < input.First().Length; column++)
                {
                    if (line[column] == '.')
                    {
                        map[row, column] = -1;
                    }
                    else
                    {
                        map[row, column] = int.Parse(line[column].ToString());
                    }

                    if (line[column] == '0')
                    {
                        trailHeads.Add((row, column));
                    }
                }
            }
            return (map, trailHeads);
        }

        private static bool WithinBounds((int row, int column) location, (int rowBound, int columnBound) bounds)
        {
            return location.row >= 0 && location.row < bounds.rowBound && location.column >= 0 && location.column < bounds.columnBound;
        }

        private static List<(int row, int column)> Neighbors((int row, int column) location, int[,] map, (int rowBound, int columnBound) bounds)
        {
            var up = (location.row - 1, location.column);
            var right = (location.row, location.column + 1);
            var down = (location.row + 1, location.column);
            var left = (location.row, location.column - 1);

            List<(int row, int column)> potentialNeighbors = [up, right, down, left];

            var inBounds = potentialNeighbors.Where(n => WithinBounds(n, bounds));
            var ascending = inBounds.Where(n => map[location.row, location.column] + 1 == map[n.row, n.column]);

            return ascending.ToList();
        }

        private static void FindTrail((int row, int column) trailHead, int[,] map, (int rowBound, int columnBound) bounds, HashSet<(int row, int column)> peaks)
        {
            if (map[trailHead.row, trailHead.column] == 9)
            {
                peaks.Add(trailHead);
            }

            var neighbors = Neighbors(trailHead, map, bounds);

            foreach (var neighbor in neighbors)
            {
                var now = map[trailHead.row, trailHead.column];
                var next = map[neighbor.row, neighbor.column];

                FindTrail(neighbor, map, bounds, peaks);
            }
        }

        private static void FindTrail2((int row, int column) trailHead, int[,] map, (int rowBound, int columnBound) bounds, List<(int row, int column)> peaks)
        {
            if (map[trailHead.row, trailHead.column] == 9)
            {
                peaks.Add(trailHead);
            }

            var neighbors = Neighbors(trailHead, map, bounds);

            foreach (var neighbor in neighbors)
            {
                var now = map[trailHead.row, trailHead.column];
                var next = map[neighbor.row, neighbor.column];

                FindTrail2(neighbor, map, bounds, peaks);
            }
        }

        private static int Problem1(IList<string> input)
        {
            var (map, trailHeads) = Parse(input);
            (int rows, int columns) bounds = (input.Count, input.First().Length);

            List<(int, int)> selected = [trailHeads[0]];

            int score = 0;

            foreach (var trailHead in trailHeads)
            {
                var peaks = new HashSet<(int, int)>();
                FindTrail(trailHead, map, bounds, peaks);
                score += peaks.Count;
            }
            
            return score;
        }

        private static int Problem2(IList<string> input)
        {
            var (map, trailHeads) = Parse(input);
            (int rows, int columns) bounds = (input.Count, input.First().Length);

            List<(int, int)> selected = [trailHeads[0]];

            int score = 0;

            foreach (var trailHead in trailHeads)
            {
                var peaks = new List<(int row, int column)>();
                FindTrail2(trailHead, map, bounds, peaks);
                score += peaks.Count;
            }

            return score;
        }



        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(482, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1094, Problem2(input));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "0123",
                "1234",
                "8765",
                "9876",
            ];
        
        private List<string> exampleInput2 =
            [
                "...0...",
                "...1...",
                "...2...",
                "6543456",
                "7.....7",
                "8.....8",
                "9.....9",
            ];
        
        private List<string> exampleInput3 =
            [
                "..90..9",
                "...1.98",
                "...2..7",
                "6543456",
                "765.987",
                "876....",
                "987....",
            ];
        
        private List<string> exampleInput4 =
            [
                "10..9..",
                "2...8..",
                "3...7..",
                "4567654",
                "...8..3",
                "...9..2",
                ".....01",
            ];

        private List<string> exampleInputLarge =
            [
                "89010123",
                "78121874",
                "87430965",
                "96549874",
                "45678903",
                "32019012",
                "01329801",
                "10456732",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample1()
        {
            Assert.Equal(1, Problem1(exampleInput));
        }
        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample2()
        {
            Assert.Equal(2, Problem1(exampleInput2));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample3()
        {
            Assert.Equal(4, Problem1(exampleInput3));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample4()
        {
            Assert.Equal(3, Problem1(exampleInput4));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExampleLarge()
        {
            Assert.Equal(36, Problem1(exampleInputLarge));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(81, Problem2(exampleInputLarge));
        }
    }
}
