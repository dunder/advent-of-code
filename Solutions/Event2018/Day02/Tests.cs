using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Solutions.Event2018.Day02
{
    public class Tests
    {
        [Theory]
        [InlineData("abcdef", 2, false)]
        [InlineData("abcdef", 3, false)]
        [InlineData("bababc", 2, true)]
        [InlineData("bababc", 3, true)]
        [InlineData("abbcde", 2, true)]
        [InlineData("abbcde", 3, false)]
        [InlineData("abcccd", 2, false)]
        [InlineData("abcccd", 3, true)]
        [InlineData("aabcdd", 2, true)]
        [InlineData("aabcdd", 3, false)]
        [InlineData("abcdee", 2, true)]
        [InlineData("abcdee", 3, false)]
        [InlineData("ababab", 2, false)]
        [InlineData("ababab", 3, true)]
        public void HasLetterExactlyTwice(string input, int count, bool expected)
        {
            var actual = Problem.HasLetterExactlyTimes(input, count);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string> {"abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" };

            var checksum = Problem.Checksum(input);

            Assert.Equal(4*3, checksum);
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string> { "abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz" };

            var similar = Problem.FindSimilar(input);

            Assert.Equal("fgij", similar);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("7470", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("kqzxdenujwcstybmgvyiofrrd", actual);
        }

    }
}
