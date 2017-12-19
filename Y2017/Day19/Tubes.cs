using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Y2017.Day19 {
    public class Tubes {
        public static string LettersOnWayOut(string[] input) {

            var pipes = new Dictionary<Point, char>();

            Point startPosition = new Point();
            
            for (var row = 0; row < input.Length; row++) {
                var line = input[row];

                for (var column = 0; column < line.Length; column++) {
                    if (row == 0 && line[column] == '|') {
                        startPosition = new Point(column, row);
                    }
                    char c = line[column];
                    pipes.Add(new Point(column, row), c);
                }
            }

            Point currentPosition = startPosition;
            char currentPipe = '|';
            Direction currentDirection = Direction.Down;

            List<char> collectedLetters = new List<char>();
            while (true) {
                if (char.IsLetter(currentPipe)) {
                    collectedLetters.Add(currentPipe);
                }
                Point previousPosition;
                if (currentPipe != '+') {
                    previousPosition = currentPosition;
                    currentPosition = currentPosition.Next(currentDirection);

                }
                else {
                    previousPosition = currentPosition;
                    (currentPosition, currentDirection) = currentPosition.NextTurn(currentDirection, pipes);
                }

                if (previousPosition == currentPosition || !pipes.ContainsKey(currentPosition)) {
                    break;
                }

                currentPipe = pipes[currentPosition];
            }

            return new string(collectedLetters.ToArray());
        }

        public static long CountSteps(string[] input) {
            var pipes = new Dictionary<Point, char>();
            long count = 0;
            Point startPosition = new Point();

            for (var row = 0; row < input.Length; row++) {
                var line = input[row];

                for (var column = 0; column < line.Length; column++) {
                    if (row == 0 && line[column] == '|') {
                        startPosition = new Point(column, row);
                    }
                    char c = line[column];
                    pipes.Add(new Point(column, row), c);
                }
            }

            Point currentPosition = startPosition;
            char currentPipe = '|';
            Direction currentDirection = Direction.Down;

            List<char> collectedLetters = new List<char>();
            while (true) {
                count++;
                if (char.IsLetter(currentPipe)) {
                    collectedLetters.Add(currentPipe);
                }
                if (currentPipe != '+') {
                    currentPosition = currentPosition.Next(currentDirection);
                } else {
                    (currentPosition, currentDirection) = currentPosition.NextTurn(currentDirection, pipes);
                }

                if (!pipes.ContainsKey(currentPosition) || pipes[currentPosition] == ' ') {
                    break;
                }

                currentPipe = pipes[currentPosition];
            }

            return count;
        }

        public static string Print(Dictionary<Point, char> pipes, Point currentPosition) {
            StringBuilder display = new StringBuilder();
            for (int y = currentPosition.Y - 10; y < currentPosition.Y + 11; y++) {

                for (int x = currentPosition.X - 10; x < currentPosition.X + 11; x++) {
                    var displayPoint = new Point(x, y);
                    if (pipes.ContainsKey(displayPoint)) {
                        display.Append(pipes[displayPoint]);
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