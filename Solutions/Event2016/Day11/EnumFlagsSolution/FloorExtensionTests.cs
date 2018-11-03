using Xunit;

namespace Solutions.Event2016.Day11.EnumFlagsSolution
{
    public class FloorExtensionTests
    {
        [Fact]
        public void Remove_Chips_OnlyGeneratorsReturned()
        {
            var floor = Floor.LithiumChip | Floor.LithiumGenerator;

            var generators = floor.Remove(FloorSetup.Chips);

            Assert.True(generators.HasFlag(Floor.LithiumGenerator));
            Assert.False(generators.HasFlag(Floor.LithiumChip));
        }

        [Theory]
        [InlineData(Floor.LithiumChip)]
        [InlineData(Floor.LithiumChip | Floor.HydrogenChip)]
        [InlineData(Floor.HydrogenChip)]
        public void IsChipsOnly_OnlyChips_True(Floor floorInput)
        {
            bool isChipsOnly = floorInput.IsChipsOnly();

            Assert.True(isChipsOnly);
        }

        [Theory]
        [InlineData(Floor.HydrogenGenerator)]
        [InlineData(Floor.HydrogenGenerator | Floor.LithiumGenerator)]
        public void IsChipsOnly_Generator_False(Floor inputFloor)
        {
            bool chipsOnly = inputFloor.IsChipsOnly();

            Assert.False(chipsOnly);
        }

        [Theory]
        [InlineData(Floor.HydrogenGenerator | Floor.LithiumChip)]
        [InlineData(Floor.HydrogenGenerator | Floor.HydrogenChip | Floor.LithiumChip)]
        [InlineData(Floor.LithiumGenerator | Floor.HydrogenChip)]
        [InlineData(Floor.LithiumGenerator | Floor.HydrogenChip | Floor.LithiumChip)]
        public void HasUnmatchedChip_ChipNotMatchingGenerator_ReturnsTrue(Floor inputFloor)
        {
            bool hasUnmatched = inputFloor.HasUnmatchedChip();

            Assert.True(hasUnmatched);
        }

        [Theory]
        [InlineData(Floor.HydrogenGenerator | Floor.HydrogenChip)]
        [InlineData(Floor.HydrogenGenerator | Floor.HydrogenChip | Floor.LithiumGenerator)]
        public void HasUnmatchedChip_NoUnmatchedChip_False(Floor inputFloor) {
            bool hasUnmatched = inputFloor.HasUnmatchedChip();

            Assert.False(hasUnmatched);
        }

        [Theory]
        [InlineData(Floor.LithiumChip)]
        [InlineData(Floor.LithiumChip | Floor.HydrogenChip)]
        [InlineData(Floor.LithiumGenerator)]
        [InlineData(Floor.LithiumGenerator | Floor.LithiumChip)]
        public void IsSafe_WhenSafe_True(Floor floorUnderTest)
        {
            var safe = floorUnderTest.IsSafe();

            Assert.True(safe);
        }

        [Theory]
        [InlineData(Floor.LithiumChip | Floor.HydrogenGenerator)]
        [InlineData(Floor.LithiumGenerator | Floor.LithiumChip | Floor.HydrogenChip)]
        [InlineData(Floor.PromethiumChip | Floor.StrontiumChip | Floor.StrontiumGenerator | Floor.ThuliumGenerator | Floor.RutheniumGenerator)]
        public void IsSafe_WhenNotSafe_False(Floor floorUnderTest)
        {
            var safe = floorUnderTest.IsSafe();

            Assert.False(safe);
        }

        [Fact]
        public void GenerateSafeCombinations()
        {
            Floor floor = Floor.HydrogenChip | Floor.LithiumChip | Floor.HydrogenGenerator | Floor.LithiumGenerator;

            var combinations = floor.GenerateSafeCombinations();

            Assert.Equal(8, combinations.Count);
            Assert.Contains(Floor.LithiumChip, combinations);
            Assert.Contains(Floor.HydrogenChip, combinations);
            Assert.Contains(Floor.LithiumGenerator, combinations);
            Assert.Contains(Floor.HydrogenGenerator, combinations);
            Assert.Contains(Floor.LithiumChip | Floor.HydrogenChip, combinations);
            Assert.Contains(Floor.LithiumChip | Floor.LithiumGenerator, combinations);
            Assert.Contains(Floor.HydrogenChip | Floor.HydrogenGenerator, combinations);
            Assert.Contains(Floor.LithiumGenerator | Floor.HydrogenGenerator, combinations);
        }
    }
}