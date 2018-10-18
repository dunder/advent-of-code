using System;
using System.Drawing;
using Xunit;

namespace Solutions.Event2017.Day11
{
    public class Tests
    {
        [Fact]
        public void FirstStarOrientation()
        {
            var destination = HexagonPath.Destination("ne,ne,sw,sw");

            Assert.Equal(new Point(0, 0), destination);
        }

        [Theory]
        [InlineData("ne,ne,ne", 3)]
        [InlineData("ne,ne,sw,sw", 0)]
        [InlineData("ne,ne,s,s", 2)]
        [InlineData("se,sw,se,sw,sw", 3)]
        public void FirstStarPath(string directions, int shortestPath)
        {
            var destination = HexagonPath.Destination(directions);

            int path = (Math.Abs(destination.Y) - Math.Abs(destination.X)) / 2 + Math.Abs(destination.X);

            Assert.Equal(shortestPath, path);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("743", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("1493", actual);
        }
    }
}