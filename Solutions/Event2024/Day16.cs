using Shared.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 16: Phrase ---
    public class Day16
    {
        private readonly ITestOutputHelper output;

        public Day16(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static (char[,], (int x, int y), (int x, int y)) Parse(IList<string> input)
        {
            char[,] map = new char[input.First().Length, input.Count];

            (int x, int y) start = (0, 0);
            (int x, int y) end = (0, 0);

            for (int y = 0; y < input.Count; y++)
            {
                string line = input[y];
                for (int x = 0; x < input.First().Length; x++)
                {
                    if (line[x] == 'S')
                    {
                        start = (x, y);
                    }
                    else if (line[x] == 'E')
                    {
                        end = (x, y);

                    }
                    map[x, y] = line[x];
                }
            }

            return (map, start, end);
        }

        public static (int x, int y) Move((int x, int y) position, int direction) => direction switch
        {
            0 => (position.x, position.y + 1),
            1 => (position.x + 1, position.y),
            2 => (position.x, position.y - 1),
            3 => (position.x - 1, position.y),
            _ => throw new InvalidOperationException()
        };

        private static bool WithinBounds(char[,] map, (int x, int y) location)
        {
            (int x, int y) = location;
            (int xmax, int ymax) = (map.GetLength(0), map.GetLength(1));

            return x >= 0 && x < xmax && y >= 0 && y < ymax;
        }

        private static List<((int x, int y, int d) position, int cost)> Neighbors(char[,] map, (int x, int y, int d) position)
        {
            (int x, int y, int d) = position;

            int rightDirection = (d + 1) % 4;
            int leftDirection = (d - 1) < 0 ? 3 : (d - 1);

            List<((int x, int y, int d), int cost)> neighbors = 
            [
                ((x, y, rightDirection), 1000),
                ((x, y, leftDirection), 1000),
            ];

            (int fx, int fy) = Move((x, y), d);

            if (WithinBounds(map, (fx, fy)) && map[fx, fy] != '#')
            {
                neighbors.Add(((fx, fy, d), 1));
            }

            return neighbors;
        }

        private static int Problem1(IList<string> input)
        {
            (char[,] map, (int x, int y) start, (int x, int y) end) = Parse(input);

            if (map.GetLength(0) != map.GetLength(1))
            {
                throw new ArgumentOutOfRangeException($"Map must be a square");
            }

            var mapDimension = map.GetLength(0);

            var queue = new PriorityQueue<(int x, int y, int d), int>();

            var startNode = (start.x, start.y, 1);

            queue.Enqueue(startNode, 0);

            var distances = new Dictionary<(int x, int y, int d), int>
            {
                { startNode, 0 }
            };

            var prev = new Dictionary<(int x, int y, int d), (int x, int y, int d)>();

            int Distance((int x, int y, int d) position)
            {
                if (distances.TryGetValue(position, out int distance))
                {
                    return distances[position];
                }
                else
                {
                    return int.MaxValue;
                }
            }

            // djikstra https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var neighbor in Neighbors(map, current))
                {
                    (var pos, int cost) = neighbor;

                    var alt = Distance(current) + cost;

                    if (alt < Distance(pos))
                    {
                        prev[pos] = current;
                        distances[pos] = alt;
                        queue.Enqueue(pos, alt);
                    }
                }
            }

            var shortestDistance = Distance((end.x, end.y, 0));

            shortestDistance = Math.Min(shortestDistance, Distance((end.x, end.y, 1)));
            shortestDistance = Math.Min(shortestDistance, Distance((end.x, end.y, 2)));
            shortestDistance = Math.Min(shortestDistance, Distance((end.x, end.y, 3)));

            return shortestDistance;
        }

        private record Deer(int X, int Y, int D, int Score);

        private static int Problem2(IList<string> input, int min)
        {


            //(char[,] map, (int x, int y) start, (int x, int y) end) = Parse(input);

            //if (map.GetLength(0) != map.GetLength(1))
            //{
            //    throw new ArgumentOutOfRangeException($"Map must be a square");
            //}

            //var visited = new HashSet<Deer>();

            //var queue = new Queue<Node<Deer>>();

            //queue.Enqueue(new Node<Deer>(new Deer(start.x, start.y, 1, 0), 0));

            //List<Deer> DeerNeighbors(Deer deer)
            //{
            //    (int x, int y, int d) = (deer.X, deer.Y, deer.D);

            //    int rightDirection = (d + 1) % 4;
            //    int leftDirection = (d - 1) < 0 ? 3 : (d - 1);

            //    List<Deer> neighbors =
            //    [
            //        new Deer(x, y, rightDirection, deer.Score + 1000),
            //        new Deer(x, y, leftDirection, deer.Score + 1000),
            //    ];

            //    (int fx, int fy) = Move((x, y), d);

            //    if (WithinBounds(map, (fx, fy)) && map[fx, fy] != '#')
            //    {
            //        neighbors.Add(new Deer(fx, fy, d, deer.Score + 1));
            //    }

            //    return neighbors;
            //}

            //int minScore = min;

            //List<Node<Deer>> paths = new ();

            //while (queue.Count != 0)
            //{
            //    var current = queue.Dequeue();

            //    if (!visited.Add(current.Data))
            //    {
            //        continue;
            //    }

            //    if (current.Data.X == end.x && current.Data.Y == end.y)
            //    {
            //        minScore = Math.Min(minScore, current.Data.Score);
            //        paths.Add(current);
            //        continue;
            //    }

            //    var neighbors = DeerNeighbors(current.Data).Where(n => !visited.Contains(n)).ToList();

            //    foreach (var neighbor in neighbors)
            //    {
            //        if (neighbor.Score > minScore)
            //        {
            //            continue;
            //        }

            //        queue.Enqueue(new Node<Deer>(neighbor, current.Depth + 1, current));
            //    }
            //}

            //var result = paths.SelectMany(p => p.Nodes.Select(n => (n.Data.X, n.Data.Y)));

            //return paths.SelectMany(p => p.Nodes.Select(n => (n.Data.X, n.Data.Y))).ToHashSet().Count();
            return 0;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(105496, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input, 105496));
        }

        private List<string> exampleInput =
            [
                "###############",
                "#.......#....E#",
                "#.#.###.#.###.#",
                "#.....#.#...#.#",
                "#.###.#####.#.#",
                "#.#.#.......#.#",
                "#.#.#####.###.#",
                "#...........#.#",
                "###.#.#####.#.#",
                "#...#.....#.#.#",
                "#.#.#.###.#.#.#",
                "#.....#...#.#.#",
                "#.###.#.#.#.#.#",
                "#S..#.....#...#",
                "###############",
            ];

        private List<string> exampleInput2 =
            [
                "#################",
                "#...#...#...#..E#",
                "#.#.#.#.#.#.#.#.#",
                "#.#.#.#...#...#.#",
                "#.#.#.#.###.#.#.#",
                "#...#.#.#.....#.#",
                "#.#.#.#.#.#####.#",
                "#.#...#.#.#.....#",
                "#.#.#####.#.###.#",
                "#.#.#.......#...#",
                "#.#.###.#####.###",
                "#.#.#...#.....#.#",
                "#.#.#.#####.###.#",
                "#.#.#.........#.#",
                "#.#.#.#########.#",
                "#S#.............#",
                "#################",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(7036, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample2()
        {
            Assert.Equal(11048, Problem1(exampleInput2));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(-1, Problem2(exampleInput, 7036));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample2()
        {
            Assert.Equal(-1, Problem2(exampleInput2, 11048));
        }
    }
}
