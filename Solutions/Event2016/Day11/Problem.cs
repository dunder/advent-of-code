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
            Assembly assembly = new Assembly();
            assembly.Chips.Add(chip);
            return assembly;
        }

        public Assembly WithGenerator(Element chip) {
            Assembly assembly = new Assembly();
            assembly.Generators.Add(chip);
            return assembly;
        }

        public Assembly WithMatchingChipAndGenerator(Element element) {
            Assembly assembly = new Assembly();
            assembly.Chips.Add(element);
            assembly.Generators.Add(element);
            return assembly;
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

        public IEnumerable<Assembly> MovableAssemblies() {
            var singleChips = Chips.Select(WithChip);
            var singleGenerators = Generators.Select(WithGenerator);
            var chipCombinations = new Combinations<Element>(Chips.ToList(), 2)
                .Select(pair => new Assembly()
                    .WithChip(pair.First())
                    .WithChip(pair.Last()));
            var chipGeneratorPairs = Chips.Where(chip => Generators.Contains(chip)).Select(WithMatchingChipAndGenerator);

            return singleChips.Concat(singleGenerators).Concat(chipCombinations).Concat(chipGeneratorPairs);
        }
    }
}