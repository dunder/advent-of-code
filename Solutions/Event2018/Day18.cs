using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day18 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day18;
        public string Name => "Settlers of The North Pole";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = TotalValueAfter(input, 10);
            return result.ToString();
        }


        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = TotalValueAfterMany(input, 1000000000);

            return result.ToString();
        }

        private long TotalValueAfter(IList<string> input, int count)
        {
            var land = Land.Parse(input);

            for (int i = 0; i < count; i++)
            {
                land = land.Next();
            }

            return land.Trees.Count * land.Lumberyard.Count;
        }

        private long TotalValueAfterMany(IList<string> input, int count)
        {
            var land = Land.Parse(input);

            var history = new List<string>();

            for (int i = 0; i < count; i++)
            {
                land = land.Next();

                int x = history.IndexOf(land.ToString());

                if (x != -1)
                {
                    int frequency = history.Count - x;
                    while (count % frequency != (i + 1) % frequency)
                    {
                        land = land.Next();
                        i++;
                    }

                    return land.Trees.Count * land.Lumberyard.Count;
                }

                history.Add(land.ToString());
                if (history.Count > 50)
                {
                    history.RemoveAt(0);
                }
            }

            return land.Trees.Count * land.Lumberyard.Count;
        }

        public class Land
        {
            private readonly int width;
            private readonly int height;

            public HashSet<Point> Lumberyard { get; }
            public HashSet<Point> Trees { get; }

            private Land(HashSet<Point> lumberYard, HashSet<Point> trees, int width, int height)
            {
                Lumberyard = lumberYard;
                Trees = trees;
                this.width = width;
                this.height = height;
            }

            public static Land Parse(IList<string> input)
            {
                var land = new Land(new HashSet<Point>(), new HashSet<Point>(), input.First().Length, input.Count);

                for (int y = 0; y < land.height; y++)
                {
                    var line = input[y];
                    for (int x = 0; x < land.width; x++)
                    {
                        var point = new Point(x,y);
                        var acre = line[x];
                        if (acre == '|')
                        {
                            land.Trees.Add(point);
                        }
                        else if (acre == '#')
                        {
                            land.Lumberyard.Add(point);
                        }

                    }
                }

                return land;
            }

            public Land Next()
            {
                var newLumberyard = new HashSet<Point>();
                var newTrees = new HashSet<Point>();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var point = new Point(x,y);
                        if (Trees.Contains(point))
                        {
                            ConvertTree(point, newLumberyard, newTrees);
                        }
                        else if (Lumberyard.Contains(point))
                        {
                            ConvertLumberyard(point, newLumberyard, newTrees);
                        }
                        else
                        {
                            ConvertOpen(point, newLumberyard, newTrees);
                        }
                    }
                }

                return new Land(newLumberyard, newTrees, width, height);
            }

            public override string ToString()
            {
                var landString = new StringBuilder();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var point = new Point(x, y);
                        if (Trees.Contains(point))
                        {
                            landString.Append('|');
                        }
                        else if (Lumberyard.Contains(point))
                        {
                            landString.Append('#');
                        }
                        else
                        {
                            landString.Append('.');
                        }
                    }

                    landString.AppendLine();
                }

                return landString.ToString();
            }

            private void ConvertOpen(Point point, HashSet<Point> newLumberyard, HashSet<Point> newTrees)
            {
                var adjacent = point.AdjacentInAllDirections()
                    .Where(p => p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height);

                if (adjacent.Count(a => Trees.Contains(a)) >= 3)
                {
                    newTrees.Add(point);
                }
            }

            private void ConvertLumberyard(Point point, HashSet<Point> newLumberyard, HashSet<Point> newTrees)
            {
                var adjacent = point.AdjacentInAllDirections()
                    .Where(p => p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height)
                    .ToList();

                if (adjacent.Count(a => Trees.Contains(a)) > 0 && adjacent.Count(a => Lumberyard.Contains(a)) > 0)
                {
                    newLumberyard.Add(point);
                }
            }

            private void ConvertTree(Point point, HashSet<Point> newLumberyard, HashSet<Point> newTrees)
            {
                var adjacent = point.AdjacentInAllDirections()
                    .Where(p => p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height)
                    .ToList();

                if (adjacent.Count(a => Lumberyard.Contains(a)) >= 3)
                {
                    newLumberyard.Add(point);
                }
                else
                {
                    newTrees.Add(point);
                }
            }
        }

        [Fact]
        public void Next()
        {
            var lines = new[]
            {
                ".#.#...|#.",
                ".....#|##|",
                ".|..|...#.",
                "..|#.....#",
                "#.#|||#|#|",
                "...#.||...",
                ".|....|...",
                "||...#|.#|",
                "|.||||..|.",
                "...#.|..|."
            };
            var expectedLines = new[]
            {
                ".......##.",
                "......|###",
                ".|..|...#.",
                "..|#||...#",
                "..##||.|#|",
                "...#||||..",
                "||...|||..",
                "|||||.||.|",
                "||||||||||",
                "....||..|."
            };

            var land = Land.Parse(lines);
            var expectedLand = Land.Parse(expectedLines);

            var newLand = land.Next();

            Assert.Equal(expectedLand.ToString(), newLand.ToString());
        }

        [Fact]
        public void FirstStarExample()
        {
            var lines = new[]
            {
                ".#.#...|#.",
                ".....#|##|",
                ".|..|...#.",
                "..|#.....#",
                "#.#|||#|#|",
                "...#.||...",
                ".|....|...",
                "||...#|.#|",
                "|.||||..|.",
                "...#.|..|."
            };

            var value = TotalValueAfter(lines, 10);

            Assert.Equal(1147, value);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("456225", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("190164", actual);
        }
    }
}
