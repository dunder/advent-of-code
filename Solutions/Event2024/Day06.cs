using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 6: Guard Gallivant ---
    public class Day06
    {
        private readonly ITestOutputHelper output;

        public Day06(ITestOutputHelper output)
        {
            this.output = output;
        }

        public (Dictionary<(int, int), bool>, (int, int), (int, int)) Parse(IList<string> input)
        {
            Dictionary<(int, int), bool> map = new Dictionary<(int, int), bool>();

            (int x, int y) guard = (0, 0);
            (int maxx, int maxy) bounds = (input.Count - 1, input.First().Length - 1);

            for (int column = 0; column < input.Count; column++)
            {
                var line = input[column];

                for (int row = 0; row < input[column].Length; row++)
                {
                    if (line[row] == '#')
                    {
                        map.Add((row, column), true);
                    }
                    else if (line[row] == '^')
                    {
                        guard = (row, column);
                    }
                }
            }

            return (map, bounds, guard);
        }

        private static bool InBounds((int x, int y) bounds, (int x, int y) guard)
        {
            return guard.x >= 0 && guard.y >= 0 && guard.x <= bounds.x && guard.y <= bounds.y;
        }

        private (int x, int y) Next((int x, int y) guard, int direction) => direction switch
        {
            0 => (guard.x, guard.y - 1),
            1 => (guard.x + 1, guard.y),
            2 => (guard.x, guard.y + 1),
            3 => (guard.x - 1, guard.y),
            _ => throw new InvalidOperationException($"Invalid direction: {direction}")
        };

        public HashSet<(int, int)> DistinctPositions(Dictionary<(int, int), bool> map, (int, int) bounds, (int, int)initialGuard)
        {
            (int x, int y) guard = initialGuard;
            int direction = 0;

            var visited = new HashSet<(int, int)>();

            while (InBounds(bounds, guard))
            {
                visited.Add(guard);

                var next = Next(guard, direction);
                
                while (map.ContainsKey(next))
                {
                    direction = ++direction % 4;
                    next = Next(guard, direction);
                }

                guard = next;
            }

            return visited;
        }
        public int Loops(Dictionary<(int, int), bool> map, (int x, int y) bounds, (int, int) initialGuard)
        {
            int obstacles = 0;

            var guardTrail = DistinctPositions(map, bounds, initialGuard);
            
            for (int row = 0; row <= bounds.x; row++)
            {
                for (int column = 0; column <= bounds.y; column++)
                {
                    var obstacle = (row, column);

                    if (obstacle == initialGuard || map.ContainsKey(obstacle) || !guardTrail.Contains((row, column)))
                    {
                        continue;
                    }

                    Dictionary<(int, int), bool> oMap = new (map)
                    {
                        { obstacle, true }
                    };

                    (int x, int y) guard = initialGuard;

                    int direction = 0;

                    HashSet<(int x, int y, int direction)> visited = new();

                    while (InBounds(bounds, guard))
                    {
                        if (!visited.Add((guard.x, guard.y, direction)))
                        {
                            obstacles++;
                            break;
                        }

                        var next = Next(guard, direction);

                        while (oMap.ContainsKey(next))
                        {
                            direction = ++direction % 4;
                            next = Next(guard, direction);
                        }

                        guard = next;
                    }
                }
            }

            return obstacles;
        }

        private int Problem1(IList<string> input)
        {
            var (map, bounds, guard) = Parse(input);
            
            return DistinctPositions(map, bounds, guard).Count;
        }

        private int Problem2(IList<string> input)
        {
            var (map, bounds, guard) = Parse(input);

            return Loops(map, bounds, guard);
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(4752, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1719, Problem2(input));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "....#.....",
                ".........#",
                "..........",
                "..#.......",
                ".......#..",
                "..........",
                ".#..^.....",
                "........#.",
                "#.........",
                "......#...",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(41, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(6, Problem2(exampleInput));
        }
    }
}
