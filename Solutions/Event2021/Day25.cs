using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2021
{
    // --- Day 25: Sea Cucumber ---
    public class Day25
    {
        private readonly ITestOutputHelper output;

        public Day25(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static char[,] Parse(IList<string> input)
        {
            char[,] cucumbers = new char[input.First().Length, input.Count];

            for (int row = 0; row < input.Count; row++)
            {
                for (int column = 0; column < input[row].Length; column++)
                {
                    cucumbers[column, row] = input[row][column];
                }
            }

            return cucumbers;
        }

        private static (int x, int y) Next(char[,] input, int x, int y)
        {
            if (input[x, y] == '>')
            {
                return ((x + 1) % input.GetLength(0), y);
            }
            else if (input[x, y] == 'v')
            {
                return (x, (y + 1) % input.GetLength(1));
            }
            return (x, y);
        }

        private static (char[,] cucumbers, bool moved) Move(char[,] cucumbers, char herd)
        {
            int moved = 0;

            List<((int x, int y) from, (int x, int y) to)> moves = [];

            for (int x = 0; x < cucumbers.GetLength(0); x++)
            {
                for (int y = 0; y < cucumbers.GetLength(1); y++)
                {
                    if (cucumbers[x, y] == herd)
                    {
                        (int x, int y) next = Next(cucumbers, x, y);

                        if (cucumbers[next.x, next.y] == '.')
                        {
                           moves.Add(((x, y), (next.x, next.y)));
                        }
                    }
                }
            }

            foreach (var move in moves)
            {
                cucumbers[move.from.x, move.from.y] = '.';
                cucumbers[move.to.x, move.to.y] = herd;
                moved++;
            }

            return (cucumbers, moves.Count > 0);
        }

        private static StringBuilder Print(char[,] cucumbers)
        {
            StringBuilder s = new StringBuilder();

            for (var y = 0; y < cucumbers.GetLength(1); y++)
            {
                for (var x = 0; x < cucumbers.GetLength(0); x++)
                {
                    s.Append(cucumbers[x, y]);
                }
                s.AppendLine();
            }

            return s;
        }

        private static int Problem1(IList<string> input)
        {
            char[,] cucumbers = Parse(input);

            int steps = 0;
            bool moving = true;

            List<string> states = [];

            while (moving)
            {
                steps++;

                (char[,] cucumbers, bool moved) east = Move(cucumbers, '>');
                (char[,] cucumbers, bool moved) south = Move(cucumbers, 'v');

                moving = east.moved || south.moved;
            }

            return steps;
        }

        private static int Problem2(IList<string> input)
        {
            return 0;
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(532, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(58, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarExample2()
        {
            var exampleInput = ReadExampleLineInput("Example2");

            Assert.Equal(-1, Problem1(exampleInput));
        }
    }
}
