using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 4: Printing Department ---
    public class Day04
    {
        private readonly ITestOutputHelper output;

        public Day04(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static char[,] ParseMap(IList<string> input)
        {
            int maxx = input.First().Length;
            int maxy = input.Count;

            char[,] map = new char[maxx, maxy];

            for (var y = 0; y < maxy; y++)
            {
                var line = input[y];

                for (var x = 0; x < maxx; x++)
                {
                    var c = line[x];

                    map[x, y] = c;
                }
            }

            return map;
        }

        private static List<(int x, int y)> Neighbors(int x, int y, int w, int h)
        {
            List<(int x, int y)> ns =
            [
                    (x - 1, y + 1),
                    (x, y + 1),
                    (x + 1, y + 1),
                    (x + 1, y),
                    (x + 1, y - 1),
                    (x, y - 1),
                    (x - 1, y - 1),
                    (x - 1, y),
            ];

            return ns
                .Where(pos => pos.x >= 0 && pos.y >= 0 && pos.x < w && pos.y < h)
                .ToList();
        }

        private static int Problem1(IList<string> input)
        {
            var map = ParseMap(input);

            var w = map.GetLength(0);
            var h = map.GetLength(1);

            var count = 0;

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    if (map[x, y] == '.')
                    {
                        continue;
                    }

                    var paper = Neighbors(x, y, w, h).Count(n => map[n.x, n.y] == '@');

                    if (paper < 4)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static int Problem2(IList<string> input)
        {
            var map = ParseMap(input);

            var w = map.GetLength(0);
            var h = map.GetLength(1);

            var count = 0;
            var turn = 0;

            do
            {
                turn = 0;

                for (var x = 0; x < w; x++)
                {
                    for (var y = 0; y < w; y++)
                    {
                        if (map[x, y] == '.')
                        {
                            continue;
                        }
                        var paper = Neighbors(x, y, w, h).Count(n => map[n.x, n.y] == '@');

                        if (paper < 4)
                        {
                            map[x, y] = '.';

                            count++;
                            turn++;
                        }
                    }
                }

            } while (turn > 0);

            return count;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1445, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(8317, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(13, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(43, Problem2(exampleInput));
        }
    }
}
