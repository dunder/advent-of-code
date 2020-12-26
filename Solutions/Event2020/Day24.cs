using Shared.MapGeometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

using static Solutions.InputReader;

namespace Solutions.Event2020
{
    // --- Day 24: Lobby Layout ---

    public class Day24
    {
        private readonly ITestOutputHelper output;


        public Day24(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<List<Direction>> Parse(List<string> lines)
        {
            return lines.Select(line => Parse(line)).ToList();
        }

        private List<Direction> Parse(string line)
        {
            var directions = new List<Direction>();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                switch (c)
                {
                    case 'n':
                        {
                            char next = line[i + 1];
                            if (next == 'e')
                            {
                                directions.Add(Direction.NorthEast);
                            }
                            else
                            {
                                directions.Add(Direction.NorthWest);
                            }
                            i += 1;
                            break;
                        }
                    case 'e':
                        directions.Add(Direction.East);
                        break;
                    case 's':
                        {
                            char next = line[i + 1];
                            if (next == 'e')
                            {
                                directions.Add(Direction.SouthEast);
                            }
                            else
                            {
                                directions.Add(Direction.SouthWest);

                            }
                            i += 1;
                            break;
                        }
                    case 'w':
                        directions.Add(Direction.West);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unexpected character: {c}");
                }
            }

            return directions;
        }

        private Point Move(List<Direction> directions)
        {
            Point current = new Point(0,0);

            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case Direction.NorthEast:
                        {
                            var x = current.Y % 2 == 0 ? 1 : 0;
                            current = new Point(current.X + x, current.Y + 1);
                            break;
                        }
                    case Direction.East:
                        current = new Point(current.X + 1, current.Y);
                        break;
                    case Direction.SouthEast:
                        {
                            var x = current.Y % 2 == 0 ? 1 : 0;
                            current = new Point(current.X + x, current.Y - 1);
                            break;
                        }
                    case Direction.SouthWest:
                        {
                            var x = current.Y % 2 == 0 ? 0 : -1;
                            current = new Point(current.X + x, current.Y - 1);
                            break;
                        }
                    case Direction.West:
                        current = new Point(current.X - 1, current.Y);
                        break;
                    case Direction.NorthWest:
                        {
                            var x = current.Y % 2 == 0 ? 0 : -1;
                            current = new Point(current.X + x, current.Y + 1);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException($"Unexpected direction: {direction}");
                }
            }

            return current;
        }

        private int CountBlackTiles(List<List<Direction>> directions)
        {
            Dictionary<Point, int> turned = new Dictionary<Point, int>();

            foreach (var line in directions)
            {
                var destination = Move(line);

                if (turned.ContainsKey(destination))
                {
                    turned[destination] += 1;
                }
                else
                {
                    turned.Add(destination, 1);
                }
            }

            return turned.Values.Where(x => x % 2 != 0).Count();
        }

        public int FirstStar()
        {
            var input = ReadLineInput().ToList();
            var directions = Parse(input);

            var count = CountBlackTiles(directions);

            return count;
        }

        public int SecondStar()
        {
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();
            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData("esenee", 3, 0)]
        [InlineData("esew", 1, -1)]
        [InlineData("nwwswee", 0, 0)]
        public void FirstStarMoveTest(string input, int x, int y)
        {
            var directions = Parse(input);
            var result = Move(directions);
            Assert.Equal(new Point(x, y), result);
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "sesenwnenenewseeswwswswwnenewsewsw",
                "neeenesenwnwwswnenewnwwsewnenwseswesw",
                "seswneswswsenwwnwse",
                "nwnwneseeswswnenewneswwnewseswneseene",
                "swweswneswnenwsewnwneneseenw",
                "eesenwseswswnenwswnwnwsewwnwsene",
                "sewnenenenesenwsewnenwwwse",
                "wenwwweseeeweswwwnwwe",
                "wsweesenenewnwwnwsenewsenwwsesesenwne",
                "neeswseenwwswnwswswnw",
                "nenwswwsewswnenenewsenwsenwnesesenew",
                "enewnwewneswsewnwswenweswnenwsenwsw",
                "sweneswneswneneenwnewenewwneswswnese",
                "swwesenesewenwneswnwwneseswwne",
                "enesenwswwswneneswsenwnewswseenwsese",
                "wnwnesenesenenwwnenwsewesewsesesew",
                "nenewswnwewswnenesenwnesewesw",
                "eneswnwswnwsenenwnwnwwseeswneewsenese",
                "neswnwewnwnwseenwseesewsenwsweewe",
                "wseweeenwnesenwwwswnew"
            };

            var directions = Parse(input);

            var count = CountBlackTiles(directions);

            Assert.Equal(10, count);
        }
    }
}
