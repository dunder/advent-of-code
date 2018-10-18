using Xunit;

namespace Solutions.Event2017.Day01
{
    public class Tests
    {
        [Theory]
        [InlineData("1122", "3")]
        [InlineData("1111", "4")]
        [InlineData("1234", "0")]
        [InlineData("91212129", "9")]
        public void FirstStarExample(string input, string expectedSum)
        {
            var result = Problem.Captcha(input);
            Assert.Equal(expectedSum, result);
        }

        [Theory]
        [InlineData("1212", "6")]
        [InlineData("1221", "0")]
        [InlineData("123425", "4")]
        [InlineData("123123", "12")]
        [InlineData("12131415", "4")]
        public void SecondStarExample(string input, string expectedSum)
        {
            var result = Problem.CaptchaHalfway(input);
            Assert.Equal(expectedSum, result);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("1216", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("1072", actual);
        }
    }
}