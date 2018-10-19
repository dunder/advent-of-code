using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Shared.MapGeometry;

namespace Solutions.Event2017.Day19
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day19;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = RoutingDiagram.LettersInRoute(input);
            return result;
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = RoutingDiagram.CountSteps(input);
            return result.ToString();
        }
    }

    public class RoutingDiagram
    {
        public static string LettersInRoute(IList<string> input)
        {
            StringBuilder collectedLetters = new StringBuilder();
            FollowRoutingDiagram(input,
                c =>
                {
                    if (char.IsLetter(c))
                    {
                        collectedLetters.Append(c);
                    }
                });
            return collectedLetters.ToString();
        }

        public static long CountSteps(IList<string> input)
        {
            int steps = 0;
            FollowRoutingDiagram(input, c => steps++);
            return steps;
        }

        public static void FollowRoutingDiagram(IList<string> input, Action<char> onStep)
        {
            var tubes = new Dictionary<Point, char>();
            Point startPosition = new Point();

            for (var row = 0; row < input.Count; row++)
            {
                var line = input[row];

                for (var column = 0; column < line.Length; column++)
                {
                    if (row == 0 && line[column] == '|')
                    {
                        startPosition = new Point(column, row);
                    }

                    char c = line[column];
                    tubes.Add(new Point(column, row), c);
                }
            }

            Point currentPosition = startPosition;
            char currentTube = '|';
            Direction currentDirection = Direction.South;

            while (true)
            {
                onStep(currentTube);

                if (currentTube != '+')
                {
                    currentPosition = currentPosition.Next(currentDirection);
                }
                else
                {
                    (currentPosition, currentDirection) = currentPosition.NextTurn(currentDirection, tubes);
                }

                if (!tubes.ContainsKey(currentPosition) || tubes[currentPosition] == ' ')
                {
                    break;
                }

                currentTube = tubes[currentPosition];
            }
        }

        public static string Print(Dictionary<Point, char> tubes, Point currentPosition)
        {
            StringBuilder display = new StringBuilder();
            for (int y = currentPosition.Y - 10; y < currentPosition.Y + 11; y++)
            {
                for (int x = currentPosition.X - 10; x < currentPosition.X + 11; x++)
                {
                    var displayPoint = new Point(x, y);
                    if (tubes.ContainsKey(displayPoint))
                    {
                        display.Append(tubes[displayPoint]);
                    }
                    else
                    {
                        display.Append('/');
                    }
                }

                display.AppendLine();
            }

            return display.ToString();
        }
    }

    public static class PointExtensions
    {
        public static Point Next(this Point position, Direction direction)
        {
            switch (direction)
            {
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

        public static IEnumerable<Point> NextInAllDirections(this Point position)
        {
            yield return new Point(position.X, position.Y - 1);
            yield return new Point(position.X + 1, position.Y);
            yield return new Point(position.X, position.Y - 1);
            yield return new Point(position.X - 1, position.Y);
        }


        public static (Point newPosition, Direction newDirection) NextTurn(this Point position, Direction direction,
            Dictionary<Point, char> pipes)
        {
            switch (direction)
            {
                case Direction.North:
                case Direction.South:

                    var right = position.Next(Direction.East);
                    if (pipes.TryGetValue(right, out char rightPipe) && (char.IsLetter(rightPipe) || rightPipe == '-'))
                    {
                        return (right, Direction.East);
                    }

                    var left = position.Next(Direction.West);
                    if (pipes.TryGetValue(left, out char leftPipe) && (char.IsLetter(leftPipe) || leftPipe == '-'))
                    {
                        return (left, Direction.West);
                    }

                    return (position, direction);

                case Direction.West:
                case Direction.East:

                    var up = position.Next(Direction.North);
                    if (pipes.TryGetValue(up, out char upPipe) && (char.IsLetter(upPipe) || upPipe == '|'))
                    {
                        return (up, Direction.North);
                    }

                    var down = position.Next(Direction.South);
                    if (pipes.TryGetValue(down, out char downPipe) && (char.IsLetter(downPipe) || downPipe == '|'))
                    {
                        return (down, Direction.South);
                    }

                    return (position, direction);
                default:
                    throw new InvalidOperationException("Invalid direction");
            }
        }
    }
}