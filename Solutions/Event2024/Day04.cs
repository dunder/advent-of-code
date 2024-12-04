using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 4: Phrase ---
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


        public static int CountRowForward(char[,] map)
        {
            bool IsXmas(int row, int column)
            {
                return 
                    map[row, column] == 'X' && 
                    map[row, column + 1] == 'M' && 
                    map[row, column + 2] == 'A' && 
                    map[row, column + 3] == 'S';
            }

            var count = 0;

            for (var row = 0; row < map.GetLength(0); row++)
            {
                for (var column = 0; column < map.GetLength(1)-3; column++)
                {
                    if (IsXmas(row, column))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        public static int CountRowBackward(char[,] map)
        {
            bool IsXmas(int row, int column)
            {
                return
                    map[row, column] == 'S' &&
                    map[row, column + 1] == 'A' &&
                    map[row, column + 2] == 'M' &&
                    map[row, column + 3] == 'X';
            }

            var count = 0;

            for (var row = 0; row < map.GetLength(0); row++)
            {
                for (var column = 0; column < map.GetLength(1) - 3; column++)
                {
                    if (IsXmas(row, column))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static int CountColumnForward(char[,] map)
        {
            bool IsXmas(int row, int column)
            {
                return
                    map[row, column] == 'X' &&
                    map[row + 1, column] == 'M' &&
                    map[row + 2, column] == 'A' &&
                    map[row + 3, column] == 'S';
            }

            var count = 0;

            for (var column = 0; column < map.GetLength(1); column++)
            {
                for (var row = 0; row < map.GetLength(0) - 3; row++)
                {
                    if (IsXmas(row, column))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        public static int CountColumnBackward(char[,] map)
        {
            bool IsXmas(int row, int column)
            {
                return
                    map[row, column] == 'S' &&
                    map[row + 1, column] == 'A' &&
                    map[row + 2, column] == 'M' &&
                    map[row + 3, column] == 'X';
            }

            var count = 0;

            for (var column = 0; column < map.GetLength(1); column++)
            {
                for (var row = 0; row < map.GetLength(0) - 3; row++)
                {
                    if (IsXmas(row, column))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static int CountDiagonalForwardRight(char[,] map)
        {
            bool IsXmas(int row, int column)
            {
                return
                    map[row, column] == 'X' &&
                    map[row + 1, column + 1] == 'M' &&
                    map[row + 2, column + 2] == 'A' &&
                    map[row + 3, column + 3] == 'S';
            }

            var count = 0;

            for (var row = 0; row < map.GetLength(0) - 3; row++)
            {
                for (var column = 0; column < map.GetLength(1) - 3; column++)
                {
                    if (IsXmas(row, column))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static int CountDiagonalForwardLeft(char[,] map)
        {
            bool IsXmas(int row, int column)
            {
                return
                    map[row, column] == 'X' &&
                    map[row + 1, column - 1] == 'M' &&
                    map[row + 2, column - 2] == 'A' &&
                    map[row + 3, column - 3] == 'S';
            }

            var count = 0;

            for (var row = 0; row < map.GetLength(0) - 3; row++)
            {
                for (var column = 3; column < map.GetLength(1); column++)
                {
                    if (IsXmas(row, column))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        public static int CountDiagonalBackwardRight(char[,] map)
        {
            bool IsXmas(int row, int column)
            {
                return
                    map[row, column] == 'S' &&
                    map[row + 1, column - 1] == 'A' &&
                    map[row + 2, column - 2] == 'M' &&
                    map[row + 3, column - 3] == 'X';
            }

            var count = 0;

            for (var row = 0; row < map.GetLength(0) - 3; row++)
            {
                for (var column = 3; column < map.GetLength(1); column++)
                {
                    if (IsXmas(row, column))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        public static int CountDiagonalBackwardLeft(char[,] map)
        {
            bool IsXmas(int row, int column)
            {
                return
                    map[row, column] == 'S' &&
                    map[row + 1, column + 1] == 'A' &&
                    map[row + 2, column + 2] == 'M' &&
                    map[row + 3, column + 3] == 'X';
            }

            var count = 0;

            for (var row = 0; row < map.GetLength(0) - 3; row++)
            {
                for (var column = 0; column < map.GetLength(1) - 3; column++)
                {
                    if (IsXmas(row, column))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            var map = Parse(input);

            int Execute()
            {
                return CountRowForward(map) +
                    CountRowBackward(map) +
                    CountColumnForward(map) +
                    CountColumnBackward(map) +
                    CountDiagonalForwardRight(map) +
                    CountDiagonalForwardLeft(map) +
                    CountDiagonalBackwardRight(map) +
                    CountDiagonalBackwardLeft(map);
            }

            Assert.Equal(-1, Execute());
        }


        public static int CountXmas(char[,] map)
        {
            bool IsDiagonalRightForward(int row, int column)
            {
                return
                    map[row, column] == 'M' &&
                    map[row + 1, column + 1] == 'A' &&
                    map[row + 2, column + 2] == 'S';
            }

            bool IsDiagonalLeftForward(int row, int column)
            {
                return
                    map[row, column + 2] == 'M' &&
                    map[row + 1, column + 1] == 'A' &&
                    map[row + 2, column] == 'S';
            }


            bool IsDiagonalLeftBackward(int row, int column)
            {
                return
                    map[row, column] == 'S' &&
                    map[row + 1, column + 1] == 'A' &&
                    map[row + 2, column + 2] == 'M';
            }

            bool IsDiagonalRightBackward(int row, int column)
            {
                return
                    map[row, column + 2] == 'S' &&
                    map[row + 1, column + 1] == 'A' &&
                    map[row + 2, column] == 'M';
            }

            var count = 0;

            for (var row = 0; row < map.GetLength(0) - 2; row++)
            {
                for (var column = 0; column < map.GetLength(1) - 2; column++)
                {
                    bool x1 = IsDiagonalRightForward(row, column); //   \
                    bool x2 = IsDiagonalLeftForward(row, column); //    /
                    bool x3 = IsDiagonalLeftBackward(row, column); //   \
                    bool x4 = IsDiagonalRightBackward(row, column); //  /

                    if ((x1 && x2) || (x1 && x4) || (x3 && x2) || (x3 && x4))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            var map = Parse(input);

            int Execute()
            {
                return CountXmas(map);
            }

            Assert.Equal(-1, Execute());
        }


        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            List<string> input = 
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



            var map = Parse(input);

            int Execute()
            {
                return CountRowForward(map) +
                    CountRowBackward(map) +
                    CountColumnForward(map) +
                    CountColumnBackward(map) +
                    CountDiagonalForwardRight(map) +
                    CountDiagonalForwardLeft(map) +
                    CountDiagonalBackwardRight(map) +
                    CountDiagonalBackwardLeft(map);
            }


            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            string inputText = "";
            List<string> inputLines = 
                [
                    "", 
                    ""
                ];

            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }
    }
}
