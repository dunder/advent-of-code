using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // 

    public class Day22
    {
        private readonly ITestOutputHelper output;

        public Day22(ITestOutputHelper output)
        {
            this.output = output;
        }


        public int FirstStar()
        {
            var input = ReadLineInput().ToList();

            return 0;
        }

        public int SecondStar()
        {
            var input = ReadLineInput().ToList();

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
            var result = SecondStar();
            Assert.Equal(-1, result);
        }
    }
}
