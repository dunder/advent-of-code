using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;
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
            var result = "Not implemented";
            return result.ToString();
        }

        public static Dictionary<Point, HashSet<Point>> BuildMap(string input)
        {
            var rooms = new Dictionary<Point, HashSet<Point>>();
            var currentRoom = new Point(0, 0);
            rooms.Add(currentRoom, new HashSet<Point>());
            var branch = new Stack<Point>();

            foreach (var direction in input.Skip(1).Take(input.Length-2))
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
            // should we also add other way around?
        }

        public static int MaxDoorsShortestPath(string input)
        {
            var rooms = BuildMap(input);
            var output = Print(rooms);
            File.WriteAllLines(@".\maze.txt", output);
            return 0;
        }

        private static List<string> Print(Dictionary<Point, HashSet<Point>> rooms)
        {
            var minX = rooms.Keys.Min(k => k.X);
            var maxX = rooms.Keys.Max(k => k.X);
            var minY = rooms.Keys.Min(k => k.Y);
            var maxY = rooms.Keys.Max(k => k.Y);

            var lines = new List<string>();
            var startRoom = new Point(0, 0);

            for (int y = minY; y <= maxY; y++)
            {
                for (int y2 = 0; y2 < 3; y2++)
                {
                    var line = new StringBuilder();
                    for (int x = minX; x <= maxX; x++)
                    {
                        var room = new Point(x, y);
                        if (y2 == 0 && y == minY)
                        {
                            // #, north, #
                            line.Append('#');
                            var print = rooms[room].Contains(room.Move(Direction.North)) ? "-" : "#";
                            line.Append(print);
                            if (x == minX)
                            { 
                                line.Append('#');
                            }
                        }
                        else if (y2 == 1)
                        {
                            // west, ., east
                            var print = rooms[room].Contains(room.Move(Direction.West)) ? "|" : "#";
                            line.Append(print);
                            var center = room == startRoom ? 'X' : '.';
                            line.Append(center);
                            if (x == minX)
                            {
                                print = rooms[room].Contains(room.Move(Direction.East)) ? "|" : "#";
                                line.Append(print);
                            }
                        }
                        else
                        {
                            // #, south, #
                            line.Append('#');
                            var print = rooms[room].Contains(room.Move(Direction.South)) ? "-" : "#";
                            line.Append(print);
                            if (x == minX)
                            {
                                line.Append('#');
                            }
                        }
                    }
                    lines.Add(line.ToString());
                }
            }

            return lines;
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual);
        }

    }
}