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

        private Dictionary<(int, int), char> Tilt((int width, int height) dimension, Dictionary<(int, int), char> rocks)
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

        private int Load(IList<string> example)
        {
            ((int width, int height), Dictionary<(int, int), char>) rocks = Parse(example);

            (int width, int height) x = rocks.Item1;
            Dictionary<(int, int), char> y = rocks.Item2;

            //Print(y);

            Dictionary<(int, int), char> tilted = Tilt(x, y);

            //Print(tilted);

            return tilted.Where(kvp => kvp.Value == 'O').Select(kvp => (x.height-kvp.Key.Item2)).Sum();
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Load(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
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

            Assert.Equal(136, Load(example));
        }



        [Fact]
        public void SecondStarExample()
        {
            
        }
    }
}
