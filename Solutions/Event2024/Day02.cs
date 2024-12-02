using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 2: Red-Nosed Reports ---
    public class Day02
    {
        private readonly ITestOutputHelper output;

        public Day02(ITestOutputHelper output)
        {
            this.output = output;
        }

        
        public List<List<int>> Parse(IList<string> input)
        {
            return input.Select(line =>
            {
                var parts = line.Split(" ").Select(int.Parse).ToList();
                return parts;

            }).ToList();
        }

        public bool Safe(List<int> list)
        {
            var first = list[0];
            var second = list[1];

            if (first == second)
            {
                return false;
            }

            bool increasing = second > first;
            bool decreasing = !increasing;

            for (int i = 0; i < list.Count - 1; i++)
            {
                var current = list[i];
                var next = list[i + 1];

                var difference = next - current;

                var absoluteDifference = Math.Abs(difference);
                var validDifference = absoluteDifference > 0 && absoluteDifference <= 3;

                if (!validDifference)
                {
                    return false;
                }

                if (increasing)
                {
                    if (difference < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (difference > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool Safe2(List<int> list)
        {
            if (Safe(list)) { return true; }

            for (int i = 0; i < list.Count; i++)
            {
                var reduced = new List<int>(list);
                reduced.RemoveAt(i);
                if (Safe(reduced))
                {
                    return true;
                }
            }

            return false;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            var lists = Parse(input);

            int Execute()
            {
                return lists.Count(Safe);
            }

            Assert.Equal(236, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            var lists = Parse(input);

            int Execute()
            {
                return lists.Count(Safe2);
            }

            Assert.Equal(308, Execute());
        }


        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            List<string> inputLines = 
                [
                    "7 6 4 2 1",
                    "1 2 7 8 9",
                    "9 7 6 2 1",
                    "1 3 2 4 5",
                    "8 6 4 4 1",
                    "1 3 6 7 9",
                ];

            var lists = Parse(inputLines);

            int Execute()
            {
                return lists.Count(Safe);
            }

            Assert.Equal(2, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            List<string> inputLines =
                [
                    "7 6 4 2 1",
                    "1 2 7 8 9",
                    "9 7 6 2 1",
                    "1 3 2 4 5",
                    "8 6 4 4 1",
                    "1 3 6 7 9",
                ];

            var lists = Parse(inputLines);

            int Execute()
            {
                return lists.Count(Safe2);
            }

            Assert.Equal(4, Execute());
        }
    }
}
