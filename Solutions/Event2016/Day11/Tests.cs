using System.Linq;
using Xunit;

namespace Solutions.Event2016.Day11 {
    public class Tests {

        [Fact]
        public void AssemblyEquality()
        {
            Assert.Equal(new Assembly().WithChip(Element.Hydrogen), new Assembly().WithChip(Element.Hydrogen));
        }

        [Fact]
        public void SelectValidAssembliesForElevator()
        {
            var floor1 = new Assembly();
            var floor2 = new Assembly();
            var floor3 = new Assembly();
            var floor4 = new Assembly();

            floor1
                .WithChip(Element.Hydrogen)
                .WithChip(Element.Lithium)
                .WithUpperFloor(floor2);

            floor2
                .WithGenerator(Element.Hydrogen)
                .WithLowerFloor(floor1)
                .WithUpperFloor(floor3);

            floor3
                .WithGenerator(Element.Lithium)
                .WithLowerFloor(floor2)
                .WithUpperFloor(floor4);

            floor4
                .WithLowerFloor(floor3);

            var moves = floor1.SelectValidAssembliesForElevator().ToList();

            Assert.Equal(3, moves.Count);
            Assert.Contains(new Assembly().WithChip(Element.Hydrogen), moves);
            Assert.Contains(new Assembly().WithChip(Element.Lithium), moves);
            Assert.Contains(new Assembly().WithChip(Element.Hydrogen).WithChip(Element.Lithium), moves);
        }

        [Fact]
        public void SafeAssembly()
        {
            var assembly = new Assembly()
                .WithChip(Element.Hydrogen);

            Assert.True(assembly.IsSafe());

            assembly = new Assembly()
                .WithChip(Element.Lithium)
                .WithChip(Element.Hydrogen);

            Assert.True(assembly.IsSafe());

            assembly = new Assembly()
                .WithGenerator(Element.Hydrogen);

            Assert.True(assembly.IsSafe());

            assembly = new Assembly()
                .WithChip(Element.Hydrogen)
                .WithGenerator(Element.Hydrogen);

            Assert.True(assembly.IsSafe());

            assembly = new Assembly()
                .WithChip(Element.Lithium)
                .WithGenerator(Element.Hydrogen);

            Assert.False(assembly.IsSafe());
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
