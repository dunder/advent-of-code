using Xunit;

namespace Solutions.Event2016.Day16
{
    public class Tests
    {
        [Theory]
        [InlineData("1", "100")]
        [InlineData("0", "001")]
        [InlineData("11111", "11111000000")]
        [InlineData("111100001010", "1111000010100101011110000")]
        public void Generate(string input, string expected)
        {
            var generated = Problem.Generate(input);

            Assert.Equal(expected, generated);
        }

        [Fact]
        public void Process()
        {
            var output = Problem.Process("10000", 20);

            Assert.Equal("10000011110010000111110", output);
        }

        [Fact]
        public void Checksum()
        {
            var checksum = Problem.Checksum("110010110100");

            Assert.Equal("100", checksum);
        }

        [Fact]
        public void Problem1_Example()
        {
            var checksum = Problem.CalculateChecksum("10000", 20);

            Assert.Equal("01100", checksum);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("10100011010101011", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("01010001101011001", actual);
        }
    }
}
