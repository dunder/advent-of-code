using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 6: Wait For It ---
    public class Day06
    {
        private readonly ITestOutputHelper output;

        public Day06(ITestOutputHelper output)
        {
            this.output = output;
        }

        private IEnumerable<long> Range(long count)
        {
            for (long i = 1; i <= count; i++)
            {
                yield return i;
            }
        }

        private long WaysToWin(long raceTime, long recordDistance)
        {
            return Range(raceTime)
                .Select(chargeTime => (raceTime - chargeTime) * chargeTime)
                .Where(distance => distance > recordDistance)
                .Count();
        }

        private long Run(IList<string> input)
        {
            var times = input.First()
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(int.Parse)
                .ToList();

            var distances = input.Last()
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(int.Parse)
                .ToList();

            return times.Select((chargeTime, i) => WaysToWin(chargeTime, distances[i])).Aggregate(1L, (total, x) => x*total);
        }

        private long Run2(IList<string> input)
        {
            var time = long.Parse(string.Join("", input.First()
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)));

            var distance = long.Parse(string.Join("", input.Last()
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)));

            return WaysToWin(time, distance);
        }

        public long FirstStar()
        {
            var input = ReadLineInput();
            
            return Run(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();
            return Run2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(588588, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(34655848, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "Time:      7  15   30",
                "Distance: 9  40  200"
            };

            Assert.Equal(288, Run(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "Time:      7  15   30",
                "Distance: 9  40  200"
            };

            Assert.Equal(71503, Run2(example));
        }
    }
}
