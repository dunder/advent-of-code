using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities;

namespace Y2017.Day11 {
    public class HexagonPath {
        public static int DistanceToDestination(string movements) {

            var destination = Destination(movements);

            return Distance(destination);
        }

        /// <summary>
        /// Distance in number of steps from Point(0,0) to another Point in a hex grid
        /// </summary>
        /// <param name="destination">The destination Point</param>
        /// <returns>Number of steps to destination from Point(0,0)</returns>
        public static int Distance(Point destination) {
            return (Math.Abs(destination.Y) - Math.Abs(destination.X)) / 2 + Math.Abs(destination.X);
        }

        /// <summary>
        /// The Point furthest away from Point(0,0) given a series of movements
        /// </summary>
        /// <param name="movements">A comma separated string of directions (any of "n", "ne", "se", "s", "sw", "nw")</param>
        /// <returns></returns>
        public static int Furthest(string movements) {
            var all = new HashSet<Point>();
            string[] directions = movements.SplitOnCommaSpaceSeparated();
            var point = new Point(0, 0);
            all.Add(point);
            foreach (var direction in directions) {
                point = Move(direction, point);
                all.Add(point);
            }
            return all.Max(p => (Math.Abs((int) p.Y) - Math.Abs((int) p.X)) / 2 + Math.Abs((int) p.X));
        }

        /// <summary>
        /// Destination Point given the movements in a hex grid from Point(0,0)
        /// </summary>
        /// <param name="movements">A comma separated list of directions (any of "n", "ne", "se", "s", "sw", "nw")</param>
        /// <returns>The destination Point</returns>
        public static Point Destination(string movements) {
            string[] directions = movements.SplitOnCommaSpaceSeparated();
            var point = new Point(0, 0);

            foreach (var direction in directions) {
                point = Move(direction, point);
            }

            return point;
        }

        /// <summary>
        /// Move one step in a direction in a hex grid
        /// </summary>
        /// <param name="direction">A direction to move (any of "n", "ne", "se", "s", "sw", "nw")</param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static Point Move(string direction, Point point) {
            switch (direction) {
                case "n":
                    point = new Point(point.X, point.Y + 2);
                    break;
                case "ne":
                    point = new Point(point.X + 1, point.Y + 1);
                    break;
                case "se":
                    point = new Point(point.X + 1, point.Y - 1);
                    break;
                case "s":
                    point = new Point(point.X, point.Y - 2);
                    break;
                case "sw":
                    point = new Point(point.X - 1, point.Y - 1);
                    break;
                case "nw":
                    point = new Point(point.X - 1, point.Y + 1);
                    break;
            }
            return point;
        }
    }
}