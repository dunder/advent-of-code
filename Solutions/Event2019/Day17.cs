using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 17: Set and Forget ---
    public class Day17
    {
        private readonly ITestOutputHelper output;

        public Day17(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static bool IsIntersection(Point aPoint, Dictionary<Point, int> map)
        {
            return map[aPoint] == '#' && aPoint.AdjacentInMainDirections().All(p => map.ContainsKey(p) && map[p] == '#');
        }

        private void Print(Dictionary<Point, int> map, int width)
        {
            var height = map.Count / width;
            for (int y = 0; y < height; y++)
            {
                var line = new StringBuilder();
                for (int x = 0; x < width; x++)
                {
                    if (map.ContainsKey(new Point(x, y)))
                    { 
                        int mapPoint = map[new Point(x, y)];
                        line.Append((char)mapPoint);
                    }
                    
                }
                output.WriteLine(line.ToString());
            }
        }

        private List<long> RunMapProgram(string program)
        {
            var computer = IntCodeComputer.Load(program);
            while (computer.Execute() != IntCodeComputer.ExecutionState.Ready) {}

            return computer.Output.ToList();
        }

        private static Dictionary<Point, int> CreateMap(List<long> mapData, int width)
        {
            var map = new Dictionary<Point, int>();
            var y = 0;
            for (int i = 0; i < mapData.Count; i++)
            {
                int mapPoint = (int) mapData[i];
                if (mapPoint == '\n')
                {
                    y++;
                }

                var x = i - y * width - y;

                if (mapPoint != 10)
                {
                    map.Add(new Point(x, y), mapPoint);
                }
            }

            return map;
        }

        private static Dictionary<Point, int> ParseMap(IEnumerable<string> input)
        {
            var map = new Dictionary<Point, int>();
            var lines = input.ToList();
            for (int y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    map.Add(new Point(x,y), line[x]);
                }
            }

            return map;
        }

        private int SumOfAlignmentParameters(Dictionary<Point, int> map)
        {
            var conjunctions = map.Keys.Where(p => IsIntersection(p, map)).ToList();
            return conjunctions.Select(p => p.X * p.Y).Sum();
        }

        private (Dictionary<Point, int> map, int width) Run(string input)
        {
            var mapData = RunMapProgram(input);
            var width = mapData.IndexOf('\n');
            var map = CreateMap(mapData, width);
            return (map,width);
        }

        private long Run2(string program, Dictionary<Point, int> map)
        {
            var directionChars = new List<int>{'^', '<', '>', 'v'};

            var locationAndChar = map.Single(kvp => directionChars.Contains(kvp.Value));
            var start = locationAndChar.Key;
            var direction = ToDirection(locationAndChar.Value);

            // analyze map 
            var segments = new List<(Turn turn, int steps)>();
            var visited = new HashSet<Point>();
            while (true)
            {
                visited.Add(start);

                var surroundings = start.AdjacentInMainDirections()
                    .Where(a => !visited.Contains(a))
                    .Where(s => map.TryGetValue(s, out int data) && data == '#')
                    .ToList();
                
                if (!surroundings.Any(map.ContainsKey))
                {
                    break;
                }
                var open = surroundings.Single();
                Turn turn = BestTurn(direction, start, open);
                
                direction = direction.Turn(turn);
                var nextTurnPoint = NextTurnAt(direction, start, map);
                var steps = 0;
                while (start != nextTurnPoint)
                {
                    start = start.Move(direction);
                    visited.Add(start);
                    steps++;
                }
                segments.Add((turn, steps));
                start = nextTurnPoint;
            }

            // use this variable for visual inspection, try to program a solution later
            var readableSegments = string.Join(",", segments.Select(s => $"{s.turn.ToShortString()},{s.steps}"));

            program = "2" + new string(program.Skip(1).ToArray());
            var computer = IntCodeComputer.Load(program);

            var mainRoutine = "A,B,A,C,B,C,B,C,A,C\n";
            var routineA = "R,12,L,10,R,12\n";
            var routineB = "L,8,R,10,R,6\n";
            var routineC = "R,12,L,10,R,10,L,8\n";
            var no = "n\n";

            var input = mainRoutine + routineA + routineB + routineC + no;

            computer.ExecuteAscii(input);

            return computer.Output.Last();
        }

        private Direction ToDirection(int ascii)
        {
            switch (ascii)
            {
                case '^':
                    return Direction.North;
                case '>':
                    return Direction.East;
                case 'v':
                    return Direction.South;
                case '<':
                    return Direction.West;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown direction character");
            }
        }

        private static Turn BestTurn(Direction currentDirection, Point currentLocation, Point nextLocation)
        {
            var turns = new[] { Turn.Right, Turn.Left };

            foreach (var turn in turns)
            {
                var newDirection = currentDirection.Turn(turn);
                var newPosition = currentLocation.Move(newDirection);
                if (newPosition == nextLocation)
                {
                    return turn;
                }
            }

            throw new InvalidOperationException($"Impossible turn at {currentLocation} facing {currentDirection}");
        }

        private Point NextTurnAt(Direction direction, Point start, Dictionary<Point, int> map)
        {
            do
            {
                start = start.Move(direction);
            } while (map.TryGetValue(start.Move(direction), out int next) && next == '#');

            return start;
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var (map,width) = Run(input);

            Print(map, width);

            return SumOfAlignmentParameters(map);
        }

        public long SecondStar()
        {
            var input = ReadInput();
            var (map, _) = Run(input);


            return Run2(input, map);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(12512, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1409507, SecondStar());
        }

        [Fact]
        public void SecondStarExample()
        {
            var mapData = new[]
            {
                "#######...#####",
                "#.....#...#...#",
                "#.....#...#...#",
                "......#...#...#",
                "......#...###.#",
                "......#.....#.#",
                "^########...#.#",
                "......#.#...#.#",
                "......#########",
                "........#...#..",
                "....#########..",
                "....#...#......",
                "....#...#......",
                "....#...#......",
                "....#####......"
            };
            var input = ReadInput();

            var map = ParseMap(mapData);
            var result = Run2(input, map);

            Assert.Equal(1409507, result);
        }
    }
}
