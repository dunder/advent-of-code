using Xunit;

namespace Solutions.Event2016.Day19
{
    public class Tests
    {
        [Fact]
        public void FirstStar_Example()
        {
            int elfLeftWithAllPresents = Problem.ElfGame(5);

            Assert.Equal(3, elfLeftWithAllPresents);
        }

        [Fact]
        public void SecondStar_Example()
        {
            int elfLeftWithAllPresents = Problem.ElfGameWithOpposites(5);

            Assert.Equal(2, elfLeftWithAllPresents);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("1834471", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("1420064", actual);
        }
    }
}
