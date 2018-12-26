using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day25 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day25;
        public string Name => "Day25";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = CalculateNumberOfConstellations(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public static int CalculateNumberOfConstellations(IList<string> input) 
        {
            var points = Parse(input);

            var firstConstellation = new Constellation(points.First());
            var constellations = new List<Constellation> { firstConstellation };

            foreach (var point in points.Skip(1))
            {
                var added = false;
                foreach (var constellation in constellations)
                {
                    if (constellation.TryAdd(point))
                    {
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    var newConstellation = new Constellation(point);
                    constellations.Add(newConstellation);
                }
            }

            var merged = new HashSet<int>();

            var mergedAny = true;

            while (mergedAny)
            {
                mergedAny = false;
                foreach (var constellation in constellations.Where(c => !merged.Contains(c.Id)))
                {
                    foreach (var constellation2 in constellations.Where(c => c.Id != constellation.Id && !merged.Contains(c.Id)))
                    {
                        if (constellation.TryMerge(constellation2))
                        {
                            mergedAny = true;
                            merged.Add(constellation2.Id);
                        }
                    }
                }
            }

            constellations = constellations.Where(c => !merged.Contains(c.Id)).ToList();


            return constellations.Count;
        }

        public static List<Point> Parse(IList<string> input)
        {
            var points = new List<Point>();

            foreach (var line in input)
            {
                var values = line.Split(",").Select(int.Parse).ToList();
                points.Add(new Point(values[0], values[1], values[2], values[3]));
            }

            return points;
        }

        public class Constellation
        {
            private static int id = 1;

            public Constellation(Point point)
            {
                Points = new HashSet<Point> {point};
                Id = id++;
            }

            public int Id { get; }
            public HashSet<Point> Points { get; set; }

            public bool TryAdd(Point point)
            {
                if (Points.Any(p => p.ManhattanDistanceTo(point) <= 3))
                {
                    Points.Add(point);
                    return true;
                };
                return false;
            }

            public bool TryMerge(Constellation constellation)
            {
                foreach (var point in Points)
                {
                    if (constellation.Points.Any(p => p.ManhattanDistanceTo(point) <= 3))
                    {
                        Points.UnionWith(constellation.Points);
                        return true;
                    }
                }

                return false;
            }
        }

        public struct Point : IEquatable<Point>
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int Q { get; set; }

            public Point(int x, int y, int z, int q)
            {
                X = x;
                Y = y;
                Z = z;
                Q = q;
            }

            public int ManhattanDistanceTo(Point other)
            {
                return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z) + Math.Abs(Q - other.Q);
            }

            public override string ToString()
            {
                return $"({X},{Y},{Z},{Q})";
            }

            public bool Equals(Point other)
            {
                return X == other.X && Y == other.Y && Z == other.Z && Q == other.Q;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Point other && Equals(other);
            }

            public static bool operator ==(Point a, Point b)
            {
                return a.Equals(b);
            }

            public static bool operator !=(Point a, Point b)
            {
                return !a.Equals(b);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = X;
                    hashCode = (hashCode * 397) ^ Y;
                    hashCode = (hashCode * 397) ^ Z;
                    hashCode = (hashCode * 397) ^ Q;
                    return hashCode;
                }
            }
        }

        [Fact]
        public void FirstStarExample1()
        {
            var input = new[]
            {
                "0,0,0,0",
                "3,0,0,0",
                "0,3,0,0",
                "0,0,3,0",
                "0,0,0,3",
                "0,0,0,6",
                "9,0,0,0",
                "12,0,0,0"
            };

            var count = CalculateNumberOfConstellations(input);

            Assert.Equal(2, count);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = new[]
            {
                "-1,2,2,0",
                "0,0,2,-2",
                "0,0,0,-2",
                "-1,2,0,0",
                "-2,-2,-2,2",
                "3,0,2,-1",
                "-1,3,2,2",
                "-1,0,-1,0",
                "0,2,1,-2",
                "3,0,0,0"
            };

            var count = CalculateNumberOfConstellations(input);

            Assert.Equal(4, count);
        }

        [Fact]
        public void FirstStarExample3()
        {
            var input = new[]
            {
                "1,-1,0,1",
                "2,0,-1,0",
                "3,2,-1,0",
                "0,0,3,1",
                "0,0,-1,-1",
                "2,3,-2,0",
                "-2,2,0,0",
                "2,-2,0,-1",
                "1,-1,0,-1",
                "3,2,0,2"
            };

            var count = CalculateNumberOfConstellations(input);

            Assert.Equal(3, count);
        }

        [Fact]
        public void FirstStarExample4()
        {
            var input = new[]
            {
                "1,-1,-1,-2",
                "-2,-2,0,1",
                "0,2,1,3",
                "-2,3,-2,1",
                "0,2,3,-2",
                "-1,-1,1,-2",
                "0,-2,-1,0",
                "-2,2,3,-1",
                "1,2,2,0",
                "-1,-2,0,-2"
            };

            var count = CalculateNumberOfConstellations(input);

            Assert.Equal(8, count);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual);
        }
    }
}