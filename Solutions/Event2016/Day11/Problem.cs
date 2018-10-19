using System.Collections.Generic;
using System.Linq;
using Facet.Combinatorics;

namespace Solutions.Event2016.Day11
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day11;

        public override string FirstStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result;
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result;
        }
    }

    public enum Element {
        Hydrogen,
        Lithium,
        Thulium,
        Plutonium,
        Promethium,
        Ruthenium,
        Strontium
    }

    public class Assembly {
        public Assembly() {
            Generators = new HashSet<Element>();
            Chips = new HashSet<Element>();
        }

        public Assembly WithChip(Element chip) {
            Chips.Add(chip);
            return this;
        }

        public Assembly WithGenerator(Element chip) {
            Generators.Add(chip);
            return this;
        }

        public Assembly WithMatchingChipAndGenerator(Element element) {
            Chips.Add(element);
            Generators.Add(element);
            return this;
        }

        public Assembly WithUpperFloor(Assembly upperFloor) {
            UpperFloor = upperFloor;
            return this;
        }

        public Assembly WithLowerFloor(Assembly lowerFloor) {
            LowerFloor = lowerFloor;
            return this;
        }

        public HashSet<Element> Generators { get; }
        public HashSet<Element> Chips { get; }

        public Assembly UpperFloor { get; private set; }
        public Assembly LowerFloor { get; private set; }

        public bool IsSafe()
        {
            if (!Chips.Any()) return true;
            if (!Generators.Any()) return true;
            return Chips.All(c => Generators.Contains(c));
        }

        public IEnumerable<Assembly> SelectValidAssembliesForElevator() {
            var singleChips = Chips.Select(c => new Assembly().WithChip(c));
            var singleGenerators = Generators.Select(g => new Assembly().WithGenerator(g));
            var chipCombinations = new Combinations<Element>(Chips.ToList(), 2)
                .Select(pair => new Assembly()
                    .WithChip(pair.First())
                    .WithChip(pair.Last()));
            var chipGeneratorPairs = Chips.Where(chip => Generators.Contains(chip)).Select(WithMatchingChipAndGenerator);

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
                return ((Generators != null ? Generators.GetHashCode() : 0) * 397) ^ (Chips != null ? Chips.GetHashCode() : 0);
            }
        }
    }
}