using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facet.Combinatorics;
using Shared.Extensions;
using Shared.Tree;

namespace Solutions.Event2016.Day11
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day11;

        public override string FirstStar()
        {
            var floor1Assembly = new Assembly().WithChip(Element.Hydrogen).WithChip(Element.Lithium);
            var floor2Assembly = new Assembly().WithGenerator(Element.Hydrogen);
            var floor3Assembly = new Assembly().WithGenerator(Element.Lithium);
            var floor4Assembly = new Assembly();

            var initialBuildingState = new BuildingState(1, new List<Assembly>
            {
                floor1Assembly,
                floor2Assembly,
                floor3Assembly,
                floor4Assembly
            });

            var result = MinimumStepsToTopFloor(initialBuildingState, 4, 10);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result;
        }

        public static int MinimumStepsToTopFloor(BuildingState initialState, int targetFloor, int targetAssemblyCount)
        {
            bool TargetCondition(BuildingState b) => b.Elevator == targetFloor;

            var (breadthFirst, visited) =
                initialState.BreadthFirst(floor => floor.SafeFloorRearrangements(), TargetCondition);

            return visited.Count();
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
        Strontium
    }

    public class BuildingState
    {
        public BuildingState(int elevator, IList<Assembly> floors)
        {
            Elevator = elevator;
            Floors = floors;
        }

        public IList<BuildingState> SafeFloorRearrangements()
        {
            var validForElevator = CurrentFloor.SafeChipGeneratorCombinations();
            var safeToRelease = validForElevator.Where(a => CurrentFloor.Release(a).IsSafe()).ToList();

            var alternatives = new List<BuildingState>();


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

                    alternatives.Add(new BuildingState(Elevator - 1, newBuildingFloors));
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

                    alternatives.Add(new BuildingState(Elevator + 1, newBuildingFloors));
                }
            }

            return alternatives;
        }

        public int Elevator { get; }
        private bool CanMoveUp => Elevator < Floors.Count;
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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BuildingState) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Elevator * 397) ^ Floors.Aggregate(397, (a, e) => a * e.GetHashCode());
            }
        }
    }


    public class Floor
    {
        public int Level { get; }
        public Assembly Assembly { get; }
        public Floor Upper { get; set; }
        public Floor Lower { get; set; }

        public Floor(int level, Assembly assembly)
        {
            Level = level;
            Assembly = assembly;
        }

        public IList<Floor> SafeFloorRearrangements()
        {
            var validForElevator = Assembly.SafeChipGeneratorCombinations();
            var safeToRelease = validForElevator.Where(a => Assembly.Release(a).IsSafe()).ToList();

            var alternatives = new List<Floor>();

            if (Lower != null)
            {
                var safeToReceive = safeToRelease.Where(a => Lower.Assembly.Merge(a).IsSafe());

                foreach (var assembly in safeToReceive)
                {
                    var newThis = new Floor(Level, Assembly.Release(assembly));
                    var newLower = new Floor(Lower.Level, Lower.Assembly.Merge(assembly));

                    newThis.Upper = Upper;
                    newThis.Lower = newLower;

                    newLower.Upper = newThis;
                    newLower.Lower = Lower.Lower;

                    alternatives.Add(newLower);
                }
            }

            if (Upper != null)
            {
                var safeToReceive = safeToRelease.Where(a => Upper.Assembly.Merge(a).IsSafe());

                foreach (var assembly in safeToReceive)
                {
                    var newThis = new Floor(Level, Assembly.Release(assembly));
                    var newUpper = new Floor(Upper.Level, Upper.Assembly.Merge(assembly));

                    newThis.Upper = newUpper;
                    newThis.Lower = Lower;

                    newUpper.Upper = Upper.Upper;
                    newUpper.Lower = newThis;

                    alternatives.Add(newUpper);
                }
            }

            return alternatives;
        }

        protected bool Equals(Floor other)
        {
            return Level == other.Level && Equals(Assembly, other.Assembly);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Floor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Level * 397) ^ (Assembly != null ? Assembly.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            string AbbreviatedElement(Element element)
            {
                switch (element)
                {
                    case Element.Hydrogen:
                        return "H";
                    case Element.Lithium:
                        return "L";
                    default:
                        return "X";
                }
            }

            var order = new[] {Element.Hydrogen, Element.Hydrogen, Element.Lithium, Element.Lithium};
            var floorDescription = new StringBuilder();
            for (var i = 0; i < order.Length; i++)
            {
                var element = order[i];
                if (i % 2 == 0)
                {
                    floorDescription.Append(Assembly.Generators.Contains(element)
                        ? $"{AbbreviatedElement(element)}G"
                        : " . ");
                }
                else
                {
                    floorDescription.Append(
                        Assembly.Chips.Contains(element) ? $"{AbbreviatedElement(element)}M" : " . ");
                }
            }

            return $"F{Level}: {floorDescription}";
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

            var chipGeneratorPairs = Chips
                .Where(chip => Generators.Contains(chip))
                .Select(c => new Assembly().WithMatchingChipAndGenerator(c));

            return singleChips.Concat(singleGenerators).Concat(chipCombinations).Concat(chipGeneratorPairs);
        }

        protected bool Equals(Assembly other)
        {
            return Equals(Generators, other.Generators) && Equals(Chips, other.Chips);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            Assembly other = (Assembly) obj;
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
                return Generators.Aggregate(397, (a, e) => a * (int) e) ^ Chips.Aggregate(1, (a, e) => a * (int) e);
            }
        }
    }
}