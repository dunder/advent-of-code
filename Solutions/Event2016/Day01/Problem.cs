using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.Extensions;
using Shared.MapGeometry;

namespace Solutions.Event2016.Day01 {
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day01;

        public override string FirstStar()
        {
            var input = ReadInput();
            var result = TaxiMap.ShortestPath(new Point(0, 0), Direction.North, input);
            return result.ToString();
        }

        public override string SecondStar() {
            var input = ReadInput();
            var result = TaxiMap.DistanceToFirstIntersection(new Point(0, 0), Direction.North, input);
            return result.ToString();
        }
    }

    public class TaxiMap {

        public static int ShortestPath(Point from, Direction facing, string movements) {
            var currentPosition = from;

            foreach ((Turn turn, int length) in movements.SplitOnCommaSpaceSeparated().Select(ToMovement)) {
                facing = facing.Turn(turn);
                currentPosition = facing.Move(currentPosition, length);
            }

            return Distance(from, currentPosition);
        }


        public static int DistanceToFirstIntersection(Point from, Direction facing, string movements) {
            var currentPosition = from;
            var visitedPoints = new HashSet<Point>();

            foreach ((Turn turn, int length) in movements.SplitOnCommaSpaceSeparated().Select(ToMovement)) {
                facing = facing.Turn(turn);

                for (var i = 0; i < length; i++) {
                    currentPosition = facing.Move(currentPosition, 1);
                    if (visitedPoints.Contains(currentPosition)) {
                        return Distance(from, currentPosition);
                    }
                    visitedPoints.Add(currentPosition);
                }
            }

            return Distance(from, currentPosition);
        }

        private static (Turn, int) ToMovement(string movementDescriptor) {
            var turn = movementDescriptor.Substring(0, 1).TurnFromString();
            var length = int.Parse(movementDescriptor.Substring(1));
            return (turn, length);
        }

        private static int Distance(Point from, Point to) {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }
    }
    public static class DirectionExtensions {

        public static Direction Turn(this Direction direction, Turn turn) {
            switch (direction) {
                case Direction.North:
                    return turn == Shared.MapGeometry.Turn.Right ? Direction.West : Direction.East;
                case Direction.East:
                    return turn == Shared.MapGeometry.Turn.Right ? Direction.North : Direction.South;
                case Direction.South:
                    return turn == Shared.MapGeometry.Turn.Right ? Direction.East : Direction.West;
                case Direction.West:
                    return turn == Shared.MapGeometry.Turn.Right ? Direction.South : Direction.North;
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

}
