using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Extensions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 6: Probably a Fire Hazard ---
    public class Day06
    {
        public enum Operation
        {
            TurnOn,
            TurnOff,
            Toggle
        }

        public class DisplayUpdateCommand
        {
            public Point UpperLeft { get; set; }
            public Point LowerRight { get; set; }
            public Operation Operation { get; set; }
        }

        public static void UpdateDisplay(int[,] display, Point upperLeft, Point lowerRight, Func<int, int> updateAction)
        {
            for (int x = upperLeft.X; x <= lowerRight.X; x++)
            {
                for (int y = upperLeft.Y; y <= lowerRight.Y; y++)
                {
                    display[x, y] = updateAction(display[x, y]);
                }
            }
        }

        public static DisplayUpdateCommand Parse(string instructionInput)
        {
            var r = new Regex(@"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)");

            var match = r.Match(instructionInput);

            if (!match.Success)
            {
                throw new ArgumentOutOfRangeException($"Cannot understand instruction: {instructionInput}");
            }

            var op = match.Groups[1].Value;
            var x1 = int.Parse(match.Groups[2].Value);
            var y1 = int.Parse(match.Groups[3].Value);
            var x2 = int.Parse(match.Groups[4].Value);
            var y2 = int.Parse(match.Groups[5].Value);

            var instruction = new DisplayUpdateCommand
            {
                UpperLeft = new Point(x1, y1),
                LowerRight = new Point(x2, y2)
            };

            switch (op)
            {
                case "turn on":
                    instruction.Operation = Operation.TurnOn;
                    break;
                case "turn off":
                    instruction.Operation = Operation.TurnOff;
                    break;
                case "toggle":
                    instruction.Operation = Operation.Toggle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown operation: {op}");
            }

            return instruction;
        }

        public static IDictionary<Operation, Func<int, int>> OperationSet1 = new Dictionary<Operation, Func<int, int>>
        {
            { Operation.TurnOn, x => 1},
            { Operation.TurnOff, x => 0},
            { Operation.Toggle, x => x == 0 ? 1 : 0}
        };
        
        public static IDictionary<Operation, Func<int, int>> OperationSet2 = new Dictionary<Operation, Func<int, int>>
        {
            { Operation.TurnOn, x => x + 1},
            { Operation.TurnOff, x => x > 0 ? x - 1 : 0},
            { Operation.Toggle, x => x + 2}
        };

        public static void ExecuteAll(int[,] display, IEnumerable<DisplayUpdateCommand> commands, IDictionary<Operation, Func<int,int>> operationSet)
        {
            foreach (var command in commands)
            {
                UpdateDisplay(display, command.UpperLeft, command.LowerRight, operationSet[command.Operation]);
            }
        }

        public static IEnumerable<T> Enumerate<T>(T[,] array2d)
        {
            foreach (var item in array2d)
            {
                yield return item;
            }
        }

        public static int Brightness(int[,] display)
        {
            return Enumerate(display).Sum();
        }

        public static int Run(int[,] display, IEnumerable<string> instructions, IDictionary<Operation, Func<int,int>> operationSet)
        {
            var commands = instructions.Select(Parse);
            ExecuteAll(display, commands, operationSet);
            return Brightness(display);
        }

        public static int FirstStar()
        {
            var input = ReadLineInput();
            var display = new int[1000, 1000];

            return Run(display, input, OperationSet1);
        }

        public static int SecondStar()
        {
            var input = ReadLineInput();
            var display = new int[1000, 1000];

            return Run(display, input, OperationSet2);
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(543903, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(14687245, result);
        }

        [Fact]
        public void Parse_Example()
        {
            var description = "turn on 0,0 through 999,999";

            var result = Parse(description);

            Assert.Equal(new Point(0,0), result.UpperLeft);
            Assert.Equal(new Point(999,999), result.LowerRight);
            Assert.Equal(Operation.TurnOn, result.Operation);
        }

        [Theory]
        [InlineData("turn on 0,0 through 999,999", 1_000_000)]
        [InlineData("toggle 0,0 through 999,0", 1_000)]
        public void FirstStar_Examples(string instruction, int expectedLit)
        {
            var display = new int[1_000, 1_000];

            var lit = Run(display, instruction.Yield(), OperationSet1);

            Assert.Equal(expectedLit, lit);
        }

        [Theory]
        [InlineData("turn off 0,0 through 999,999", 0)]
        [InlineData("toggle 0,0 through 999,999", 2_000_000)]
        [InlineData("turn on 0,0 through 999,999", 1_000_000)]
        public void SecondStar_Examples(string instruction, int expectedLit)
        {
            var display = new int[1_000, 1_000];

            var lit = Run(display, instruction.Yield(), OperationSet2);

            Assert.Equal(expectedLit, lit);
        }
    }
}
