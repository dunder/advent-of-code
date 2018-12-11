using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Solutions.Event2018.Day11
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day11;

        private const int Input = 6303;

        public override string FirstStar()
        {
            var result = CoordinateOfHighestPowerGrid(Input);
            return result;
        }

        public override string SecondStar()
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
                    power = power + grid[xi -1, yi - 1];
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
    }
}