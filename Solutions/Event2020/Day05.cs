using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 5: Binary Boarding ---
    public class Day05
    {
        private readonly ITestOutputHelper output;

        public Day05(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record BoardingPass(string code, int row, int column)
        {
            public int SeatId => row * 8 + column;
        }


        public enum RangeSelection { Lower, Upper }

        private (int, int) CutRange(int start, int end, RangeSelection selection)
        {

            if (end < start)
            {
                throw new ArgumentException($"Unvalid range: {start}-{end}");
            }

            if ((end - start + 1) % 2 != 0)
            {
                throw new ArgumentException($"Uneven range: {start}-{end}");
            }

            switch (selection)
            {
                case RangeSelection.Lower:
                    return (start, start + (end-start) / 2);
                case RangeSelection.Upper:
                    return (start + (end-start) / 2 + 1, end);
                default:
                    throw new InvalidOperationException($"Unkonwn range: {selection}");
            }
        }

        private int ReduceRange(string code, int size)
        {
            List<char> cs = code.ToCharArray().ToList();

            int Loop(List<char> remainingCode, int start, int end)
            {
                if (remainingCode.Count == 0)
                {
                    return start;
                }

                var nextInstruction = remainingCode[0];

                var selection = nextInstruction == 'F' || nextInstruction == 'L' ? RangeSelection.Lower : RangeSelection.Upper;

                var (nextStart, nextEnd) = CutRange(start, end, selection);
                
                return Loop(remainingCode.Skip(1).ToList(), nextStart, nextEnd);
            }

            return Loop(cs, 0, size-1);
        }


        private BoardingPass Parse(string code)
        {
            var rows = code.Substring(0, 7);
            var cols = code.Substring(7);

            var assignedRow = ReduceRange(rows, 128);
            var assignedColumn = ReduceRange(cols, 8);

            return new BoardingPass(code, assignedRow, assignedColumn);
        }

        private IList<BoardingPass> Parse(IList<string> input)
        {
            return input.Select(Parse).ToList();
        }

        private int FindHighestPassportId(IList<string> input)
        {
            var boardingPasses = Parse(input);
            return boardingPasses.Select(boardingPass => boardingPass.SeatId).Max();
        }

        private int FindMySeatId(IList<string> input)
        {
            var boardingPasses = Parse(input);

            var sorted = boardingPasses.OrderBy(b => b.row).ThenBy(b => b.column).ToList();

            var previous = sorted.First();

            foreach (var boardingPass in sorted.Skip(1))
            {
                if (boardingPass.SeatId - previous.SeatId > 1)
                {
                    if (previous.column == 7)
                    {
                        var myBoardingPass = new BoardingPass("", previous.row + 1, 0);
                        return myBoardingPass.SeatId;
                    }
                    else
                    {
                        var myBoardingPass = new BoardingPass("", previous.row, previous.column + 1);
                        return myBoardingPass.SeatId;
                    }
                }
                previous = boardingPass;
            }

            throw new Exception("Unable to find seat");
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return FindHighestPassportId(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return FindMySeatId(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(855, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(552, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "BFFFBBFRRR",
                "FFFBBBFRRR",
                "BBFFBBFRLL"
            };

            Assert.Equal(820, FindHighestPassportId(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            
        }

        [Theory]
        [InlineData(0, 7, RangeSelection.Lower, 0, 3)]
        [InlineData(0, 7, RangeSelection.Upper, 4, 7)]
        [InlineData(0, 127, RangeSelection.Lower, 0, 63)]
        [InlineData(0, 127, RangeSelection.Upper, 64, 127)]
        [InlineData(0, 63, RangeSelection.Upper, 32, 63)]
        [InlineData(32, 63, RangeSelection.Lower, 32, 47)]
        [InlineData(32, 47, RangeSelection.Lower, 32, 39)]
        [InlineData(32, 47, RangeSelection.Upper, 40, 47)]
        public void CutRangeTests(int from, int to, RangeSelection rangeSelection, int expectedFrom, int expectedTo)
        {
            var (actualFrom, actualTo) = CutRange(from, to, rangeSelection);

            Assert.Equal(expectedFrom, actualFrom);
            Assert.Equal(expectedTo, actualTo);
        }

        [Theory]
        [InlineData("FBFBBFF", 128, 44)]
        [InlineData("RLR", 8, 5)]
        public void ReduceRangeTests(string code, int size, int expected)
        {
            var row = ReduceRange(code, size);

            Assert.Equal(expected, row);
        }
    }
}
