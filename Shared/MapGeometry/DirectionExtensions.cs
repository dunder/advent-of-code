using System;
using System.Drawing;

namespace Shared.MapGeometry
{
    public static class DirectionExtensions
    {

        public static Direction Turn(this Direction direction, Turn turn)
        {
            switch (direction)
            {
                case Direction.North:
                    return turn == MapGeometry.Turn.Right ? Direction.East : Direction.West;
                case Direction.East:
                    return turn == MapGeometry.Turn.Right ? Direction.South : Direction.North;
                case Direction.South:
                    return turn == MapGeometry.Turn.Right ? Direction.West : Direction.East;
                case Direction.West:
                    return turn == MapGeometry.Turn.Right ? Direction.North : Direction.South;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Point Move(this Direction direction, Point position, int distance)
        {
            switch (direction)
            {
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
