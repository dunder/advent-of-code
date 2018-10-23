using System;
using System.Linq;
using Facet.Combinatorics;
using Xunit;
using Xunit.Abstractions;

namespace Solutions.Event2016.Day11 {
    public class Tests {

        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void AssemblyEquality()
        {
            Assert.Equal(new Assembly().WithChip(Element.Hydrogen), new Assembly().WithChip(Element.Hydrogen));
        }

        [Fact]
        public void SelectValidAssembliesForElevator()
        {
            var floor1 = new Assembly();

            floor1
                .WithChip(Element.Hydrogen)
                .WithChip(Element.Lithium);

            var moves = floor1.SelectValidAssembliesForElevator().ToList();

            Assert.Equal(3, moves.Count);
            Assert.Contains(new Assembly().WithChip(Element.Hydrogen), moves);
            Assert.Contains(new Assembly().WithChip(Element.Lithium), moves);
            Assert.Contains(new Assembly().WithChip(Element.Hydrogen).WithChip(Element.Lithium), moves);
        }

        [Fact]
        public void SafeAssembly_SingleChip_Safe()
        {
            var assembly = new Assembly()
                .WithChip(Element.Hydrogen);

            Assert.True(assembly.IsSafe());
        }

        [Fact]
        public void SafeAssembly_MultipleChips_Safe()
        {
            var assembly = new Assembly()
                .WithChip(Element.Lithium)
                .WithChip(Element.Hydrogen);

            Assert.True(assembly.IsSafe());
        }

        [Fact]
        public void SafeAssembly_SingleGenerator_Safe()
        {
            var assembly = new Assembly()
                .WithGenerator(Element.Hydrogen);

            Assert.True(assembly.IsSafe());
        }

        [Fact]
        public void SafeAssembly_MatchingChipAndGenerator_Safe()
        {
            var assembly = new Assembly()
                .WithChip(Element.Hydrogen)
                .WithGenerator(Element.Hydrogen);
            Assert.True(assembly.IsSafe());
        }

        [Fact]
        public void SafeAssembly_MismatchingChipAndGenerator_NotSafe()
        {
            var assembly = new Assembly()
                .WithChip(Element.Lithium)
                .WithGenerator(Element.Hydrogen);

            Assert.False(assembly.IsSafe());
        }

        [Fact]
        public void Merge()
        {
            var assembly1 = new Assembly()
                .WithChip(Element.Lithium)
                .WithGenerator(Element.Hydrogen);

            var assembly2 = new Assembly()
                .WithChip(Element.Hydrogen);

            var merged = assembly1.Merge(assembly2);

            var expected = new Assembly()
                .WithChip(Element.Hydrogen)
                .WithChip(Element.Lithium)
                .WithGenerator(Element.Hydrogen);

            Assert.NotSame(merged, assembly1);
            Assert.NotSame(merged, assembly2);
            Assert.Equal(expected, merged);

        }

        [Fact]
        public void Release()
        {
            var assembly1 = new Assembly()
                .WithChip(Element.Lithium)
                .WithChip(Element.Hydrogen)
                .WithGenerator(Element.Lithium)
                .WithGenerator(Element.Hydrogen);

            var assembly2 = new Assembly()
                .WithChip(Element.Lithium)
                .WithGenerator(Element.Lithium);

            var released = assembly1.Release(assembly2);

            var expected = new Assembly()
                .WithChip(Element.Hydrogen)
                .WithGenerator(Element.Hydrogen);

            Assert.NotSame(released, assembly1);
            Assert.NotSame(released, assembly2);
            Assert.Equal(expected, released);
        }

        [Fact]
        public void ValidAlternatives()
        {
            var assembly1 = new Assembly();
            var assembly2 = new Assembly();
            var assembly3 = new Assembly();
            var assembly4 = new Assembly();

            assembly1
                .WithChip(Element.Hydrogen)
                .WithChip(Element.Lithium)
                .WithUpperFloor(assembly2);

            assembly2
                .WithGenerator(Element.Hydrogen)
                .WithLowerFloor(assembly1)
                .WithUpperFloor(assembly3);

            assembly3
                .WithGenerator(Element.Lithium)
                .WithLowerFloor(assembly2)
                .WithUpperFloor(assembly4);

            assembly4
                .WithLowerFloor(assembly3);

            var floor1 = new Floor(1, assembly1);
            var floor2 = new Floor(2, assembly1);
            var floor3 = new Floor(3, assembly1);
            var floor4 = new Floor(4, assembly1);

            floor1.Upper = floor2;
            floor2.Lower = floor1;
            floor2.Upper = floor3;
            floor3.Lower = floor2;
            floor3.Upper = floor4;
            floor4.Lower = floor3;

            var validAlternatives = floor1.ValidAlternatives();

            
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
