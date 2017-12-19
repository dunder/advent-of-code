using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.MapGeometry;

namespace Y2017.Day19 {
    public static class PointExtensions {
        public static Point Next(this Point position, Direction direction) {

            switch (direction) {
                case Direction.North:
                    return new Point(position.X, position.Y - 1);
                case Direction.East:
                    return new Point(position.X + 1, position.Y);
                case Direction.South:
                    return new Point(position.X, position.Y + 1);
                case Direction.West:
                    return new Point(position.X - 1, position.Y);
                default:
                    throw new InvalidOperationException($"Unknown direction: {direction}");
            }
        }

        public static IEnumerable<Point> NextInAllDirections(this Point position) {
            yield return new Point(position.X, position.Y - 1);
            yield return new Point(position.X + 1, position.Y);
            yield return new Point(position.X, position.Y - 1);
            yield return new Point(position.X - 1, position.Y);
        }


        public static (Point newPosition, Direction newDirection) NextTurn(this Point position, Direction direction, Dictionary<Point, char> pipes) {

            switch (direction) {
                case Direction.North:
                case Direction.South:

                    var right = position.Next(Direction.East);
                    if (pipes.TryGetValue(right, out char rightPipe) && (char.IsLetter(rightPipe) || rightPipe == '-')) {
                        return (right, Direction.East);
                    }

                    var left = position.Next(Direction.West);
                    if (pipes.TryGetValue(left, out char leftPipe) && (char.IsLetter(leftPipe) || leftPipe == '-')) {
                        return (left, Direction.West);
                    }
                    return (position, direction);

                case Direction.West:
                case Direction.East:

                    var up = position.Next(Direction.North);
                    if (pipes.TryGetValue(up, out char upPipe) && (char.IsLetter(upPipe) || upPipe == '|')) {
                        return (up, Direction.North);
                    }
                    var down = position.Next(Direction.South);
                    if (pipes.TryGetValue(down, out char downPipe) && (char.IsLetter(downPipe) || downPipe == '|')) {
                        return (down, Direction.South);
                    }
                    return (position, direction);
                default:
                    throw new InvalidOperationException("Invalid direction");
            }
        }
    }
}