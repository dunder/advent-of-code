using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2022
{
    // --- Day 1: Calorie Counting ---
    public class Day01
    {
        private readonly ITestOutputHelper output;

        public Day01(ITestOutputHelper output)
        {
            this.output = output;
        }

        private int CountMostCalories(IList<string> input)
        {
            return input
                .Split(string.IsNullOrEmpty)
                .Select(elf => elf.Select(int.Parse).Sum())
                .Max();
        }
        private int CountTopThreeMostCalories(IList<string> input)
        {
            return input
                .Split(string.IsNullOrEmpty)
                .Select(elf => elf.Select(int.Parse).Sum())
                .OrderByDescending(x => x)
                .Take(3)
                .Sum();
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return CountMostCalories(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return CountTopThreeMostCalories(input);
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarTest()
        {
            Assert.Equal(69912, FirstStar());
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarTest()
        {
            Assert.Equal(208180, SecondStar());
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "1000",
                "2000",
                "3000",
                "",
                "4000",
                "",
                "5000",
                "6000",
                "",
                "7000",
                "8000",
                "9000",
                "",
                "10000"
            };

            Assert.Equal(24000, CountMostCalories(example));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "1000",
                "2000",
                "3000",
                "",
                "4000",
                "",
                "5000",
                "6000",
                "",
                "7000",
                "8000",
                "9000",
                "",
                "10000"
            };

            Assert.Equal(45000, CountTopThreeMostCalories(example));
        }
    }
}
