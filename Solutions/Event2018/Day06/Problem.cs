using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.MapGeometry;

namespace Solutions.Event2018.Day06
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day06;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = LargestArea(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = LargestRegion(input, 10_000);
            return result.ToString();
        }

        public static List<Point> ParseCoordinates(IList<string> input)
        {
            var coordinates = new List<Point>();

            for (var i = 0; i < input.Count; i++)
            {
                var value = input[i];
                var split = value.Split(", ");
                var x = int.Parse(split[0]);
                var y = int.Parse(split[1]);

                coordinates.Add(new Point(x,y));
            }
            return coordinates;
        }

        public static int LargestArea(IList<string> input)
        {
            var coordinates = ParseCoordinates(input);

            var coordinatesExpandingToInfinity = ExpandingToInfinity(coordinates);
            var coordinatesNotExpandingToInfinity = coordinates
                .Where(c => !coordinatesExpandingToInfinity.Contains(c))
                .ToList();

            var areas = new Dictionary<Point, int>();

            foreach (var coordinate in coordinatesNotExpandingToInfinity)
            {
                var radius = 1;
                var area = 1;
                while (true)
                {
                    var pointsAtRadius = PointsAtRadius(coordinate, radius);

                    var pointsWithMinDistance = pointsAtRadius.Where(p => HasMinDistance(coordinate, p, coordinates)).ToList();
                    area += pointsWithMinDistance.Count;
                    if (pointsWithMinDistance.Count == 0)
                    {
                        areas.Add(coordinate, area);
                        break;
                    }

                    radius++;
                }
            }

            return areas.Values.Max();
        }

        public static int LargestRegion(IList<string> input, int maxDistance)
        {
            var coordinates = ParseCoordinates(input);

            var minX = coordinates.Min(c => c.X);
            var maxX = coordinates.Max(c => c.X);
            var minY = coordinates.Min(c => c.Y);
            var maxY = coordinates.Max(c => c.Y);

            var pointInRegion = new HashSet<Point>();

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    var point = new Point(x, y);

                    var totalDistance = coordinates.Sum(c => c.ManhattanDistance(point));
                    if (totalDistance < maxDistance)
                    {
                        pointInRegion.Add(point);
                    }
                }
            }

            return pointInRegion.Count;
        }

        public static HashSet<Point> ExpandingToInfinity(List<Point> coordinates)
        {
            var expandingToInfinity = new HashSet<Point>();

            foreach (var coordinate in coordinates)
            {
                var others = coordinates.Where(c => c != coordinate).ToList();

                var rightOf = others.Where(o => o.X > coordinate.X).ToList();
                if (!rightOf.Any())
                {
                    expandingToInfinity.Add(coordinate);
                    continue;
                }

                var leftOf = others.Where(o => o.X < coordinate.X).ToList();
                if (!leftOf.Any())
                {
                    expandingToInfinity.Add(coordinate);
                    continue;
                }

                var above = others.Where(o => o.Y < coordinate.Y).ToList();
                if (!above.Any())
                {
                    expandingToInfinity.Add(coordinate);
                    continue;
                }

                var below = others.Where(o => o.Y > coordinate.Y).ToList();
                if (!below.Any())
                {
                    expandingToInfinity.Add(coordinate);
                    continue;
                }

                var aboveBlock = above.Any(o => Math.Abs(coordinate.Y - o.Y) >= Math.Abs(coordinate.X - o.X));
                var rightBlock = rightOf.Any(o => Math.Abs(coordinate.X - o.X) >= Math.Abs(coordinate.Y - o.Y));
                var leftBlock = leftOf.Any(o => Math.Abs(coordinate.X - o.X) >= Math.Abs(coordinate.Y - o.Y));
                var belowBlock = below.Any(o => Math.Abs(coordinate.Y - o.Y) >= Math.Abs(coordinate.X - o.X));

                if (!aboveBlock || !rightBlock || !leftBlock || !belowBlock)
                {
                    expandingToInfinity.Add(coordinate);
                }
            }

            return expandingToInfinity;
        }

        public static IList<Point> PointsAtRadius(Point point, int radius)
        {
            var points = new List<Point>();

            // top and bottom bottom, fixed y:s iterate x from left to right
            for (int x = point.X - radius; x <= point.X + radius; x++)
            {
                var topY = point.Y - radius;
                var bottomY = point.Y + radius;
                var top = new Point(x, topY);
                var bottom = new Point(x, bottomY);
                points.Add(top);
                points.Add(bottom);
            }

            // left and right columns, fixed x:s iterate y from top to bottom, skip overlaps with top and bottom 
            for (int y = point.Y - radius + 1; y < point.Y + radius; y++)
            {
                var leftX = point.X - radius;
                var rightX = point.X + radius;
                var left = new Point(leftX, y);
                var right = new Point(rightX, y);

                points.Add(left);
                points.Add(right);

            }

            return points;
        }

        public static bool HasMinDistance(Point me, Point point, List<Point> all)
        {
            var sortedByDistance = all.Where(x => x != me).OrderBy(x => x.ManhattanDistance(point)).ToList();
            var minDistance = sortedByDistance.First().ManhattanDistance(point);
            var mine = me.ManhattanDistance(point);
            return mine < minDistance;
        }
    }
}