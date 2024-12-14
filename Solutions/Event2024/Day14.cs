using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 14: Restroom Redoubt ---
    public class Day14
    {
        private readonly ITestOutputHelper output;

        public Day14(ITestOutputHelper output)
        {
            this.output = output;
        }

        public record Robot(int X, int Y, int VX, int VY)
        {
            public Robot Move(int width, int height)
            {
                var x = X + VX;

                if (x >= 0)
                {
                    x = x % width;
                }
                else
                {
                    x = width - ((-x) % width);
                }

                var y = Y + VY;

                if (y >= 0)
                {
                    y = y % height;
                }
                else
                {
                    y = height - ((-y) % height);
                }

                return this with { X = x, Y = y };
            }
        }

        private static List<Robot> Parse(IList<string> input)
        {
            return input.Select(line => 
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var pRegexp = new Regex(@"p=(\-?\d+),(\-?\d+)");
                var vRegexp = new Regex(@"v=(\-?\d+),(\-?\d+)");

                var location = pRegexp.Match(parts[0]);
                var velocity = vRegexp.Match(parts[1]);

                int x = int.Parse(location.Groups[1].Value);
                int y = int.Parse(location.Groups[2].Value);

                int vx = int.Parse(velocity.Groups[1].Value);
                int vy = int.Parse(velocity.Groups[2].Value);

                return new Robot(x, y, vx, vy);
            }).ToList();
        }

        private static int Problem1(IList<string> input, int width, int height)
        {
            var robots = Parse(input);

            for (var r = 0; r < robots.Count; r++)
            {
                var robot = robots[r];
                for (int t = 1; t <= 100; t++)
                {
                    robots[r] = robots[r].Move(width, height);
                }
            }

            int q1 = 0;
            int q2 = 0;
            int q3 = 0;
            int q4 = 0;

            foreach (var robot in robots)
            {
                int x = robot.X;
                int y = robot.Y;

                var left = x < width / 2;
                var right = x > width / 2;
                var upper = y < height / 2;
                var lower = y > height / 2;

                if (upper && left)
                {
                    q1++;
                }
                else if (upper && right)
                {
                    q2++;
                }
                else if (lower && right)
                {
                    q3++;
                }
                else if (lower && left)
                {
                    q4++;
                }
            }

            return q1 * q2 * q3 * q4;
        }

        private void Print(List<Robot> robots, int t, int width, int height)
        {
            var lookup = robots.Select(r => (r.X, r.Y)).ToHashSet();
            var map = new StringBuilder();
            map.AppendLine($"t={t}");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (lookup.Contains((x, y)))
                    {
                        map.Append("#");
                    }
                    else
                    {
                        map.Append(" ");
                    }
                }
                map.AppendLine();
            }

            output.WriteLine(map.ToString());
        }

        private bool FindChristmasTree(List<Robot> robots, int width, int height)
        {
            var robotLocations = robots.Select(r => (r.X, r.Y)).ToHashSet();

            // search for a vertical line of a certain length at the center of the guarded area (assumed to be
            // the trunk of the christmas tree), an alternative could be a square of a certain width or
            // extending the search outside of the center of the map

            int x = width / 2;

            int length = 6;

            HashSet<(int, int)> SearchedArea(int y)
            {
                return Enumerable.Range(0, length).Select(i => (x, y + i)).ToHashSet();
            }

            for (int y = 0; y < height - length; y++)
            {
                HashSet<(int, int)> searchedArea = SearchedArea(y);

                if (searchedArea.IsSubsetOf(robotLocations))
                {
                    return true;
                }
            }

            return false;
        }

        private int Problem2(IList<string> input, int width, int height)
        {
            var robots = Parse(input);

            bool found = false;

            int t = 0;

            while (!found)
            {
                for (var r = 0; r < robots.Count; r++)
                {
                    robots[r] = robots[r].Move(width, height);
                }

                t++;

                found = FindChristmasTree(robots, width, height);
            }

            Print(robots, t, width, height);

            return t;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(229980828, Problem1(input, 101, 103));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(7132, Problem2(input, 101, 103));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "p=0,4 v=3,-3",
                "p=6,3 v=-1,-3",
                "p=10,3 v=-1,2",
                "p=2,0 v=2,-1",
                "p=0,0 v=1,3",
                "p=3,0 v=-2,-2",
                "p=7,6 v=-1,-3",
                "p=3,0 v=-1,-2",
                "p=9,3 v=2,3",
                "p=7,3 v=-1,2",
                "p=2,4 v=2,-3",
                "p=9,5 v=-3,-3",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(12, Problem1(exampleInput, 11, 7));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarMoveExample()
        {
            var robot = new Robot(2, 4, 2, -3);

            robot = robot.Move(11, 7);
            robot = robot.Move(11, 7);
            robot = robot.Move(11, 7);
            robot = robot.Move(11, 7);
            robot = robot.Move(11, 7);

            Assert.Equal(1, robot.X);
            Assert.Equal(3, robot.Y);
        }
    }
}
