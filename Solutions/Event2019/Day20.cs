using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.Extensions;
using Shared.MapGeometry;
using Shared.Tree;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 20: A Regular Map ---
    public class Day20
    {
        private Dictionary<Point, char> ParseMap(List<string> input) 
        {
            var portals = new Dictionary<Point, char>();

            var width = input.First().Length;
            var height = input.Count;

            for (int y = 0; y < height; y++)
            {
                var line = input[y];
                for (int x = 0; x < width; x++)
                {
                    var c = line[x];
                    portals.Add(new Point(x-2,y-2), c);
                }
            }

            return portals;
        }

        private static bool AdjacentToPortal(Point p, Dictionary<Point, char> mapData)
        {
            var neighbors = p.AdjacentInMainDirections();
            return neighbors.Any(n => mapData.TryGetValue(n, out char data) && char.IsLetter(data));
        }

        private static string PortalName(Direction direction, char first, char second)
        {
            switch (direction)
            {
                case Direction.East:
                case Direction.South:
                {
                    return string.Join("", first, second);
                }
                case Direction.North:
                case Direction.West:
                {
                    return string.Join("", second, first);
                }
                default:
                    throw new InvalidOperationException($"Unmatched Direction enum {(int) direction}");
            }
        }
        private string ParsePortalName(Point p, Dictionary<Point, char> mapData)
        {
            var directions = new[] {Direction.North, Direction.East, Direction.South, Direction.West};

            foreach (var direction in directions)
            {
                var adjacent = p.Move(direction);
                if (mapData.TryGetValue(adjacent, out char data) && char.IsLetter(data))
                {
                    var otherChar = mapData[adjacent.Move(direction)];
                    return PortalName(direction, data, otherChar);
                }
            }
            throw new ArgumentOutOfRangeException($"Point did not have adjacent portal {p}");
        }

        private const string StartLabel = "AA";
        private const string ExitLabel = "ZZ";

        private (Dictionary<Point, string>, Point,Point) ParseLabels(Dictionary<Point, char> mapData)
        {
            var portals = new Dictionary<Point, string>();
            var nextToPortal = mapData.Where(d => d.Value == '.' && AdjacentToPortal(d.Key, mapData)).Select(m => m.Key);
            Point start = new Point();
            Point exit = new Point();
            foreach (var tile in nextToPortal)
            {
                var portal = ParsePortalName(tile, mapData);

                if (portal == StartLabel)
                {
                    start = tile;
                } 
                else if (portal == ExitLabel)
                {
                    exit = tile;
                }
                else
                {
                    portals.Add(tile, portal);
                }
            }

            return (portals, start, exit);
        }

        private Dictionary<Point, List<Point>> CreateNeighborMap(Dictionary<Point, char> mapData, Dictionary<Point, string> portals)
        {
            var neighborMap = new Dictionary<Point, List<Point>>();

            foreach (var mapEntry in mapData)
            {
                if (mapEntry.Value == '.')
                {
                    var neighbors = new List<Point>();

                    foreach (var potentialNeighbor in mapEntry.Key.AdjacentInMainDirections())
                    {
                        
                        if (mapData.TryGetValue(potentialNeighbor, out char c) && c == '.')
                        {
                            neighbors.Add(potentialNeighbor);
                        }
                    }

                    neighborMap.Add(mapEntry.Key, neighbors);
                    
                    if (portals.TryGetValue(mapEntry.Key, out string portal))
                    {
                        var portalExit = portals.Single(p => p.Key != mapEntry.Key && p.Value == portal).Key;
                        neighborMap[mapEntry.Key].Add(portalExit);
                    }
                }
            }

            return neighborMap;
        }

        private int ShortestPath(IEnumerable<string> input)
        {
            var mapData = ParseMap(input.ToList());
            var (portals, start, exit) = ParseLabels(mapData);
            var neighbors = CreateNeighborMap(mapData, portals);

            var (depthFirst, _) = start.DepthFirst(p => neighbors[p], n => n.Data == exit, true);
            var actualPaths = depthFirst.Where(n => n.Data == exit);
            return actualPaths.OrderBy(n => n.Depth).First().Depth;
        }


        public int FirstStar()
        {
            var input = ReadLineInput();
            return ShortestPath(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
            //Assert.Equal(1187, FirstStar()); // too high
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample1()
        {
            var input = new[]
            {
                "         A           ",
                "         A           ",
                "  #######.#########  ",
                "  #######.........#  ",
                "  #######.#######.#  ",
                "  #######.#######.#  ",
                "  #######.#######.#  ",
                "  #####  B    ###.#  ",
                "BC...##  C    ###.#  ",
                "  ##.##       ###.#  ",
                "  ##...DE  F  ###.#  ",
                "  #####    G  ###.#  ",
                "  #########.#####.#  ",
                "DE..#######...###.#  ",
                "  #.#########.###.#  ",
                "FG..#########.....#  ",
                "  ###########.#####  ",
                "             Z       ",
                "             Z       "
            };

            var shortest = ShortestPath(input);

            Assert.Equal(23, shortest);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = new[]
            {
              "                   A               ",
              "                   A               ",
              "  #################.#############  ",
              "  #.#...#...................#.#.#  ",
              "  #.#.#.###.###.###.#########.#.#  ",
              "  #.#.#.......#...#.....#.#.#...#  ",
              "  #.#########.###.#####.#.#.###.#  ",
              "  #.............#.#.....#.......#  ",
              "  ###.###########.###.#####.#.#.#  ",
              "  #.....#        A   C    #.#.#.#  ",
              "  #######        S   P    #####.#  ",
              "  #.#...#                 #......VT",
              "  #.#.#.#                 #.#####  ",
              "  #...#.#               YN....#.#  ",
              "  #.###.#                 #####.#  ",
              "DI....#.#                 #.....#  ",
              "  #####.#                 #.###.#  ",
              "ZZ......#               QG....#..AS",
              "  ###.###                 #######  ",
              "JO..#.#.#                 #.....#  ",
              "  #.#.#.#                 ###.#.#  ",
              "  #...#..DI             BU....#..LF",
              "  #####.#                 #.#####  ",
              "YN......#               VT..#....QG",
              "  #.###.#                 #.###.#  ",
              "  #.#...#                 #.....#  ",
              "  ###.###    J L     J    #.#.###  ",
              "  #.....#    O F     P    #.#...#  ",
              "  #.###.#####.#.#####.#####.###.#  ",
              "  #...#.#.#...#.....#.....#.#...#  ",
              "  #.#####.###.###.#.#.#########.#  ",
              "  #...#.#.....#...#.#.#.#.....#.#  ",
              "  #.###.#####.###.###.#.#.#######  ",
              "  #.#.........#...#.............#  ",
              "  #########.###.###.#############  ",
              "           B   J   C               ",
              "           U   P   P               ", 
            };

            var shortest = ShortestPath(input);

            Assert.Equal(58, shortest);
        }
    }
}
