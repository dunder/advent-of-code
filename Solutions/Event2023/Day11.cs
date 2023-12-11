using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 11: Cosmic Expansion ---
    public class Day11
    {
        private readonly ITestOutputHelper output;

        public Day11(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Position(long X, long Y);

        private List<Position> Parse(IList<string> input, int expansion)
        {
            var emptyColumn = new List<int>();

            var height = input.Count;
            var width = input[0].Length;

            var emptyRows = Enumerable.Range(0, height)
                .Where(y => input[y].All(c => c == '.'))
                .ToHashSet();

            var emptyColumns = Enumerable.Range(0, width)
                .Where(x => Enumerable.Range(0, height).All(y => input[y][x] == '.'))
                .ToHashSet();

            var data = new char[width + emptyColumns.Count, height + emptyRows.Count];

            var galaxies = new List<Position>();

            var expandx = 0L;
            var expandy = 0L;

            for (int y = 0; y < height; y++)
            {
                if (emptyRows.Contains(y))
                {
                    expandy += expansion-1;
                }
                
                expandx = 0;

                for (int x = 0; x < width; x++)
                {
                    if (emptyColumns.Contains(x))
                    {
                        expandx += expansion-1;
                    }

                    if (input[y][x] == '#')
                    {
                        galaxies.Add(new Position(x + expandx, y + expandy));
                    }
                }
            }

            return galaxies;
        }

        private long ShortestPath(IList<string> input, int expansion = 2)
        {
            var galaxies = Parse(input, expansion);

            long ManhattanDistance((Position position1,  Position position2) pair)
            {
                var (position1, position2) = pair;

                return Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);

            }

            IEnumerable<(Position, Position)> AllPairs(List<Position> pairs)
            {
                for (int i = 0; i < galaxies.Count - 1; i++)
                {
                    for (int j = i + 1; j < galaxies.Count; j++)
                    {
                        var g1 = galaxies[i];
                        var g2 = galaxies[j];

                        yield return (g1, g2);
                        
                    }
                }
            }

            return AllPairs(galaxies).Select(ManhattanDistance).Sum();

        }

        public long FirstStar()
        {
            var input = ReadLineInput();
            return ShortestPath(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();
            return ShortestPath(input, 1_000_000);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(10494813, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(840988812853, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "...#......",
                ".......#..",
                "#.........",
                "..........",
                "......#...",
                ".#........",
                ".........#",
                "..........",
                ".......#..",
                "#...#.....",
            };

            Assert.Equal(374, ShortestPath(example));
        }

        [Fact]
        public void SecondStarExample2()
        {
            var example = new List<string>
            {
                "...#......",
                ".......#..",
                "#.........",
                "..........",
                "......#...",
                ".#........",
                ".........#",
                "..........",
                ".......#..",
                "#...#.....",
            };

            Assert.Equal(1030, ShortestPath(example, 10));
        }

        [Fact]
        public void SecondStarExample3()
        {
            var example = new List<string>
            {
                "...#......",
                ".......#..",
                "#.........",
                "..........",
                "......#...",
                ".#........",
                ".........#",
                "..........",
                ".......#..",
                "#...#.....",
            };

            Assert.Equal(8410, ShortestPath(example, 100));
        }
    }
}
