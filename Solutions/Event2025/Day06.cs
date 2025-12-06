using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 6: Trash Compactor ---
    public class Day06
    {
        private readonly ITestOutputHelper output;

        public Day06(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static long Problem1(IList<string> input)
        {
            long total = 0;

            var operations = input.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            List<List<int>> values = [];

            for (int i = 0; i < input.Count - 1; i++)
            {
                var lineValues = input[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                values.Add(lineValues);
            }

            for (int column = 0; column < operations.Count; column++)
            {
                var operation = operations[column];
                long subTotal = operation == "+" ? 0 : 1;

                for (int row = 0; row < values.Count; row++)
                {
                    var columnValue = values[row][column];

                    subTotal = operation == "*" ? subTotal * columnValue : subTotal + columnValue;

                }
                total += subTotal;
            }

            return total;
        }

        private static long Problem2(IList<string> input)
        {
            string operationsRow = input.Last();
            var operations = operationsRow.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            List<int> operationIndexes = [];

            int depth = input.Count - 1;
            int width = input.First().Length;
            List<string> values = Enumerable.Repeat("", width).ToList();

            for (int row = 0; row < input.Count - 1; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var value = input[row][column] == ' ' ? "" : input[row][column].ToString();
                    values[column] += value ;
                }
            }

            long total = 0;
            long subTotal = operations.First() == "+" ? 0 : 1;
            var operation = operations[0];
            int oi = 0;

            for (int i = 0; i < values.Count;i++)
            {
                if (values[i] == "")
                {
                    total += subTotal;
                    oi++;
                    operation = operations[oi];
                    subTotal = operation == "+" ? 0 : 1;
                    continue;
                }

                var columnValue = int.Parse(values[i]);

                subTotal = operation == "*" ? subTotal * columnValue : subTotal + columnValue;
            }

            return total + subTotal;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(5524274308182, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(8843673199391, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(4277556, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(3263827, Problem2(exampleInput));
        }
    }
}
