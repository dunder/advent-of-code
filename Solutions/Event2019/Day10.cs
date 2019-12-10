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
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
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

            var relativeX2 = other.X - block.X;
            var relativeY2 = other.Y - block.Y;

            return relativeX * relativeY2 == relativeX2 * relativeY;
        }

        private static bool IsWithinSameQuadrant(Point me, Point block, Point other)
        {
            return IsWithinQuadrant1(me, block) && IsWithinQuadrant1(me, other) ||
                   IsWithinQuadrant2(me, block) && IsWithinQuadrant2(me, other) ||
                   IsWithinQuadrant3(me, block) && IsWithinQuadrant3(me, other) ||
                   IsWithinQuadrant4(me, block) && IsWithinQuadrant4(me, other);
        }
        private static int CountWithinSight(Point me, HashSet<Point> asteroids)
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

            return asteroids.Count - blocked.Count - 1;
        }

        private static int Max(bool[,] map)
        {
            var asteroids = Asteroids(map);
            return asteroids.Select(me => CountWithinSight(me, asteroids)).Max();
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
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(276, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
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
            var count = CountWithinSight(new Point(0, 0), Asteroids(map));
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
            var count = CountWithinSight(new Point(5, 8), Asteroids(map));
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
            var count = CountWithinSight(new Point(1, 2), Asteroids(map));
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
            var count = CountWithinSight(new Point(6, 3), Asteroids(map));
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
            var count = CountWithinSight(new Point(11, 13), Asteroids(map));
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
    }
}
