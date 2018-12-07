using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace Solutions.Event2018.Day06
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "1, 1",
                "1, 6",
                "8, 3",
                "3, 4",
                "5, 5",
                "8, 9"
            };

            var x = Problem.LargestArea(input);

            Assert.Equal(17, x);
        }

        [Theory]
        [InlineData(3,4,3,4,true)]
        [InlineData(3,4,2,5,false)]
        [InlineData(3,4,3,5,true)]
        public void HasMinDinstance(int myX, int myY, int px, int py, bool expected)
        {
            var input = new List<string>
            {
                "1, 1",
                "1, 6",
                "8, 3",
                "3, 4",
                "5, 5",
                "8, 9"
            };

            var coordinates = Problem.ParseCoordinates(input);

            var hasMin = Problem.HasMinDistance(new Point(myX, myY), new Point(px, py), coordinates);

            Assert.Equal(expected, hasMin);
        }

        [Fact]
        public void PointsAtRadius()
        {
            var points = Problem.PointsAtRadius(new Point(0, 0), 1);

            var expected = new List<Point>
            {
                // top and bottom pairwise left->right
                new Point(-1,-1),
                new Point(-1,1),
                new Point(0,-1),
                new Point(0,1),
                new Point(1,-1),
                new Point(1,1),
                // left and right pairwise up->down
                new Point(-1,0),
                new Point(1,0)
            };

            Assert.Equal(expected, points);

            points = Problem.PointsAtRadius(new Point(0, 0), 2);

            expected = new List<Point>
            {
                // top and bottom pairwise left->right
                new Point(-2,-2),
                new Point(-2, 2),
                new Point(-1,-2),
                new Point(-1,2),
                new Point(0,-2),
                new Point(0,2),
                new Point(1,-2),
                new Point(1,2),
                new Point(2,-2),
                new Point(2,2),
                // left and right pairwise up->down
                new Point(-2, -1),
                new Point(2, -1),
                new Point(-2, 0),
                new Point(2, 0),
                new Point(-2, 1),
                new Point(2, 1),
            };

            Assert.Equal(expected, points);
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
