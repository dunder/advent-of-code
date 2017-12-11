using System;
using System.Drawing;
using Xunit;

namespace Y2017.Day11 {
    public class Tests {
        [Fact]
        public void Problem1_Orientation() {

            var destination = HexagonPath.Destination("ne,ne,sw,sw");

            Assert.Equal(new Point(0,0), destination);
        }

        [Theory]
        [InlineData("ne,ne,ne", 3)]
        [InlineData("ne,ne,sw,sw", 0)]
        [InlineData("ne,ne,s,s", 2)]
        [InlineData("se,sw,se,sw,sw", 3)]
        public void Problem1_Path(string directions, int shortestPath) {
            var destination = HexagonPath.Destination(directions);

            int path = (Math.Abs(destination.Y) - Math.Abs(destination.X))/ 2 + Math.Abs(destination.X);

            Assert.Equal(shortestPath, path);
        }
    }
}
