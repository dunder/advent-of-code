using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Y2016 {
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
}