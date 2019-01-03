using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day23 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day23;
        public string Name => "Experimental Emergency Teleportation";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = BotsInRange(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = ShortestDistanceToPointInRangeOfMostBots(input);
            return result.ToString();
        }

        public static List<Bot> ParseBots(IList<string> input)
        {
            var bots = new List<Bot>();

            var botExpression = new Regex(@"pos=<(\-?\d+),(\-?\d+),(\-?\d+)>, r=(\d+)");

            foreach (var line in input)
            {
                var match = botExpression.Match(line);

                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                var z = int.Parse(match.Groups[3].Value);
                var r = int.Parse(match.Groups[4].Value);

                bots.Add(new Bot(new Point(x, y, z), r));
            }

            return bots;
        }

        public class Bot
        {
            public Point Position { get; }
            public int SignalRadius { get; }

            public Bot(Point position, int signalRadius)
            {
                Position = position;
                SignalRadius = signalRadius;
            }

            public bool IsInRange(Box box)
            {
                var c = box.BottomFrontLeftCorner;
                var w = box.Width;

                var maxX = c.X + w;
                var minX = c.X;
                var maxY = c.Y + w;
                var minY = c.Y;
                var maxZ = c.Z + w;
                var minZ = c.Z;

                // inside box
                if (Position.X <= maxX && Position.X >= minX && 
                    Position.Y <= maxY && Position.Y >= minY &&
                    Position.Z <= maxZ && Position.Z >= minZ)
                {
                    return true;
                }

                // within Y-axis 
                if (Position.Y <= maxY && Position.Y >= minY)
                {
                    // perpendicular to front or back (within X- and Y- bounds)
                    if (Position.X <= maxX && Position.X >= minX)
                    {
                        return Position.Z >= minZ - SignalRadius && Position.Z <= maxZ + SignalRadius;
                    }

                    // perpendicular to left or right (within Y- and Z- bounds)
                    if (Position.Z <= maxZ && Position.Z >= minZ)
                    {
                        return Position.X >= minX - SignalRadius && Position.X <= maxX + SignalRadius;
                    }

                    // the area between front and right
                    if (Position.Z < minZ && Position.X > maxX)
                    {
                        var pointOnLine = new Point(maxX, Position.Y, minZ);
                        return pointOnLine.ManhattanDistanceTo(Position) <= SignalRadius;
                    }

                    // the area between back and right
                    if (Position.Z > maxZ && Position.X > maxX)
                    {
                        var pointOnLine = new Point(maxX, Position.Y, maxZ);
                        return pointOnLine.ManhattanDistanceTo(Position) <= SignalRadius;
                    }

                    // the area between front and left
                    if (Position.Z < minZ && Position.X < minX)
                    {
                        var pointOnLine = new Point(minX, Position.Y, minZ);
                        return pointOnLine.ManhattanDistanceTo(Position) <= SignalRadius;
                    }

                    // the area between back and left
                    if (Position.Z > maxZ && Position.X < minX)
                    {
                        var pointOnLine = new Point(minX, Position.Y, maxZ);
                        return pointOnLine.ManhattanDistanceTo(Position) <= SignalRadius;
                    }
                }

                if (Position.X <= maxX && Position.X >= minX)
                {
                    // perpendicular to top or bottom (within the Z-bounds)
                    if (Position.Z <= maxZ && Position.Z >= minZ)
                    {
                        return Position.Y <= maxY + SignalRadius && Position.Y >= minY - SignalRadius;
                    }

                    // the area between front and top
                    if (Position.Z < minZ && Position.Y > maxY)
                    {
                        var pointOnLine = new Point(Position.X, maxY, minZ);
                        return pointOnLine.ManhattanDistanceTo(Position) <= SignalRadius;
                    }

                    // the area between top and back
                    if (Position.Z > maxZ && Position.Y > maxY)
                    {
                        var pointOnLine = new Point(Position.X, maxY, maxZ);
                        return pointOnLine.ManhattanDistanceTo(Position) <= SignalRadius;
                    }

                    // the area between back and bottom
                    if (Position.Z > maxZ && Position.Y < minY)
                    {
                        var pointOnLine = new Point(Position.X, minY, maxZ);
                        return pointOnLine.ManhattanDistanceTo(Position) <= SignalRadius;
                    }

                    // the area between bottom and front
                    if (Position.Z < minZ && Position.Y < minY)
                    {
                        var pointOnLine = new Point(Position.X, minY, minZ);
                        return pointOnLine.ManhattanDistanceTo(Position) <= SignalRadius;
                    }

                }

                // the rest must have a corner of the box as its closest point
                return box.Corners.Any(corner => corner.ManhattanDistanceTo(Position) <= SignalRadius);
            }

            protected bool Equals(Bot other)
            {
                return Position.Equals(other.Position) && SignalRadius == other.SignalRadius;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Bot) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Position.GetHashCode() * 397) ^ SignalRadius;
                }
            }
        }

        public struct Point : IEquatable<Point>
        {
            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public Point(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public int ManhattanDistanceTo(Point other)
            {
                return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
            }

            public override string ToString()
            {
                return $"({X},{Y},{Z})";
            }

            public bool Equals(Point other)
            {
                return X == other.X && Y == other.Y && Z == other.Z;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Point other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = X;
                    hashCode = (hashCode * 397) ^ Y;
                    hashCode = (hashCode * 397) ^ Z;
                    return hashCode;
                }
            }
        }

        public class Box
        {
            private List<Point> corners;

            public Point BottomFrontLeftCorner { get; }
            public int Width { get; }

            public Box(Point bottomFrontLeftCorner, int width)
            {
                BottomFrontLeftCorner = bottomFrontLeftCorner;
                Width = width;
            }

            public List<Point> Corners => corners ?? (corners = CornersForBox(BottomFrontLeftCorner, Width));

            public List<Box> Split()
            {
                var newWidth = Width / 2;

                return CornersForBox(BottomFrontLeftCorner, newWidth).Select(c => new Box(c, newWidth)).ToList();
            }


            public List<Point> Points
            {
                get
                {
                    var c = BottomFrontLeftCorner;
                    var w = Width;

                    var maxX = c.X + w;
                    var minX = c.X;
                    var maxY = c.Y + w;
                    var minY = c.Y;
                    var maxZ = c.Z + w;
                    var minZ = c.Z;

                    var points = new List<Point>();

                    for (int x = minX; x <= maxX; x++)
                    {
                        for (int y = minY; y <= maxY; y++)
                        {
                            for (int z = minZ; z <= maxZ; z++)
                            {
                                points.Add(new Point(x,y,z));
                            }
                        }
                    }

                    return points;
                }
            }

            // Create the corner points for a cube with a given bottom bottom front left corner and the width
            // T = top,  B = bottom, F = front, B = back, L = left, R = right
            // order of the created corners: BFL, BBL, TBL, TFL, BFR, BBR, TBR, TFR
            private static List<Point> CornersForBox(Point bottomFrontLeft, int width)
            {
                var corners = 
                    from x in new[] { bottomFrontLeft.X, bottomFrontLeft.X + width }
                    from y in new[] { bottomFrontLeft.Y, bottomFrontLeft.Y + width }
                    from z in new[] { bottomFrontLeft.Z, bottomFrontLeft.Z + width }
                    select new Point(x, y, z);
                return corners.ToList();
            }

            public override string ToString()
            {
                return $"Box at {BottomFrontLeftCorner} width {Width}";
            }

            protected bool Equals(Box other)
            {
                return BottomFrontLeftCorner.Equals(other.BottomFrontLeftCorner) && Width == other.Width;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Box) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (BottomFrontLeftCorner.GetHashCode() * 397) ^ Width;
                }
            }
        }

        public static int BotsInRange(IList<string> input)
        {
            var bots = ParseBots(input);

            var strongest = bots.OrderByDescending(r => r.SignalRadius).First();

            return bots.Count(b => b.Position.ManhattanDistanceTo(strongest.Position) <= strongest.SignalRadius);
        }

        public static int ShortestDistanceToPointInRangeOfMostBots(IList<string> input)
        {
            var bots = ParseBots(input);

            var me = new Point(0, 0, 0);

            var maxX = bots.Max(b => b.Position.X);
            var maxY = bots.Max(b => b.Position.Y);
            var maxZ = bots.Max(b => b.Position.Z);

            var minX = bots.Min(b => b.Position.X);
            var minY = bots.Min(b => b.Position.Y);
            var minZ = bots.Min(b => b.Position.Z);

            var totalMax = new[] {maxX, maxY, maxZ, Math.Abs(minX), Math.Abs(minY), Math.Abs(minZ)}.Max();

            int i = 1;
            while (Math.Pow(2, i) < totalMax)
            {
                i++;
            }

            totalMax = (int) Math.Pow(2, i);

            var width = totalMax;
            var boxes = new List<Box>
            {
                new Box(new Point(-width, -width, -width), width * 2)
            };

            while (boxes.First().Width > 2)
            {
                boxes = boxes.SelectMany(b => b.Split()).ToList();

                var inRangeCount = boxes
                    .Select(b => new
                    {
                        Box = b, InRangeCount = bots.Count(bot => bot.IsInRange(b))
                    })
                    .ToDictionary(x => x.Box, x => x.InRangeCount);

                var maxInRange = inRangeCount.Max(x => x.Value);

                boxes = inRangeCount.Where(x => x.Value == maxInRange).Select(x => x.Key).ToList();
            }

            var bestPoint = boxes
                .SelectMany(b => b.Points)
                .OrderByDescending(p => bots.Count(b => b.Position.ManhattanDistanceTo(p) <= b.SignalRadius))
                .ThenBy(p => p.ManhattanDistanceTo(me))
                .First();

            return bestPoint.ManhattanDistanceTo(me);
        }                

        [Fact]
        public void FirstStarExample()
        {
            var input = new[]
            {
                "pos=<0,0,0>, r=4",
                "pos=<1,0,0>, r=1",
                "pos=<4,0,0>, r=3",
                "pos=<0,2,0>, r=1",
                "pos=<0,5,0>, r=3",
                "pos=<0,0,3>, r=1",
                "pos=<1,1,1>, r=1",
                "pos=<1,1,2>, r=1",
                "pos=<1,3,1>, r=1",
            };

            var botsInRange = BotsInRange(input);

            Assert.Equal(7, botsInRange);
        }

        [Fact]
        public void SplitExample()
        {
            var box = new Box(new Point(-2, -2, -2), 4);

            var boxesWhenSplit = box.Split();

            Assert.All(boxesWhenSplit, b => Assert.Equal(2, b.Width));
            Assert.Collection(boxesWhenSplit, 
                b => Assert.Equal(new Point(-2, -2, -2), b.BottomFrontLeftCorner),
                b => Assert.Equal(new Point(-2, -2, 0), b.BottomFrontLeftCorner),
                b => Assert.Equal(new Point(-2, 0, -2), b.BottomFrontLeftCorner),
                b => Assert.Equal(new Point(-2, 0, 0), b.BottomFrontLeftCorner),
                b => Assert.Equal(new Point(0, -2, -2), b.BottomFrontLeftCorner),
                b => Assert.Equal(new Point(0, -2, 0), b.BottomFrontLeftCorner),
                b => Assert.Equal(new Point(0, 0, -2), b.BottomFrontLeftCorner),
                b => Assert.Equal(new Point(0, 0, 0), b.BottomFrontLeftCorner));
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new[]
            {
                "pos=<10,12,12>, r=2",
                "pos=<12,14,12>, r=2",
                "pos=<16,12,12>, r=4",
                "pos=<14,14,14>, r=6",
                "pos=<50,50,50>, r=200",
                "pos=<10,10,10>, r=5"
            };

            var botsInRange = ShortestDistanceToPointInRangeOfMostBots(input);

            Assert.Equal(36, botsInRange);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("270", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("106323091", actual);
        }
    }
}