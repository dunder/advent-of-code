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

        public static List<Point> Positions(List<StraightWire> segments)
        {
            var initialState = new List<Point> {new Point(0,0)};

            var finalState = segments.Aggregate(initialState, (state, segment) =>
                {
                    var newPoints = state.Last().Line(segment.Direction, segment.Length);
                    state.AddRange(newPoints.Skip(1));
                    return state;
                });

            return finalState.Skip(1).ToList();
        }

        public int DistanceWhenCrossed(List<StraightWire> wire1, List<StraightWire> wire2)
        {
            var wire1Points = Positions(wire1).ToHashSet();
            var wire2Points = Positions(wire2).ToHashSet();

            var start = new Point(0,0);

            return wire1Points.Intersect(wire2Points).Min(x => x.ManhattanDistance(start));
        }

        public int FewestSteps(List<StraightWire> wire1Segments, List<StraightWire> wire2Segments)
        {
            int counter1 = 0;
            var steps1 = new Dictionary<Point, int>();
            var wire1Points = Positions(wire1Segments);
            foreach (var p in wire1Points)
            {
                counter1++;
                if (!steps1.ContainsKey(p))
                {
                    steps1.Add(p, counter1);
                }
            }

            int counter2 = 0;
            var steps2 = new Dictionary<Point, int>();
            var wire2Points = Positions(wire2Segments);
            foreach (var p in wire2Points)
            {
                counter2++;
                if (!steps2.ContainsKey(p))
                {
                    steps2.Add(p, counter2);
                }
            }

            return wire1Points.Intersect(wire2Points).Min(x => steps1[x] + steps2[x]);
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
