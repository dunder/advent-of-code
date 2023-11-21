using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 9: Encoding Error ---
    public class Day09
    {
        private readonly ITestOutputHelper output;

        public Day09(ITestOutputHelper output)
        {
            this.output = output;
        }

        private IEnumerable<(long,long)> AllPairs(IEnumerable<long> list)
        {
            var items = list.ToList();

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = i + 1; j < items.Count(); j++)
                {
                    yield return (items[i], items[j]);
                }
            }
        }

        private long FindFirst(int preamble, IList<string> input)
        {
            var xmas = input.Select(long.Parse).ToList();

            long firstInvalid = -1L;

            for (int i = preamble; i < xmas.Count; i++)
            {
                long next = xmas[i];

                IEnumerable<long> previous = xmas.Slice(i-preamble, preamble);

                if (!AllPairs(previous).Any(x => x.Item1 + x.Item2 == next))
                {
                    firstInvalid = next;
                    break;
                }
            }

            return firstInvalid;
        }

        private long FindContiguous(int preamble, IList<string> input)
        {
            var first = FindFirst(preamble, input);

            var xmas = input.Select(long.Parse).ToList();

            long total = 0;
            int i = 0;
            int j = 0;

            for (i = 0; i < xmas.Count; i++)
            {
                total = xmas[i];

                for (j = i+1; i < xmas.Count; j++)
                {
                    total += xmas[j];
                    if (total >= first)
                    {
                        break;
                    }
                }

                if (total == first)
                {
                    break;
                }
            }

            var range = xmas.Slice(i, j - i + 1);

            return range.Min() + range.Max();
        }

        public long FirstStar()
        {
            var input = ReadLineInput();

            return FindFirst(25, input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();

            return FindContiguous(25, input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(15690279, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(2174232, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "35",
                "20",
                "15",
                "25",
                "47",
                "40",
                "62",
                "55",
                "65",
                "95",
                "102",
                "117",
                "150",
                "182",
                "127",
                "219",
                "299",
                "277",
                "309",
                "576"
            };

            Assert.Equal(127, FindFirst(5, example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "35",
                "20",
                "15",
                "25",
                "47",
                "40",
                "62",
                "55",
                "65",
                "95",
                "102",
                "117",
                "150",
                "182",
                "127",
                "219",
                "299",
                "277",
                "309",
                "576"
            };

            Assert.Equal(62, FindContiguous(5, example));
        }
    }
}
