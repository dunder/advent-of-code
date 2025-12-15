using FluentAssertions.Formatting;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 12: Phrase ---
    public class Day12
    {
        private readonly ITestOutputHelper output;

        public Day12(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Region(int Width, int Length, List<int> Quantities);

        private static char[,] ParseShape(List<string> lines)
        {
            var shape = new char[lines[0].Length,lines.Count];

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    shape[x,y] = lines[y][x];
                }
            }

            return shape;
        }

        private static Region ParseRegion(string line)
        {
            var parts = line.Split(": ");
            var dimensions = parts[0].Split("x").Select(int.Parse).ToList();
            var quantities = parts[1..][0].Split(" ").Select(int.Parse).ToList();

            return new Region(dimensions[0], dimensions[1], quantities);
        }

        private static (List<char[,]> shapes, List<Region> regions) Parse(IList<string> input)
        {
            List<List<string>> groups = input.Split("").Select(g => g.ToList()).ToList();
            List<char[,]> shapes = groups[..^1].Select(lines => ParseShape(lines[1..])).ToList();
            List<Region> regions = groups[^1].Select(ParseRegion).ToList();

            return (shapes, regions);
        }

        private static int Problem1(IList<string> input)
        {
            (List<char[,]> shapes, List<Region> regions) = Parse(input);

            return 0;
        }

        private static int Problem2(IList<string> input)
        {
            return 0;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem2(exampleInput));
        }
    }
}