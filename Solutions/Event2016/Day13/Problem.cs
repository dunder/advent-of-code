using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Shared.Tree;

namespace Solutions.Event2016.Day13
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day13;

        public override string FirstStar()
        {
            var result = FirstStarSolution(1362, 31, 39);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var result = SecondStarSolution(1362, 50);
            return result.ToString();
        }

        public static int FirstStarSolution(int input, int targetX, int targetY)
        {
            var initialPosition = new Point(1, 1);

            IEnumerable<Point> Neighbors(Point p)
            {
                return p.AdjacentInMainDirections().Where(x => !IsWall(input, x.X, x.Y));
            }

            bool ReachedTarget(Point p)
            {
                return p.X == targetX && p.Y == targetY;
            }

            var (targetNode, _) = initialPosition.ShortestPath(Neighbors, ReachedTarget);

            return targetNode.Depth;
        }

        public static int SecondStarSolution(int input, int targetDepth)
        {
            var initialPosition = new Point(1, 1);

            bool IsPositiveCoordinate(Point p)
            {
                return p.X >= 0 && p.Y >= 0;
            }

            IEnumerable<Point> Neighbors(Point p)
            {
                return p.AdjacentInMainDirections()
                    .Where(x => !IsWall(input, x.X, x.Y) && IsPositiveCoordinate(x));
            }

            var (_, visited) = initialPosition.BreadthFirst(Neighbors, targetDepth);

            var wrong = visited.Count(v => IsWall(input, v.X, v.Y));

            return visited.Count;
        }

        public static bool IsWall(int input, int x, int y)
        {
            var checksum = x*x + 3*x + 2*x*y + y + y*y + input;

            var binary = Convert.ToString(checksum, 2);

            return binary.Count(b => b == '1') % 2 != 0;
        }

        public static IList<string> Print(int input, int xMax, int yMax)
        {
            var rows = new List<string>
            {
                $"  { string.Join("", Enumerable.Range(0, xMax + 1).Select(x => (x % 10).ToString()))}"
            };
            for (int y = 0; y <= yMax; y++)
            {
                var s = new StringBuilder($"{(y%10).ToString()} ");

                for (int x = 0; x <= xMax; x++)
                {
                    var positionType = IsWall(input, x, y) ? "#" : ".";
                    s.Append($"{positionType}");
                }
                rows.Add(s.ToString());
            }
            return rows;
        }


    }
}