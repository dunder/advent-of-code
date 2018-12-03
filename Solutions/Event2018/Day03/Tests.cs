using System.Collections.Generic;
using Xunit;

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

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("104241", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("806", actual); 
        }
    }
}
