using System;
using System.Drawing;
using Xunit;

namespace Solutions.Event2018
{
    public class Day11
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day11;

        private const int Input = 6303;

        public string FirstStar()
        {
            var result = CoordinateOfHighestPowerGrid(Input);
            return result;
        }

        public string SecondStar()
        {
            var result = VaryingGridSize(Input);
            return result;
        }

        public static string CoordinateOfHighestPowerGrid(int serialNumber)
        {
            var grid = PowerGrid(serialNumber);
            var minX = 1;
            var maxX = 300 - 3;
            var minY = 1;
            var maxY = 300 - 3;
            var maxPowerLevel = 0;
            Point maxAt = new Point();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var topLeft = new Point(x, y);
                    var power = PowerForGrid(serialNumber, topLeft, 3, grid);

                    if (power > maxPowerLevel)
                    {
                        maxPowerLevel = power;
                        maxAt = topLeft;
                    }
                }
            }
            return $"{maxAt.X},{maxAt.Y}";
        }

        public static int PowerForGrid(int serialNumber, Point topLeft, int size, int[,] grid)
        {
            var power = 0;
            for (int xi = topLeft.X; xi < topLeft.X + size; xi++)
            {
                for (int yi = topLeft.Y; yi < topLeft.Y + size; yi++)
                {
                    power = power + grid[xi - 1, yi - 1];
                }
            }
            return power;
        }

        public static int[,] PowerGrid(int serialNumber)
        {
            var grid = new int[300, 300];
            for (int y = 1; y <= 300; y++)
            {
                for (int x = 1; x <= 300; x++)
                {
                    var power = CalculatePower(new Point(x, y), serialNumber);
                    grid[x - 1, y - 1] = power;
                }
            }

            return grid;
        }

        public static string VaryingGridSize(int serialNumber)
        {
            var grid = PowerGrid(serialNumber);

            int sizeAtMax = 0;
            var maxPowerLevel = 0;
            Point maxAt = new Point();

            string CurrentMax()
            {
                return $"{maxAt.X},{maxAt.Y},{sizeAtMax}";
            }

            var previousMax = CurrentMax();
            var noChangeCounter = 0;

            for (int size = 1; size <= 300; size++)
            {
                var minX = 1;
                var maxX = 300 - size;
                var minY = 1;
                var maxY = 300 - size;


                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        var topLeft = new Point(x, y);
                        var power = PowerForGrid(serialNumber, topLeft, size, grid);
                        if (power > maxPowerLevel)
                        {
                            maxPowerLevel = power;
                            maxAt = topLeft;
                            sizeAtMax = size;
                        }
                    }
                }

                var maxAfterNewSize = CurrentMax();
                if (previousMax == maxAfterNewSize)
                {
                    noChangeCounter++;
                }

                if (noChangeCounter > 3)
                {
                    break;
                }

                previousMax = CurrentMax();
            }

            return CurrentMax();
        }

        public static int CalculatePower(Point topLeft, int serialNumber)
        {
            var rackId = topLeft.X + 10;
            var powerLevel = rackId * topLeft.Y;
            powerLevel = powerLevel + serialNumber;
            powerLevel = powerLevel * rackId;
            powerLevel = Math.Abs(powerLevel / 100 % 10);
            powerLevel = powerLevel - 5;

            return powerLevel;
        }

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
        [InlineData(3, 5, 8, 4)]
        [InlineData(122, 79, 57, -5)]
        [InlineData(217, 196, 39, 0)]
        [InlineData(101, 153, 71, 4)]
        public void PowerCalculation(int x, int y, int serialNumber, int expectedPower)
        {
            var power = CalculatePower(new Point(x, y), serialNumber);
            Assert.Equal(expectedPower, power);
        }

        [Theory]
        [InlineData(33, 45, 18, 29)]
        public void PowerForGridTest(int x, int y, int serialNumber, int expectedPower)
        {
            var grid = PowerGrid(serialNumber);
            var power = PowerForGrid(serialNumber, new Point(x, y), 3, grid);

            Assert.Equal(expectedPower, power);
        }

        [Theory]
        [InlineData(18, "33,45")]
        [InlineData(42, "21,61")]
        public void FirstStarExamples(int serialNumber, string expected)
        {
            var coordinate = CoordinateOfHighestPowerGrid(serialNumber);
            Assert.Equal(expected, coordinate);
            Assert.Equal(expected, coordinate);
        }
        [Theory]
        [InlineData(18, "90,269,16")]
        [InlineData(42, "232,251,12")]
        public void SecondStarExamples(int serialNumber, string expected)
        {
            var actual = VaryingGridSize(serialNumber);

            Assert.Equal(expected, actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("243,27", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("284,172,12", actual);
        }
    }
}
