using Xunit;

namespace Solutions.Event2017.Day24
{
    public class Tests
    {
        [Fact]
        public static void FirstStarExample()
        {
            string[] input =
            {
                "0/2",
                "2/2",
                "2/3",
                "3/4",
                "3/5",
                "0/1",
                "10/1",
                "9/10"
            };

            var result = ElectromagneticMoat.StrongestBridge(input);

            Assert.Equal(31, result);
        }

        [Fact]
        public static void SecondStarExample()
        {
            string[] input =
            {
                "0/2",
                "2/2",
                "2/3",
                "3/4",
                "3/5",
                "0/1",
                "10/1",
                "9/10"
            };

            var result = ElectromagneticMoat.StrongestLongestBridge(input);

            Assert.Equal(19, result);
        }

        [Fact]
        public static void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("1695", actual);
        }

        [Fact]
        public static void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("1673", actual);
        }
    }
}