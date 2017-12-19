using System.Drawing;
using System.IO;
using Utilities.MapGeometry;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day01 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            string input = File.ReadAllText(@".\Day01\input.txt");
            var result = TaxiMap.ShortestPath(new Point(0, 0), Direction.North, input);

            Assert.Equal(209, result);
            _output.WriteLine($"Day 1 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string input = File.ReadAllText(@".\Day01\input.txt");

            var result = TaxiMap.DistanceToFirstIntersection(new Point(0, 0), Direction.North, input);

            Assert.Equal(136, result);
            _output.WriteLine($"Day 1 problem 2: {result}");
        }
    }
}
