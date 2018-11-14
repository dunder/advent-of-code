using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Solutions.Event2016.Day15
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {

        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("122318", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("3208583", actual);
        }
    }
}
