using Shared.Crypto;
using Xunit;


namespace Solutions.Event2015
{
    // --- Day 4: The Ideal Stocking Stuffer ---
    public class Day04
    {
        private const string SecretKey = "bgvyzdsv";


        public static int Crunch(string secretKey, string startsWith)
        {
            int counter = 0;
            string result = Md5.Hash(secretKey + counter);

            while (!result.StartsWith(startsWith))
            {
                counter++;
                result = Md5.Hash(secretKey + counter);
            }

            return counter;
        }

        public static int FirstStar()
        {
            return Crunch(SecretKey, "00000");
        }

        public static int SecondStar()
        {
            return Crunch(SecretKey, "000000");
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(254575, result);
        }
        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(1038736, result);
        }

        [Theory]
        [InlineData("abcdef", 609043)]
        [InlineData("pqrstuv", 1048970)]
        public void FirstStar_Examples(string secretKey, int expectedCount)
        {
            var result = Crunch(secretKey, "00000");

            Assert.Equal(expectedCount, result);
        }
    }
}
