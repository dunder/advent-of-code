using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;
using Shared.MapGeometry;
using Shared.Tree;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day20 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day20;
        public string Name => "A Regular Map";

        public string FirstStar()
        {
            var input = ReadInput();
            var result = MaxDoorsShortestPath(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = MaxDoors1000(input);
            return result.ToString();
        }

        public static Dictionary<Point, HashSet<Point>> BuildMap(string input)
        {
            var rooms = new Dictionary<Point, HashSet<Point>>();
            var currentRoom = new Point(0, 0);
            rooms.Add(currentRoom, new HashSet<Point>());
            var branch = new Stack<Point>();

            foreach (var direction in input.Skip(1).Take(input.Length - 2))
            {
                switch (direction)
                {
                    case 'N':
                        currentRoom = AddDoor(currentRoom, Direction.North, rooms);
                        break;
                    case 'E':
                        currentRoom = AddDoor(currentRoom, Direction.East, rooms);
                        break;
                    case 'S':
                        currentRoom = AddDoor(currentRoom, Direction.South, rooms);
                        break;
                    case 'W':
                        currentRoom = AddDoor(currentRoom, Direction.West, rooms);
                        break;
                    case '(':
                        branch.Push(currentRoom);
                        break;
                    case ')':
                        currentRoom = branch.Pop();
                        break;
                    case '|':
                        currentRoom = branch.Peek();
                        break;
                }
            }

            return rooms;
        }

        private static Point AddDoor(Point room, Direction direction, Dictionary<Point, HashSet<Point>> rooms)
        {
            var next = room.Move(direction);
            if (!rooms.ContainsKey(room))
            {
                rooms.Add(room, new HashSet<Point>());
            }

            rooms[room].Add(next);

            if (!rooms.ContainsKey(next))
            {
                rooms.Add(next, new HashSet<Point>());
            }

            rooms[next].Add(room);

            return next;
        }

        public static int MaxDoorsShortestPath(string input)
        {
            return PathsOnMap(input).OrderBy(s => s.Depth).Last().Depth;
        }

        public static int MaxDoors1000(string input)
        {
            return PathsOnMap(input).Count(s => s.Depth >= 1000);
        }

        public static IEnumerable<Shared.Tree.Node<Point>> PathsOnMap(string input)
        {
            var rooms = BuildMap(input);

            IEnumerable<Point> Neighbors(Point point)
            {
                return rooms[point];
            }

            var start = new Point(0, 0);
            var (paths, _) = start.DepthFirst(Neighbors);

            return paths;
        }

        private static List<string> Print(Dictionary<Point, HashSet<Point>> rooms, Node<Point> endNode = null)
        {
            var pathPoints = endNode?.Path.ToHashSet();

            var minX = rooms.Keys.Min(k => k.X);
            var maxX = rooms.Keys.Max(k => k.X);
            var minY = rooms.Keys.Min(k => k.Y);
            var maxY = rooms.Keys.Max(k => k.Y);

            var lines = new List<string>();
            var startRoom = new Point(0, 0);

            for (int y = minY; y <= maxY; y++)
            {
                // build 3 rows to fit surrounding walls and doors
                for (int row = 0; row < 3; row++)
                {
                    // rows share walls except for the first and last, print all three for first row
                    // and just center and bottom for the rest
                    if ((row == 0 && y == minY) || row > 0)
                    {
                        var line = new StringBuilder();
                        for (int x = minX; x <= maxX; x++)
                        {
                            var room = new Point(x, y);
                            if (row == 0)
                            {
                                // # , north , #
                                if (x == minX)
                                {
                                    line.Append('#');

                                }

                                var print = rooms[room].Contains(room.Move(Direction.North)) ? "-" : "#";
                                line.Append(print);
                                line.Append('#');
                            }
                            else if (row == 1)
                            {
                                // west , . , east

                                if (x == minX)
                                {
                                    var west = rooms[room].Contains(room.Move(Direction.West)) ? "|" : "#";
                                    line.Append(west);
                                }

                                var center = room == startRoom ? 'X' : '.';
                                if (pathPoints != null && pathPoints.Contains(room))
                                {
                                    center = 'O';
                                }
                                line.Append(center);

                                var east = rooms[room].Contains(room.Move(Direction.East)) ? "|" : "#";
                                line.Append(east);
                            }
                            else
                            {
                                // # , south , #
                                if (x == minX)
                                {
                                    line.Append('#');
                                }

                                var print = rooms[room].Contains(room.Move(Direction.South)) ? "-" : "#";
                                line.Append(print);
                                line.Append('#');
                            }
                        }

                        lines.Add(line.ToString());
                    }
                }
            }

            return lines;
        }

        [Fact]
        public void FirstStarMapExample1()
        {
            var input = "^ENWWW(NEEE|SSE(EE|N))$";

            var rooms = BuildMap(input);
            var output = Print(rooms);

            var expected = new List<string>
            {
                "#########",
                "#.|.|.|.#",
                "#-#######",
                "#.|.|.|.#",
                "#-#####-#",
                "#.#.#X|.#",
                "#-#-#####",
                "#.|.|.|.#",
                "#########"
            };

            Assert.Equal(expected, output);
        }

        [Fact]
        public void FirstStarMapExample2()
        {
            var input = "^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$";

            var rooms = BuildMap(input);
            var output = Print(rooms);

            var expected = new List<string>
            {
                "#############",
                "#.|.|.|.|.|.#",
                "#-#####-###-#",
                "#.#.|.#.#.#.#",
                "#-#-###-#-#-#",
                "#.#.#.|.#.|.#",
                "#-#-#-#####-#",
                "#.#.#.#X|.#.#",
                "#-#-#-###-#-#",
                "#.|.#.|.#.#.#",
                "###-#-###-#-#",
                "#.|.#.|.|.#.#",
                "#############",
            };

            Assert.Equal(expected, output);
        }

        [Fact]
        public void FirstStarMapExample3()
        {
            var input = "^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$";

            var rooms = BuildMap(input);
            var output = Print(rooms);

            var expected = new List<string>
            {
                "###############",
                "#.|.|.|.#.|.|.#",
                "#-###-###-#-#-#",
                "#.|.#.|.|.#.#.#",
                "#-#########-#-#",
                "#.#.|.|.|.|.#.#",
                "#-#-#########-#",
                "#.#.#.|X#.|.#.#",
                "###-#-###-#-#-#",
                "#.|.#.#.|.#.|.#",
                "#-###-#####-###",
                "#.|.#.|.|.#.#.#",
                "#-#-#####-#-#-#",
                "#.#.|.|.|.#.|.#",
                "###############",
            };

            Assert.Equal(expected, output);
        }

        [Fact]
        public void FirstStarExample1()
        {
            var input = "^ENWWW(NEEE|SSE(EE|N))$";

            var max = MaxDoorsShortestPath(input);

            Assert.Equal(10, max);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = "^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$";

            var max = MaxDoorsShortestPath(input);

            Assert.Equal(23, max);
        }

        [Fact]
        public void FirstStarExample3()
        {
            var input = "^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$";

            var max = MaxDoorsShortestPath(input);

            Assert.Equal(31, max);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("4274", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("8547", actual);
        }
    }
}
