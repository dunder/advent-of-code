using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 14: Parabolic Reflector Dish ---
    public class Day14
    {
        private readonly ITestOutputHelper output;

        public Day14(ITestOutputHelper output)
        {
            this.output = output;
        }

        private ((int width, int height), Dictionary<(int, int), char>) Parse(IList<string> input)
        {
            Dictionary<(int, int), char> rocks = new();

            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input.Count; x++)
                {
                    var rock = input[y][x];

                    if (rock != '.')
                    {
                        rocks[(x, y)] = rock;
                    }
                }
            }

            return ((input[0].Length, input.Count), rocks);
        }

        private enum Direction {  North, West, South, East }

        private Dictionary<(int, int), char> Tilt((int width, int height) dimension, Dictionary<(int, int), char> rocks, Direction direction = Direction.North)
        {
            if (direction == Direction.North)
            {
                for (int x = 0; x < dimension.width; x++)
                {
                    for (int y = 0; y < dimension.height; y++)
                    {
                        var blocked = rocks.ContainsKey((x, y));
                        if (blocked)
                        {
                            continue;
                        }
                        var from = y;
                        while (++from < dimension.height)
                        {
                            if (rocks.TryGetValue((x, from), out char rock))
                            {
                                if (rock == 'O')
                                {
                                    rocks[(x, y)] = 'O';
                                    rocks.Remove((x, from));
                                    break;
                                }
                                y = from;
                                break;
                            }
                        }
                    }
                }
            }
            else if (direction == Direction.West)
            {
                for (int y = 0; y < dimension.height; y++)
                {
                    for (int x = 0; x < dimension.width; x++)
                    {
                        var blocked = rocks.ContainsKey((x, y));
                        if (blocked)
                        {
                            continue;
                        }
                        var from = x;
                        while (++from < dimension.width)
                        {
                            if (rocks.TryGetValue((from, y), out char rock))
                            {
                                if (rock == 'O')
                                {
                                    rocks[(x, y)] = 'O';
                                    rocks.Remove((from, y));
                                    break;
                                }
                                x = from;
                                break;
                            }
                        }
                    }
                }
            }
            else if (direction == Direction.South)
            {
                for (int x = 0; x < dimension.width; x++)
                {
                    for (int y = dimension.height-1; y >= 0; y--)
                    {
                        var blocked = rocks.ContainsKey((x, y));
                        if (blocked)
                        {
                            continue;
                        }
                        var from = y;
                        while (--from >= 0)
                        {
                            if (rocks.TryGetValue((x, from), out char rock))
                            {
                                if (rock == 'O')
                                {
                                    rocks[(x, y)] = 'O';
                                    rocks.Remove((x, from));
                                    break;
                                }
                                y = from;
                                break;
                            }
                        }
                    }
                }

            }
            else if (direction == Direction.East)
            {
                for (int y = 0; y < dimension.height; y++)
                {
                    for (int x = dimension.width-1; x >= 0; x--)
                    {
                        var blocked = rocks.ContainsKey((x, y));
                        if (blocked)
                        {
                            continue;
                        }
                        var from = x;
                        while (--from >= 0)
                        {
                            if (rocks.TryGetValue((from, y), out char rock))
                            {
                                if (rock == 'O')
                                {
                                    rocks[(x, y)] = 'O';
                                    rocks.Remove((from, y));
                                    break;
                                }
                                x = from;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Eh?");
            } 
            return rocks; 
        }

        private void Print(Dictionary<(int, int), char> rocks)
        {
            var height = rocks.Keys.Select(x => x.Item2).Max();
            var width = rocks.Keys.Select(x => x.Item1).Max();
            output.WriteLine("");
            for (int y = 0; y <= height; y++)
            {
                var line = new StringBuilder();
                for (int x = 0; x <= width; x++)
                {
                    if (rocks.TryGetValue((x,y), out char rock)) {
                        line.Append(rock);
                    }
                    else
                    {
                        line.Append('.');
                    }
                }
                output.WriteLine(line.ToString());
            }
            output.WriteLine("");
        }

        private int CalculateWeight(Dictionary<(int, int), char> rocks, int height)
        {
            return rocks.Where(kvp => kvp.Value == 'O').Select(kvp => (height - kvp.Key.Item2)).Sum();
        }

        private int Solve1(IList<string> example)
        {
            ((int width, int height), Dictionary<(int, int), char>) rocks = Parse(example);

            (int width, int height) dimensions = rocks.Item1;
            Dictionary<(int, int), char> rockLocations = rocks.Item2;

            Dictionary<(int, int), char> tilted = Tilt(dimensions, rockLocations);

            return CalculateWeight(tilted, dimensions.height);
        }

        private string ToKey(Dictionary<(int, int), char> rocks, (int width, int height) dimensions)
        {
            return string.Join("|", rocks.Where(x => x.Value == 'O').Select(r => $"{r.Key.Item1},{r.Key.Item2}"));
        }

        private int Solve2(IList<string> example, int iterations = 1)
        {
            ((int width, int height), Dictionary<(int, int), char>) rocks = Parse(example);

            (int width, int height) dimensions = rocks.Item1;
            Dictionary<(int, int), char> rockLocations = rocks.Item2;

            var directions = new[] { Direction.North, Direction.West, Direction.South, Direction.East };

            var sequence = new List<int>();

            var keepGoing = true;
           
            var sampleSize = 200;
            var sampleEvaluationSize = 3;

            while (keepGoing)
            {
                foreach (var direction in directions)
                {
                    rockLocations = Tilt(dimensions, rockLocations, direction);
                }
                var weight = CalculateWeight(rockLocations, dimensions.height);

                sequence.Add(weight);
                if (sequence.Count == sampleSize)
                {
                    break;
                }
            }

            var sampleSequence = (sequence as IEnumerable<int>).Reverse().Take(sampleEvaluationSize).ToList();

            int i;

            for (i = 1; i < sequence.Count; i++)
            {
                var compareSequence = (sequence as IEnumerable<int>).Reverse().Skip(i).Take(sampleEvaluationSize).ToList();

                if (Enumerable.SequenceEqual(sampleSequence, compareSequence))
                {
                    break;
                }
            }

            var loop = sequence.Skip(sequence.Count - i).ToList();

            var index = (iterations - (sampleSize-i) - 1) % i;

            return loop[index];
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Solve1(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return Solve2(input, 1000000000);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(105003, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(93742, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "O....#....",
                "O.OO#....#",
                ".....##...",
                "OO.#O....O",
                ".O.....O#.",
                "O.#..O.#.#",
                "..O..#O..O",
                ".......O..",
                "#....###..",
                "#OO..#....",
            };

            Assert.Equal(136, Solve1(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "O....#....",
                "O.OO#....#",
                ".....##...",
                "OO.#O....O",
                ".O.....O#.",
                "O.#..O.#.#",
                "..O..#O..O",
                ".......O..",
                "#....###..",
                "#OO..#....",
            };
            
            Assert.Equal(64, Solve2(example, 1000000000));
        }
    }
}
