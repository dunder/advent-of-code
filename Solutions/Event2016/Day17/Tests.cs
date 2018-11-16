using Xunit;

namespace Solutions.Event2016.Day17
{
    public class Tests
    {
        [Fact]
        public void IsDoorOpen()
        {
            Assert.All("bcdef0123456789", c => Problem.IsDoorOpen(c));
        }

        [Fact]
        public void IsDoorClosed()
        {
            Assert.False(Problem.IsDoorOpen('a'));
        }

        [Theory]
        [InlineData("ihgpwlah", "DDRRRD")]
        [InlineData("kglvqrro", "DDUDRLRRUDRD")]
        [InlineData("ulqzkmiv", "DRURDRUDDLLDLUURRDULRLDUUDDDRR")]
        public void FirstStar_Example(string passcode, string expectedPath)
        {
            var path = Problem.ShortestPath(passcode);

            Assert.Equal(expectedPath, path);
        }

        [Theory]
        [InlineData("ihgpwlah", 370)]
        [InlineData("kglvqrro", 492)]
        [InlineData("ulqzkmiv", 830)]
        public void SecondStar_Example(string passcode, int expectedLength)
        {
            var length = Problem.LongestPath(passcode);

            Assert.Equal(expectedLength, length);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("DUDRDLRRRD", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("502", actual);
        }
    }
}
