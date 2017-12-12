﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day13 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day13\input.txt");

            var result = "";

            _output.WriteLine($"Day 13 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day13\input.txt");

            var result = "";

            _output.WriteLine($"Day 13 problem 2: {result}");
        }
    }
}
