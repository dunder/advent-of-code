using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shared.MapGeometry;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day17 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day17;
        public string Name => "Reservoir Research";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var (reached, _) = CountTiles(input);
            return reached.ToString();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var (_, settled) = CountTiles(input);
            return settled.ToString();
        }

        private (int reached, int settled) CountTiles(IList<string> input)
        {
            var clay = ParseClay(input);

            var dripping = new HashSet<Point>();
            var settled = new HashSet<Point>();
            var overflowed = new HashSet<Point>();

            var maxY = clay.Max(c => c.Y);

            var open = new Stack<Point>();
            open.Push(new Point(500,1));

            while (open.Any())
            {
                var current = open.Pop();
                
                var under = current.Move(Direction.South);

                while (!clay.Contains(under) && !settled.Contains(under) && current.Y <= maxY)
                {
                    dripping.Add(current);
                    current = under;
                    under = current.Move(Direction.South);
                }

                if (current.Y >= maxY)
                {
                    continue;
                }

                // check left and right, if any open mark as overflowing, if closed mark as settled
                var openCount = open.Count;

                // check left until water can fall down (no clay under) or hits a left wall of clay
                var left = current;

                while ((clay.Contains(under) || settled.Contains(under)) && !clay.Contains(left.Move(Direction.West)))
                {
                    left = left.Move(Direction.West);
                    under = left.Move(Direction.South);
                }

                // water can fall down to the left
                if (!clay.Contains(under) && !settled.Contains(under))
                {
                    open.Push(left);
                }

                // check right
                var right = current;
                under = right.Move(Direction.South);

                while ((clay.Contains(under) || settled.Contains(under)) && !clay.Contains(right.Move(Direction.East)))
                {
                    right = right.Move(Direction.East);
                    under = right.Move(Direction.South);
                }

                // water can fall down to the right
                if (!clay.Contains(under) && !settled.Contains(under))
                {
                    open.Push(right);
                }

                if (open.Count > openCount)
                {
                    // water could not settle
                    PointsBetween(left, right).ForEach(p => overflowed.Add(p));
                }
                else
                {
                    // water settles
                    PointsBetween(left, right).ForEach(p => settled.Add(p));
                    var back = current.Move(Direction.North);
                    open.Push(back);
                }
            }
            //WriteToFile(clay, settled, overflowed, dripping);
            var tilesReached = new HashSet<Point>(dripping);
            tilesReached.UnionWith(settled);
            tilesReached.UnionWith(overflowed);

            var reached = tilesReached.Count(t => t.Y >= clay.Min(c => c.Y));
            

            return (reached, settled.Count);
        }

        private static List<Point> PointsBetween(Point left, Point right)
        {
            var points = new List<Point>();

            while (left != right)
            {
                points.Add(left);
                left = left.Move(Direction.East);
            }

            points.Add(right);

            return points;
        }

        

        public static void WriteToFile(HashSet<Point> clay, HashSet<Point> settled, HashSet<Point> overflowed, HashSet<Point> dripping)
        {
            var minX = clay.Min(p => p.X);
            var maxX = clay.Max(p => p.X) + 3;
            var minY = 0;
            var maxY = clay.Max(p => p.Y) + 3;

            var lines = new List<string>();

            for (int y = minY; y <= maxY; y++)
            {
                var line = new StringBuilder(maxX - minX + 1);
                for (int x = minX; x <= maxX; x++)
                {
                    var point = new Point(x, y);
                    var print = ".";
                    if (clay.Contains(point))
                    {
                        print = "#";
                    }
                    else if (settled.Contains(point))
                    {
                        print = "~";
                    }
                    else if (overflowed.Contains(point) || dripping.Contains(point))
                    {
                        print = "|";
                    }

                    if (point.X == 500 && point.Y == 0)
                    {
                        print = "+";
                    }
                    line.Append(print);
                }
                lines.Add(line.ToString());
            }

            File.WriteAllLines(@".\underground.txt", lines);
        }


        public HashSet<Point> ParseClay(IList<string> input)
        {
            var clay = new HashSet<Point>();

            var horizontalExpression = new Regex(@"y=(\d+), x=(\d+)..(\d+)");
            var verticalExpression = new Regex(@"x=(\d+), y=(\d+)..(\d+)");
            foreach (var line in input)
            {
                if (horizontalExpression.IsMatch(line))
                {
                    var match = horizontalExpression.Match(line);
                    var y = int.Parse(match.Groups[1].Value);
                    var xFrom = int.Parse(match.Groups[2].Value);
                    var xTo = int.Parse(match.Groups[3].Value);

                    for (int x = xFrom; x <= xTo; x++)
                    {
                        clay.Add(new Point(x, y));
                    }
                }
                else
                {
                    var match = verticalExpression.Match(line);
                    var x = int.Parse(match.Groups[1].Value);
                    var yFrom = int.Parse(match.Groups[2].Value);
                    var yTo = int.Parse(match.Groups[3].Value);

                    for (int y = yFrom; y <= yTo; y++)
                    {
                        clay.Add(new Point(x, y));
                    }
                }
            }

            return clay;
        }

        [Theory]
        [InlineData(-1,0,1,0,3)]
        public void PointsBetweenTest(int xl, int yl, int xr, int yr, int expectedCount)
        {
            if (yl != yr) throw new ArgumentOutOfRangeException($"left and right must be on same line: left = ({xl},{yl}), right = ({xr},{yr})");
            if (xl > xr) throw new ArgumentOutOfRangeException($"left is right of right: left = ({xl},{yl}), right = ({xr},{yr})");

            var left = new Point(xl, yl);
            var right = new Point(xr, yr);

            var between = PointsBetween(left, right);

            Assert.Equal(expectedCount, between.Count);
        }

        [Fact]
        public void FirstStarExample()
        {
            var lines = new[]
            {
                "x=495, y=2..7",
                "y=7, x=495..501",
                "x=501, y=3..7",
                "x=498, y=2..4",
                "x=506, y=1..2",
                "x=498, y=10..13",
                "x=504, y=10..13",
                "y=13, x=498..504"
            };

            var result = CountTiles(lines);

            Assert.Equal(57, result.reached);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("27331", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("22245", actual);
        }
    }
}