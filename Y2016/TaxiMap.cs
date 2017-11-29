using System;
using System.Collections.Generic;
using System.Drawing;

namespace Y2016 {
    public class TaxiMap {
        public Point Position { get; private set; }
        public Direction Facing { get; private set; }

        public TaxiMap(Point position, Direction facing) {
            Position = position;
            Facing = facing;
        }

        public int ShortestPath(string movements) {
            var movementsList = movements.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var movement in movementsList) {
                var turn = movement.Substring(0, 1).TurnFromString();
                var length = int.Parse(movement.Substring(1));
                Facing = Facing.Turn(turn);
                Position = Facing.Move(Position, length);
            }
            return Math.Abs(Position.X) + Math.Abs(Position.Y);
        }

        public int DistanceToFirstRoundabout(string movements) {
            var movementsList = movements.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var visitedPoints = new HashSet<Point>();

            foreach (var movement in movementsList) {
                var turn = movement.Substring(0, 1).TurnFromString();
                var length = int.Parse(movement.Substring(1));
                Facing = Facing.Turn(turn);
                Position = Facing.Move(Position, length);
                if (visitedPoints.Contains(Position)) {
                    break;
                }
                visitedPoints.Add(Position);
            }
            return Math.Abs(Position.X) + Math.Abs(Position.Y);
        }
    }

    public enum Direction {
        North,
        East,
        South,
        West
    }

    public enum Turn {
        Left,
        Right
    }

    public static class TurnExtensions {
        public static Turn TurnFromString(this string turn) {
            switch (turn) {
                case "R":
                    return Turn.Right;
                case "L":
                    return Turn.Left;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static class DirectionExtensions {

        public static Direction Turn(this Direction direction, Turn turn) {
            switch (direction) {
                case Direction.North:
                    return turn == Y2016.Turn.Right ? Direction.West : Direction.East;
                case Direction.East:
                    return turn == Y2016.Turn.Right ? Direction.North : Direction.South;
                case Direction.South:
                    return turn == Y2016.Turn.Right ? Direction.East : Direction.West;
                case Direction.West:
                    return turn == Y2016.Turn.Right ? Direction.South : Direction.North;
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