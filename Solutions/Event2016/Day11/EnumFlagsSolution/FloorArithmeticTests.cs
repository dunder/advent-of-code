using Xunit;

namespace Solutions.Event2016.Day11.EnumFlagsSolution
{
    public class FloorArithmeticTests
    {
        [Fact]
        public void Chips_HasAllChips()
        {
            Assert.True(FloorSetup.Chips.HasFlag(Floor.HydrogenChip));
            Assert.True(FloorSetup.Chips.HasFlag(Floor.LithiumChip));
        }
        [Fact]
        public void Chips_HasNoGenerators()
        {
            Assert.False(FloorSetup.Chips.HasFlag(Floor.HydrogenGenerator));
            Assert.False(FloorSetup.Chips.HasFlag(Floor.LithiumGenerator));
        }
        [Fact]
        public void Generators_HasAllGenerators()
        {
            Assert.True(FloorSetup.Generators.HasFlag(Floor.HydrogenGenerator));
            Assert.True(FloorSetup.Generators.HasFlag(Floor.LithiumGenerator));
        }
        [Fact]
        public void Generators_HasNoChips()
        {
            bool hasHydrogenChip = FloorSetup.Generators.HasFlag(Floor.HydrogenChip);
            bool hasLithiumChip = FloorSetup.Generators.HasFlag(Floor.LithiumChip);

            Assert.False(hasHydrogenChip);
            Assert.False(hasLithiumChip);
        }
    }
}