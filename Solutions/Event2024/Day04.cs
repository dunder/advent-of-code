using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 4: Ceres Search ---
    public class Day04
    {
        private readonly ITestOutputHelper output;

        public Day04(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static readonly string TARGET = "XMAS";

        public static char[,] Parse(IList<string> input)
        {
            var map = new char[input.Count, input.First().Length];
            for (var row = 0; row < input.Count; row++)
            {
                var line = input[row];
                for (var column = 0; column < input.First().Length; column++)
                {
                    map[row, column] = line[column];
                }
            }
            return map;
        }

        private static char SafeRead(char[,] map, int row, int colum)
        {
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);

            bool outOfBounds = row < 0 || row >= rows || colum < 0 || colum >= columns;

            return outOfBounds ? '.' : map[row, colum];
        }

        public static IEnumerable<char[]> AllDirections(char[,] map, string word)
        {
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);
            var wordLength = word.Length;

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    char[] horizontal = new char[wordLength];
                    char[] vertical = new char[wordLength];
                    char[] backslash = new char[wordLength];
                    char[] slash = new char[wordLength];

                    for (var i = 0; i < wordLength; i++)
                    {
                        horizontal[i] = SafeRead(map, r, c + i);
                        vertical[i] = SafeRead(map, r + i, c);
                        backslash[i] = SafeRead(map, r + i, c + i);
                        slash[i] = SafeRead(map, r + i, c - i);
                    }
                    yield return horizontal;
                    yield return vertical;
                    yield return backslash;
                    yield return slash;
                }
            }
        }

        private static bool IsXmas(char[] cs)
        {
            return new string(cs) == "XMAS" || new string(cs) == "SAMX";
        }

        public static IEnumerable<(char[], char[])> XDirections(char[,] map, string word)
        {
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);
            var wordLength = word.Length;

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    char[] backslash = new char[wordLength];
                    char[] slash = new char[wordLength];

                    for (var i = 0; i < wordLength; i++)
                    {
                        backslash[i] = SafeRead(map, r + i, c + i);
                        slash[i] = SafeRead(map, r + i, c + wordLength - 1 - i);
                    }

                    yield return (backslash, slash);
                }
            }
        }

        private static bool IsMas((char[] slash, char[] backslash) x)
        {
            string slash = new string(x.slash);
            string backslash = new string(x.backslash);

            bool slashMas = slash == "MAS";
            bool slashSam = slash == "SAM";
            bool backslashMas = backslash == "MAS";
            bool backslashSam = backslash == "SAM";

            return slashMas && (backslashMas || backslashSam) || slashSam && (backslashMas || backslashSam);
        }

        public int Problem1(IList<string> input)
        {
            var map = Parse(input);

            return AllDirections(map, "XMAS").Count(IsXmas);
        }

        public int Problem2(IList<string> input)
        {
            var map = Parse(input);

            return XDirections(map, "MAS").Count(IsMas);
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(2593, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1950, Problem2(input));
        }

        private static List<string> example =
            [
                "MMMSXXMASM",
                "MSAMXMSMSA",
                "AMXSXMAAMM",
                "MSAMASMSMX",
                "XMASAMXAMM",
                "XXAMMXXAMA",
                "SMSMSASXSS",
                "SAXAMASAAA",
                "MAMMMXMMMM",
                "MXMXAXMASX",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(18, Problem1(example));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(9, Problem2(example));
        }
    }
}
