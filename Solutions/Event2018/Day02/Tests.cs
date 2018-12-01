using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Solutions.Event2018.Day02
{
    public class Tests
    {
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
