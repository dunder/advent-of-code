using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2019
{

    // --- Day 11: Space Police ---
    public class Day11
    {
        private readonly ITestOutputHelper output;

        public Day11(ITestOutputHelper output)
        {
            this.output = output;
        }

        private Direction MakeTurn(int code, Direction currentDirection)
        {
            if (code > 1 || code < 0)
            {
                throw new ArgumentOutOfRangeException($"Invalid turn: {code}");
            }
            return code == 0 ? currentDirection.Turn(Turn.Left) : currentDirection.Turn(Turn.Right);
        }

        private static int Read(Dictionary<Point, int> painted, Point p)
        {
            if (!painted.ContainsKey(p))
            {
                painted.Add(p, 0);
            }

            return painted[p];
        }

        private static void Paint(Dictionary<Point, int> painted, Point p, int color)
        {
            if (!painted.ContainsKey(p))
            {
                painted.Add(p, 0);
            }

            painted[p] = color;
        }

        private void Print(Dictionary<Point, int> path)
        {
            var maxX = path.Keys.Max(p => p.X);
            var minX = path.Keys.Min(p => p.X);

            var maxY = path.Keys.Max(p => p.Y);
            var minY = path.Keys.Min(p => p.Y);

            for (int y = minY; y <= maxY; y++)
            {
                var line = new StringBuilder();
                for (int x = minX; x <= maxX; x++)
                {
                    var symbol = path.TryGetValue(new Point(x, y), out int value) && value == 1 ? "#" : " ";
                    line.Append(symbol);
                }
                output.WriteLine(line.ToString());
            }

        }

        public int Run(string program, int startColor)
        {
            // all initial black = 0 (1 = white)
            // input from camera color below robot
            // output 1 = color to paint the panel it is over
            // output 2 = direction 0 is left, 1 is right
            // after turn move forward one panel

            var startingPoint = new Point(0, 0);
            var painted = new Dictionary<Point, int>();
            if (startColor == 1)
            {
                painted.Add(startingPoint, startColor);

            }
            var currentDirection = Direction.North;
            var currentLocation = startingPoint;

            var computer = IntCodeComputer.Load(program);

            var exitCode = IntCodeComputer.ExecutionState.WaitingForInput;
            while (exitCode != IntCodeComputer.ExecutionState.Ready)
            {
                computer.Input.Enqueue(Read(painted, currentLocation));

                exitCode = computer.Execute();
                if (exitCode == IntCodeComputer.ExecutionState.Ready)
                {
                    continue;
                }
                var color = computer.Output.First();
                int turnCode = (int) computer.Output.Last();
                currentDirection = MakeTurn(turnCode, currentDirection);
                Paint(painted, currentLocation, (int)color);
                currentLocation = currentLocation.Move(currentDirection);
            }

            Print(painted);

            return painted.Keys.Count;
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var count = Run(input, 0);
            return count;
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var count = Run(input, 1);
            return count;
        }

        [Fact]
        public void FirstStarTest() 
        {
            Assert.Equal(2184, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(249, SecondStar()); // AHCHZEPK
        }
    }
}
