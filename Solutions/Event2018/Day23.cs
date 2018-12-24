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

        public struct Point : IEquatable<Point>
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

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
                    return hashCode;
                }
            }


        }

        public static int BotsInRange(IList<string> input)
        {
            var bots = ParseBots(input);

            var strongest = bots.OrderByDescending(r => r.Value).First();

            return bots.Count(b => b.Key.ManhattanDistanceTo(strongest.Key) <= strongest.Value);
        }

        public static int ShortestDistanceToPointInRangeOfMostBots(IList<string> input)
        {
            var bots = ParseBots(input);

            var botsInRangeCount = new Dictionary<Point, int>();

            foreach (var bot in bots)
            {
                var botsInRange = bots.Count(b => b.Key.ManhattanDistanceTo(bot.Key) <= bot.Value);
                botsInRangeCount.Add(bot.Key, botsInRange);
            }

            var ordered = botsInRangeCount.OrderByDescending(b => b.Value).ToDictionary(b => b.Key);

            var botWithMostInRange = ordered.First();
            var locationOfBotWithMostInRange = botWithMostInRange.Key;
            var radiusForBotWithMostInRange = bots[locationOfBotWithMostInRange];

            // not feasible to list all points in a sphere ... Is it possible to find overlapping number of points between each pair?
           
           // try finding the box of overlaps between two spheres?

            foreach (var botInRange in botsInRangeCount)
            {
                var botInRangeLocation = botInRange.Key;
                var botInRangeRadius = bots[botInRangeLocation];

                var (xOverlap, yOverlap, zOverlap) = Overlap(locationOfBotWithMostInRange, radiusForBotWithMostInRange,
                    botInRangeLocation, botInRangeRadius);
            }

            // try to calculate overlap between botWithMostInRange and someBotInRange

            return 0;
        }

        public static (int xOverlap, int yOverlap, int zOverlap) Overlap(Point p1, int r1, Point p2, int r2)
        {
            var b1x = p1.X - r1;
            var b1y = p1.Y - r1;
            var b1z = p1.Z - r1;

            var b1xMax = p1.X + r1;
            var b1yMax = p1.Y + r1;
            var b1zMax = p1.Z + r1;

            var b2x = p2.X - r2;
            var b2y = p2.Y - r2;
            var b2z = p2.Z - r2;

            var b2xMax = p2.X + r2;
            var b2yMax = p2.Y + r2;
            var b2zMax = p2.Z + r2;

            var xOverlap = Math.Max(0, Math.Min(b1xMax, b2xMax) - Math.Max(b1x, b2x));
            var yOverlap = Math.Max(0, Math.Min(b1yMax, b2yMax) - Math.Max(b1y, b2y));
            var zOverlap = Math.Max(0, Math.Min(b1zMax, b2zMax) - Math.Max(b1z, b2z));

            return (xOverlap, yOverlap, zOverlap);
        }

        public static HashSet<Point> AllPointsInRange(Point point, int radius)
        {
            var pointsInRange = new HashSet<Point>();
            for (int x = point.X - radius; x <= point.X + radius; x++)
            {
                for (int y = point.Y - radius; y <= point.Y + radius; y++)
                {
                    for (int z = point.Z - radius; z <= point.Z + radius; z++)
                    {
                        pointsInRange.Add(new Point(x, y, z));
                    }
                }
            }

            return pointsInRange;
        }

        public static Dictionary<Point, int> ParseBots(IList<string> input)
        {
            var bots = new Dictionary<Point, int>();

            var botExpression = new Regex(@"pos=<(\-?\d+),(\-?\d+),(\-?\d+)>, r=(\d+)");

            foreach (var line in input)
            {
                var match = botExpression.Match(line);

                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                var z = int.Parse(match.Groups[3].Value);
                var r = int.Parse(match.Groups[4].Value);

                bots.Add(new Point(x,y,z), r);
            }

            return bots;
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
        public void OverlapTests()
        {
            var p1 = new Point(0,0,0);
            var p2 = new Point(0,0,3);
            var r1 = 2;
            var r2 = 2;

            var overlap = Overlap(p1, r1, p2, r2);


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
            Assert.Equal("", actual);
        }
    }
}