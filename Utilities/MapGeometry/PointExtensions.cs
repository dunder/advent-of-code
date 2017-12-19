using System.Collections.Generic;
using System.Drawing;

namespace Utilities.MapGeometry {
    public static class PointExtensions {

        public static IEnumerable<Point> AdjacentInMainDirections(this Point point) {
            yield return new Point(point.X, point.Y + 1);
            yield return new Point(point.X + 1, point.Y);
            yield return new Point(point.X, point.Y - 1);
            yield return new Point(point.X - 1, point.Y);
        }

        public static IEnumerable<Point> AdjacentInAllDirections(this Point point) {
            yield return new Point(point.X, point.Y + 1);
            yield return new Point(point.X + 1, point.Y + 1);
            yield return new Point(point.X + 1, point.Y);
            yield return new Point(point.X + 1, point.Y - 1);
            yield return new Point(point.X, point.Y - 1);
            yield return new Point(point.X - 1, point.Y - 1);
            yield return new Point(point.X - 1, point.Y);
            yield return new Point(point.X - 1, point.Y + 1);
        }
    }
}
