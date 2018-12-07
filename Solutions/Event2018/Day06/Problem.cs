using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Shared.MapGeometry;

namespace Solutions.Event2018.Day06
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day06;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = LargestArea(input);
            return result.ToString();
        }

        public static List<Point> ParseCoordinates(IList<string> input)
        {
            var coordinates = new List<Point>();

            for (var i = 0; i < input.Count; i++)
            {
                var value = input[i];
                var split = value.Split(", ");
                var x = int.Parse(split[0]);
                var y = int.Parse(split[1]);

                coordinates.Add(new Point(x,y));
            }
            return coordinates;
        }

        public static int LargestArea(IList<string> input)
        {
            var coordinates = ParseCoordinates(input);

            var coordinatesWithinBoundry = new HashSet<Point>();


            foreach (var coordinate in coordinates)
            {
                var others = coordinates.Where(c => c != coordinate).ToList();
                var surroundingPoints = PointsAtRadius(coordinate, 1);

                foreach (var point in others)
                {
                    var distance = point.ManhattanDistance(coordinate);
                    
                    var closingIn = surroundingPoints.Where(s => s.ManhattanDistance(point) < distance).ToList();
                }

                

            }
            //foreach (var coordinate in coordinates)
            //{
            //    var other = coordinates.Where(c => c != coordinate).ToList();

            //    bool left = false;
            //    bool up = false;
            //    bool right = false;
            //    bool down = false;


            //    foreach (var point in other)
            //    {
            //        left = left || point.X < coordinate.X;
            //        up = up || point.Y > coordinate.Y;
            //        right = right || point.X > coordinate.X;
            //        down = down || point.Y < coordinate.Y;
            //    }

            //    if (left && up && right && down)
            //    {
            //        coordinatesWithinBoundry.Add(coordinate);
            //    }
            //}

            var notIn = coordinates.Where(c => !coordinatesWithinBoundry.Contains(c)).ToHashSet();

            //Print(coordinates, coordinatesWithinBoundry, notIn);

            var areas = new Dictionary<Point, int>();

            foreach (var coordinate in coordinatesWithinBoundry)
            {
                var radius = 1;
                var area = 1;
                while (true)
                {
                    var pointsAtRadius = PointsAtRadius(coordinate, radius);

                    var pointsWithMinDistance = pointsAtRadius.Select(p => HasMinDistance(coordinate, p, coordinates)).ToList();
                    area += pointsWithMinDistance.Count;
                    if (pointsWithMinDistance.Count == 0)
                    {
                        areas.Add(coordinate, area);
                        break;
                    }

                    radius++;
                }
            }

            return areas.Values.Max();
        }

        public static void Print(IList<Point> coordinates, HashSet<Point> withinBoundry, HashSet<Point> notWithin)
        {
            var minX = coordinates.Min(c => c.X);
            var maxX = coordinates.Max(c => c.X);
            var minY = coordinates.Min(c => c.Y);
            var maxY = coordinates.Max(c => c.Y);
            var lines = new List<string>();
            for (int y = minY; y <= maxY; y++)
            {
                var line = new StringBuilder();
                for (int x = minX; x <= maxX; x++)
                {
                    var point = new Point(x, y);
                    var print = " ";
                    if (withinBoundry.Contains(point))
                    {
                        print = $"O({x},{y})";
                    }

                    if (notWithin.Contains(point))
                    {
                        print = $"X({x},{y})";
                    }
                    line.Append(print);
                    //Console.Write(print);
                }

                lines.Add(line.ToString());
                //Console.WriteLine();
            }

            File.WriteAllLines(@".\output.txt", lines);
        }

        public static IList<Point> PointsAtRadius(Point point, int radius)
        {
            var points = new List<Point>();

            // top and bottom bottom, fixed y:s iterate x from left to right
            for (int x = point.X - radius; x <= point.X + radius; x++)
            {
                var topY = point.Y - radius;
                var bottomY = point.Y + radius;
                var top = new Point(x, topY);
                var bottom = new Point(x, bottomY);
                points.Add(top);
                points.Add(bottom);
            }

            // left and right columns, fixed x:s iterate y from top to bottom, skip overlaps with top and bottom 
            for (int y = point.Y - radius + 1; y < point.Y + radius; y++)
            {
                var leftX = point.X - radius;
                var rightX = point.X + radius;
                var left = new Point(leftX, y);
                var right = new Point(rightX, y);

                points.Add(left);
                points.Add(right);

            }

            return points;
        }

        public static bool HasMinDistance(Point me, Point point, List<Point> all)
        {
            var sortedByDistance = all.Where(x => x != me).OrderBy(x => x.ManhattanDistance(point)).ToList();
            var minDistance = sortedByDistance.First().ManhattanDistance(point);
            var mine = me.ManhattanDistance(point);
            return mine < minDistance;
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }
    }

}