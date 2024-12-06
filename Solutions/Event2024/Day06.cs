using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day X: Phrase ---
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

        public int DistinctPositions(Dictionary<(int, int), bool> map, (int, int) bounds, (int, int)initialGuard)
        {
            (int x, int y) guard = initialGuard;
            int direction = 0;

            var visited = new HashSet<(int, int)>();

            void Turn()
            {
                direction++;
                if (direction == 4)
                {
                    direction = 0;
                }
            }


            (int x, int y) Next() => direction switch
            {
                0 => (guard.x, guard.y - 1),
                1 => (guard.x + 1, guard.y),
                2 => (guard.x, guard.y + 1),
                3 => (guard.x - 1, guard.y),
            };

            while (InBounds(bounds, guard))
            {
                visited.Add(guard);

                var next = Next();
                
                while (map.ContainsKey(next))
                {
                    Turn();
                    next = Next();
                }

                guard = next;
            }

            return visited.Count;
        }
        public int Loops(Dictionary<(int, int), bool> map, (int x, int y) bounds, (int, int) initialGuard)
        {
            int counter = 0;
            List<(int x, int y)> obstacles = new List<(int, int)>();

            for (int row = 0; row <= bounds.x; row++)
            {
                for (int column = 0; column <= bounds.y; column++)
                {
                    if ((row, column) == (9,7))
                    {
                        Console.WriteLine();
                    }

                    if ((row, column) == initialGuard || map.ContainsKey((row, column)))
                    {
                        continue;
                    }

                    var oMap = new Dictionary<(int, int), bool>(map)
                    {
                        { (row, column), true }
                    };

                    (int x, int y) guard = initialGuard;
                    int direction = 0;

                    var visited = 1;

                    void Turn()
                    {
                        direction++;
                        if (direction == 4)
                        {
                            direction = 0;
                        }
                    }

                    (int x, int y) Next() => direction switch
                    {
                        0 => (guard.x, guard.y - 1),
                        1 => (guard.x + 1, guard.y),
                        2 => (guard.x, guard.y + 1),
                        3 => (guard.x - 1, guard.y),
                    };

                    while (InBounds(bounds, guard))
                    {
                        visited++;

                        var next = Next();

                        while (oMap.ContainsKey(next))
                        {
                            Turn();
                            next = Next();
                        }

                        guard = next;

                        if (visited > bounds.x * bounds.y)
                        {
                            counter++;
                            obstacles.Add((row, column));
                            break;
                        }
                    }
                }
            }

            return counter;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            var (map, bounds, guard) = Parse(input);

            int Execute()
            {
                return DistinctPositions(map, bounds, guard);
            }

            Assert.Equal(4752, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            var (map, bounds, guard) = Parse(input);

            int Execute()
            {
                return Loops(map, bounds, guard);
            }

            Assert.Equal(1719, Execute()); // 1720 too high
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
            var (map, bounds, guard) = Parse(exampleInput);

            int Execute()
            {
                return DistinctPositions(map, bounds, guard);
            }
            Assert.Equal(41, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            var (map, bounds, guard) = Parse(exampleInput);

            int Execute()
            {
                return Loops(map, bounds, guard);
            }

            Assert.Equal(6, Execute());
        }
    }
}
