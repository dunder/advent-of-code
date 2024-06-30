using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day X: Phrase ---
    public class Day10
    {
        private readonly ITestOutputHelper output;

        public Day10(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Position(int X, int Y);
        private enum Direction { North, East, South, West }

        private Dictionary<Position, char> Parse(IList<string> input)
        {
            var map = new Dictionary<Position, char>();
            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    char pipe = input[y][x];
                    map.Add(new Position(x, y), pipe);
                }
            }
            return map;
        }

        private record State(Position position, Direction direction);

        private static Direction NewDirection(Direction current, char pipe)
        {
            switch((pipe, current))
            {
                case ('|', _): return current;
                case ('-', _): return current;
                case ('L', Direction.South): return Direction.East;
                case ('L', Direction.West): return Direction.North;
                case ('J', Direction.South): return Direction.West;
                case ('J', Direction.East): return Direction.North;
                case ('7', Direction.North): return Direction.West;
                case ('7', Direction.East): return Direction.South;
                case ('F', Direction.North): return Direction.East;
                case ('F', Direction.West): return Direction.South;
                case ('S', _): return current;
                default: throw new ArgumentException($"Imossible piping: {current}, {pipe}");
            }
        }

        private static State Walk(Dictionary<Position, char> map, State state)
        {
            var (position,  direction) = state;

            var newPosition = direction switch
            {
                Direction.North => new Position(position.X, position.Y - 1),
                Direction.East => new Position(position.X + 1, position.Y),
                Direction.South => new Position(position.X, position.Y + 1),
                Direction.West => new Position(position.X - 1, position.Y),
                _ => throw new NotImplementedException(),
            };

            var newDirection = NewDirection(direction, map[newPosition]);

            return new State(newPosition, newDirection);

        }

        private int Steps(IList<string> input, Direction startDirection)
        {
            var map = Parse(input);

            var startValue = map.Single(kvp => kvp.Value == 'S');
            var start = startValue.Key;

            var visited = new HashSet<Position>();
            var state = new State(start, startDirection);

            while (visited.Add(state.position))
            {
                state = Walk(map, state);
            }
            return visited.Count / 2;
        }

        private int CalculateEnclosedTiles(IList<string> input, Direction startDirection, char startPipe)
        {
            var map = Parse(input);
            var startValue = map.Single(kvp => kvp.Value == 'S');
            var start = startValue.Key;
            map[start] = startPipe;
            var visited = new HashSet<Position>();
            var state = new State(start, startDirection);

            while (visited.Add(state.position))
            {
                state = Walk(map, state);
            }

            var inside = 0;

            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    var position = new Position(x, y);
                    if (visited.Contains(position))
                    {
                        continue;
                    }

                    int crossing = 0;
                    var rayx = x;
                    var rayy = y;

                    while (rayx < input[y].Length)
                    {
                        var rayPosition = new Position(rayx, rayy);
                        var tile = map[rayPosition];

                        if (visited.Contains(rayPosition) && (tile == 'L' || tile == 'J' || tile == '|'))
                        {
                            crossing++;
                        }

                        rayx = rayx + 1;
                        
                    }

                    if (crossing % 2 == 1)
                    {
                        inside++;
                    }
                }
            }

            return inside;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Steps(input, Direction.West);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return CalculateEnclosedTiles(input, Direction.West, '-');
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(6882, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(491, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                ".....",
                ".S-7.",
                ".|.|.",
                ".L-J.",
                "....."
            };

            Assert.Equal(4, Steps(example, Direction.East));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "...........",
                ".S-------7.",
                ".|F-----7|.",
                ".||.....||.",
                ".||.....||.",
                ".|L-7.F-J|.",
                ".|..|.|..|.",
                ".L--J.L--J.",
                "..........."
            };

            Assert.Equal(4, CalculateEnclosedTiles(example, Direction.East, 'F'));
        }

        [Fact]
        public void SecondStarExample2()
        {
            var example = new List<string>
            {
                ".F----7F7F7F7F-7....",
                ".|F--7||||||||FJ....",
                ".||.FJ||||||||L7....",
                "FJL7L7LJLJ||LJ.L-7..",
                "L--J.L7...LJS7F-7L7.",
                "....F-J..F7FJ|L7L7L7",
                "....L7.F7||L7|.L7L7|",
                ".....|FJLJ|FJ|F7|.LJ",
                "....FJL-7.||.||||...",
                "....L---J.LJ.LJLJ...",
            };

            Assert.Equal(8, CalculateEnclosedTiles(example, Direction.East, 'F'));
        }
    }
}
