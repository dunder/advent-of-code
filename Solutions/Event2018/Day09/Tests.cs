using System.Collections.Generic;
using Xunit;

namespace Solutions.Event2018.Day09
{
    public class Tests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(9, -7, 2)]
        [InlineData(0, -7, 3)]
        [InlineData(1, -7, 4)]
        public void ClockwiseIndexing(int index, int steps, int expectedIndex)
        {
            List<int> list = new List<int> {0,1,2,3,4,5,6,7,8,9};

            var actual = Problem.ClockwiseIndex(list, index, steps);
            Assert.Equal(expectedIndex, actual);
        }

        [Fact]
        public void TestEnd()
        {
            List<int> list = new List<int> {0, 1, 2, 3, 4, 5};
            var index = 5;

            list.RemoveAt(index);

            var current = index;
            if (current >= list.Count)
            {
                current = 0;
            }

            Assert.Equal(5, list.Count);
            Assert.Equal(0, current);

        }

        [Theory]
        [InlineData(9, 25, 32)]
        [InlineData(10, 1618, 8317)]
        [InlineData(13, 7999, 146373)]
        [InlineData(17, 1104, 2764)]
        [InlineData(21, 6111, 54718)]
        [InlineData(30, 5807, 37305)]
        public void FirstStar_Example(int players, int scoreLastMarble, int expectedScore)
        {
            var score = Problem.WinnerScore(players, scoreLastMarble);

            Assert.Equal(expectedScore, score);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("371284", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("3038972494", actual);
        }
    }
}
