using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2022
{
    // --- Day 6: Tuning Trouble ---
    public class Day06
    {
        private readonly ITestOutputHelper output;

        public Day06(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static int Problem1(IList<string> input)
        {
            var buffer = input[0];

            for (int i = 0; i < buffer.Length - 3; i++)
            {
                var test = buffer.Substring(i, 4);

                var set = test.Select(c => c).ToHashSet();

                if (set.Count == 4)
                {
                    return i + 4;
                }

            }

            return 0;
        }

        private static int Problem2(IList<string> input)
        {
            var buffer = input[0];

            for (int i = 0; i < buffer.Length - 13; i++)
            {
                var test = buffer.Substring(i, 14);

                var set = test.Select(c => c).ToHashSet();

                if (set.Count == 14)
                {
                    return i + 14;
                }

            }

            return 0;
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1640, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(3613, Problem2(input));
        }

        [Theory]
        [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
        [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
        [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6)]
        [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
        [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
        [Trait("Example", "2022")]
        public void FirstStarExample(string buffer, int expected)
        {
            Assert.Equal(expected, Problem1([buffer]));
        }

        [Theory]
        [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
        [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
        [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 23)]
        [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
        [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
        [Trait("Example", "2022")]
        public void SecondStarExample(string buffer, int expected)
        {
            Assert.Equal(expected, Problem2([buffer]));
        }
    }
}