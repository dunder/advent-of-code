using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 13: Shuttle Search ---
    public class Day13
    {
        private readonly ITestOutputHelper output;

        public Day13(ITestOutputHelper output)
        {
            this.output = output;
        }

        public int FirstStar()
        {
            return 0;
        }

        public long SecondStar()
        {
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(153, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(471793476184394, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            
        }

        [Fact]
        public void SecondStarExample()
        {
            
        }
    }
}
