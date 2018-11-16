using Xunit;

namespace Solutions.Event2016.Day18
{
    public class Tests
    {
        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("1951", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("20002936", actual);
        }
    }
}
