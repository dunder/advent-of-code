using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 3: Toboggan Trajectory ---
    public class Day03
    {
        private readonly ITestOutputHelper output;

        public Day03(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static IDictionary<(int,int), bool> ParseMap(IList<string> lines)
        {
            var map = new Dictionary<(int,int), bool>();

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0;  x < lines[y].Length; x++)
                {

                    map.Add((x, y), lines[y][x] == '#');
                }
            }

            return map;
        }

        private static IEnumerable<(int,int)> Walk(int right, int down, int depth)
        {
            int x = 0, y = 0;

            do
            {
                x = x + right;
                y = y + down;
                yield return (x, y);
            } while (y < depth);
        }

        private static bool TreeAtPosition((int, int) position, IDictionary<(int,int), bool> map)
        {
            var (x, y) = position;
            
            var maxX = map.Keys.Select(key => key.Item1).Max();

            var transformedPosition = (x % (maxX + 1), y);

            return map.ContainsKey(transformedPosition) && map[transformedPosition];
        }

        private static int CountTrees(int right, int down, IList<string> input)
        {
            var map = ParseMap(input);

            return Walk(right, down, input.Count - 1).Count(position => TreeAtPosition(position, map));
        }

        private static long MultiplyTreeCountsForlopes(IList<string> input,  IList<(int,int)> slopes)
        {
            return slopes.Select(slope =>
            {
                var (right, down) = slope;
                return CountTrees(right, down, input);
            })
            .Aggregate(1L, (a, n) => a * n);
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return CountTrees(3, 1, input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();

            var slopes = new List<(int, int)>
            {
                (1,1),
                (3,1),
                (5,1),
                (7,1),
                (1,2)
            };

            var treeCount = slopes.Select(slope =>
            {
                var (right, down) = slope;
                return CountTrees(right, down, input);
            })
            .Aggregate(1L, (a, n) => a * n);

            return MultiplyTreeCountsForlopes(input, slopes);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(162, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(3064612320L, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "..##.......",
                "#...#...#..",
                ".#....#..#.",
                "..#.#...#.#",
                ".#...##..#.",
                "..#.##.....",
                ".#.#.#....#",
                ".#........#",
                "#.##...#...",
                "#...##....#",
                ".#..#...#.#"
            };

            int trees = CountTrees(3, 1, input);

            Assert.Equal(7, trees); 
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string>
            {
                "..##.......",
                "#...#...#..",
                ".#....#..#.",
                "..#.#...#.#",
                ".#...##..#.",
                "..#.##.....",
                ".#.#.#....#",
                ".#........#",
                "#.##...#...",
                "#...##....#",
                ".#..#...#.#"
            };

            var slopes = new List<(int, int)>
            {
                (1,1),
                (3,1),
                (5,1),
                (7,1),
                (1,2)
            };

            Assert.Equal(336, MultiplyTreeCountsForlopes(input, slopes));
        }
    }
}
