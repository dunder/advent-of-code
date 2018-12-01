using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Shared.MapGeometry;
using Xunit;

namespace Shared.Tests.MapGeometry
{
    public class PointExtensionsTests
    {
        /// <summary>
        ///     +---------+---------+--------+---------+---------+
        ///     | (-2,2)  | (-1,2)  | (0,2)  | (1,2)   | (2,2)   |
        ///     +---------+---------+--------+---------+---------+
        ///     | (-2,1)  | (-1,1)  | (0,1)  | (1,1)   | (2,1)   |
        ///     +---------+---------+--------+---------+---------+
        ///     | (-2,0)  | (-1,0)  | (0,0)  | (1,0)   | (2,0)   |
        ///     +---------+---------+--------+---------+---------+
        ///     | (-2,-1) | (-1,-1) | (0,-1) | (1, -1) | (2, -1) |
        ///     +---------+---------+--------+---------+---------+
        ///     | (-2,-2) | (-1,-2) | (0,-2) | (1, -2) | (2, -2) |
        ///     +---------+---------+--------+---------+---------+
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="expectedDistance"></param>
        [Theory]
        [InlineData(0, 0, 1, 1, 2)]
        [InlineData(-1, -1, 1, 1, 4)]
        [InlineData(-1, -1, -1, 1, 2)]
        [InlineData(-1, -2, -1, -1, 1)]
        [InlineData(-2, -2, 2, 2, 8)]
        public void ManhattanDistanceToTarget(int x1, int y1, int x2, int y2, int expectedDistance)
        {
            var start = new Point(x1, y1);
            var end = new Point(x2, y2);

            var distance = start.ManhattanDistance(end);

            Assert.Equal(distance, expectedDistance);
        }
    }
}
