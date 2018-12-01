using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace Solutions.Event2016.Day22
{
    public class AStarThreeTests
    {
        [Fact]
        public void Example()
        {
            var start = new Point(2,2);
            var target = new Point(6, 2);

            var nonWalkable = new HashSet<Point>
            {
                new Point(4, 1),
                new Point(4, 2),
                new Point(4, 3)
            };

            var terminatingNode = AStar.Search(start, target, p => !nonWalkable.Contains(p) && p.X >= 0 && p.Y >= 0);

            Assert.Equal(target, terminatingNode.Position);
        }
    }
}
