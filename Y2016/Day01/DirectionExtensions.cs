using System;
using System.Drawing;
using Utilities.Grid;
using Utilities.MapGeometry;

namespace Y2016.Day01 {
    public static class DirectionExtensions {

        public static Direction Turn(this Direction direction, Turn turn) {
            switch (direction) {
                case Direction.North:
                    return turn == Utilities.MapGeometry.Turn.Right ? Direction.West : Direction.East;
                case Direction.East:
                    return turn == Utilities.MapGeometry.Turn.Right ? Direction.North : Direction.South;
                case Direction.South:
                    return turn == Utilities.MapGeometry.Turn.Right ? Direction.East : Direction.West;
                case Direction.West:
                    return turn == Utilities.MapGeometry.Turn.Right ? Direction.South : Direction.North;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Point Move(this Direction direction, Point position, int distance) {
            switch (direction) {
                case Direction.North:
                    return new Point(position.X, position.Y + distance);
                case Direction.East:
                    return new Point(position.X + distance, position.Y);
                case Direction.South:
                    return new Point(position.X, position.Y - distance);
                case Direction.West:
                    return new Point(position.X - distance, position.Y);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}