using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            var input = ReadLineInput();
            MoveToMessage(input);
            return "The message must be read from the console";
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            MoveToMessage(input);
            
            return "The t was printed to the console";
        }
        public void FirstStarExample()
        {
            var input = FirstStarExampleInput();
            MoveToMessage(input);
        }


        public void MoveToMessage(IList<string> input)
        {
            Console.WriteLine("Press [Enter] to generate the next second. Press [t] to exit and print second");

            var points = Parse(input);
            var pointSet = new HashSet<PointOfLight>(points);
            int t = 1;
            for (; ; t++)
            {
                foreach (var point in points)
                {
                    point.Move();
                }

                if (pointSet.First().Position.ManhattanDistance(pointSet.Last().Position) < 100)
                {
                    var point = pointSet.First().Position;
                    var xFrom = point.X - 60;
                    var xTo = point.X + 20;
                    var yFrom = point.Y - 10;
                    var yTo = point.Y + 10;
                    Console.WriteLine($"t={t}");
                    Print(points, xFrom, xTo, yFrom, yTo);
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == 't')
                    {
                        break;
                    }

                }
            }

            Console.WriteLine($"Exit at t = {t}");
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

        private IList<string> FirstStarExampleInput()
        {
            var input = new List<string>
            {
                "position=< 9,  1> velocity=< 0,  2>",
                "position=< 7,  0> velocity=<-1,  0>",
                "position=< 3, -2> velocity=<-1,  1>",
                "position=< 6, 10> velocity=<-2, -1>",
                "position=< 2, -4> velocity=< 2,  2>",
                "position=<-6, 10> velocity=< 2, -2>",
                "position=< 1,  8> velocity=< 1, -1>",
                "position=< 1,  7> velocity=< 1,  0>",
                "position=<-3, 11> velocity=< 1, -2>",
                "position=< 7,  6> velocity=<-1, -1>",
                "position=<-2,  3> velocity=< 1,  0>",
                "position=<-4,  3> velocity=< 2,  0>",
                "position=<10, -3> velocity=<-1,  1>",
                "position=< 5, 11> velocity=< 1, -2>",
                "position=< 4,  7> velocity=< 0, -1>",
                "position=< 8, -2> velocity=< 0,  1>",
                "position=<15,  0> velocity=<-2,  0>",
                "position=< 1,  6> velocity=< 1,  0>",
                "position=< 8,  9> velocity=< 0, -1>",
                "position=< 3,  3> velocity=<-1,  1>",
                "position=< 0,  5> velocity=< 0, -1>",
                "position=<-2,  2> velocity=< 2,  0>",
                "position=< 5, -2> velocity=< 1,  2>",
                "position=< 1,  4> velocity=< 2,  1>",
                "position=<-2,  7> velocity=< 2, -2>",
                "position=< 3,  6> velocity=<-1, -1>",
                "position=< 5,  0> velocity=< 1,  0>",
                "position=<-6,  0> velocity=< 2,  0>",
                "position=< 5,  9> velocity=< 1, -2>",
                "position=<14,  7> velocity=<-2,  0>",
                "position=<-3,  6> velocity=< 2, -1>"
            };
            return input;
        }
    }
}