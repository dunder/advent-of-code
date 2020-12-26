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
            Point current = new Point(0, 0);

            foreach (var direction in directions)
            {
                current = Move(current, direction);
            }

            return current;
        }

        private Point Move(Point from, Direction direction)
        {
            switch (direction)
            {
                case Direction.NorthEast:
                    {
                        var x = from.Y % 2 == 0 ? 1 : 0;
                        from = new Point(from.X + x, from.Y + 1);
                        break;
                    }
                case Direction.East:
                    from = new Point(from.X + 1, from.Y);
                    break;
                case Direction.SouthEast:
                    {
                        var x = from.Y % 2 == 0 ? 1 : 0;
                        from = new Point(from.X + x, from.Y - 1);
                        break;
                    }
                case Direction.SouthWest:
                    {
                        var x = from.Y % 2 == 0 ? 0 : -1;
                        from = new Point(from.X + x, from.Y - 1);
                        break;
                    }
                case Direction.West:
                    from = new Point(from.X - 1, from.Y);
                    break;
                case Direction.NorthWest:
                    {
                        var x = from.Y % 2 == 0 ? 0 : -1;
                        from = new Point(from.X + x, from.Y + 1);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected direction: {direction}");
            }

            return from;
        }

        private static readonly List<Direction> Directions = new List<Direction> {
            Direction.NorthEast,
            Direction.East,
            Direction.SouthEast,
            Direction.SouthWest,
            Direction.West,
            Direction.NorthWest
        };

        public List<Point> Adjacent(Point to)
        {
            return Directions.Select(direction => Move(to, direction)).ToList();
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

        private int CountBlackTilesAfterDays(List<List<Direction>> directions, int days)
        {
            Dictionary<Point, bool> turned = new Dictionary<Point, bool>();

            foreach (var line in directions)
            {
                var destination = Move(line);

                if (turned.ContainsKey(destination))
                {
                    turned[destination] = !turned[destination];
                }
                else
                {
                    turned.Add(destination, true);
                }
            }

            var previousBlackTiles = new HashSet<Point>(turned.Where(t => t.Value).Select(t => t.Key));

            foreach (var i in Enumerable.Range(1, 100))
            {
                var nextBlackTiles = new HashSet<Point>();

                foreach (var blackTile in previousBlackTiles)
                {
                    var adjacent = Adjacent(blackTile);

                    var adjacentBlacks = adjacent.Where(a => previousBlackTiles.Contains(a)).Count();

                    var turn = adjacentBlacks == 0 || adjacentBlacks > 2;
                    if (!turn)
                    {
                        nextBlackTiles.Add(blackTile);
                    }

                    foreach (var adjacentWhiteTile in adjacent.Where(a => !previousBlackTiles.Contains(a)))
                    {
                        var adjacentToWhite = Adjacent(adjacentWhiteTile);
                        var adjacentToWhiteBlacks = adjacentToWhite.Where(a => previousBlackTiles.Contains(a)).Count();
                        if (adjacentToWhiteBlacks == 2)
                        {
                            nextBlackTiles.Add(adjacentWhiteTile);
                        }
                    }
                }

                previousBlackTiles = nextBlackTiles;
            }
            return previousBlackTiles.Count;
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
            var input = ReadLineInput().ToList();
            var directions = Parse(input);

            var count = CountBlackTilesAfterDays(directions, 100);

            return count;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(391, FirstStar());
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

        [Fact]
        public void SecondStarExample()
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

            var count = CountBlackTilesAfterDays(directions, 100);

            Assert.Equal(2208, count);
        }
    }
}
