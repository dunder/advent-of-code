﻿using Xunit;

namespace Solutions.Event2017.Day1 {
    public class Tests {
        [Theory]
        [InlineData("1122", "3")]
        [InlineData("1111", "4")]
        [InlineData("1234", "0")]
        [InlineData("91212129", "9")]
        public void Problem1_Example1(string input, string expectedSum) {
            var result = Event2017.Day1.Problem.Captcha(input);
            Assert.Equal(expectedSum, result);
        }

        [Theory]
        [InlineData("1212", "6")]
        [InlineData("1221", "0")]
        [InlineData("123425", "4")]
        [InlineData("123123", "12")]
        [InlineData("12131415", "4")]
        public void Problem2_Example1(string input, string expectedSum) {
            var result = Event2017.Day1.Problem.CaptchaHalfway(input);
            Assert.Equal(expectedSum, result);
        }
    }
}
