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

            public static Point operator *(Point a, int b)
            {
                return new Point(a.X * b, a.Y * b, a.Z * b);
            }

            public static Point operator *(Point a, Point b)
            {
                return new Point(a.X*b.X, a.Y*b.Y, a.Z*b.Z);
            }

            public static Point operator +(Point a, int b)
            {
                return new Point(a.X + b, a.Y + b, a.Z + b);
            }
            public static Point operator +(Point a, Point b)
            {
                return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            }

            public static Point operator -(Point a, Point b)
            {
                return new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
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
            // create a bounding box

            var bots = ParseBots(input);
            var me = new Point(0, 0, 0);

            var maxX = bots.Keys.Max(b => b.X);
            var maxY = bots.Keys.Max(b => b.Y);
            var maxZ = bots.Keys.Max(b => b.Z);

            var minX = bots.Keys.Min(b => b.X);
            var minY = bots.Keys.Min(b => b.Y);
            var minZ = bots.Keys.Min(b => b.Z);

            var totalMax = new[] {maxX, maxY, maxZ, Math.Abs(minX), Math.Abs(minY), Math.Abs(minZ)}.Max();

            if (totalMax % 2 != 0)
            {
                totalMax = totalMax + 1;
            }

            var binDepths = new[] {0};
            var binParents = new[] {0};
            var binCorners = new List<List<Point>> {Corners(totalMax, totalMax)};

            var pointBins = bots.Keys.ToDictionary(b => b, b => 1);

            Split(0, binDepths, binParents, binCorners, pointBins);

            return 0;
        }

        public static void Split(int binNo, IList<int> binDepths, IList<int> binParents, List<List<Point>> binCorners, Dictionary<Point, int> pointBins)
        {
            //% If this bin meets any exit conditions, do not divide it any further.
            //    binPointCount = nnz(pointBins == binNo)
            //binEdgeLengths = binCorners(binNo, 1:3) - binCorners(binNo, 4:6)

            var binEdgeLengths = BoxEdgeLength(binCorners[binNo]);
            

            //binDepth = binDepths(binNo)
            var binDepth = binDepths[0];

            //exitConditionsMet = binPointCount < value || min(binEdgeLengths) < value || binDepth > value
            //if exitConditionsMet
            //return; % Exit recursive function
            //end

            //    % Otherwise, split this bin into 8 new sub- bins with a new division point
            //    newDiv = (binCorners(binNo, 1:3) +binCorners(binNo, 4:6)) / 2
            //for i = 1:8
            for (int i = 1; i <= 8; i++)
            {
                //newBinNo = length(binDepths) + 1

                var newBinNo = binDepths.Count + 1;

                //binDepths(newBinNo) = binDepths(binNo) + 1
                
                binDepths.Add(binDepth + 1);
                //binParents(newBinNo) = binNo
                binParents.Add(binNo);
                //binCorners(newBinNo) = [one of the 8 pairs of the newDiv with minCorner or maxCorner]
                
                //oldBinMask = pointBins == binNo
                //             % Calculate which points in pointBins == binNo now belong in newBinNo
                //pointBins(newBinMask) = newBinNo
                //                        % Recursively divide this newly created bin
                //divide(newBinNo)
                Split(newBinNo, binDepths, binParents, binCorners, pointBins);
            }
        }

        public static List<Point> NewBox(int i, IList<Point> corners)
        {
            switch (i)
            {
                case 1:
                    break;
            }

            return null;
        }

        public static int BoxEdgeLength(IList<Point> corners)
        {
            return corners[1].Z - corners[0].Z;
        }

        // T = top,  B = bottom, F = front, B = back, L = left, R = right
        // order of corners: BFL, BBL, TBL, TFL, BFR, BBR, TBR, TFR
        public static List<Point> Corners(int min ,int max)
        {
            var corners = from x in new[] { -min, max }
                from y in new[] { -min, max }
                from z in new[] { -min, max }
                select new Point(x, y, z);
            return corners.ToList();
        }

        public static bool IsBoxInRange(Point point, int signalRadius, IList<Point> corners)
        {
            return corners.Any(p => point.ManhattanDistanceTo(p) <= signalRadius);
        }

        public static List<Point> InRangeForDirection(Point point, int signalRadius, Point direction)
        {
            var xProjection = new Point(point.X, point.X, point.X) * direction;
            var yProjection = new Point(point.Y, point.Y, point.Y) * direction;
            var zProjection = new Point(point.Z, point.Z, point.Z) * direction;

            IEnumerable<Point> projections = new List<Point> {xProjection, yProjection, zProjection};
            projections = projections.OrderBy(p => p.X + p.Y + p.Y);

            var from = projections.First();
            var to = projections.Last();

            var distanceToLine = point.ManhattanDistanceTo(from);

            if (distanceToLine > signalRadius)
            {
                return Enumerable.Empty<Point>().ToList();
            }

            var diff = signalRadius - distanceToLine;

            var missingPoints = diff / 2;

            from = from - direction * missingPoints;
            to = to + direction * missingPoints;

            //var onLine = from;
            //points.Add(onLine);
            //while (onLine != to)
            //{
            //    onLine = onLine + direction;
            //    points.Add(onLine);
            //}

            return new List<Point> {from,to};
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

        [Theory]
        [InlineData(4, 4, 0, 2, 1, 0, 0, 0)]
        [InlineData(4,4,4,2,1,1,1,2)]
        [InlineData(73918241,31307474,5543451,86899473,1,1,1,0)]
        public void InRangeForDirectionTests(int x, int y, int z, int radius, int dx, int dy, int dz, int expected)
        {
            var point = new Point(x, y, z);
            var direction = new Point(dx, dy, dz);

            var inRangeOnLine = InRangeForDirection(point, radius, direction);

            Assert.Equal(expected, inRangeOnLine.Count);
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
            Assert.Equal("", actual); // 57053824 too low, 106323090 too low
        }
    }
}