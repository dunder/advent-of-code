using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.Tree;
using Solutions.Event2017.Day10;

namespace Solutions.Event2017.Day14
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day14;

        private const string input = "nbysizxe";

        public override string FirstStar()
        {
            var result = HashGrid.CountLit(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var result = HashGrid.ContinousRegions(input);
            return result.ToString();
        }
    }

    public class HashGrid
    {
        public static int CountLit(string input)
        {
            int count = 0;
            for (int row = 0; row < 128; row++)
            {
                var rowInput = input + "-" + row;
                var hash = StringHash.HashAscii(256, rowInput);
                var binary = string.Join(string.Empty,
                    hash.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

                count += binary.Count(c => c == '1');
            }

            return count;
        }

        public static int ContinousRegions(string input)
        {
            bool[,] grid = new bool[128, 128];

            for (int row = 0; row < 128; row++)
            {
                var rowInput = input + "-" + row;
                var hash = StringHash.HashAscii(256, rowInput);
                var binary = string.Join(string.Empty,
                    hash.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                for (int column = 0; column < 128; column++)
                {
                    grid[row, column] = binary[column] == '1';
                }
            }

            HashSet<Point> allVisited = new HashSet<Point>();
            var count = 0;
            for (int row = 0; row < 128; row++)
            {
                for (int column = 0; column < 128; column++)
                {
                    if (grid[row, column] && !allVisited.Contains(new Point(column, row)))
                    {
                        var closureRow = row;
                        var closureColumn = column;
                        // search neighbours
                        (_, var visited) = new Point(closureColumn, closureRow).DepthFirst(p =>
                        {
                            var neighbours = new List<Point>();

                            // up
                            var pc = p.X;
                            var pr = p.Y - 1;
                            if (pr > -1 && grid[pr, pc])
                            {
                                var point = new Point(pc, pr);
                                if (!allVisited.Contains(point))
                                {
                                    neighbours.Add(new Point(pc, pr));
                                }
                            }

                            // right
                            pc = p.X + 1;
                            pr = p.Y;
                            if (pc < 128 && grid[pr, pc])
                            {
                                var point = new Point(pc, pr);

                                if (!allVisited.Contains(point))
                                {
                                    neighbours.Add(new Point(pc, pr));
                                }
                            }

                            // down
                            pc = p.X;
                            pr = p.Y + 1;
                            if (pr < 128 && grid[pr, pc])
                            {
                                var point = new Point(pc, pr);
                                if (!allVisited.Contains(point))
                                {
                                    neighbours.Add(point);
                                }
                            }

                            // left
                            pc = p.X - 1;
                            pr = p.Y;
                            if (pc >= 0 && grid[pr, pc])
                            {
                                var point = new Point(pc, pr);
                                if (!allVisited.Contains(point))
                                {
                                    neighbours.Add(point);
                                }
                            }

                            return neighbours;
                        });

                        allVisited.UnionWith(visited);
                        count += 1;
                    }
                }
            }

            return count;
        }
    }
}