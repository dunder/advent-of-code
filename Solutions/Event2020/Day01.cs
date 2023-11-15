using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 1: Report Repair ---
    public class Day01
    {
        private readonly ITestOutputHelper output;

        public Day01(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<int> Parse(IList<string> lines)
        {
            return lines.Select(int.Parse).ToList();
        }

        private int FindProductOfTwo(List<int> input)
        {

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input.Count; j++)
                {
                    if (i == j) continue;

                    var x = input[i];
                    var y = input[j];

                    if (x + y == 2020)
                    {
                        return x*y;
                    }
                }
            }

            throw new Exception("Two numbers adding up to 2020 not found");
        }

        private int FindProductOfThree(List<int> input)
        {

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input.Count; j++)
                {
                    for (int k = 0; k < input.Count; k++)
                    {
                        if (i == j || j == k || i == k) continue;

                        var x = input[i];
                        var y = input[j];
                        var z = input[k];

                        if (x + y + z == 2020)
                        {
                            return x*y*z;
                        }
                    }
                }
            }

            throw new Exception("Three numbers not found");
        }

        public int FirstStar()
        {
            IList<string> input = ReadLineInput();

            int product = FindProductOfTwo(Parse(input));

            return product;
        }

        public int SecondStar()
        {
            IList<string> input = ReadLineInput();

            int product = FindProductOfThree(Parse(input));

            return product;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(646779, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(246191688, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "1721",
                "979",
                "366",
                "299",
                "675",
                "1456",
            };

            var product = FindProductOfTwo(Parse(input));

            Assert.Equal(514579, product);
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string>
            {
                "1721",
                "979",
                "366",
                "299",
                "675",
                "1456",
            };

            int product = FindProductOfThree(Parse(input));

            Assert.Equal(241861950, product);
        }
    }
}
