using MoreLinq.Extensions;
using Shared.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 07: Phrase ---
    public class Day07
    {
        private readonly ITestOutputHelper output;

        public Day07(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static (char[,] map, (int x, int y) start, HashSet<(int x, int y)> splitters) ParseMap(IList<string> input)
        {
            int maxx = input.First().Length;
            int maxy = input.Count;

            char[,] map = new char[maxx, maxy];
            HashSet<(int x, int y)> splitters = [];
            (int x, int y) start = (0,0);
            for (var y = 0; y < maxy; y++)
            {
                var line = input[y];

                for (var x = 0; x < maxx; x++)
                {
                    var c = line[x];

                    map[x, y] = c;

                    if (c == 'S')
                    {
                        start = (x, y);
                    }
                    else if (c == '^')
                    {
                        splitters.Add((x, y));
                    }

                }
            }

            return (map, start, splitters);
        }

        private static List<List<(int x, int y)>> ParseMap2(IList<string> input)
        {
            int maxx = input.First().Length;
            int maxy = input.Count;

            List<List<int>> splitters = [];

            for (var y = 1; y < maxy; y++)
            {
                var line = input[y];

                List<int> lineSplitters = line.Where((c, i) => c == '^').Select((c, i) => i).ToList();

                if (lineSplitters.Any())
                {
                    splitters.Add(lineSplitters);
                }
            }

            return splitters.Select((line, y) => line.Select(x => (x, y)).ToList()).ToList();
        }

        private static int Problem1(IList<string> input)
        {
            var (map, start, splitters) = ParseMap(input);

            HashSet<(int x, int y)> beams = [start];

            var count = 0;

            for (int t = 1; t < map.GetLength(1); t++)
            {
                HashSet<(int x, int y)> newBeams = [];

                foreach (var beam in beams)
                {
                    (int x, int y) location = (beam.x, beam.y);

                    if (splitters.Contains(location))
                    {
                        newBeams.Add((location.x-1, location.y+1));
                        newBeams.Add((location.x+1, location.y+1));
                        count++;
                    }
                    else
                    {
                        newBeams.Add((location.x, location.y + 1));
                    }
                }

                beams = newBeams;
            }

            return count;
        }

        private record Splitter(int X, int Y);

        private class Node
        {
            public Node((int x, int y) splitter)
            {
                Splitter = new Splitter(splitter.x, splitter.y);
            }
            public Splitter Splitter { get; set; }
            public Splitter Right { get; set; }
            public Splitter Left { get; set; }

            public override string ToString()
            {
                return $"Left({Left}) Right({Right})";
            }
        }

        private static long Problem2(IList<string> input)
        {
            var (map, _, splitters) = ParseMap(input);

            Dictionary<Splitter, Node> nodes = splitters.ToDictionary(s => new Splitter(s.x, s.y), s => new Node(s));

            foreach (var splitter in splitters.Select(s => new Splitter(s.x, s.y)).ToList())
            {
                var right = splitters.Where(s => s.x == splitter.X + 1).Where(s => s.y > splitter.Y).OrderBy(s => s.y);
                var left = splitters.Where(s => s.x == splitter.X - 1).Where(s => s.y > splitter.Y).OrderBy(s => s.y);

                if (right.Any())
                {
                    var rightSplitter = right.First();
                    nodes[splitter].Right = new Splitter(rightSplitter.x, rightSplitter.y);
                }

                if (left.Any())
                {
                    var leftSplitter = left.First();
                    nodes[splitter].Left = new Splitter(leftSplitter.x, leftSplitter.y);
                }
            }

            var root = nodes.Where(n => n.Key.Y == 2).Single().Value;

            Dictionary<Splitter, long> cache = [];

            long Count(Node root)
            {
                if (root == null)
                {
                    return 1;
                }

                if (cache.ContainsKey(root.Splitter))
                {
                    return cache[root.Splitter];
                }

                if (root.Right == null && root.Left == null)
                {
                    return 2;
                }

                var left = root.Left == null ? null : nodes[root.Left];
                var right = root.Right == null ? null : nodes[root.Right];

                var leftCount = Count(left);

                if (left != null && !cache.ContainsKey(left.Splitter))
                {
                    cache.Add(left.Splitter, leftCount);
                }

                var rightCount = Count(right);

                if (right != null && !cache.ContainsKey(right.Splitter))
                {
                    cache.Add(right.Splitter, rightCount);
                }

                return leftCount + rightCount;
            }

            return Count(root);
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1573, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(15093663987272, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(21, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(40, Problem2(exampleInput));
        }
    }
}