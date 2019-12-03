using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.MapGeometry;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 3: Crossed Wires ---
    public class Day03
    {
        public class StraightWire
        {
            public Direction Direction { get; set; }
            public int Length { get; set; }

            public override string ToString()
            {
                return $"{Direction}, {Length}";
            }
        }

        public static Direction ParseDirection(string c)
        {
            switch (c)
            {
                case "D":
                    return Direction.South;
                case "U":
                    return Direction.North;
                case "L":
                    return Direction.West;
                case "R":
                    return Direction.East;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown direction {c}");
            }
        }
        public static (List<StraightWire>, List<StraightWire>) Parse(IEnumerable<string> input)
        {
            var lines = input.ToList();
            var wires = input.Select(line =>
            {
                return line.Split(",").Select(x => new StraightWire
                {
                    Direction = ParseDirection(x.Substring(0, 1)),
                    Length = int.Parse(x.Substring(1))
                }).ToList();
            }).ToList();

            return (wires.First(), wires.Last());
        }

        public static List<Point> Positions(List<StraightWire> wire)
        {
            var initialState = (new List<Point>(), new Point(0, 0));

            var x = wire.Aggregate<StraightWire, (List<Point> Visited, Point Current)>(initialState, (a, w) =>
            {
                var p = a.Current;
                for (var i = 0; i < w.Length; i++)
                {
                    p = p.Move(w.Direction);
                    a.Visited.Add(p);
                }

                return (a.Visited, p);
            });

            return x.Visited;
        }

        public int DistanceWhenCrossed(List<StraightWire> line1, List<StraightWire> line2)
        {
            var passedPositions1 = new HashSet<Point>(Positions(line1));
            var passedPositions2 = new HashSet<Point>(Positions(line2));

            var intersections = new HashSet<Point>(passedPositions1);

            intersections.IntersectWith(passedPositions2);
            var start = new Point(0,0);
            return intersections.Min(x => x.ManhattanDistance(start));
        }

        public int FewestSteps(List<StraightWire> line1, List<StraightWire> line2)
        {
            var position1 = new Point(0, 0);
            var position2 = new Point(0, 0);

            var passedPositions1 = new HashSet<Point>();
            var passedPositions2 = new HashSet<Point>();

            int counter1 = 0;
            var steps1 = new Dictionary<Point, int>();
            foreach (var x in line1)
            {
                for (var i = 0; i < x.Length; i++)
                {
                    position1 = position1.Move(x.Direction);
                    passedPositions1.Add(position1);
                    counter1++;
                    if (!steps1.ContainsKey(position1))
                    {
                        steps1.Add(position1, counter1);
                    }
                }
            }

            int counter2 = 0;
            var steps2 = new Dictionary<Point, int>();
            foreach (var x in line2)
            {
                for (var i = 0; i < x.Length; i++)
                {
                    position2 = position2.Move(x.Direction);
                    passedPositions2.Add(position2);

                    counter2++;
                    if (!steps2.ContainsKey(position2))
                    {
                        steps2.Add(position2, counter2);
                    }
                }
            }

            var intersections = new HashSet<Point>(passedPositions1);

            intersections.IntersectWith(passedPositions2);
            var start = new Point(0, 0);
            return intersections.Min(x => steps1[x] + steps2[x]);
        }


        public int FirstStar()
        {
            var input = ReadLineInput();
            var (line1, line2) = Parse(input);
            return DistanceWhenCrossed(line1, line2);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            var (line1, line2) = Parse(input);
            return FewestSteps(line1, line2);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(860, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(9238, SecondStar());
        }

        [Fact]
        public void FirstStarExample0()
        {
            var (line1, line2) = Parse(new List<string>
            {
                "R8,U5,L5,D3",
                "U7,R6,D4,L4"
            });
            var distance = DistanceWhenCrossed(line1, line2);

            Assert.Equal(6, distance);
        }

        [Fact]
        public void FirstStarExample1()
        {
            var (line1, line2) = Parse(new List<string>
            {
                "R75,D30,R83,U83,L12,D49,R71,U7,L72",
                "U62,R66,U55,R34,D71,R55,D58,R83"
            });
            var distance = DistanceWhenCrossed(line1, line2);

            Assert.Equal(159, distance);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var (line1, line2) = Parse(new List<string>
            {
                "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
                "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"
            });
            var distance = DistanceWhenCrossed(line1, line2);

            Assert.Equal(135, distance);
        }

        [Fact]
        public void SecondStarExample0()
        {
            var (line1, line2) = Parse(new List<string>
            {
                "R8,U5,L5,D3",
                "U7,R6,D4,L4"
            });
            var distance = FewestSteps(line1, line2);

            Assert.Equal(30, distance);
        }
    }
}
