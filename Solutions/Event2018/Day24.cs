using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day24 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day24;
        public string Name => "Day24";

        public string FirstStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual);
        }
    }
}