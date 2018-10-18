using Xunit;

namespace Solutions.Event2016.Day11 {
    public class Tests {

        [Fact]
        public void Assembly_Moves()
        {
            var floor1 = new Assembly()
                .WithChip(Element.Hydrogen)
                .WithChip(Element.Lithium)
                .WithUpperFloor(new Assembly());

            var moves = floor1.MovableAssemblies();
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("31", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("55", actual);
        }
    }
}
