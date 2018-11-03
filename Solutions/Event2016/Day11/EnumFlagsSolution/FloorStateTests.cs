using System.Collections.Generic;
using Xunit;

namespace Solutions.Event2016.Day11.EnumFlagsSolution
{
    public class FloorStateTests
    {
        [Fact]
        public void Equals_TwoIdenticalStates_AreEqual()
        {
            var floor10 = Floor.HydrogenChip | Floor.HydrogenGenerator;
            var floor11 = Floor.LithiumChip | Floor.LithiumGenerator;
            var state1 = new FloorState(1, new List<Floor> {floor10, floor11});

            var floor20 = Floor.HydrogenChip | Floor.HydrogenGenerator;
            var floor21 = Floor.LithiumChip | Floor.LithiumGenerator;
            var state2 = new FloorState(1, new List<Floor> {floor20, floor21});

            Assert.Equal(state1, state2);
        }

        [Fact]
        public void GetHashCode_TwoIdenticalStates_SameHashCode()
        {
            var floor10 = Floor.HydrogenChip | Floor.HydrogenGenerator;
            var floor11 = Floor.LithiumChip | Floor.LithiumGenerator;
            var state1 = new FloorState(1, new List<Floor> { floor10, floor11 });

            var floor20 = Floor.HydrogenChip | Floor.HydrogenGenerator;
            var floor21 = Floor.LithiumChip | Floor.LithiumGenerator;
            var state2 = new FloorState(1, new List<Floor> { floor20, floor21 });

            var hashCode1 = state1.GetHashCode();
            var hashCode2 = state2.GetHashCode();

            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void Next_WhenSafeGeneratorChipCombinationExists_ReturnsNewStates()
        {
            var floor0 = Floor.HydrogenChip | Floor.LithiumChip;
            var floor1 = Floor.HydrogenGenerator;

            var state = new FloorState(0, new List<Floor> {floor0, floor1});

            var expectedFloor0 = Floor.LithiumChip;
            var expectedFloor1 = Floor.HydrogenChip | Floor.HydrogenGenerator;

            var expectedState = new FloorState(1, new List<Floor> {expectedFloor0, expectedFloor1}, 1);

            var nextStates = state.Next();

            Assert.Single(nextStates, expectedState);
        }
    }
}