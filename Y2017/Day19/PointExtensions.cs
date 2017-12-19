using System;
using System.Collections.Generic;
using System.Drawing;

namespace Y2017.Day19 {
    public static class PointExtensions {
        public static Point Next(this Point position, Direction direction) {

            switch (direction) {
                case Direction.Up:
                    return new Point(position.X, position.Y - 1);
                case Direction.Right:
                    return new Point(position.X + 1, position.Y);
                case Direction.Down:
                    return new Point(position.X, position.Y + 1);
                case Direction.Left:
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
                case Direction.Up:
                case Direction.Down:

                    var right = position.Next(Direction.Right);
                    if (pipes.TryGetValue(right, out char rightPipe) && (char.IsLetter(rightPipe) || rightPipe == '-')) {
                        return (right, Direction.Right);
                    }

                    var left = position.Next(Direction.Left);
                    if (pipes.TryGetValue(left, out char leftPipe) && (char.IsLetter(leftPipe) || leftPipe == '-')) {
                        return (left, Direction.Left);
                    }
                    return (position, direction);

                case Direction.Left:
                case Direction.Right:

                    var up = position.Next(Direction.Up);
                    if (pipes.TryGetValue(up, out char upPipe) && (char.IsLetter(upPipe) || upPipe == '|')) {
                        return (up, Direction.Up);
                    }
                    var down = position.Next(Direction.Down);
                    if (pipes.TryGetValue(down, out char downPipe) && (char.IsLetter(downPipe) || downPipe == '|')) {
                        return (down, Direction.Down);
                    }
                    return (position, direction);
                default:
                    throw new InvalidOperationException("Invalid direction");
            }
        }
    }
}