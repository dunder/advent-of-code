using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Solutions.Event2016.Day8
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day8;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = new Display(6, 50).CountPixelsLit(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var display = new Display(6, 50);
            display.SendInstructions(input);
            var result = display.Print();
            return result; // AFBUPZBJPS (no unit test, needs manual verification)
        }
    }

    public class Display {
        private readonly bool[,] _display;
        public Display(int rows, int columns) {
            _display = new bool[rows, columns];
        }

        public int CountPixelsLit(IList<string> input) {
            SendInstructions(input);
            return (from bool pixel in _display
                    where pixel
                    select pixel).Count();
        }

        public void SendInstructions(IList<string> input) {
            foreach (var line in input) {
                switch (line) {
                    case var rect when line.Contains("rect"):
                        CreateRectangle(rect);
                        break;
                    case var rotateColumn when line.Contains("rotate column"):
                        RotateColumn(rotateColumn);
                        break;
                    case var rotateRow when line.Contains("rotate row"):
                        RotateRow(rotateRow);
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected instruction: {line}");
                }
            }
        }

        public void CreateRectangle(string instruction) {
            var dimension = instruction.Substring("rect ".Length).Split('x');
            var width = int.Parse(dimension[0]);
            var height = int.Parse(dimension[1]);

            for (int row = 0; row < height; row++) {
                for (int column = 0; column < width; column++) {
                    _display[row, column] = true;
                }
            }
        }

        public void RotateColumn(string instruction) {
            var columnAndShift = Regex.Split(instruction.Substring("rotate column x=".Length), @" by ");
            var column = int.Parse(columnAndShift[0]);
            var shift = int.Parse(columnAndShift[1]);
            var columnBefore = new bool[_display.GetLength(0)];

            for (int row = 0; row < _display.GetLength(0); row++) {
                columnBefore[row] = _display[row, column];
            }

            for (int row = 0; row < _display.GetLength(0); row++) {
                _display[row, column] = false;
            }

            for (int row = 0; row < _display.GetLength(0); row++) {
                if (columnBefore[row]) {
                    var newRow = row + shift >= _display.GetLength(0) ? (row + shift) % _display.GetLength(0) : row + shift;
                    _display[newRow, column] = true;
                }
            }
        }

        public void RotateRow(string instruction) {
            var rowAndShift = Regex.Split(instruction.Substring("rotate row y=".Length), @" by ");
            var row = int.Parse(rowAndShift[0]);
            var shift = int.Parse(rowAndShift[1]);
            var rowBefore = new bool[_display.GetLength(1)];

            for (int column = 0; column < _display.GetLength(1); column++) {
                rowBefore[column] = _display[row, column];
            }

            for (int column = 0; column < _display.GetLength(1); column++) {
                _display[row, column] = false;
            }

            for (int column = 0; column < _display.GetLength(1); column++) {
                if (rowBefore[column]) {
                    var newColumn = column + shift >= _display.GetLength(1) ? (column + shift) % _display.GetLength(1) : column + shift;
                    _display[row, newColumn] = true;
                }
            }
        }

        public string Print() {
            StringBuilder output = new StringBuilder();
            for (int row = 0; row < _display.GetLength(0); row++) {
                for (int column = 0; column < _display.GetLength(1); column++) {
                    if (_display[row, column]) {
                        output.Append("#");
                    } else {
                        output.Append(".");
                    }
                }
                output.Append(Environment.NewLine);
            }
            return output.ToString();
        }
    }

}