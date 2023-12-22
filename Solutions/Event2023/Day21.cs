
using Shared.MapGeometry;
using Shared.Tree;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day X: Phrase ---
    public class Day21
    {
        private readonly ITestOutputHelper output;

        public Day21(ITestOutputHelper output)
        {
            this.output = output;
        }


        public Dictionary<(int, int), char> Parse(IList<string> input)
        {
            Dictionary<(int, int), char> map = new();

            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    map.Add((x, y), input[y][x]);
                }
            }

            return map;
        }


        private IEnumerable<(int, int)> Neighbors(Dictionary<(int, int), char> map, (int, int) position)
        {             

            (int x, int y) = position;

            bool TryGenerateNeighbor((int x, int y) potential, out (int, int) neighbor)
            {
                if (map.TryGetValue(potential, out var value) && value != '#')
                {
                    neighbor = potential;
                    return true;
                }
                neighbor = default;
                return false;
            }

            var up = (x, y - 1);
            var right = (x + 1, y);
            var down = (x, y + 1);
            var left = (x - 1, y);

            if (TryGenerateNeighbor(up, out var neighborUp))
            {
                yield return neighborUp;
            }
            if (TryGenerateNeighbor(right, out var neighborRight))
            {
                yield return neighborRight;
            }
            if (TryGenerateNeighbor(down, out var neighborDown))
            {
                yield return neighborDown;
            }
            if (TryGenerateNeighbor(left, out var neighborLeft))
            {
                yield return neighborLeft;
            }
        }

        public int Run1(IList<string> input, int? limit = 64)
        {
            Dictionary<(int, int), char> map = Parse(input);

            var start = map.Where(kvp => kvp.Value == 'S').Single().Key;

            var size = input.Count;

            var work = new HashSet<(int, int)> { start };

            for (var i = 0; i < limit; i++)
            {
                work = new HashSet<(int,int)>(work
                    .SelectMany(it => Neighbors(map, it))
                    .Where(dest => map.ContainsKey(dest) && map[dest] != '#'));
            }

            return work.Count;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Run1(input, 64);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(3574, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>()
            {
                "...........",
                ".....###.#.",
                ".###.##..#.",
                "..#.#...#..",
                "....#.#....",
                ".##..S####.",
                ".##..#...#.",
                ".......##..",
                ".##.#.####.",
                ".##..##.##.",
                "..........."
            };

            Assert.Equal(2, Run1(example, 1));
            Assert.Equal(4, Run1(example, 2));
            Assert.Equal(6, Run1(example, 3));
            Assert.Equal(16, Run1(example, 6));
        }

        [Fact]
        public void SecondStarExample()
        {
            
        }
    }
}
