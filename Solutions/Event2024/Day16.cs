using Shared.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public static (int x, int y, int d) Move(int x, int y, int d) => d switch
        {
            0 => (x, y - 1, d),
            1 => (x + 1, y, d),
            2 => (x, y + 1, d),
            3 => (x - 1, y, d),
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

            int dr = (d + 1) % 4;
            int dl = (d - 1) < 0 ? 3 : (d - 1);

            (int x, int y, int d) forward = Move(x, y, d);
            (int x, int y, int d) right = Move(x, y, dr);
            (int x, int y, int d) left = Move(x, y, dl);


            List<((int x, int y, int d) pos, int cost)> neighbors = 
            [
                (right, 1001),
                (left, 1001),
                (forward, 1)
            ];

            var ns = neighbors.Where(n => WithinBounds(map, (n.pos.x, n.pos.x)) && map[n.pos.x, n.pos.y] != '#').ToList();

            return ns;
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

            return Enumerable.Range(0, 4).Select(d => Distance((end.x, end.y, d))).Min();
        }




        private void Print(char[,] map, HashSet<(int x, int y)> visited)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            var s = new StringBuilder();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (visited.Contains((x,y)))
                    {
                        s.Append('O');
                    }
                    else
                    {
                        s.Append(map[x, y]);
                    }
                }
                s.AppendLine();
            }

            output.WriteLine(s.ToString());
        }

        private int Problem2(IList<string> input)
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

            var prev = new Dictionary<(int x, int y, int d), List<(int x, int y, int d)>>();

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

                    if (alt == Distance(pos))
                    {
                        prev[pos].Add(current);
                    }

                    if (alt < Distance(pos))
                    {
                        prev[pos] = [current];
                        distances[pos] = alt;
                        queue.Enqueue(pos, alt);
                    }
                }
            }

            

            var min = Enumerable.Range(0, 4).Select(d => Distance((end.x, end.y, d))).Min();

            // by inspection there is only one route to E at the end of the maze
            (int x, int y, int d) bestEnd = distances.Where(d => d.Key.x == end.x && d.Key.y == end.y && d.Value == min).Select(kvp => kvp.Key).Single();

            void CountSeats(HashSet<(int x, int y)> seasts, (int x, int y, int d) currentNode)
            {
                (int x, int y) currentPosition = (currentNode.x, currentNode.y);

                seasts.Add(currentPosition);

                if (currentPosition == start)
                {
                    return;
                }

                foreach (var parent in prev[(currentNode)])
                {
                    CountSeats(seasts, parent);
                }
            }

            HashSet<(int x, int y)> visited = new();

            CountSeats(visited, bestEnd);

            return visited.Count;
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

            Assert.Equal(-1, Problem2(input));  // 582 too high
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
            Assert.Equal(45, Problem2(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample2()
        {
            Assert.Equal(64, Problem2(exampleInput2));
        }
    }
}
