﻿using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day06 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day06\input.txt");
            var result = SignalDecoder.Decode(input, SignalDecoder.Frequency.Most);

            Assert.Equal("tzstqsua", result);
            _output.WriteLine($"Day 6 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day06\input.txt");
            var result = SignalDecoder.Decode(input, SignalDecoder.Frequency.Least);

            Assert.Equal("myregdnr", result);
            _output.WriteLine($"Day 6 problem 2: {result}");
        }
    }
}
