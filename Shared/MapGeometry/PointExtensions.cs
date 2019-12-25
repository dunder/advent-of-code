using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shared.MapGeometry
{
    public static class PointExtensions
    {
        public static IEnumerable<Point> AdjacentInMainDirections(this Point point)
        {
            yield return new Point(point.X, point.Y + 1);
            yield return new Point(point.X + 1, point.Y);
            yield return new Point(point.X, point.Y - 1);
            yield return new Point(point.X - 1, point.Y);
        }

        public static IEnumerable<Point> AdjacentInAllDirections(this Point point)
        {
            yield return new Point(point.X, point.Y + 1);
            yield return new Point(point.X + 1, point.Y + 1);
            yield return new Point(point.X + 1, point.Y);
            yield return new Point(point.X + 1, point.Y - 1);
            yield return new Point(point.X, point.Y - 1);
            yield return new Point(point.X - 1, point.Y - 1);
            yield return new Point(point.X - 1, point.Y);
            yield return new Point(point.X - 1, point.Y + 1);
        }

        public static Point Move(this Point point, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Point(point.X, point.Y - 1);
                case Direction.NorthEast:
                    return new Point(point.X + 1, point.Y - 1);
                case Direction.East:
                    return new Point(point.X + 1, point.Y);
                case Direction.SouthEast:
                    return new Point(point.X + 1, point.Y + 1);
                case Direction.South:
                    return new Point(point.X, point.Y + 1);
                case Direction.SouthWest:
                    return new Point(point.X - 1, point.Y + 1);
                case Direction.West:
                    return new Point(point.X - 1, point.Y);
                case Direction.NorthWest:
                    return new Point(point.X - 1, point.Y - 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction),
                        $"Cannot move in this direction: {direction}");
            }
        }

        public static IEnumerable<Point> Line(this Point point, Direction direction, int length)
        {
            var current = point;

            for (int i = 0; i <= length; i++)
            {
                yield return current;
                current = current.Move(direction);
            }
        }

        public static int ManhattanDistance(this Point point, Point toPoint)
        {
            return Math.Abs(point.X - toPoint.X) + Math.Abs(point.Y - toPoint.Y);
        }
    }
}