using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day X: Phrase ---
    public class Day02
    {
        private readonly ITestOutputHelper output;

        public Day02(ITestOutputHelper output)
        {
            this.output = output;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
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
