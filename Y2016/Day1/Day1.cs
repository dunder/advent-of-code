﻿using System.Drawing;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day1 {
    public class Day1 {
        private readonly ITestOutputHelper output;

        public Day1(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public void Problem1() {

            string input = File.ReadAllText(@".\Day1\input.txt");
            var taxiMap = new TaxiMap(new Point(0,0), Direction.North);
            var result = taxiMap.ShortestPath(input);


            output.WriteLine($"Day 1 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day1\input.txt");
            var taxiMap = new TaxiMap(new Point(0, 0), Direction.North);

            var result = "Not solved yet";

            output.WriteLine($"Day 1 problem 2: {result}");
        }
    }
}
