using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 8: Resonant Collinearity ---
    public class Day08
    {
        private readonly ITestOutputHelper output;

        public Day08(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static Dictionary<(int, int), char> Parse(IList<string> input)
        {
            Dictionary<(int, int), char> map = new();

            for (int row = 0; row < input.Count; row++)
            {
                for (int column = 0; column < input.Count; column++)
                {
                    if (input[row][column] != '.')
                    {
                        map.Add((row, column), input[row][column]);
                    }
                }
            }

            return map;
        }

        private static int CountAntinodes(Dictionary<(int x, int y), char> map, int maxx, int maxy, bool withResonantHarmonics)
        {
            var antennas = map.GroupBy(x => x.Value);
            var antennaGroups = antennas.Where(a => a.Count() > 1).ToList();

            bool WithinBounds((int x, int y) location)
            {
                return location.x >= 0 && location.x < maxx && location.y >= 0 && location.y < maxy;
            }

            var antinodes = new HashSet<(int, int)>();

            foreach (var antennaGroup in antennaGroups)
            {
                int i = 1;

                foreach (var antenna1 in antennaGroup)
                {
                    if (withResonantHarmonics)
                    {
                        antinodes.Add(antenna1.Key);
                    }

                    foreach (var antenna2 in antennaGroup.Skip(i))
                    {
                        var xdiff = antenna1.Key.x - antenna2.Key.x;
                        var ydiff = antenna1.Key.y - antenna2.Key.y;

                        (int x, int y) antinode1 = (antenna1.Key.x + xdiff, antenna1.Key.y + ydiff);

                        while (WithinBounds(antinode1)) {
                            antinodes.Add(antinode1);
                            antinode1 = (antinode1.x + xdiff, antinode1.y + ydiff);

                            if (!withResonantHarmonics)
                            {
                                break;
                            }
                        }

                        (int x, int y) antinode2 = (antenna2.Key.x - xdiff, antenna2.Key.y - ydiff);

                        while (WithinBounds(antinode2))
                        {
                            antinodes.Add(antinode2);
                            antinode2 = (antinode2.x - xdiff, antinode2.y - ydiff);

                            if (!withResonantHarmonics)
                            {
                                break;
                            }
                        }
                    }

                    i++;
                } 
            }

            return antinodes.Count();
        }

        private static int Problem1(IList<string> input)
        {
            var map = Parse(input);

            return CountAntinodes(map, input.First().Length, input.Count, false);
        }

        private static int Problem2(IList<string> input)
        {
            var map = Parse(input);

            return CountAntinodes(map, input.First().Length, input.Count, true);
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(320, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1157, Problem2(input));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "............",
                "........0...",
                ".....0......",
                ".......0....",
                "....0.......",
                "......A.....",
                "............",
                "............",
                "........A...",
                ".........A..",
                "............",
                "............",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(14, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {            
            Assert.Equal(34, Problem2(exampleInput));
        }
    }
}
