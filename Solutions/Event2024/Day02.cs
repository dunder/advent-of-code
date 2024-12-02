using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 2: Phrase ---
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
            if (list[0] == list[1])
            {
                return false;
            }

            bool increasing = list[1] > list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                var x = list[i];
                var y = list[i + 1];

                if (increasing)
                {
                    int increase = y - x;
                    if (!(increase == 1 || increase == 2 || increase == 3))
                    {
                        return false;
                    }
                }
                else
                {
                    int decrease = x - y;
                    if (!(decrease == 1 || decrease == 2 || decrease == 3))
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

            Assert.Equal(-1, Execute());
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

            Assert.Equal(-1, Execute()); // inte 298
        }


        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            string inputText = "";
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
                var safe = lists.Where(Safe).ToArray();
                return lists.Count(Safe);
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
                var safe = lists.Where(Safe2).ToArray();
                return lists.Count(Safe2);
            }

            Assert.Equal(-1, Execute());
        }
    }
}
