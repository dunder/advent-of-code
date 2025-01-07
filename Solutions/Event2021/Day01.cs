using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2021
{
    // --- Day 1: Sonar Sweep ---
    public class Day01
    {
        private readonly ITestOutputHelper output;

        public Day01(ITestOutputHelper output)
        {
            this.output = output;
        }

        public int CalculateIncreasingDepths(IList<string> input)
        {
            return input
                .Select(int.Parse)
                .Pairwise((first,  second) => (first, second))
                .Aggregate(0, (total, pair) => pair.second > pair.first ? total + 1 : total);
        }

        public int CalculateWindowedIncreasingDepths(IList<string> input)
        {
            var depths = input.Select(int.Parse).ToList();

            int counter = 0;

            IList<int> previous = depths.Take(3).ToList();

            foreach (var window in depths.Window(3).Skip(1))
            {
                if (window.Sum() > previous.Sum()) counter++;

                previous = window;
            }

            return counter;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return CalculateIncreasingDepths(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return CalculateWindowedIncreasingDepths(input);
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarTest()
        {
            Assert.Equal(1462, FirstStar());
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarTest()
        {
            Assert.Equal(1497, SecondStar());
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "199",
                "200",
                "208",
                "210",
                "200",
                "207",
                "240",
                "269",
                "260",
                "263"
            };

            Assert.Equal(7, CalculateIncreasingDepths(example));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "199",
                "200",
                "208",
                "210",
                "200",
                "207",
                "240",
                "269",
                "260",
                "263"
            };

            Assert.Equal(5, CalculateWindowedIncreasingDepths(example));
        }
    }
}
