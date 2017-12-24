using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Grid;
using Utilities.MapGeometry;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day22 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day22\input.txt");

            var result = SporificaVirus.BurstsCausingInfection(input);

            Assert.Equal(5433, result);
            _output.WriteLine($"Day 22 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day22\input.txt");

            var result = SporificaVirus.BurstsCausingInfectionV2(input);

            _output.WriteLine($"Day 22 problem 2: {result}");
        }
    }

    public class SporificaVirus {
        public static int BurstsCausingInfection(string[] input) {

            Grid infectionGridMap = GridParser.Parse(input);

            HashSet<Point> originallyInfected = new HashSet<Point>(infectionGridMap.Where(x => x.value).Select(x => x.point));
            HashSet<Point> currentlyInfected = new HashSet<Point>(originallyInfected);

            var currentX = infectionGridMap.Width / 2;
            var currentY = infectionGridMap.Height / 2;

            Point currentPosition = new Point(currentX, currentY);
            Direction currentDirection = Direction.North;
            int infectedCount = 0;

            for (int i = 0; i < 10000; i++) {

                var currentDisplay = PrintCurrent(currentPosition, currentlyInfected, new HashSet<Point>(), new HashSet<Point>());

                if (currentlyInfected.Contains(currentPosition)) {
                    currentDirection = currentDirection.Turn(Turn.Right);
                    currentlyInfected.Remove(currentPosition);
                }
                else {
                    currentDirection = currentDirection.Turn(Turn.Left);
                    currentlyInfected.Add(currentPosition);
                    infectedCount++;
                }
                currentPosition = currentPosition.Move(currentDirection);
            }

            return infectedCount;
        }

        public static int BurstsCausingInfectionV2(string[] input) {

            Grid infectionGridMap = GridParser.Parse(input);

            HashSet<Point> originallyInfected = new HashSet<Point>(infectionGridMap.Where(x => x.value).Select(x => x.point));
            HashSet<Point> currentlyInfected = new HashSet<Point>(originallyInfected);
            HashSet<Point> currentlyWeakened = new HashSet<Point>();
            HashSet<Point> currentlyFlagged = new HashSet<Point>();

            var currentX = infectionGridMap.Width / 2;
            var currentY = infectionGridMap.Height / 2;

            Point initialPosition = new Point(currentX, currentY);
            Point currentPosition = initialPosition;
            Direction currentDirection = Direction.North;
            int infectedCount = 0;

            for (int i = 0; i < 10000000; i++) {

                //var currentDisplay = PrintCurrent(currentPosition, currentlyInfected, currentlyWeakened, currentlyFlagged);

                if (currentlyInfected.Contains(currentPosition)) {
                    currentDirection = currentDirection.Turn(Turn.Right);
                    currentlyInfected.Remove(currentPosition);
                    currentlyFlagged.Add(currentPosition);
                } else if (currentlyWeakened.Contains(currentPosition)) {
                    currentlyWeakened.Remove(currentPosition);
                    currentlyInfected.Add(currentPosition);
                    infectedCount++;
                } else if (currentlyFlagged.Contains(currentPosition)) {
                    currentDirection = currentDirection.Turn(Turn.Left);
                    currentDirection = currentDirection.Turn(Turn.Left);
                    currentlyFlagged.Remove(currentPosition);
                } else {
                    currentDirection = currentDirection.Turn(Turn.Left);
                    currentlyWeakened.Add(currentPosition);
                }

                currentPosition = currentPosition.Move(currentDirection);

                //if (i % 1000000 == 0) { 
                //    continue;
                //}
            }

            return infectedCount;
        }

        private static string PrintCurrent(Point currentPosition, HashSet<Point> currentlyInfected, HashSet<Point> currentlyWeakened, HashSet<Point> currentlyFlagged) {
            StringBuilder display = new StringBuilder();
            for (int y = currentPosition.Y - 5; y < currentPosition.Y + 5; y++) {
                for (int x = currentPosition.X - 5; x < currentPosition.X + 5; x++) {

                    Point point = new Point(x,y);

                    display.Append(currentPosition == point ? "[" : " ");

                    if (currentlyInfected.Contains(point)) {
                        display.Append("#");
                    } else if (currentlyFlagged.Contains(point)) {
                        display.Append("F");
                    } else if (currentlyWeakened.Contains(point)) {
                        display.Append("W");
                    } else {
                        display.Append(".");
                    }

                    display.Append(currentPosition == point ? "]" : " ");
                }
                display.AppendLine();
            }

            return display.ToString();
        }
    }

    public static class DirectionExtensions {
        public static Direction Turn(this Direction facing, Turn turn) {
            switch (facing) {
                case Direction.North:
                    return turn == Utilities.MapGeometry.Turn.Left ? Direction.West : Direction.East;
                case Direction.East:
                    return turn == Utilities.MapGeometry.Turn.Left ? Direction.North : Direction.South;
                case Direction.South:
                    return turn == Utilities.MapGeometry.Turn.Left ? Direction.East : Direction.West;
                case Direction.West:
                    return turn == Utilities.MapGeometry.Turn.Left ? Direction.South : Direction.North;
            }
            throw new ArgumentOutOfRangeException(nameof(turn), "Invalid turn");
        }
    }
}
