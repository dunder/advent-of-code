
using Shared.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day X: Phrase ---
    public class Day23
    {
        private readonly ITestOutputHelper output;

        public Day23(ITestOutputHelper output)
        {
            this.output = output;
        }

        private Dictionary<(int, int), char> Parse(IList<string> input)
        {
            Dictionary<(int, int), char> map = new();

            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    map.Add((x, y), input[y][x]);
                }
            }

            return map;
        }

        private enum Direction { Up, Right, Down, Left }

        private record Position(int X, int Y)
        {
            public (int, int) Location => (X, Y);
        }

        private List<Node<Position>> Neighbors(Dictionary<(int, int), char> map, Node<Position> node)
        {
            (int x, int y) = node.Data;

            var up = (x, y - 1);
            var right = (x + 1, y);
            var down = (x, y + 1);
            var left = (x - 1, y);

            return new[] { up, right, down, left }
                .Where(map.ContainsKey)
                .Where(pos =>
                {
                    switch (map[pos])
                    {
                        case '#':
                            return false;
                        case '^':
                            return pos == up;
                        case '>':
                            return pos == right;
                        case 'v':
                            return pos == down;
                        case '<':
                            return pos == left;
                        case '.':
                            return true;
                        default:
                            return false;

                    }
                })
                .Select(pos => {
                    var newPosition = new Position(pos.Item1, pos.Item2);
                    return new Node<Position>(newPosition, node.Depth + 1, node);
                })
                .ToList();
        }

        public int Run1(IList<string> input)
        {
            Dictionary<(int, int), char> map = Parse(input);

            HashSet<Position> v = new();

            Stack<(Node<Position>, HashSet<Position>)> stack = new();

            List<Node<Position>> endNodes = new();

            var start = map.Where(kvp => kvp.Key.Item2 == 0 && kvp.Value == '.').Select(kvp => kvp.Key).Single();
            var end = map.Where(kvp => kvp.Key.Item2 == input.Count-1 && kvp.Value == '.').Select(kvp => kvp.Key).Single();
            
            stack.Push((new Node<Position>(new Position(start.Item1, start.Item2), 0), v));

            while (stack.Count > 0)
            {
                (Node<Position> current, HashSet<Position> visited) = stack.Pop();

                if (!visited.Add((current.Data)))
                {
                    continue;
                }

                //Draw(map, visited, stack, endNodes, input[0].Length, input.Count);

                if (current.Data.Location == end)
                {
                    endNodes.Add(current);
                    continue;
                }

                var neighbors = Neighbors(map, current).Where(pos => !visited.Contains(pos.Data));

                foreach (var neighbor in neighbors)
                {
                    stack.Push((neighbor, new HashSet<Position>(visited)));
                }
            }

            return endNodes.Select(node => node.Depth).Max();
        }

        private List<Node<Position>> Neighbors2(Dictionary<(int, int), char> map, Node<Position> node)
        {
            (int x, int y) = node.Data;

            var up = (x, y - 1);
            var right = (x + 1, y);
            var down = (x, y + 1);
            var left = (x - 1, y);

            return new[] { up, right, down, left }
                .Where(map.ContainsKey)
                .Where(pos =>
                {
                    switch (map[pos])
                    {
                        case '#':
                            return false;
                        case '^':
                        case '>':
                        case 'v':
                        case '<':
                        case '.':
                            return true;
                        default:
                            return false;

                    }
                })
                .Select(pos => {
                    var newPosition = new Position(pos.Item1, pos.Item2);
                    return new Node<Position>(newPosition, node.Depth + 1, node);
                })
                .ToList();
        }

        public int Run2(IList<string> input)
        {
            Dictionary<(int, int), char> map = Parse(input);

            HashSet<Position> v = new();

            Stack<(Node<Position>, HashSet<Position>)> stack = new();

            List<Node<Position>> endNodes = new();
            Dictionary<(int, int), int> currentBest = new();

            var start = map.Where(kvp => kvp.Key.Item2 == 0 && kvp.Value == '.').Select(kvp => kvp.Key).Single();
            var end = map.Where(kvp => kvp.Key.Item2 == input.Count - 1 && kvp.Value == '.').Select(kvp => kvp.Key).Single();

            stack.Push((new Node<Position>(new Position(start.Item1, start.Item2), 0), v));

            while (stack.Count > 0)
            {
                (Node<Position> current, HashSet<Position> visited) = stack.Pop();

                if (!visited.Add(current.Data))
                {
                    continue;
                }

                if (currentBest.ContainsKey(current.Data.Location) && currentBest[current.Data.Location] > current.Depth)
                {
                    continue;
                }

                if (current.Data.Location == end)
                {
                    endNodes.Add(current);

                    foreach (var node in current.Nodes)
                    {
                        currentBest[node.Data.Location] = node.Depth;
                    }

                    continue;
                }

                var neighbors = Neighbors2(map, current).Where(pos => !visited.Contains(pos.Data));

                foreach (var neighbor in neighbors)
                {
                    stack.Push((neighbor, new HashSet<Position>(visited)));
                }
            }

            return endNodes.Select(node => node.Depth).Max();
        }

        private void Draw(Dictionary<(int, int), char> map,
            HashSet<Position> visited, 
            Stack<(Node<Position>, HashSet<Position>)> stack,
            List<Node<Position>> endNodes,
            int xmax, 
            int ymax)
        {
            var visitedLocations = visited.Select(v => v.Location).ToHashSet();
            Console.WriteLine("");
            for (int y = 0; y < ymax; y++)
            {
                var line = new StringBuilder();

                for (int x = 0; x < xmax; x++)
                {
                    var position = (x, y);
                    if (visitedLocations.Contains(position))
                    {
                        Console.Write("O");
                    } 
                    else
                    {
                        Console.Write(map[position]);
                    }
                    
                }
                Console.Write("  ");
                for (int x = 0; x < xmax; x++)
                {
                    var position = (x, y);
                    Console.Write(map[position]);
                }

                Console.WriteLine(line.ToString());
            }
            Console.WriteLine("");
            Console.Write("Depths: ");
            Console.WriteLine(string.Join(", ", endNodes.Select(node => node.Depth)));
            Console.Write("Stack: ");
            Console.WriteLine(string.Join(", ", stack));
            Console.WriteLine("");
            Console.ReadKey();
            Console.Clear();
            Console.CursorTop = 0;
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
            Assert.Equal(-1, FirstStar());
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
                "#.#####################",
                "#.......#########...###",
                "#######.#########.#.###",
                "###.....#.>.>.###.#.###",
                "###v#####.#v#.###.#.###",
                "###.>...#.#.#.....#...#",
                "###v###.#.#.#########.#",
                "###...#.#.#.......#...#",
                "#####.#.#.#######.#.###",
                "#.....#.#.#.......#...#",
                "#.#####.#.#.#########v#",
                "#.#...#...#...###...>.#",
                "#.#.#v#######v###.###v#",
                "#...#.>.#...>.>.#.###.#",
                "#####v#.#.###v#.#.###.#",
                "#.....#...#...#.#.#...#",
                "#.#########.###.#.#.###",
                "#...###...#...#...#.###",
                "###.###.#.###v#####v###",
                "#...#...#.#.>.>.#.>.###",
                "#.###.###.#.###.#.#v###",
                "#.....###...###...#...#",
                "#####################.#"
            };

            Assert.Equal(94, Run1(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "#.#####################",
                "#.......#########...###",
                "#######.#########.#.###",
                "###.....#.>.>.###.#.###",
                "###v#####.#v#.###.#.###",
                "###.>...#.#.#.....#...#",
                "###v###.#.#.#########.#",
                "###...#.#.#.......#...#",
                "#####.#.#.#######.#.###",
                "#.....#.#.#.......#...#",
                "#.#####.#.#.#########v#",
                "#.#...#...#...###...>.#",
                "#.#.#v#######v###.###v#",
                "#...#.>.#...>.>.#.###.#",
                "#####v#.#.###v#.#.###.#",
                "#.....#...#...#.#.#...#",
                "#.#########.###.#.#.###",
                "#...###...#...#...#.###",
                "###.###.#.###v#####v###",
                "#...#...#.#.>.>.#.>.###",
                "#.###.###.#.###.#.#v###",
                "#.....###...###...#...#",
                "#####################.#"
            };

            Assert.Equal(154, Run2(example));

        }
    }
}
