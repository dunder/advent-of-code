using Shared.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.Event2018.Day24;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 20: Race Condition ---
    public class Day20
    {
        private readonly ITestOutputHelper output;

        public Day20(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static (char[,] map, (int x, int y) start, (int x, int y) end) Parse(IList<string> input)
        {
            int width = input.First().Length;
            int height = input.Count;
            
            var map = new char[width, height];
            (int x, int y) start = (0, 0);
            (int x, int y) end = (0, 0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var c = input[y][x];

                    if (c == 'S')
                    {
                        map[x, y] = '.';
                        start = (x, y);
                    }
                    else if (c == 'E')
                    {
                        map[x, y] = '.';
                        end = (x, y);
                    }
                    else
                    {
                        map[x, y] = c;
                    }
                }
            }

            return (map, start, end);
        }

        private void Print(char[,] map)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            var s = new StringBuilder();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    s.Append(map[x, y]);
                }
                s.AppendLine();
            }

            output.WriteLine(s.ToString());
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

        private static List<((int x, int y) position, int cost)> Neighbors(char[,] map, (int x, int y) position)
        {
            (int x, int y) = position;

            List<((int x, int y) position, int cost)> neighbors =
            [
                ((x, y - 1), 1),
                ((x + 1, y), 1),
                ((x, y + 1), 1),
                ((x - 1, y), 1),
            ];

            return neighbors.Where(n => WithinBounds(map, n.position) && map[n.position.x, n.position.y] != '#').ToList();
        }

        private static List<((int x, int y) position, int cost)> AllNeighbors(char[,] map, (int x, int y) position)
        {
            (int x, int y) = position;

            List<((int x, int y) position, int cost)> neighbors =
            [
                ((x, y - 1), 1),
                ((x + 1, y), 1),
                ((x, y + 1), 1),
                ((x - 1, y), 1),
            ];

            return neighbors.Where(n => WithinBounds(map, n.position)).ToList();
        }

        private static List<char[,]> ValidCheats(char[,] map)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            List<char[,]> cheats = new List<char[,]>();

            for (int y = 1; y < height-1; y++)
            {
                for (int x = 1; x < width-1; x++)
                {
                    if (map[x, y] == '#')
                    {
                        var ns = AllNeighbors(map, (x, y));
                        var space = ns.Count(n => map[n.position.x, n.position.y] == '.');
                        if (space > 1)
                        {
                            var cheatMap = map.Clone() as char[,];
                            cheatMap[x, y] = '.';
                            cheats.Add(cheatMap);
                        }
                    }

                }
            }

            return cheats;

        }

        private int Problem1(IList<string> input)
        {
            (char[,] map, (int x, int y) start, (int x, int y) end) = Parse(input);

            Print(map);

            var distances = new Dictionary<(int x, int y), int>
            {
                { start, 0 }
            };

            int min = Djikstra(map, start, end, distances);

            List<char[,]> cheats = ValidCheats(map);
            
            var cheatMins = cheats.Select(c => Djikstra(c, start, end, new Dictionary<(int x, int y), int> {{ start, 0 }})).ToList();

            var cheatSaves = cheatMins.Select(c => min - c).GroupBy(x => x);

            return cheatSaves.Where(g => g.Key >= 100).Select(g => g.Count()).Sum(); // 14 fel
        }

        private static int Djikstra(char[,] map, (int x, int y) start, (int x, int y) end, Dictionary<(int x, int y), int> distances)
        {
            var queue = new PriorityQueue<(int x, int y), int>();

            queue.Enqueue(start, 0);



            var prev = new Dictionary<(int x, int y), (int x, int y)>();

            int Distance((int x, int y) position)
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

            return Distance((end.x, end.y));
        }

        public static int ManhattanDistance((int x, int y) point, (int x, int y) toPoint)
        {
            return Math.Abs(point.x - toPoint.x) + Math.Abs(point.y - toPoint.y);
        }

        private static List<(int x, int y, int time)> Neighbors(char[,] map, (int x, int y) position, int length)
        {
            (int x, int y) = position;

            List<(int x, int y, int time)> neighbors = [];

            for (int i = 2; i <= length; i++)
            {
                neighbors.Add((x, y - i, i));
                neighbors.Add((x + i, y, i));
                neighbors.Add((x, y + i, i));
                neighbors.Add((x - i, y, i));

                for (int yi = 1; yi < i; yi++)
                {
                    neighbors.Add((x - (i - yi), y - yi, i));
                    neighbors.Add((x + (i - yi), y - yi, i));

                    neighbors.Add((x - (i - yi), y + yi, i));
                    neighbors.Add((x + (i - yi), y + yi, i));
                }

            }

            return neighbors.Where(n => WithinBounds(map, (n.x, n.y)) && map[n.x, n.y] != '#').ToList();
        }


        private static int Problem2(IList<string> input, int limit)
        {
            (char[,] map, (int x, int y) start, (int x, int y) end) = Parse(input);

            int min = Djikstra(map, start, end, new Dictionary<(int x, int y), int> {{ start, 0 }});

            var startDistances = new Dictionary<(int x, int y), int> { { start, 0 } };
            var endDistances = new Dictionary<(int x, int y), int>() { { end, 0 } };
            var minFromStart = new Dictionary<(int x, int y), int>();
            var minFromEnd = new Dictionary<(int x, int y), int>();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] != '#')
                    {
                        minFromStart.Add((x, y), Djikstra(map, start, (x, y), startDistances));
                        minFromEnd.Add((x, y), Djikstra(map, end, (x, y), endDistances));
                    }
                }
            }
            
            List<int> cheats = [];

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] != '#')
                    {
                        var s = minFromStart[(x, y)];
                        
                        var neighbors = Neighbors(map, (x, y), 20);

                        foreach (var n in neighbors)
                        {
                            int e = minFromEnd[(n.x, n.y)];

                            int win = min - (s + n.time + e);

                            if (win >= limit)
                            {
                                cheats.Add(win);
                            }
                        }
                    }
                }
            }

            var gs = cheats.GroupBy(x => x).ToList();

            return cheats.Count;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input, 100)); // 2346354 That's not the right answer; your answer is too high.
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "###############",
                "#...#...#.....#",
                "#.#.#.#.#.###.#",
                "#S#...#.#.#...#",
                "#######.#.#.###",
                "#######.#.#...#",
                "#######.#.###.#",
                "###..E#...#...#",
                "###.#######.###",
                "#...###...#...#",
                "#.#####.#.###.#",
                "#.#...#.#.#...#",
                "#.#.#.#.#.#.###",
                "#...#...#...###",
                "###############",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(-1, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(-1, Problem2(exampleInput, 50));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void NeighborsTest()
        {
            List<string> input =
            [
                "#########",
                "#.......#",
                "#.......#",
                "#.......#",
                "#.......#",
                "#.......#",
                "#.......#",
                "#########",
            ];


            List<string> expectedInput =
            [
                "#########",
                "#..XXX..#",
                "#.XX.XX.#",
                "#XX...XX#",
                "#.XX.XX.#",
                "#..XXX..#",
                "#...X...#",
                "#########",
            ];

            var empty = Parse(input);
            var expected = Parse(expectedInput);

            var ns = Neighbors(empty.map, (4, 3), 3);

            foreach (var n in ns)
            {
                empty.map[n.x, n.y] = 'X';
            }

            Print(empty.map);

            for (int y = 0;  y < expected.map.GetLength(1); y++)
            {
                for (int x = 0;  x < expected.map.GetLength(0); x++)
                {
                    Assert.Equal(expected.map[x, y], empty.map[x, y]);
                }
            }
        }
    }
}
