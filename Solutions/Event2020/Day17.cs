using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 17: Conway Cubes ---

    public class Day17
    {
        private readonly ITestOutputHelper output;

        private struct Point : IEquatable<Point>
        {
            public Point (int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is Point)
                {
                    return Equals((Point)obj);
                }
                return false;
            }

            public bool Equals([AllowNull] Point p)
            {
                return (X == p.X) && (Y == p.Y) && (Z == p.Z);
            }

            public override int GetHashCode()
            {
                return X ^ Y ^ Z;
            }

            public static bool operator ==(Point lhs, Point rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(Point lhs, Point rhs)
            {
                return !(lhs.Equals(rhs));
            }

            public override string ToString()
            {
                return $"({X},{Y},{Z})";
            }
        }



        private struct Point4D : IEquatable<Point4D>
        {
            public Point4D(int x, int y, int z, int w)
            {
                X = x;
                Y = y;
                Z = z;
                W = w;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int W { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is Point4D)
                {
                    return Equals((Point4D)obj);
                }
                return false;
            }

            public bool Equals([AllowNull] Point4D p)
            {
                return (X == p.X) && (Y == p.Y) && (Z == p.Z) && (W == p.W);
            }

            public override int GetHashCode()
            {
                return X ^ Y ^ Z ^ W;
            }

            public static bool operator ==(Point4D lhs, Point4D rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(Point4D lhs, Point4D rhs)
            {
                return !(lhs.Equals(rhs));
            }

            public override string ToString()
            {
                return $"({X},{Y},{Z},{W})";
            }
        }

        private class Cube : IEnumerable<(Point point, bool value)>
        {
            public Cube(HashSet<Point> squares)
            {
                Squares = squares;
            }

            public int XMin => Squares.Min(p => p.X);
            public int XMax => Squares.Max(p => p.X);
            public int YMin => Squares.Min(p => p.Y);
            public int YMax => Squares.Max(p => p.Y);
            public int ZMin => Squares.Min(p => p.Z);
            public int ZMax => Squares.Max(p => p.Z);

            public HashSet<Point> Squares { get; }

            public int Active => this.Count(x => x.value);

            public Cube Copy()
            {
                return new Cube(new HashSet<Point>(Squares));
            }

            public bool IsActive(Point point)
            {
                return Squares.Contains(point);
            }

            public void SetActive(Point point)
            {
                Squares.Add(point);
            }

            public void SetInactive(Point point)
            {
                Squares.Remove(point);
            }

            public HashSet<Point> Space()
            {
                var space = new HashSet<Point>();
                foreach (var p in Squares)
                {
                    space.Add(p);
                    foreach (var (n, _) in Neighbors(p))
                    {
                        space.Add(n);
                    }
                }
                return space;
            }

            public IEnumerable<(Point point, bool value)> Neighbors(Point point)
            {
                for (int z = point.Z-1; z <= point.Z+1; z++)
                {
                    for (int y = point.Y-1; y <= point.Y+1; y++)
                    {
                        for (int x = point.X-1; x <= point.X+1; x++)
                        {
                            var neighbor = new Point(x, y, z);
                            if (neighbor != point)
                            {
                                yield return (neighbor, Squares.Contains(neighbor));
                            }
                        }
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<(Point point, bool value)> GetEnumerator()
            {
                for (int z = ZMin; z <= ZMax; z++)
                {
                    for (int y = YMin; y <= YMax; y++)
                    {
                        for (int x = XMin; x <= XMax; x++)
                        {
                            var p = new Point(x, y, z);
                            yield return (p, Squares.Contains(p));
                        }
                    }
                }
            }

            public override string ToString()
            {
                var stringBuilder = new StringBuilder();

                for (int z = ZMin; z <= ZMax; z++)
                {
                    stringBuilder.AppendLine($"z={z}");

                    for (int y = YMin; y <= YMax; y++)
                    {
                        for (int x = XMin; x <= XMax; x++)
                        {
                            var p = new Point(x, y, z);
                            stringBuilder.Append(Squares.Contains(p) ? "#" : ".");
                        }

                        stringBuilder.AppendLine();
                    }
                    stringBuilder.AppendLine();
                }

                return stringBuilder.ToString();
            }

            public Cube Cycle()
            {
                var previous = this;
                var copy = Copy();

                foreach (var i in Enumerable.Range(0, 6))
                {
                    foreach (var p in previous.Space())
                    {
                        var neighbors = previous.Neighbors(p);
                        var activeNeighbors = neighbors.Count(n => n.value);

                        if (previous.IsActive(p))
                        {
                            if (activeNeighbors == 2 || activeNeighbors == 3)
                            {
                                copy.SetActive(p);
                            }
                            else
                            {
                                copy.SetInactive(p);
                            }
                        }
                        else
                        {
                            if (activeNeighbors == 3)
                            {
                                copy.SetActive(p);
                            }
                            else
                            {
                                copy.SetInactive(p);
                            }
                        }
                    }

                    var debug = copy.ToString();
                    previous = copy;
                    copy = previous.Copy();
                }

                return copy;
            }
        }

        private class Cube4D : IEnumerable<(Point4D point, bool value)>
        {
            public Cube4D(HashSet<Point4D> squares)
            {
                Squares = squares;
            }

            public int XMin => Squares.Min(p => p.X);
            public int XMax => Squares.Max(p => p.X);
            public int YMin => Squares.Min(p => p.Y);
            public int YMax => Squares.Max(p => p.Y);
            public int ZMin => Squares.Min(p => p.Z);
            public int ZMax => Squares.Max(p => p.Z);
            public int WMin => Squares.Min(p => p.W);
            public int WMax => Squares.Max(p => p.W);

            public HashSet<Point4D> Squares { get; }

            public int Active => this.Count(x => x.value);

            public Cube4D Copy()
            {
                return new Cube4D(new HashSet<Point4D>(Squares));
            }

            public bool IsActive(Point4D point)
            {
                return Squares.Contains(point);
            }

            public void SetActive(Point4D point)
            {
                Squares.Add(point);
            }

            public void SetInactive(Point4D point)
            {
                Squares.Remove(point);
            }

            public HashSet<Point4D> Space()
            {
                var space = new HashSet<Point4D>();
                foreach (var p in Squares)
                {
                    space.Add(p);
                    foreach (var (n, _) in Neighbors(p))
                    {
                        space.Add(n);
                    }
                }
                return space;
            }

            public IEnumerable<(Point4D point, bool value)> Neighbors(Point4D point)
            {
                for (int w = point.W - 1; w <= point.W + 1; w++)
                {
                    for (int z = point.Z - 1; z <= point.Z + 1; z++)
                    {
                        for (int y = point.Y - 1; y <= point.Y + 1; y++)
                        {
                            for (int x = point.X - 1; x <= point.X + 1; x++)
                            {
                                var neighbor = new Point4D(x, y, z, w);
                                if (neighbor != point)
                                {
                                    yield return (neighbor, Squares.Contains(neighbor));
                                }
                            }
                        }
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<(Point4D point, bool value)> GetEnumerator()
            {
                for (int w = WMin; w <= WMax; w++)
                {
                    for (int z = ZMin; z <= ZMax; z++)
                    {
                        for (int y = YMin; y <= YMax; y++)
                        {
                            for (int x = XMin; x <= XMax; x++)
                            {
                                var p = new Point4D(x, y, z, w);
                                yield return (p, Squares.Contains(p));
                            }
                        }
                    }
                }
            }

            public override string ToString()
            {
                var stringBuilder = new StringBuilder();

                for (int w = WMin; w <= WMax; w++)
                {
                    for (int z = ZMin; z <= ZMax; z++)
                    {
                        stringBuilder.AppendLine($"z={z}, w={w}");

                        for (int y = YMin; y <= YMax; y++)
                        {
                            for (int x = XMin; x <= XMax; x++)
                            {
                                var p = new Point4D(x, y, z, w);
                                stringBuilder.Append(Squares.Contains(p) ? "#" : ".");
                            }

                            stringBuilder.AppendLine();
                        }
                        stringBuilder.AppendLine();
                    }
                }

                return stringBuilder.ToString();
            }

            public Cube4D Cycle()
            {
                var previous = this;
                var copy = Copy();

                foreach (var i in Enumerable.Range(0, 6))
                {
                    foreach (var p in previous.Space())
                    {
                        var neighbors = previous.Neighbors(p);
                        var activeNeighbors = neighbors.Count(n => n.value);

                        if (previous.IsActive(p))
                        {
                            if (activeNeighbors == 2 || activeNeighbors == 3)
                            {
                                copy.SetActive(p);
                            }
                            else
                            {
                                copy.SetInactive(p);
                            }
                        }
                        else
                        {
                            if (activeNeighbors == 3)
                            {
                                copy.SetActive(p);
                            }
                            else
                            {
                                copy.SetInactive(p);
                            }
                        }
                    }

                    var debug = copy.ToString();
                    previous = copy;
                    copy = previous.Copy();
                }

                return copy;
            }
        }

        private static class CubeParser
        {
            public static Cube Parse(IList<string> input)
            {
                var cube = new HashSet<Point>();

                for (int y = 0; y < input.Count; y++)
                {
                    var line = input[y];
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (line[x] == '#')
                        {
                            cube.Add(new Point(x, y, 0));
                        }
                    }
                }

                return new Cube(cube);
            }
        }

        private static class Cube4DParser
        {
            public static Cube4D Parse(IList<string> input)
            {
                var cube = new HashSet<Point4D>();

                for (int y = 0; y < input.Count; y++)
                {
                    var line = input[y];
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (line[x] == '#')
                        {
                            cube.Add(new Point4D(x, y, 0, 0));
                        }
                    }
                }

                return new Cube4D(cube);
            }
        }

        public int CountCycles(List<string> input)
        {
            var cube = CubeParser.Parse(input);

            var cycled = cube.Cycle();

            int active = cycled.Active;

            return active;
        }

        public int CountCycles4D(List<string> input)
        {
            var cube = Cube4DParser.Parse(input);

            var cycled = cube.Cycle();

            int active = cycled.Active;

            return active;
        }

        public Day17(ITestOutputHelper output)
        {
            this.output = output;
        }

        public int FirstStar()
        {
            var input = ReadLineInput().ToList();

            return CountCycles(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput().ToList();

            return CountCycles4D(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(368, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(2696, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                ".#.",
                "..#",
                "###"
            };

            var cube = CubeParser.Parse(input);

            var cycled = cube.Cycle();

            int active = cycled.Active;

            Assert.Equal(112, active);
        }
    }
}
