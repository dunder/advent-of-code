﻿using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day12 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day12\input.txt");

            var result = MemoryBank.CountConnectedTo0(input);

            _output.WriteLine($"Day 12 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day12\input.txt");

            var result = MemoryBank.CountGroups(input);

            _output.WriteLine($"Day 12 problem 2: {result}");
        }
    }
}