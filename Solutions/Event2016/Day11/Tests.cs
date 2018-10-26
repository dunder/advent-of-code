using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Solutions.Event2016.Day11
{
    public class Tests
    {

        [Fact]
        public void BuildingEqualityChips()
        {
            var building1 = new BuildingState(1, new List<Assembly>
            {
                new Assembly().WithChip(Element.Hydrogen),
            });

            var building2 = new BuildingState(1, new List<Assembly>
            {
                new Assembly().WithChip(Element.Hydrogen),
            });

            Assert.Equal(building1, building2);
        }

        [Fact]
        public void BuildingEqualityMixOfChipAndGenerator()
        {
            var building1 = new BuildingState(1, new List<Assembly>
            {
                new Assembly().WithChip(Element.Hydrogen),
                new Assembly().WithGenerator(Element.Hydrogen)
            });

            var building2 = new BuildingState(1, new List<Assembly>
            {
                new Assembly().WithChip(Element.Hydrogen),
                new Assembly().WithGenerator(Element.Hydrogen)
            });

            Assert.Equal(building1, building2);
        }

        [Fact]
        public void BuildingHashCode()
        {
            var building1 = new BuildingState(1, new List<Assembly>
            {
                new Assembly().WithChip(Element.Hydrogen),
                new Assembly().WithGenerator(Element.Hydrogen)
            });

            var building2 = new BuildingState(1, new List<Assembly>
            {
                new Assembly().WithChip(Element.Hydrogen),
                new Assembly().WithGenerator(Element.Hydrogen)
            });

            var hashCode1 = building1.GetHashCode();
            var hashCode2 = building2.GetHashCode();

            Assert.Equal(hashCode1, hashCode2);
        }


        [Fact]
        public void AssemblyEquality()
        {
            Assert.Equal(new Assembly().WithChip(Element.Hydrogen), new Assembly().WithChip(Element.Hydrogen));
        }

        [Fact]
        public void AssemblyGetHashCode()
        {
            var assembly1 = new Assembly().WithChip(Element.Hydrogen).WithGenerator(Element.Hydrogen);
            var assembly2 = new Assembly().WithGenerator(Element.Hydrogen).WithChip(Element.Hydrogen);

            Assert.Equal(assembly1.GetHashCode(), assembly2.GetHashCode());
        }

        [Fact]
        public void SelectValidAssembliesForElevator()
        {
            var assembly = new Assembly()
                .WithChip(Element.Hydrogen)
                .WithChip(Element.Lithium);

            var moves = assembly.SafeChipGeneratorCombinations().ToList();

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
        public void SafeAssembly_ChipWithoutGenerator_NotSafe()
        {
            var assembly = new Assembly()
                .WithChip(Element.Lithium)
                .WithChip(Element.Hydrogen)
                .WithGenerator(Element.Hydrogen);

            Assert.False(assembly.IsSafe());
        }

        [Fact]
        public void SafeAssembly_ChipWithGeneratorWhenOtherGenerators_Safe()
        {
            var assembly = new Assembly()
                .WithChip(Element.Hydrogen)
                .WithGenerator(Element.Hydrogen)
                .WithGenerator(Element.Lithium);

            Assert.True(assembly.IsSafe());
        }

        [Fact]
        public void AssemblyMerge()
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
        public void AssemblyRelease()
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
        public void FirstStarExample()
        {
            var floor1Assembly = new Assembly().WithChip(Element.Hydrogen).WithChip(Element.Lithium);
            var floor2Assembly = new Assembly().WithGenerator(Element.Hydrogen);
            var floor3Assembly = new Assembly().WithGenerator(Element.Lithium);
            var floor4Assembly = new Assembly();

            var initialBuildingState = new BuildingState(0, new List<Assembly>
            {
                floor1Assembly,
                floor2Assembly,
                floor3Assembly,
                floor4Assembly
            });

            int steps = Problem.MinimumStepsToTopFloor(initialBuildingState, 4, 4);

            Assert.Equal(11, steps);
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