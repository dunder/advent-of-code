using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Solutions.Event2018
{
    public class Day09
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day09;

        public string FirstStar()
        {
            var score = WinnerScore(473, 70904);
            return score.ToString();
        }

        public string SecondStar()
        {
            var score = WinnerScore(473, 70904 * 100);
            return score.ToString();
        }

        public static long WinnerScore(int players, int lastMarblePoint)
        {
            var circle = new LinkedList<int>();
            circle.AddFirst(1);
            circle.AddFirst(0);

            var current = circle.Last;

            var scores = new Dictionary<int, long>();
            for (var marble = 2; marble <= lastMarblePoint; marble++)
            {
                var player = marble % players;
                if (player > players) player = 1;
                if (marble % 23 != 0)
                {
                    var next = current?.Next ?? circle.First;
                    current = circle.AddAfter(next, marble);
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        current = current.Previous ?? circle.Last;
                    }
                    var score = marble + current.Value;
                    var tmp = current.Next;
                    circle.Remove(current);
                    current = tmp;

                    if (!scores.ContainsKey(player))
                    {
                        scores.Add(player, score);
                    }
                    else
                    {
                        scores[player] += score;
                    }
                }
            }

            return scores.Values.Max();
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
            var score = WinnerScore(players, scoreLastMarble);

            Assert.Equal(expectedScore, score);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("371284", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("3038972494", actual);
        }
    }
}
