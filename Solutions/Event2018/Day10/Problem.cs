using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shared.MapGeometry;

namespace Solutions.Event2018.Day10
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day10;

        public override string FirstStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public IList<string> ReadInput()
        {
            return ReadLineInput();
        }

        public int MoveToMessage(IList<string> input)
        {
            var points = Parse(input);
            var pointSet = new HashSet<PointOfLight>(points);
            int t = 0;
            for (; ; t++)
            {
                foreach (var point in points)
                {
                    point.Move();
                }

                if (pointSet.First().Position.ManhattanDistance(pointSet.Last().Position) < 100)
                {
                    var point = pointSet.First().Position;
                    var xFrom = point.X - 25;
                    var xTo = point.X + 25;
                    var yFrom = point.Y - 25;
                    var yTo = point.Y + 25;
                    Console.WriteLine($"t={t}");
                    Print(points, xFrom, xTo, yFrom, yTo);
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.X)
                    {
                        break;
                    }

                }
                Console.ReadKey();
            }

            return t;
        }

        public void Print(IList<PointOfLight> points, int xFrom, int xTo, int yFrom, int yTo)
        {
            var pointSet = new HashSet<Point>(points.Select(p => p.Position));

            for (int y = yFrom; y <= yTo; y++)
            {
                for (int x = xFrom; x <= xTo; x++)
                {
                    var drawPoint = new Point(x,y);
                    var print = pointSet.Contains(drawPoint) ? "#" : ".";
                    Console.Write(print);
                }
                Console.WriteLine();
            }
        }

        public static List<PointOfLight> Parse(IList<string> input)
        {
            var pointsExpression = new Regex(@"position=<\s*(\-?\d+),\s*(\-?\d+)> velocity=<\s*(\-?\d),\s*(\-?\d)>");
            var points = new List<PointOfLight>();

            foreach (var line in input)
            {
                var match = pointsExpression.Match(line);
                var pX = int.Parse(match.Groups[1].Value);
                var pY = int.Parse(match.Groups[2].Value);
                var vX = int.Parse(match.Groups[3].Value);
                var vY = int.Parse(match.Groups[4].Value);

                var point = new PointOfLight
                {
                    Position = new Point(pX, pY),
                    Velocity = new Point(vX, vY)
                };

                points.Add(point);
            }

            return points;
        }

        public class PointOfLight
        {
            public Point Position { get; set; }
            public Point Velocity { get; set; }

            public void Move()
            {
                Position = new Point(Position.X + Velocity.X, Position.Y + Velocity.Y);
            }
        }
    }
}