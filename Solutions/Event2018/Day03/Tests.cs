using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace Solutions.Event2018.Day03
{
    public class Tests
    {
        [Fact]
        public void Parse()
        {
            var descriptions = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            };

            var claims = Problem.Parse(descriptions);

            Assert.Equal(descriptions[0], claims[0].ToString());
            Assert.Equal(descriptions[1], claims[1].ToString());
            Assert.Equal(descriptions[2], claims[2].ToString());
        }

        [Fact]
        public void Overlaps()
        {
            var descriptions = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            };

            var claims = Problem.Parse(descriptions);

            var overlaps = claims[0].Overlaps(claims[1]);

            Assert.Equal(4, overlaps.Count);

            overlaps = claims[0].Overlaps(claims[2]);

            Assert.Equal(0, overlaps.Count);

        }

        [Fact]
        public void Count()
        {
            var descriptions = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            };

            var claims = Problem.Parse(descriptions);

            var count = Problem.CountOverlaps(claims);

            Assert.Equal(4, count);
        }

        [Fact]
        public void FindNonOverlapping()
        {
            var descriptions = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            };

            var claims = Problem.Parse(descriptions);

            var id = Problem.FindOnlyNonOverlapping(claims);

            Assert.Equal(3, id);
        }

        [Trait("Category", "LongRunning")] // 2 m 0 s
        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("104241", actual);
        }

        [Trait("Category", "LongRunning")] // 55 s
        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("806", actual); 
        }
    }
}
