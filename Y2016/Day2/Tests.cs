using System.Collections.Generic;
using Xunit;

namespace Y2016.Day2 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {
            var input = new [] {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            };

            var result = KeyPad.NumericKeyPad.Sequence(input);

            Assert.Equal("1985", result);
        }

        [Fact]
        public void Problem2_Example1() {
            var input = new [] {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            };

            var result = KeyPad.AlphaNumericKeyPad.Sequence(input);

            Assert.Equal("5DB3", result);
        }
    }
}
