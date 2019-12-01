using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 10: Elves Look, Elves Say ---
    public class Day10
    {
        private const string Input = "3113322113";

        public class NumberGroup
        {
            public int Value { get; set; }
            public int Count { get; set; }

            public string Expand()
            {
                return $"{Count}{Value}";
            }

            public override string ToString()
            {
                return $"{Value} is repeated {Count} times";
            }
        }

        public static IEnumerable<NumberGroup> EnumerateNumberGroups(string input)
        {
            var previous = int.Parse(input.Substring(0, 1));
            var counter = 1;
            foreach (var current in input.Substring(1))
            {
                counter++;
                if (current == previous)
                {
                    previous = current;
                    continue;
                }
                
                yield return new NumberGroup
                {
                    Count = counter,
                    Value = int.Parse(previous.ToString())
                };
                counter = 0;
                previous = current;
            }
        }

        public static int FirstStar()
        {
            return 0;
        }

        public static int SecondStar()
        {
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(329356, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(4666278, result);
        }

        [Fact]
        public void EnumerateNumberGroupTest()
        {
            var groups = EnumerateNumberGroups("111455661").ToList();

            Assert.Equal(5, groups.Count);
        }
    }
}
