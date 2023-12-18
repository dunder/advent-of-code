using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 17: Clumsy Crucible ---
    public class Day17
    {
        private readonly ITestOutputHelper output;

        public Day17(ITestOutputHelper output)
        {
            this.output = output;
        }

        private enum Direction { Up, Right, Down, Left };

        
        private record Position(int X, int Y, Direction Direction, int SinceTurn);
        private record HeatedPosition(int X, int Y, Direction Direction, int SinceTurn, int AccumulatedHeat, HeatedPosition Parent)
        {
            public Position Position => new Position(X, Y, Direction, SinceTurn);
        }

        private record HeatLossMap(Dictionary<(int, int), int> Data, int Width, int Height)
        {
            public bool IsStartingPoint(int x, int y)
            {
                return x == 0 && y == 0;
            }

            public bool IsDestinationPoint(int x, int y)
            {
                return x == Width-1 && y == Height-1;
            }

            public bool IsWithin((int x, int y) location)
            {
                (int x, int y) = location;
                return x >= 0 && x < Width && y >= 0 && y < Height;
            }
        }

        private HeatLossMap Parse(IList<string> input)
        {
            Dictionary<(int,int), int> heatLossMap = new();

            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    heatLossMap.Add((x,y), input[y][x] - '0');
                }
            }

            return new HeatLossMap(heatLossMap, input.First().Length, input.Count);
        }

        private bool MustTurn(Position position)
        {
            return position.SinceTurn > 1;
        }

        private static (int x, int y) Next(Direction direction, int x, int y) => direction switch
        {
            Direction.Up => (x, y - 1),
            Direction.Right => (x + 1, y),
            Direction.Down => (x, y + 1),
            Direction.Left => (x - 1, y),
        };

        private static Direction TurnRightFrom(Direction direction) => direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
        };

        private static Direction TurnLeftFrom(Direction direction) => direction switch
        {
            Direction.Up => Direction.Left,
            Direction.Right => Direction.Up,
            Direction.Down => Direction.Right,
            Direction.Left => Direction.Down,
        };

        private IEnumerable<HeatedPosition> Neighbors(HeatedPosition position, HeatLossMap heatLossMap)
        {
            var straight = position.Direction switch
            {
                Direction.Up => Next(Direction.Up, position.X, position.Y),
                Direction.Right => Next(Direction.Right, position.X, position.Y),
                Direction.Down => Next(Direction.Down, position.X, position.Y),
                Direction.Left => Next(Direction.Left, position.X, position.Y),
            };

            var right = position.Direction switch
            {
                Direction.Up => Next(TurnRightFrom(position.Direction), position.X, position.Y),
                Direction.Right => Next(TurnRightFrom(position.Direction), position.X, position.Y),
                Direction.Down => Next(TurnRightFrom(position.Direction), position.X, position.Y),
                Direction.Left => Next(TurnRightFrom(position.Direction), position.X, position.Y),
            };

            var left = position.Direction switch
            {
                Direction.Up => Next(TurnLeftFrom(position.Direction), position.X, position.Y),
                Direction.Right => Next(TurnLeftFrom(position.Direction), position.X, position.Y),
                Direction.Down => Next(TurnLeftFrom(position.Direction), position.X, position.Y),
                Direction.Left => Next(TurnLeftFrom(position.Direction), position.X, position.Y),
            };

            HeatedPosition NewPosition((int x, int y) location, Direction direction, bool turn = false)
            {
                (int x , int y) = location;
                var heatLoss = heatLossMap.Data[(x, y)];

                return position with { 
                    X = location.x, 
                    Y = location.y, 
                    Direction = direction,
                    SinceTurn = turn ? 0 : position.SinceTurn + 1,
                    AccumulatedHeat = position.AccumulatedHeat + heatLoss,
                    Parent = position};
            }
            
            if (!MustTurn(position.Position) && heatLossMap.IsWithin(straight))
            {
                yield return NewPosition(straight, position.Direction);
            }
            
            if (heatLossMap.IsWithin(right))
            {
                yield return NewPosition(right, TurnRightFrom(position.Direction), true);
            }
            
            if (heatLossMap.IsWithin(left))
            {
                yield return NewPosition(left, TurnLeftFrom(position.Direction), true);
            }
        }     

        private int Djikstra(HeatLossMap map)
        {
            PriorityQueue<HeatedPosition, int> q = new();

            q.Enqueue(new HeatedPosition(0, 0, Direction.Right, 0, 0, null), 0);
            q.Enqueue(new HeatedPosition(0, 0, Direction.Down, 0, 0, null), 0);

            HashSet<Position> visited = new();

            HeatedPosition u = null;

            while(q.Count > 0)
            {
                u = q.Dequeue();

                if (map.IsDestinationPoint(u.X, u.Y))
                {
                    break;
                }

                if (!visited.Add(u.Position))
                {
                    continue;
                }

                var neighbors = Neighbors(u, map).Where(n => !visited.Contains(n.Position)).ToList();

                foreach (var neighbor in neighbors)
                {
                    q.Enqueue(neighbor, neighbor.AccumulatedHeat);
                }
            }

            return u.AccumulatedHeat;
        }


        private IEnumerable<HeatedPosition> Neighbors2(HeatedPosition position, HeatLossMap heatLossMap)
        {
            var straight = position.Direction switch
            {
                Direction.Up => Next(Direction.Up, position.X, position.Y),
                Direction.Right => Next(Direction.Right, position.X, position.Y),
                Direction.Down => Next(Direction.Down, position.X, position.Y),
                Direction.Left => Next(Direction.Left, position.X, position.Y),
            };

            var right = position.Direction switch
            {
                Direction.Up => Next(TurnRightFrom(position.Direction), position.X, position.Y),
                Direction.Right => Next(TurnRightFrom(position.Direction), position.X, position.Y),
                Direction.Down => Next(TurnRightFrom(position.Direction), position.X, position.Y),
                Direction.Left => Next(TurnRightFrom(position.Direction), position.X, position.Y),
            };

            var left = position.Direction switch
            {
                Direction.Up => Next(TurnLeftFrom(position.Direction), position.X, position.Y),
                Direction.Right => Next(TurnLeftFrom(position.Direction), position.X, position.Y),
                Direction.Down => Next(TurnLeftFrom(position.Direction), position.X, position.Y),
                Direction.Left => Next(TurnLeftFrom(position.Direction), position.X, position.Y),
            };

            HeatedPosition NewPosition((int x, int y) location, Direction direction, bool turn = false)
            {
                (int x, int y) = location;
                var heatLoss = heatLossMap.Data[(x, y)];

                return position with
                {
                    X = location.x,
                    Y = location.y,
                    Direction = direction,
                    SinceTurn = turn ? 0 : position.SinceTurn + 1,
                    AccumulatedHeat = position.AccumulatedHeat + heatLoss,
                    Parent = position
                };
            }

            if (position.SinceTurn < 8 && heatLossMap.IsWithin(straight))
            {
                yield return NewPosition(straight, position.Direction);
            }

            if (position.SinceTurn > 2 && heatLossMap.IsWithin(right))
            {
                yield return NewPosition(right, TurnRightFrom(position.Direction), true);
            }

            if (position.SinceTurn > 2 && heatLossMap.IsWithin(left))
            {
                yield return NewPosition(left, TurnLeftFrom(position.Direction), true);
            }
        }

        private int Djikstra2(HeatLossMap map)
        {
            PriorityQueue<HeatedPosition, int> q = new();

            q.Enqueue(new HeatedPosition(0, 0, Direction.Right, 1, 0, null), 0);
            q.Enqueue(new HeatedPosition(0, 0, Direction.Down, 1, 0, null), 0);

            HashSet<Position> visited = new();

            HeatedPosition u = null;

            while(q.Count > 0)
            {
                u = q.Dequeue();

                if (map.IsDestinationPoint(u.X, u.Y) && u.Position.SinceTurn > 2)
                {
                    break;
                }

                if (!visited.Add(u.Position))
                {
                    continue;
                }

                var neighbors = Neighbors2(u, map).Where(n => !visited.Contains(n.Position)).ToList();

                foreach (var neighbor in neighbors)
                {
                    q.Enqueue(neighbor, neighbor.AccumulatedHeat);
                }
            }

            return u.AccumulatedHeat;
        }


        private int Run1(IList<string> input)
        {
            HeatLossMap heatLossMap = Parse(input);
            
            return Djikstra(heatLossMap);
        }

        private int Run2(IList<string> input)
        {
            HeatLossMap heatLossMap = Parse(input);

            return Djikstra2(heatLossMap);
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Run1(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return Run2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            // 740 your answer is too low
            // 799 your answer is too high 
            Assert.Equal(791, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "2413432311323",
                "3215453535623",
                "3255245654254",
                "3446585845452",
                "4546657867536",
                "1438598798454",
                "4457876987766",
                "3637877979653",
                "4654967986887",
                "4564679986453",
                "1224686865563",
                "2546548887735",
                "4322674655533"
            };

            Assert.Equal(102, Run1(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "2413432311323",
                "3215453535623",
                "3255245654254",
                "3446585845452",
                "4546657867536",
                "1438598798454",
                "4457876987766",
                "3637877979653",
                "4654967986887",
                "4564679986453",
                "1224686865563",
                "2546548887735",
                "4322674655533"
            };

            Assert.Equal(94, Run2(example));
        }

        [Fact]
        public void SecondStarExample2()
        {
            var example = new List<string>
            {
                "111111111111",
                "999999999991",
                "999999999991",
                "999999999991",
                "999999999991"
            };

            Assert.Equal(71, Run2(example));
        }
    }
}
