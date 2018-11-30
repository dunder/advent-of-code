using System.Collections.Generic;
using System.Drawing;
using Xunit;

namespace Solutions.Event2016.Day22
{
    public class Tests
    {
        [Fact]
        public void TestParse()
        {
            var nodeDescriptions = new List<string>
            {
                "/dev/grid/node-x0-y0   10T    8T     2T   80%",
                "/dev/grid/node-x0-y1   11T    6T     5T   54%",
                "/dev/grid/node-x0-y2   32T   28T     4T   87%",
                "/dev/grid/node-x1-y0    9T    7T     2T   77%",
                "/dev/grid/node-x1-y1    8T    0T     8T    0%",
                "/dev/grid/node-x1-y2   11T    7T     4T   63%",
                "/dev/grid/node-x2-y0   10T    6T     4T   60%",
                "/dev/grid/node-x2-y1    9T    8T     1T   88%",
                "/dev/grid/node-x2-y2    9T    6T     3T   66%"
            };

            var result = Problem.ParseNodes(nodeDescriptions);

            Assert.Equal(9, result.Count);
            Assert.Contains(new Problem.MemoryNode(new Point(0, 0), 10, 8), result);
            Assert.Contains(new Problem.MemoryNode(new Point(2, 2), 9, 6), result);
        }

        [Fact]
        public void TargetNode()
        {
            var nodeDescriptions = new List<string>
            {
                "/dev/grid/node-x0-y0   10T    8T     2T   80%",
                "/dev/grid/node-x0-y1   11T    6T     5T   54%",
                "/dev/grid/node-x0-y2   32T   28T     4T   87%",
                "/dev/grid/node-x1-y0    9T    7T     2T   77%",
                "/dev/grid/node-x1-y1    8T    0T     8T    0%",
                "/dev/grid/node-x1-y2   11T    7T     4T   63%",
                "/dev/grid/node-x2-y0   10T    6T     4T   60%",
                "/dev/grid/node-x2-y1    9T    8T     1T   88%",
                "/dev/grid/node-x2-y2    9T    6T     3T   66%"
            };

            var nodes = Problem.ParseNodes(nodeDescriptions);

            var targetNode = Problem.TargetNode(nodes);

            Assert.Equal(new Problem.MemoryNode(new Point(2, 0), 0, 0), targetNode);
        }

        [Fact]
        public void CountViablePairs()
        {
            var nodeDescriptions = new List<string>
            {
                "/dev/grid/node-x0-y0   10T    8T     2T   80%",
                "/dev/grid/node-x0-y1   11T    6T     5T   54%",
                "/dev/grid/node-x1-y0    9T    7T     2T   77%",
                "/dev/grid/node-x1-y1    8T    0T     8T    0%"
            };

            var count = Problem.CountViablePairs(nodeDescriptions);

            Assert.Equal(10, count);

        }
        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("1045", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("265", actual);
        }
    }
}
