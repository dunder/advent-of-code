using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Solutions.Event2025
{
    // --- Day 9: Movie Theater ---
    public class Day09
    {
        private readonly ITestOutputHelper output;

        public Day09(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static long Problem1(IList<string> input)
        {
            List<(int x, int y)> tiles = input.Select(line =>
            {
                var parts = line.Split(",");
                var x = int.Parse(parts[0]);
                var y = int.Parse(parts[1]);

                return (x, y);
            }).ToList();

            List<long> result = [];

            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = i + 1; j < tiles.Count; j++)
                {
                    var a = tiles[i];
                    var b = tiles[j];

                    var maxx = Math.Max(a.x, b.x);
                    var maxy = Math.Max(a.y, b.y);

                    var minx = Math.Min(a.x, b.x);
                    var miny = Math.Min(a.y, b.y);

                    result.Add((long)(maxx - minx + 1) * (maxy - miny + 1));
                }
            }

            return result.Max();
        }

        private record VerticalSide(int X, int From, int To)
        {
            public bool IsWithin(int y)
            {
                return From <= y && y < To;
            }
            public bool IsWithin(int x, int y)
            {
                return x == X && From <= y && y <= To;
            }
        }

        private record HorizontalSide(int Y, int From, int To)
        {
            public bool IsWithin(int x, int y)
            {
                return y == Y && From <= x && x <= To;
            }
        }
        private static List<Rectangle> ParseEdges(IList<string> input)
        {
            List<Rectangle> rectangles = [];

            (int x, int y) ToPoint(string line)
            {
                var parts = line.Split(",");

                var x = int.Parse(parts[0]);
                var y = int.Parse(parts[1]);

                return (x, y);
            }

            input = [.. input, input[0]];

            for (int i = 0; i < input.Count - 1; i++)
            {
                var a = ToPoint(input[i]);
                var b = ToPoint(input[i + 1]);

                rectangles.Add(new Rectangle(a, b));
            }

            return rectangles;
        }

        private static long Problem2(IList<string> input)
        {
            List<(int x, int y)> tiles = ParseTiles(input);

            List<Rectangle> edges = ParseEdges(input);
            List<Rectangle> rectangles = AllRectangles(tiles);

            var checkedRectangles = rectangles.Where(r => !edges.Any(t => r.Overlaps(t))).ToList();

            var max = checkedRectangles.MaxBy(a => a.Area());

            return max.Area();
        }

        private record Rectangle((int x, int y) A, (int x, int y) B)
        {
            public long Area()
            {
                var maxx = Math.Max(A.x, B.x);
                var maxy = Math.Max(A.y, B.y);

                var minx = Math.Min(A.x, B.x);
                var miny = Math.Min(A.y, B.y);

                return (long)(maxx - minx + 1) * (maxy - miny + 1);
            }

            public bool Overlaps(Rectangle other)
            {
                var maxx = Math.Max(A.x, B.x);
                var maxy = Math.Max(A.y, B.y);

                var minx = Math.Min(A.x, B.x);
                var miny = Math.Min(A.y, B.y);

                var omaxx = Math.Max(other.A.x, other.B.x);
                var omaxy = Math.Max(other.A.y, other.B.y);

                var ominx = Math.Min(other.A.x, other.B.x);
                var ominy = Math.Min(other.A.y, other.B.y);

                return !(maxx <= ominx || omaxx <= minx || maxy <= ominy || omaxy <= miny);
            }
        }

        private static List<(int x, int y)> ParseTiles(IList<string> input)
        {
            return input.Select(line =>
            {
                var parts = line.Split(",");
                var x = int.Parse(parts[0]);
                var y = int.Parse(parts[1]);

                return (x, y);
            }).ToList();
        }

        private static List<Rectangle> AllRectangles(List<(int x, int y)> tiles)
        {
            List<Rectangle> potentialSquares = [];

            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = i + 1; j < tiles.Count; j++)
                {
                    var a = tiles[i];
                    var b = tiles[j];

                    var maxx = Math.Max(a.x, b.x);
                    var maxy = Math.Max(a.y, b.y);

                    var minx = Math.Min(a.x, b.x);
                    var miny = Math.Min(a.y, b.y);

                    potentialSquares.Add(new Rectangle(a, b));
                }
            }

            return potentialSquares;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(4755064176, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1613305596, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(50, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(24, Problem2(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample2()
        {
            var exampleInput = ReadExampleLineInput("Example2");

            Assert.Equal(39, Problem2(exampleInput));
        }
    }
}