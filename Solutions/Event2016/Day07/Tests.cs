﻿using Xunit;

namespace Solutions.Event2016.Day07 {
    public class Tests {
        [Theory]
        [InlineData("abba[mnop]qrst", true)]
        [InlineData("abcd[bddb]xyyx", false)]
        [InlineData("aaaa[qwer]tyui", false)]
        [InlineData("ioxxoj[asdfgh]zxcvbn", true)]
        public void FirstStarExampl(string candidate, bool isAbba) {

            bool result = Tls.IsAutonomousBridgeBypassingAnnotation(candidate);

            Assert.Equal(isAbba, result);
        }

        [Theory]
        [InlineData("aba[bab]xyz", true)]
        [InlineData("xyx[xyx]xyx", false)]
        [InlineData("aaa[kek]eke", true)]
        [InlineData("zazbz[bzb]cdb", true)]
        public void SecondStarExample(string candidate, bool isAba) {

            bool result = Tls.IsAreaBroadcastAccessor(candidate);

            Assert.Equal(isAba, result);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("110", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("242", actual);
        }
    }
}
