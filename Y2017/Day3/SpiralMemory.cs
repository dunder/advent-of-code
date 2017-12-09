using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Y2017.Day3 {
    public class SpiralMemory {

        public static int Distance(int address) {
            if (address == 1) {
                return 0;
            }
            int level = 1;
            while (true) {
                var levelSide = 1 + 2*level;
                var lowerRightCorner = (int) Math.Pow(levelSide, 2);
                int levelSize = (int)(lowerRightCorner - Math.Pow(level, 2));

                if (address <= lowerRightCorner) {
                    var midpoints = Enumerable.Range((int)(lowerRightCorner - levelSize), levelSize).Where(x => (x - (levelSide + 1) / 2) % (levelSide - 1) == 0);
                    var distanceToMidpoints = midpoints.Select(x => Math.Abs(x - address));
                    var distanceToNearestMidpoint = distanceToMidpoints.OrderBy(x => x).First();
                    var distance = level + distanceToNearestMidpoint;
                    return distance;
                }
                level++;
            }
        }

        public static int FirstWrittenLargerThan(int value) {

            var values = new Dictionary<Point, int>();

            var currentPoint = new Point(0,0);
            values.Add(currentPoint, 1);

            int level = 1;
            while (true) {
                currentPoint = new Point(currentPoint.X+1, currentPoint.Y);
                values.Add(currentPoint, AdjacentSum(currentPoint, values));
                while (Math.Abs(currentPoint.Y) < level) {
                    currentPoint = new Point(currentPoint.X, currentPoint.Y+1);
                    var adjacentSum = AdjacentSum(currentPoint, values);
                    if (adjacentSum > value) {
                        return adjacentSum;
                    }
                    values.Add(currentPoint, adjacentSum);
                }
                while (currentPoint.X > -level) {
                    currentPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                    var adjacentSum = AdjacentSum(currentPoint, values);
                    if (adjacentSum > value) {
                        return adjacentSum;
                    }
                    values.Add(currentPoint, adjacentSum);
                }
                while (currentPoint.Y > -level) {
                    currentPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                    var adjacentSum = AdjacentSum(currentPoint, values);
                    if (adjacentSum > value) {
                        return adjacentSum;
                    }
                    values.Add(currentPoint, adjacentSum);
                }
                while (currentPoint.X < level) {
                    currentPoint = new Point(currentPoint.X+1, currentPoint.Y);
                    var adjacentSum = AdjacentSum(currentPoint, values);
                    if (adjacentSum > value) {
                        return adjacentSum;
                    }
                    values.Add(currentPoint, adjacentSum);
                }
                level++;
            }
        }

        private static int AdjacentSum(Point point, Dictionary<Point, int> written) {
            var adjacent = Adjacent(point);
            return written.Where(kvp => adjacent.Contains(kvp.Key)).Sum(kvp => kvp.Value);
        }

        private static HashSet<Point> Adjacent(Point point) {
            return new HashSet<Point> {
                new Point(point.X+1, point.Y),
                new Point(point.X+1, point.Y-1),
                new Point(point.X, point.Y-1),
                new Point(point.X-1, point.Y-1),
                new Point(point.X-1, point.Y),
                new Point(point.X-1, point.Y + 1),
                new Point(point.X, point.Y + 1),
                new Point(point.X+1, point.Y + 1)
            };
        }
    }
}