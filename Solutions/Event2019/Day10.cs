using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.MapGeometry;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // 
    public class Day10
    {
        private bool[,] ParseMap(IEnumerable<string> input)
        {
            var lines = input.ToList();
            bool[,] map = new bool[lines.First().Length, lines.Count()];
            int y = 0;
            foreach (var line in lines)
            {
                int x = 0;
                foreach (var c in line)
                {
                    map[x, y] = c == '#';
                    x++;
                }

                y++;
            }

            return map;
        }

        private static HashSet<Point> Asteroids(bool[,] map)
        {
            var asteroids = new HashSet<Point>();
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y])
                    {
                        asteroids.Add(new Point(x, y));
                    }
                }
            }

            return asteroids;
        }

        private static bool IsWithinQuadrant1(Point me, Point other)
        {
            return other.X >= me.X && other.Y >= me.Y;
        }

        private static bool IsWithinQuadrant2(Point me, Point other)
        {
            return other.X <= me.X && other.Y >= me.Y;
        }

        private static bool IsWithinQuadrant3(Point me, Point other)
        {
            return other.X <= me.X && other.Y <= me.Y;
        }

        private static bool IsWithinQuadrant4(Point me, Point other)
        {
            return other.X >= me.X && other.Y <= me.Y;
        }

        private static bool IsBlockedLineOfSight(Point me, Point block, Point other)
        {
            if (other.ManhattanDistance(me) < block.ManhattanDistance(me))
            {
                return false;
            }

            if (!IsWithinSameQuadrant(me, block, other))
            {
                return false;
            }

            var relativeX = block.X - me.X;
            var relativeY = block.Y - me.Y;

            var xy = relativeX > relativeY ? relativeX == relativeY ? 0 : -1 : 1;

            var relativeX2 = other.X - block.X;
            var relativeY2 = other.Y - block.Y;

            var xy2 = relativeX2 > relativeY2 ? relativeX2 == relativeY2 ? 0 : -1 : 1;


            return relativeX * relativeY2 == relativeX2 * relativeY && xy == xy2;
        }

        private static bool IsWithinSameQuadrant(Point me, Point block, Point other)
        {
            return IsWithinQuadrant1(me, block) && IsWithinQuadrant1(me, other) ||
                   IsWithinQuadrant2(me, block) && IsWithinQuadrant2(me, other) ||
                   IsWithinQuadrant3(me, block) && IsWithinQuadrant3(me, other) ||
                   IsWithinQuadrant4(me, block) && IsWithinQuadrant4(me, other);
        }
        private static (Point, int) CountWithinSight(Point me, HashSet<Point> asteroids)
        {
            var blocked = new HashSet<Point>();

            foreach (var potentiallyBlocking in asteroids)
            {
                if (me.Equals(potentiallyBlocking) || blocked.Contains(potentiallyBlocking))
                {
                    continue;
                }

                foreach (var potentiallyBlocked in asteroids)
                {
                    if (potentiallyBlocked.Equals(potentiallyBlocking) || potentiallyBlocked.Equals(me))
                    {
                        continue;
                    }

                    if (IsBlockedLineOfSight(me, potentiallyBlocking, potentiallyBlocked))
                    {
                        blocked.Add(potentiallyBlocked);
                    }
                }
            }

            return (me, asteroids.Count - blocked.Count - 1);
        }

        private static int Max(bool[,] map)
        {
            var asteroids = Asteroids(map);
            return asteroids.Select(me => CountWithinSight(me, asteroids)).Select(x => x.Item2).Max();
        }

        private static bool IsSameLineOfSight(Point me, Point point, Point other)
        {
            if (!IsWithinSameQuadrant(me, point, other))
            {
                return false;
            }

            var relativeX = point.X - me.X;
            var relativeY = point.Y - me.Y;

            var xy = relativeX > relativeY ? relativeX == relativeY ? 0 : -1 : 1;

            var relativeX2 = other.X - me.X;
            var relativeY2 = other.Y - me.Y;

            var xy2 = relativeX2 > relativeY2 ? relativeX2 == relativeY2 ? 0 : -1 : 1;


            return relativeX * relativeY2 == relativeX2 * relativeY && xy == xy2;
        }

        private static List<List<Point>> Group(Point baseAt, HashSet<Point> asteroids)
        {
            asteroids.Remove(baseAt);

            var groups = new List<List<Point>>();

            var visited = new HashSet<Point>();

            foreach (var asteroid in asteroids)
            {

                if (visited.Contains(asteroid))
                {
                    continue;
                }

                visited.Add(asteroid);
                var group = new List<Point>();
                group.Add(asteroid);

                var others = new HashSet<Point>(asteroids);
                others.Remove(asteroid);

                foreach (var other in others)
                {
                    if (visited.Contains(other))
                    {
                        continue;
                        
                    }

                    if (IsSameLineOfSight(baseAt, asteroid, other))
                    {
                        group.Add(other);
                        visited.Add(other);
                    }
                }
                groups.Add(new List<Point>(group.OrderBy(g => baseAt.ManhattanDistance(g))));
            }

            return groups;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            var map = ParseMap(input);
            return Max(map);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            var map = ParseMap(input);
            var asteroids = Asteroids(map);

            var baseAt = asteroids.Select(me => CountWithinSight(me, asteroids))
                .Single(x => x.Item2 == 276).Item1;

            var groups = Group(baseAt, asteroids);
            var firsts = groups.Select(g => g.First()).ToList();
            firsts.Sort(new ClockwiseComparer(baseAt));
            var twoHundredth = firsts[199];
            return twoHundredth.X * 100 + twoHundredth.Y;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(276, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1321, SecondStar());
        }

        [Fact]
        public void FirstStarExample0()
        {
            var input = new[]
            {
                "#.........",
                "...#......",
                "...#..#...",                
                ".####....#",                
                "..#.#.#...",                
                ".....#....",                
                "..###.#.##",                
                ".......#..",                
                "....#...#.",                
                "...#..#..#"
            };

            var map = ParseMap(input);
            var (_, count) = CountWithinSight(new Point(0, 0), Asteroids(map));
            Assert.Equal(7, count);
        }

        [Fact]
        public void FirstStarExample1()
        {
            var input = new[]
            {
                "......#.#.",
                "#..#.#....",
                "..#######.",
                ".#.#.###..",
                ".#..#.....",
                "..#....#.#",
                "#..#....#.",
                ".##.#..###",
                "##...#..#.",
                ".#....####"
            };

            var map = ParseMap(input);
            var (_, count) = CountWithinSight(new Point(5, 8), Asteroids(map));
            Assert.Equal(33, count);
            Assert.Equal(33, Max(map));

        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = new[]
            {
                "#.#...#.#.",
                ".###....#.",
                ".#....#...",
                "##.#.#.#.#",
                "....#.#.#.",
                ".##..###.#",
                "..#...##..",
                "..##....##",
                "......#...",
                ".####.###."
            };

            var map = ParseMap(input);
            var (_, count) = CountWithinSight(new Point(1, 2), Asteroids(map));
            Assert.Equal(35, count);
        }

        [Fact]
        public void FirstStarExample3()
        {
            var input = new[]
            {
                ".#..#..###",
                "####.###.#",
                "....###.#.",
                "..###.##.#",
                "##.##.#.#.",
                "....###..#",
                "..#.#..#.#",
                "#..#.#.###",
                ".##...##.#",
                ".....#.#..",
            };

            var map = ParseMap(input);
            var (_, count) = CountWithinSight(new Point(6, 3), Asteroids(map));
            Assert.Equal(41, count);
        }

        [Fact]
        public void FirstStarExample4()
        {
            var input = new[]
            {
                ".#..##.###...#######",
                "##.############..##.",
                ".#.######.########.#",
                ".###.#######.####.#.",
                "#####.##.#.##.###.##",
                "..#####..#.#########",
                "####################",
                "#.####....###.#.#.##",
                "##.#################",
                "#####.##.###..####..",
                "..######..##.#######",
                "####.##.####...##..#",
                ".#####..#.######.###",
                "##...#.##########...",
                "#.##########.#######",
                ".####.#.###.###.#.##",
                "....##.##.###..#####",
                ".#.#.###########.###",
                "#.#.#.#####.####.###",
                "###.##.####.##.#..##",
            };

            var map = ParseMap(input);
            var (_, count) = CountWithinSight(new Point(11, 13), Asteroids(map));
            Assert.Equal(210, count);
        }

        [Fact]
        public void LineInSightExample()
        {
            var me = new Point(0,0);
            var block = new Point(1,3);
            var outOfSight1 = new Point(2,6);
            var outOfSight2 = new Point(3,9);
            var inSight1 = new Point(3,6);
            var inSight2 = new Point(4,6);

            Assert.True(IsBlockedLineOfSight(me, block, outOfSight1));
            Assert.True(IsBlockedLineOfSight(me, block, outOfSight2));
            Assert.False(IsBlockedLineOfSight(me, block, inSight1));
            Assert.False(IsBlockedLineOfSight(me, block, inSight2));
        }

        [Fact]
        public void LineInSightNotBlockedCloserExample()
        {
            var me = new Point(0, 0);
            var block = new Point(1, 3);
            var outOfSight1 = new Point(2, 6);
            var outOfSight2 = new Point(3, 9);

            Assert.False(IsBlockedLineOfSight(me,outOfSight1, block));
            Assert.False(IsBlockedLineOfSight(me, outOfSight2, outOfSight1));
        }

        [Fact]
        public void LineInSightExample2()
        {
            var me = new Point(0, 0);
            var block = new Point(2, 3);
            var outOfSight1 = new Point(4, 6);
            var outOfSight2 = new Point(6, 9);
            var inSight1 = new Point(3, 6);
            var inSight2 = new Point(5, 6);

            Assert.True(IsBlockedLineOfSight(me, block, outOfSight1));
            Assert.True(IsBlockedLineOfSight(me, block, outOfSight2));  
            Assert.False(IsBlockedLineOfSight(me, block, inSight1));
            Assert.False(IsBlockedLineOfSight(me, block, inSight2));
        }

        [Fact]

        public void IsWithinSameQuadrantTests()
        {
            var me = new Point(1,2);

            Assert.True(IsWithinSameQuadrant(me, new Point(0,0), new Point(1,1)));
            Assert.True(IsWithinSameQuadrant(me, new Point(2,0), new Point(6,2)));
            Assert.True(IsWithinSameQuadrant(me, new Point(3,3), new Point(5,4)));
            Assert.True(IsWithinSameQuadrant(me, new Point(3,3), new Point(3,5)));
        }

        [Fact]
        public void IsBlockedLineOfSightQuadrantTests()
        {
            var me = new Point(3,3);
            var potentialBlock = new Point(4,4);
            var notBlocked = new Point(1,1);

            Assert.False(IsBlockedLineOfSight(me, potentialBlock, notBlocked));
        }

        [Fact]
        public void IsBlockedLineOfSightExtraTest1()
        {
            var me = new Point(5,8);
            var potentialBlock = new Point(3,3);
            var notBlocked = new Point(2,2);

            Assert.False(IsBlockedLineOfSight(me, potentialBlock, notBlocked));
        }

        [Fact]
        public void SecondStarExampleGroup()
        {
            var input = new[]
            {
                "#.........",
                "...#......",
                "...#..#...",
                ".####....#",
                "..#.#.#...",
                ".....#....",
                "..###.#.##",
                ".......#..",
                "....#...#.",
                "...#..#..#"
            };

            var map = ParseMap(input);
            var asteroids = Asteroids(map);
            var groups = Group(new Point(0, 0), asteroids);
            var totalAsteroids = groups.Sum(g => g.Count);
            Assert.Equal(7, groups.Count);
            Assert.Equal(24, totalAsteroids);
        }

        [Fact]
        public void SecondStarExample2()
        {
            var input = new[]
            {
                ".#....#####...#..",
                "##...##.#####..##",
                "##...#...#.#####.",
                "..#.....#...###..",
                "..#.#.....#....##" 
            };

            var map = ParseMap(input);
            var asteroids = Asteroids(map);
            var groups = Group(new Point(8, 3), asteroids);
            var firsts = groups.Select(g => g.First()).ToList();
            firsts.Sort(new ClockwiseComparer(new Point(8,3)));
            
            Assert.Equal(new Point(7,0), firsts.Last());
        }

        [Theory]
        [InlineData(8,0,8,1)]
        [InlineData(9,2,10,1)]
        public void IsSameLineOfSightTest1(int x1, int y1, int x2, int y2)
        {
            var me = new Point(8, 3);
            Assert.True(IsSameLineOfSight(me, new Point(x1, y1), new Point(x2, y2)));
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new[]
            {
                ".#..##.###...#######",
                "##.############..##.",
                ".#.######.########.#",
                ".###.#######.####.#.",
                "#####.##.#.##.###.##",
                "..#####..#.#########",
                "####################",
                "#.####....###.#.#.##",
                "##.#################",
                "#####.##.###..####..",
                "..######..##.#######",
                "####.##.####...##..#",
                ".#####..#.######.###",
                "##...#.##########...",
                "#.##########.#######",
                ".####.#.###.###.#.##",
                "....##.##.###..#####",
                ".#.#.###########.###",
                "#.#.#.#####.####.###",
                "###.##.####.##.#..##",
            };

            var map = ParseMap(input);
            var asteroids = Asteroids(map);
            var groups = Group(new Point(11, 13), asteroids);
            var firsts = groups.Select(g => g.First()).ToList();
            firsts.Sort(new ClockwiseComparer(new Point(11,13)));
            var x = firsts[199];
            Assert.Equal(802, x.X * 100 + x.Y);
        }

        [Theory]
        [InlineData(11,12,0)]
        [InlineData(12,13,90)]
        [InlineData(12,14,135)]
        [InlineData(11,14,180)]
        [InlineData(10,14,225)]
        [InlineData(10,13,270)]
        [InlineData(10,12,315)]
        public void AngleTest(int x, int y, int expectedAngle)
        {
            // based on large example
            var baseAt = new Point(11,13);
            var angle = Angle(baseAt, new Point(x, y));

            Assert.Equal(expectedAngle, angle);
        }

        [Fact]
        public void ClockwiseComparerTests()
        {
            var asteroids = new List<Point>
            {
                new Point(11,1),
                new Point(9,2),
                new Point(10,0),
                new Point(9,1),
                new Point(9,0),
                new Point(8,1),
            };
            var clockwiseComparer = new ClockwiseComparer(new Point(8,3));
            asteroids.Sort(clockwiseComparer);

            Assert.Collection(asteroids, 
                a => Assert.Equal(new Point(8, 1), a),
                a => Assert.Equal(new Point(9, 0), a),
                a => Assert.Equal(new Point(9, 1), a),
                a => Assert.Equal(new Point(10, 0), a),
                a => Assert.Equal(new Point(9, 2), a),
                a => Assert.Equal(new Point(11, 1), a));
        }

        private class ClockwiseComparer : IComparer<Point>
        {
            private readonly Point baseAt;

            public ClockwiseComparer(Point baseAt)
            {
                this.baseAt = baseAt;
            }

            public int Compare(Point p1, Point p2)
            {
                var p1Angle = Angle(baseAt, p1);
                var p2Angle = Angle(baseAt, p2);

                if (p1Angle < p2Angle)
                {
                    return -1;
                }

                return 1;
            }
        }

        private static double Angle(Point me, Point p)
        {
            int q = GetQuadrant(me, p);

            var yDiff = me.Y - p.Y;
            var xDiff = me.X - p.X;

            if (q == 1)
            {
                if (yDiff == 0)
                {
                    return 90;
                }
                return (180 / Math.PI) * Math.Atan((float)Math.Abs(xDiff) / Math.Abs(yDiff));
            }

            if (q == 2)
            {
                if (xDiff == 0)
                {
                    return 180;
                }
                return 90 + (180 / Math.PI) * Math.Atan((float)Math.Abs(yDiff) / Math.Abs(xDiff));
            }

            if (q == 3)
            {
                if (yDiff == 0)
                {
                    return 270;
                }
                return 180 + (180 / Math.PI) * Math.Atan((float)Math.Abs(xDiff) / Math.Abs(yDiff)); 
            }

            return 270 + (180 / Math.PI) * Math.Atan((float)Math.Abs(yDiff) / Math.Abs(xDiff));
        }

        private static int GetQuadrant(Point me, Point other)
        {
            if (other.X >= me.X && other.Y <= me.Y)
            {
                return 1;
            }

            if (other.X >= me.X && other.Y >= me.Y)
            {
                return 2;
            }

            if (other.X <= me.X && other.Y >= me.Y)
            {
                return 3;
            }

            return 4;
        }

    }
}
