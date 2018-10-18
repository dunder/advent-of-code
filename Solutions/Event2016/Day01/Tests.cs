using System.Drawing;
using Shared.MapGeometry;
using Xunit;

namespace Solutions.Event2016.Day01
{
    public class Tests
    {
        [Theory]
        [InlineData("R2, L3", 5)]
        [InlineData("R2, R2, R2", 2)]
        [InlineData("R5, L5, R5, R3", 12)]
        public void FirstStarExample(string movements, int expectedDistance)
        {
            var distance = TaxiMap.ShortestPath(new Point(0, 0), Direction.North, movements);

            Assert.Equal(expectedDistance, distance);
        }

        [Theory]
        [InlineData("R8, R4, R4, R8", 4)]
        public void SecondStarExample(string movements, int expectedDistance)
        {
            var distance = TaxiMap.DistanceToFirstIntersection(new Point(0, 0), Direction.North, movements);

            Assert.Equal(expectedDistance, distance);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("209", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("136", actual);
        }
    }
}