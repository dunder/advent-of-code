﻿using Xunit;

namespace Solutions.Event2017.Day04 {
    public class Tests {
        [Theory]
        [InlineData("aa bb cc dd ee", 1)]
        [InlineData("aa bb cc dd aa", 0)]
        [InlineData("aa bb cc dd aaa", 1)]
        public void Problem1_Example1(string input, int expectedValidCount) {
            var count = PassPhrase.Count(new[] {input});
            Assert.Equal(expectedValidCount, count);
        }

        [Theory]
        [InlineData("abcde fghij", 1)]
        [InlineData("abcde xyz ecdab", 0)]
        [InlineData("a ab abc abd abf abj", 1)]
        [InlineData("iiii oiii ooii oooi oooo", 1)]
        [InlineData("oiii ioii iioi iiio", 0)]
        public void Problem2_Example1(string input, int expected) {
            var count = PassPhrase.CountAnagrams(new[] { input });
            Assert.Equal(expected, count);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("477", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("167", actual);
        }
    }
}
