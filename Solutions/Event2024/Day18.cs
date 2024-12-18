using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.Event2016.Day20.Problem;
using static Solutions.Event2024.Day14;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 18: Phrase ---
    public class Day18
    {
        private readonly ITestOutputHelper output;

        public Day18(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static List<(int x, int y)> Parse(IList<string> input)
        {
            return input.Select(x =>
            {
                var parts = x.Split(",").Select(int.Parse).ToList();
                return (parts[0], parts[1]);
            }).ToList();
        }

        private static char[,] CreateMap(IList<string> input, int range, int take)
        {
            HashSet<(int x, int y)> bytes = Parse(input).Take(take).ToHashSet();

            var map = new char[range+1, range+1];

            for (int y = 0; y <= range; y++)
            { 
                for (int x = 0; x <= range; x++)
                {
                    var location = (x, y);

                    if (bytes.Contains(location))
                    {
                        map[x, y] = '#';
                    }
                    else
                    {
                        map[x, y] = '.';
                    }
                }
            }

            return map;
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

        private int Problem1(IList<string> input, int range, int bytes)
        {
            char[,] map = CreateMap(input, range, bytes);

            //Print(map);

            (int x, int y) end = (range, range);

            List<int> mins = new();

            (int x, int y) start = (0, 0);
                

            if (map.GetLength(0) != map.GetLength(1))
            {
                throw new ArgumentOutOfRangeException($"Map must be a square");
            }

            var mapDimension = map.GetLength(0);

            var queue = new PriorityQueue<(int x, int y), int>();


            queue.Enqueue(start, 0);

            var distances = new Dictionary<(int x, int y), int>
            {
                { start, 0 }
            };

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
            

            return Enumerable.Range(0, 4).Select(d => Distance((end.x, end.y))).Min();
        }

        private string Problem2(IList<string> input, int range, int bytes)
        {
            (int x, int y) min = (0, 0);

            for (int b = bytes; b < input.Count; b++)
            {
                char[,] map = CreateMap(input, range, b);

                //Print(map);

                (int x, int y) end = (range, range);

                List<int> mins = new();

                (int x, int y) start = (0, 0);


                if (map.GetLength(0) != map.GetLength(1))
                {
                    throw new ArgumentOutOfRangeException($"Map must be a square");
                }

                var mapDimension = map.GetLength(0);

                var queue = new PriorityQueue<(int x, int y), int>();


                queue.Enqueue(start, 0);

                var distances = new Dictionary<(int x, int y), int>
                {
                    { start, 0 }
                };

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

                var minimum = Distance((end.x, end.y));

                if (minimum == int.MaxValue)
                {
                    Print(map);
                    var bs = Parse(input);
                    min = bs.Take(b).Last();
                    break;
                }

            }

            return $"{min.x},{min.y}";
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem1(input, 70, 1024));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal("", Problem2(input, 70, 1024));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "5,4",
                "4,2",
                "4,5",
                "3,0",
                "2,1",
                "6,3",
                "2,4",
                "1,5",
                "0,6",
                "3,3",
                "2,6",
                "5,1",
                "1,2",
                "5,5",
                "2,5",
                "6,5",
                "1,4",
                "0,4",
                "6,4",
                "1,1",
                "6,1",
                "1,0",
                "0,5",
                "1,6",
                "2,0",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(-1, Problem1(exampleInput, 6, 12));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal("6,1", Problem2(exampleInput, 6, 12));
        }
    }
}
