using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solutions.Event2016.Day11.EnumFlagsSolution
{
    public class FloorState
    {
        public int ElevatorFloor { get; }
        public List<Floor> Floors { get; }
        public int Generation { get; }
        private Floor CurrentFloor => Floors[ElevatorFloor];
        private bool CanMoveUp => ElevatorFloor < Floors.Count - 1;
        private bool CanMoveDown => ElevatorFloor > 0;
        private Floor UpperFloor => Floors[ElevatorFloor + 1];
        private Floor LowerFloor => Floors[ElevatorFloor - 1];

        public FloorState(int elevatorFloor, List<Floor> floors, int generation = 0)
        {
            ElevatorFloor = elevatorFloor;
            Floors = floors;
            Generation = generation;
        }

        public IList<FloorState> Next()
        {
            var safeCombinations = CurrentFloor.GenerateSafeCombinations();
            var nextStates = new List<FloorState>();

            if (CanMoveUp)
            {
                foreach (var safeCombination in safeCombinations)
                {
                    var newUpperFloor = UpperFloor | safeCombination;
                    var newCurrentFloor = CurrentFloor ^ safeCombination;
                    if (newUpperFloor.IsSafe() && newCurrentFloor.IsSafe())
                    {
                        var newFloors = new List<Floor>(Floors)
                        {
                            [ElevatorFloor + 1] = newUpperFloor,
                            [ElevatorFloor] = newCurrentFloor
                        };

                        nextStates.Add(new FloorState(ElevatorFloor + 1, newFloors, Generation + 1));
                    }
                }
            }

            if (CanMoveDown) {
                foreach (var safeCombination in safeCombinations) {
                    var newLowerFloor = LowerFloor | safeCombination;
                    var newCurrentFloor = CurrentFloor ^ safeCombination;
                    if (newLowerFloor.IsSafe() && newCurrentFloor.IsSafe()) {
                        var newFloors = new List<Floor>(Floors) {
                            [ElevatorFloor - 1] = newLowerFloor,
                            [ElevatorFloor] = newCurrentFloor
                        };

                        nextStates.Add(new FloorState(ElevatorFloor - 1, newFloors, Generation + 1));
                    }
                }
            }

            return nextStates;
        }

        protected bool Equals(FloorState other)
        {
            if (ElevatorFloor != other.ElevatorFloor) return false;

            for (int i = 0; i < Floors.Count; i++)
            {
                if (Floors[i] != other.Floors[i]) return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FloorState) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ElevatorFloor * 397) ^ (Floors != null ? Floors.Aggregate(13, (s, f) => s * (int)f) : 0);
            }
        }

        public override string ToString()
        {
            var elementOrder = new[]
            {
                Floor.ThuliumGenerator,
                Floor.ThuliumChip,
                Floor.PromethiumGenerator,
                Floor.PromethiumChip,
                Floor.StrontiumGenerator,
                Floor.StrontiumChip,
                Floor.PlutoniumGenerator,
                Floor.PlutoniumChip,
                Floor.RutheniumGenerator,
                Floor.RutheniumChip
            };

            var elementAbbreviations = new Dictionary<Floor, string>
            {
                {Floor.ThuliumGenerator, "TG "},
                {Floor.ThuliumChip, "TM "},
                {Floor.PromethiumGenerator, "PG"},
                {Floor.PromethiumChip, "PM"},
                {Floor.StrontiumGenerator, "SG"},
                {Floor.StrontiumChip, "SM"},
                {Floor.PlutoniumGenerator, "OG"},
                {Floor.PlutoniumChip, "OM"},
                {Floor.RutheniumGenerator, "RG"},
                {Floor.RutheniumChip, "RM"}
            };

            var state = new StringBuilder();
            state.AppendLine($"Generation: {Generation}");

            for (int i = Floors.Count - 1; i >= 0; i--)
            {
                var floor = Floors[i];
                var elevator = ElevatorFloor == i ? "E" : ".";

                var assemblyLayout = new StringBuilder();
                foreach (var element in elementOrder)
                {
                    var elementOut = floor.HasFlag(element) ? $"{elementAbbreviations[element]} " : ". ";
                    assemblyLayout.Append(elementOut);
                }

                state.AppendLine($"{elevator} {assemblyLayout}");
            }

            return state.ToString();
        }

    }
}