using System;
using System.Drawing;
using Xunit;

namespace Solutions.Event2018.Day11
{
    public class Tests
    {
        [Theory]
        [InlineData(12345, 3)]
        [InlineData(30000000, 0)]
        [InlineData(45, 0)]
        public void HundredsDigit(int input, int expected)
        {
            int actual = Math.Abs(input / 100 % 10);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3,5,8,4)]
        [InlineData(122,79,57,-5)]
        [InlineData(217,196,39,0)]
        [InlineData(101,153,71,4)]
        public void PowerCalculation(int x, int y, int serialNumber, int expectedPower)
        {
            var power = Problem.CalculatePower(new Point(x, y), serialNumber);
            Assert.Equal(expectedPower, power);
        }

        [Theory]
        [InlineData(33, 45, 18, 29)]
        public void PowerForGrid(int x, int y, int serialNumber, int expectedPower)
        {
            var grid = Problem.PowerGrid(serialNumber);
            var power = Problem.PowerForGrid(serialNumber, new Point(x, y), 3, grid);

            Assert.Equal(expectedPower, power);
        }

        [Theory]
        [InlineData(18, "33,45")]
        [InlineData(42, "21,61")]
        public void FirstStarExamples(int serialNumber, string expected)
        {
            var coordinate = Problem.CoordinateOfHighestPowerGrid(serialNumber);
            Assert.Equal(expected, coordinate);
            Assert.Equal(expected, coordinate);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("243,27", actual);
        }

        [Theory]
        [InlineData(18, "90,269,16")]
        [InlineData(42, "232,251,12")]
        public void SecondStarExamples(int serialNumber, string expected)
        {
            var actual = Problem.VaryingGridSize(serialNumber);

            Assert.Equal(expected, actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("284,172,12", actual);
        }
    }
}
