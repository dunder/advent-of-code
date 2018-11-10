using System.Drawing;
using System.Linq;
using Xunit;

namespace Solutions.Event2016.Day13
{
    public class Tests
    {
        [Theory]
        [InlineData(1,0)]
        [InlineData(3,0)]
        [InlineData(4,0)]
        [InlineData(5,0)]
        [InlineData(6,0)]
        [InlineData(8,0)]
        [InlineData(9,0)]
        [InlineData(0,6)]
        [InlineData(4,6)]
        [InlineData(5,6)]
        [InlineData(7,6)]
        [InlineData(8,6)]
        [InlineData(9,6)]
        public void IsWall_WhenExpectWall_ReturnsTrue(int x, int y)
        {
            var isWall = Problem.IsWall(10, x, y);
            Assert.True(isWall);
        }

        [Theory]
        [InlineData(0,0)]
        public void IsWall_WhenExpectOpenSpace_ReturnsFalse(int x, int y)
        {
            var isWall = Problem.IsWall(10, x, y);
            Assert.False(isWall);
        }

        [Theory]
        [InlineData(1, 7)] // index 0 first row is the header
        [InlineData(2, 3)]
        [InlineData(3, 3)]
        [InlineData(4, 7)]
        [InlineData(5, 4)]
        [InlineData(6, 3)]
        [InlineData(7, 6)]
        public void Print(int row, int expectedWallCount)
        {
            var rows = Problem.Print(10, 9, 6);

            Assert.Equal(expectedWallCount, rows[row].Count(x => x == '#'));
        }

        [Fact]
        public void FirstStarExample()
        {
            var result = Problem.FirstStarSolution(10, 7, 4);

            Assert.Equal(11, result);
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(2, 5)]
        [InlineData(3, 6)]
        public void SecondStarExample(int maxDepth, int expectedUniqueVisited)
        {
            var result = Problem.SecondStarSolution(10, maxDepth);

            Assert.Equal(expectedUniqueVisited, result);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("82", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("138", actual);
        }
    }
}
