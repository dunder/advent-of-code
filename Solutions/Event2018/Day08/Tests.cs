using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Solutions.Event2018.Day08
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            var input = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
            var sum = Problem.CalculateSumOfMetadata(input);

            Assert.Equal(138, sum);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("", actual);
        }
    }
}
