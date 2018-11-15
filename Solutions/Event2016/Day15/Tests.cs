using System.Collections.Generic;
using Xunit;

namespace Solutions.Event2016.Day15
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            var discs = new List<Problem.Disc>
            {
                new Problem.Disc(1, 4, 5),
                new Problem.Disc(2, 1, 2)
            };

            int tRelease = Problem.ReleaseAtToGetCapsule(discs);

            Assert.Equal(5, tRelease);
        }

        [Theory]
        [InlineData(0,3,0,0)]
        [InlineData(0,3,1,1)]
        [InlineData(0,3,2,2)]
        [InlineData(0,3,3,0)]
        [InlineData(0,3,4,1)]
        [InlineData(0,3,5,2)]
        [InlineData(0,3,6,0)]
        [InlineData(0,3,7,1)]
        [InlineData(1,7,0,1)]
        [InlineData(1,7,1,2)]
        [InlineData(1,7,8,2)]
        [InlineData(1,7,15,2)]
        public void Disc_PositionAtPassage(int discInitialPosition, int discSize, int t, int expectedPosition)
        {
            var disc = new Problem.Disc(0, discInitialPosition, discSize);

            int position = disc.PositionAtPassage(t);

            Assert.Equal(expectedPosition, position);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 0)]
        public void Disc_PositionAtPassageWithOffset(int tPress, int expectedPosition)
        {
            var disc = new Problem.Disc(1, 2, 3);

            int position = disc.PositionAtPassage(tPress);

            Assert.Equal(expectedPosition, position);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("122318", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("3208583", actual);
        }
    }
}
