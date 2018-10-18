using Xunit;

namespace Solutions.Event2017.Day13
{
    public class Tests
    {
        [Fact]
        public static void Problem1_Example()
        {
            string[] input =
            {
                "0: 3",
                "1: 2",
                "4: 4",
                "6: 4"
            };
            var severity = Firewall.CountSeverity(input);

            Assert.Equal(24, severity);
        }

        [Fact]
        public static void Problem2_Example()
        {
            string[] input =
            {
                "0: 3",
                "1: 2",
                "4: 4",
                "6: 4"
            };
            var delay = Firewall.DelayToSafe(input);

            Assert.Equal(10, delay);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("2160", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("3907470", actual);
        }
    }
}