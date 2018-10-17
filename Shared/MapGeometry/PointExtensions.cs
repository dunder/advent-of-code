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
                case Direction.East:
                    return new Point(point.X + 1, point.Y);
                case Direction.South:
                    return new Point(point.X, point.Y + 1);
                case Direction.West:
                    return new Point(point.X - 1, point.Y);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction),
                        $"Cannot move in this direction: {direction}");
            }
        }
    }
}