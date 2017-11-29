using System.Drawing;
using Xunit;

namespace Y2016.Tests {
    public class Day1Tests {

        [Theory]
        [InlineData("R2, L3", 5)]
        [InlineData("R2, R2, R2", 2)]
        [InlineData("R5, L5, R5, R3", 12)]
        public void Day1_Problem1(string movements, int expectedDistance) {
            
            var taxiMap = new TaxiMap(new Point(0,0), Direction.North);

            var distance = taxiMap.ShortestPath(movements);

            Assert.Equal(expectedDistance, distance);
        }


        [Theory]
        [InlineData("R8, R4, R4, R8", 4)]
        public void Day2_Problem2(string movements, int expectedDistance) {

            var taxiMap = new TaxiMap(new Point(0, 0), Direction.North);

            var distance = taxiMap.DistanceToFirstRoundabout(movements);

            Assert.Equal(expectedDistance, distance);
        }
    }
}
