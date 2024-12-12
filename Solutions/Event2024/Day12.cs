using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 12: Phrase ---
    public class Day12
    {
        private readonly ITestOutputHelper output;

        public Day12(ITestOutputHelper output)
        {
            this.output = output;
        }

        public static char[,] Parse(IList<string> input)
        {
            var map = new char[input.Count, input.First().Length];

            for (var row = 0; row < input.Count; row++)
            {
                var line = input[row];
                for (var column = 0; column < input.First().Length; column++)
                {
                    map[column, row] = line[column];

                }
            }

            return map;
        }


        private static bool WithinBounds(char[,] map, (int x, int y) location)
        {
            (int x, int y) = location;
            return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
        }

        private static List<(int x, int y)> Neighbors(char[,] map, (int x, int y) pixel)
        {
            (int x, int y) = pixel;

            return [
                (x, y - 1),
                (x + 1, y),
                (x, y + 1),
                (x - 1, y)
                ];
        }

        private static HashSet<(int x, int y)> FloodFill(char[,] map, HashSet<(int x, int y)> totalFilled, int x, int y)
        {
            char crop = map[x, y];

            HashSet<(int x, int y)> filled = new();
            Queue<(int x, int y)> Q = new Queue<(int x, int y)>();

            Q.Enqueue((x, y));

            bool Inside((int x, int y) pixel)
            {
                (int x, int y) = pixel;

                return map[x, y] == crop && !filled.Contains((x, y));
            }

            while (Q.Count > 0)
            {
                var n = Q.Dequeue();

                if (Inside(n))
                {
                    filled.Add(n);

                    foreach (var neighbor in Neighbors(map, n).Where(n => WithinBounds(map, n)))
                    {
                        Q.Enqueue(neighbor);
                    }
                }
            }

            return filled;
        }

        private static bool Outside(char[,] map, (int x, int y) pixel)
        {
            (int x, int y) = pixel;

            return x < 0 || x >= map.GetLength(0) || y < 0 || y >= map.GetLength(1);
        }

        private static int Perimeter(char[,] map, HashSet<(int x, int y)> field)
        {
            return field.Select(p => Neighbors(map, p).Count(n => Outside(map, n) || map[p.x, p.y] != map[n.x, n.y])).Sum();
        }

        private static int CountContinous(List<int> ints)
        {
            if (ints.Count == 0) return 0;
            if (ints.Count == 1) return 1;

            int counter = 1;

            for (int i = 1; i < ints.Count; i++)
            {
                if (ints[i] - ints[i-1] > 1)
                {
                    counter++;
                }
            }

            return counter;
        }

        private static int Sides(char[,] map, HashSet<(int x, int y)> field)
        {
            var byRow = field.GroupBy(f => f.y);

            var any = field.First();

            var crop = map[any.x, any.y];

            bool OutsideField((int x, int y) p)
            {
                (int x, int y) = p;
                return Outside(map, p) || map[x, y] != crop;
            }

            int count = 0;

            foreach (var row in byRow)
            {
                var up = row.Where(r => OutsideField((r.x, r.y - 1))).Select(r => r.x).OrderBy(x => x).ToList();                
                var down = row.Where(r => OutsideField((r.x, r.y + 1))).Select(r => r.x).OrderBy(x => x).ToList();
                
                count += CountContinous(up);
                count += CountContinous(down);
            }

            var byColumn = field.GroupBy(f => f.x);

            foreach (var column in byColumn)
            {
                var right = column.Where(r => OutsideField((r.x + 1, r.y))).Select(r => r.y).OrderBy(x => x).ToList();
                var left = column.Where(r => OutsideField((r.x - 1, r.y))).Select(r => r.y).OrderBy(x => x).ToList();

                count += CountContinous(right);
                count += CountContinous(left);
            }

            return count;
        }

        private static long Problem1(IList<string> input)
        {
            var map = Parse(input);

            var fields = new List<HashSet<(int x, int y)>>();
            var totalFilled = new HashSet<(int x, int y)>();
            
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (totalFilled.Contains((x, y)))
                    {
                        continue;
                    }

                    var fieldFill = FloodFill(map, totalFilled, x, y);

                    fields.Add(fieldFill);

                    totalFilled.UnionWith(fieldFill);
                }
            }

            var result = fields.Select(f => f.Count * Perimeter(map, f)).Sum();

            return result;
        }

        private static int Problem2(IList<string> input)
        {

            var map = Parse(input);

            var fields = new List<HashSet<(int x, int y)>>();
            var totalFilled = new HashSet<(int x, int y)>();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (totalFilled.Contains((x, y)))
                    {
                        continue;
                    }

                    var fieldFill = FloodFill(map, totalFilled, x, y);

                    fields.Add(fieldFill);

                    totalFilled.UnionWith(fieldFill);
                }
            }

            var result = fields.Select(f => f.Count * Sides(map, f)).Sum();

            return result;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input));
        }

        private List<string> exampleInput =
            [
                "AAAA",
                "BBCD",
                "BBCC",
                "EEEC",
            ];

        private List<string> exampleInput2 =
            [
                "OOOOO",
                "OXOXO",
                "OOOOO",
                "OXOXO",
                "OOOOO",
            ];

        private List<string> exampleInput3 =
            [
                "AAAA",
                "BBCD",
                "BBCC",
                "EEEC",
            ];

        private List<string> exampleInput4 =
            [
"AAAAAA",
"AAABBA",
"AAABBA",
"ABBAAA",
"ABBAAA",
"AAAAAA",
            ];


        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(140, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample2()
        {
            Assert.Equal(772, Problem1(exampleInput2));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(80, Problem2(exampleInput3));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample2()
        {
            Assert.Equal(368, Problem2(exampleInput4));
        }
    }
}
