using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Utilities.MapGeometry;

namespace Y2017.Day19 {
    public class RoutingDiagram {

        public static string LettersInRoute(string[] input) {
            StringBuilder collectedLetters = new StringBuilder();
            FollowRoutingDiagram(input,
                c => {
                    if (char.IsLetter(c)) {
                        collectedLetters.Append(c);
                    }
                });
            return collectedLetters.ToString();
        }

        public static long CountSteps(string[] input) {
            int steps = 0;
            FollowRoutingDiagram(input, c => steps++);
            return steps;
        }

        public static void FollowRoutingDiagram(string[] input, Action<char> onStep) {
            var tubes = new Dictionary<Point, char>();
            Point startPosition = new Point();

            for (var row = 0; row < input.Length; row++) {
                var line = input[row];

                for (var column = 0; column < line.Length; column++) {
                    if (row == 0 && line[column] == '|') {
                        startPosition = new Point(column, row);
                    }
                    char c = line[column];
                    tubes.Add(new Point(column, row), c);
                }
            }

            Point currentPosition = startPosition;
            char currentTube = '|';
            Direction currentDirection = Direction.South;

            while (true) {

                onStep(currentTube);

                if (currentTube != '+') {
                    currentPosition = currentPosition.Next(currentDirection);
                } else {
                    (currentPosition, currentDirection) = currentPosition.NextTurn(currentDirection, tubes);
                }

                if (!tubes.ContainsKey(currentPosition) || tubes[currentPosition] == ' ') {
                    break;
                }

                currentTube = tubes[currentPosition];
            }
        }

        public static string Print(Dictionary<Point, char> tubes, Point currentPosition) {
            StringBuilder display = new StringBuilder();
            for (int y = currentPosition.Y - 10; y < currentPosition.Y + 11; y++) {

                for (int x = currentPosition.X - 10; x < currentPosition.X + 11; x++) {
                    var displayPoint = new Point(x, y);
                    if (tubes.ContainsKey(displayPoint)) {
                        display.Append(tubes[displayPoint]);
                    }
                    else {
                        display.Append('/');
                    }
                }
                display.AppendLine();
            }
            return display.ToString();
        }
    }
}