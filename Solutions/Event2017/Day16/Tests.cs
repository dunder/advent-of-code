using Xunit;

namespace Solutions.Event2017.Day16
{
    public class Tests
    {
        [Theory]
        [InlineData("s1", "eabcd")]
        [InlineData("s1,x3/4", "eabdc")]
        [InlineData("s1,x3/4,pe/b", "baedc")]
        public static void FirstStarExample(string instructions, string expected)
        {
            var positions = Dancing.Read("abcde", instructions);

            Assert.Equal(expected, positions);
        }

        [Fact]
        public static void SecondStarExample()
        {
            string instructions = "s3";
            var order = Dancing.Read("abcde", instructions);

            Assert.Equal("cdeab", order);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("kgdchlfniambejop", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("fjpmholcibdgeakn", actual);
        }
    }
}