using Xunit;

namespace Solutions.Event2017.Day12
{
    public class Tests
    {
        [Fact]
        public static void FirstStarExample()
        {
            var input = new[]
            {
                "0 <-> 2      ",
                "1 <-> 1      ",
                "2 <-> 0, 3, 4",
                "3 <-> 2, 4   ",
                "4 <-> 2, 3, 6",
                "5 <-> 6      ",
                "6 <-> 4, 5   "
            };

            var connectionsToZero = MemoryBank.CountConnectedTo0(input);

            Assert.Equal(6, connectionsToZero);
        }

        [Fact]
        public static void SecondStarExample()
        {
            var input = new[]
            {
                "0 <-> 2      ",
                "1 <-> 1      ",
                "2 <-> 0, 3, 4",
                "3 <-> 2, 4   ",
                "4 <-> 2, 3, 6",
                "5 <-> 6      ",
                "6 <-> 4, 5   "
            };

            var connectionsToZero = MemoryBank.CountGroups(input);

            Assert.Equal(2, connectionsToZero);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("175", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("213", actual);
        }
    }
}