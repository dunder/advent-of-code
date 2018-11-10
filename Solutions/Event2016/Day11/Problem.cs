using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Facet.Combinatorics;
using Shared.Tree;
using Solutions.Event2016.Day11.EnumFlagsSolution;

namespace Solutions.Event2016.Day11
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day11;

        public override string FirstStar()
        {
            return FirstStarUsingClasses();
        }

        private static string FirstStarUsingEnumFlags()
        {
            var initialState = new FloorState(0, new List<Floor>
            {
                Floor.ThuliumGenerator | Floor.ThuliumChip | Floor.PlutoniumGenerator | Floor.StrontiumGenerator,
                Floor.PlutoniumChip | Floor.StrontiumChip,
                Floor.PromethiumGenerator | Floor.PromethiumChip | Floor.RutheniumGenerator | Floor.RutheniumChip,
                Floor.Empty
            });

            var targetTopFloor = Floor.PlutoniumChip | Floor.PlutoniumGenerator |
                                 Floor.PromethiumChip | Floor.PromethiumGenerator |
                                 Floor.RutheniumChip | Floor.RutheniumGenerator |
                                 Floor.StrontiumChip | Floor.StrontiumGenerator |
                                 Floor.ThuliumChip | Floor.ThuliumGenerator;

            var result = MinimumStepsToTopFloor2(initialState, 3, targetTopFloor);

            return result.ToString();
        }

        private string FirstStarUsingClasses()
        {
            var floor1Assembly = new Assembly()
                .WithGenerator(Element.Thulium)
                .WithChip(Element.Thulium)
                .WithGenerator(Element.Plutonium)
                .WithGenerator(Element.Strontium);
            var floor2Assembly = new Assembly()
                .WithChip(Element.Plutonium)
                .WithChip(Element.Strontium);
            var floor3Assembly = new Assembly()
                .WithGenerator(Element.Promethium)
                .WithChip(Element.Promethium)
                .WithGenerator(Element.Ruthenium)
                .WithChip(Element.Ruthenium);
            var floor4Assembly = new Assembly();

            var initialBuildingState = new BuildingState(0,
                new List<Assembly>
                {
                    floor1Assembly,
                    floor2Assembly,
                    floor3Assembly,
                    floor4Assembly
                },
                0);

            var result = MinimumStepsToTopFloor(initialBuildingState, 3, 10);

            return result.ToString();
        }

        public override string SecondStar()
        {
            var floor1Assembly = new Assembly()
                .WithGenerator(Element.Thulium)
                .WithChip(Element.Thulium)
                .WithGenerator(Element.Plutonium)
                .WithGenerator(Element.Strontium)
                .WithGenerator(Element.Elerium)
                .WithChip(Element.Elerium)
                .WithGenerator(Element.Dilithium)
                .WithChip(Element.Dilithium);
            var floor2Assembly = new Assembly()
                .WithChip(Element.Plutonium)
                .WithChip(Element.Strontium);
            var floor3Assembly = new Assembly()
                .WithGenerator(Element.Promethium)
                .WithChip(Element.Promethium)
                .WithGenerator(Element.Ruthenium)
                .WithChip(Element.Ruthenium);
            var floor4Assembly = new Assembly();

            var initialBuildingState = new BuildingState(0,
                new List<Assembly>
                {
                    floor1Assembly,
                    floor2Assembly,
                    floor3Assembly,
                    floor4Assembly
                },
                0);

            var result = MinimumStepsToTopFloor(initialBuildingState, 3, 14);

            return result.ToString();
        }

        public static int MinimumStepsToTopFloor(BuildingState initialState, int targetFloor, int targetAssemblyCount)
        {
            bool TargetCondition(BuildingState b) => 
                b.Elevator == targetFloor && 
                b.FloorSetup[targetFloor].Chips.Count + b.FloorSetup[targetFloor].Generators.Count == targetAssemblyCount;

            var (terminationNode, _) = initialState.ShortestPath(floor => floor.SafeFloorRearrangements(), TargetCondition);

            return terminationNode.Depth;
        }

        public static int MinimumStepsToTopFloor2(FloorState initialState, int targetFloor, Floor targetTopFloor)
        {
            bool TargetCondition(FloorState f)
            {
                return f.ElevatorFloor == targetFloor && f.Floors[targetFloor] == targetTopFloor;
            }

            var (terminationNode, _) = initialState.ShortestPath(floor => floor.Next(), TargetCondition);

            return terminationNode.Depth;
        }
    }


    public enum Element
    {
        Hydrogen,
        Lithium,
        Thulium,
        Plutonium,
        Promethium,
        Ruthenium,
        Strontium,
        Elerium,
        Dilithium
    }

    public class BuildingState
    {
        public BuildingState(int elevator, IList<Assembly> floors, int stateDepth)
        {
            Elevator = elevator;
            Floors = floors;
            StateDepth = stateDepth;
        }

        public IList<BuildingState> SafeFloorRearrangements()
        {
            var validForElevator = CurrentFloor.SafeChipGeneratorCombinations();
            var safeToRelease = validForElevator.Where(a => CurrentFloor.Release(a).IsSafe()).ToList();

            var safeRearrangements = new List<BuildingState>();

            if (CanMoveDown)
            {
                var safeToReceive = safeToRelease.Where(a => LowerFloor.Merge(a).IsSafe());

                foreach (var assembly in safeToReceive)
                {
                    var newBuildingFloors = Floors.Select(a => a.Copy()).ToList();

                    var newThis = CurrentFloor.Release(assembly);
                    var newLower = LowerFloor.Merge(assembly);

                    newBuildingFloors[Elevator] = newThis;
                    newBuildingFloors[Elevator - 1] = newLower;

                    safeRearrangements.Add(new BuildingState(Elevator - 1, newBuildingFloors, StateDepth + 1));
                }
            }

            if (CanMoveUp)
            {
                var safeToReceive = safeToRelease.Where(a => UpperFloor.Merge(a).IsSafe());

                foreach (var assembly in safeToReceive) {
                    var newBuildingFloors = Floors.Select(a => a.Copy()).ToList();

                    var newThis = CurrentFloor.Release(assembly);
                    var newUpper = UpperFloor.Merge(assembly);

                    newBuildingFloors[Elevator] = newThis;
                    newBuildingFloors[Elevator + 1] = newUpper;

                    safeRearrangements.Add(new BuildingState(Elevator + 1, newBuildingFloors, StateDepth + 1));
                }
            }

            return safeRearrangements;
        }

        public int Elevator { get; }
        public IReadOnlyList<Assembly> FloorSetup => new ReadOnlyCollection<Assembly>(Floors);
        public int StateDepth { get; }
        private bool CanMoveUp => Elevator < Floors.Count - 1;
        private bool CanMoveDown => Elevator > 0;
        private Assembly CurrentFloor => Floors[Elevator];
        private Assembly UpperFloor => Floors[Elevator + 1];
        private Assembly LowerFloor => Floors[Elevator - 1];

        private IList<Assembly> Floors { get; }

        protected bool Equals(BuildingState other)
        {
            if (Elevator != other.Elevator) return false;
            for (var i = 0; i < Floors.Count; i++)
            {
                if (!Floors[i].Equals(other.Floors[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BuildingState) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Elevator * 397) ^ Floors.Aggregate(397, (a, e) => a * e.GetHashCode());
            }
        }

        public override string ToString()
        {
            var elementOrder = new[] { Element.Thulium, Element.Plutonium, Element.Strontium, Element.Promethium, Element.Ruthenium };
            var elementAbbreviations = new Dictionary<Element, string>
            {
                {Element.Thulium, "T"},
                {Element.Plutonium, "P"},
                {Element.Strontium, "S"},
                {Element.Promethium, "O"},
                {Element.Ruthenium, "R"},
            };

            var state = new StringBuilder();
            state.AppendLine($"Depth: {StateDepth}");

            for (int i = Floors.Count - 1; i >= 0; i--) {
                var assembly = Floors[i];
                var elevator = Elevator == i ? "E" : ".";

                var assemblyLayout = new StringBuilder();
                foreach (var element in elementOrder) {
                    var generator = assembly.Generators.Contains(element)
                        ? $"{elementAbbreviations[element]}G "
                        : ".  ";
                    assemblyLayout.Append(generator);
                    var chip = assembly.Chips.Contains(element) ? $"{elementAbbreviations[element]}M " : ".  ";
                    assemblyLayout.Append(chip);
                }

                state.AppendLine($"{elevator} {assemblyLayout}");
            }

            return state.ToString();
        }
    }

    public class Assembly
    {
        public Assembly()
        {
            Generators = new HashSet<Element>();
            Chips = new HashSet<Element>();
        }

        public Assembly WithChip(Element chip)
        {
            Chips.Add(chip);
            return this;
        }

        public Assembly WithGenerator(Element chip)
        {
            Generators.Add(chip);
            return this;
        }

        public Assembly WithMatchingChipAndGenerator(Element element)
        {
            Chips.Add(element);
            Generators.Add(element);
            return this;
        }

        public HashSet<Element> Generators { get; }
        public HashSet<Element> Chips { get; }

        public bool IsSafe()
        {
            if (!Chips.Any()) return true;
            if (!Generators.Any()) return true;
            return Chips.All(c => Generators.Contains(c));
        }

        public Assembly Copy()
        {
            var copy = new Assembly();

            foreach (var c in Chips)
            {
                copy.WithChip(c);
            }

            foreach (var g in Generators)
            {
                copy.WithGenerator(g);
            }

            return copy;
        }

        public Assembly Merge(Assembly assembly)
        {
            var merged = new Assembly();
            foreach (var chip in Chips)
            {
                merged.WithChip(chip);
            }

            foreach (var generator in Generators)
            {
                merged.WithGenerator(generator);
            }

            foreach (var chip in assembly.Chips)
            {
                merged.WithChip(chip);
            }

            foreach (var generator in assembly.Generators)
            {
                merged.WithGenerator(generator);
            }

            return merged;
        }

        public Assembly Release(Assembly assembly)
        {
            var released = new Assembly();

            foreach (var chip in Chips.Where(c => !assembly.Chips.Contains(c)))
            {
                released.WithChip(chip);
            }

            foreach (var generator in Generators.Where(g => !assembly.Generators.Contains(g)))
            {
                released.WithGenerator(generator);
            }

            return released;
        }

        public IEnumerable<Assembly> SafeChipGeneratorCombinations()
        {
            var singleChips = Chips.Select(c => new Assembly().WithChip(c));
            var singleGenerators = Generators.Select(g => new Assembly().WithGenerator(g));
            IEnumerable<Assembly> chipCombinations = new List<Assembly>();
            if (Chips.Count > 1)
            {
                chipCombinations = new Combinations<Element>(Chips.ToList(), 2)
                    .Select(pair => new Assembly()
                        .WithChip(pair.First())
                        .WithChip(pair.Last()));
            }

            IEnumerable<Assembly> generatorCombinations = new List<Assembly>();
            if (Generators.Count > 1)
            {
                generatorCombinations = new Combinations<Element>(Generators.ToList(), 2)
                    .Select(pair => new Assembly()
                        .WithGenerator(pair.First())
                        .WithGenerator(pair.Last()));
            }

            var chipGeneratorPairs = Chips
                .Where(chip => Generators.Contains(chip))
                .Select(c => new Assembly().WithMatchingChipAndGenerator(c));

            return singleChips
                .Concat(singleGenerators)
                .Concat(chipCombinations)
                .Concat(chipGeneratorPairs)
                .Concat(generatorCombinations);
        }

        protected bool Equals(Assembly other)
        {
            return Equals(Generators, other.Generators) && Equals(Chips, other.Chips);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            Assembly other = (Assembly)obj;
            if (!Generators.SetEquals(other.Generators))
            {
                return false;
            }

            return Chips.SetEquals(other.Chips);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Generators.Aggregate(397, (a, e) => a * (int)e) ^ Chips.Aggregate(1, (a, e) => a * (int)e);
            }
        }
    }

}