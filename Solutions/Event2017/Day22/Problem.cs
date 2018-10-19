using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.Grid;
using Shared.MapGeometry;
using Solutions.Event2016.Day01;

namespace Solutions.Event2017.Day22
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day22;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = SporificaVirus.BurstsCausingInfection(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = SporificaVirus.BurstsCausingInfectionV2(input);
            return result.ToString();
        }
    }
    public class SporificaVirus {
        public static int BurstsCausingInfection(IList<string> input) {

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
                } else {
                    currentDirection = currentDirection.Turn(Turn.Left);
                    currentlyInfected.Add(currentPosition);
                    infectedCount++;
                }
                currentPosition = currentPosition.Move(currentDirection);
            }

            return infectedCount;
        }

        public static int BurstsCausingInfectionV2(IList<string> input) {

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

                    Point point = new Point(x, y);

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

}