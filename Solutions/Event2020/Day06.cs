using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 6: Custom Customs ---
    public class Day06
    {
        private readonly ITestOutputHelper output;

        public Day06(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<HashSet<char>> Parse(IList<string> input)
        {
            var groups = input.Split(string.IsNullOrWhiteSpace);

            var result = new List<HashSet<char>>();

            foreach (var group in groups)
            {
                var groupAnswers = new HashSet<char>();

                foreach (var groupAnswer in group)
                {
                    foreach (var answer in groupAnswer)
                    {
                        groupAnswers.Add(answer);
                    }
                }

                result.Add(groupAnswers);
            }

            return result;
        }

        private int SumPositiveAnswers(IList<string> input)
        {
            var groups = Parse(input);

            return groups.Select(answers => answers.Count).Sum();
        }

        private List<HashSet<char>> Parse2(IList<string> input)
        {
            var groups = input.Split(string.IsNullOrWhiteSpace);

            var result = new List<HashSet<char>>();

            foreach (var group in groups)
            {
                var groupAnswers = new HashSet<char>(group.First().ToCharArray());

                foreach (var groupAnswer in group)
                {
                    groupAnswers.IntersectWith(groupAnswer.ToCharArray());
                }

                result.Add(groupAnswers);
            }

            return result;
        }

        private int SumPositiveCommonAnswers(IList<string> input)
        {
            var groups = Parse2(input);

            return groups.Select(answers => answers.Count).Sum();
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return SumPositiveAnswers(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return SumPositiveCommonAnswers(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(6310, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(3193, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "abc",
                "",
                "a",
                "b",
                "c",
                "",
                "ab",
                "ac",
                "",
                "a",
                "a",
                "a",
                "a",
                "",
                "b"
            };

            Assert.Equal(11, SumPositiveAnswers(example));
        }

        [Fact]
        public void SecondStarExample()
        {

            var example = new List<string>
            {
                "abc",
                "",
                "a",
                "b",
                "c",
                "",
                "ab",
                "ac",
                "",
                "a",
                "a",
                "a",
                "a",
                "",
                "b"
            };

            Assert.Equal(6, SumPositiveCommonAnswers(example));
        }
    }
}
