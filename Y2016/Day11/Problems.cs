using System.Collections.Generic;
using System.Linq;
using Facet.Combinatorics;
using Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day11
{
    public class Problems
    {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Problem1()
        {
            int elevator = 1;

            // find combinations that might enter the elevator single chip, single generator, dual chips or combination of chip and corresponding generator
            // unmatched chips must not be left on floor with a generator
            // floor 1 -> floor 2
            // floor 2 -> floor 1 or floor 3
            // floor 3 -> floor 2 or floor 4
            // floor 4 -> floor 3

            var floor1 = new Assembly()
                .WithChip(Element.Hydrogen)
                .WithChip(Element.Lithium)
                .WithUpperFloor(new Assembly());

            var floor2 = new Assembly()
                .WithGenerator(Element.Hydrogen);

            var floor3 = new Assembly()
                .WithGenerator(Element.Lithium);

            var floor4 = new Assembly();

            floor1.WithUpperFloor(floor2);
            floor2.WithLowerFloor(floor1).WithUpperFloor(floor3);
            floor3.WithLowerFloor(floor2).WithUpperFloor(floor4);
            floor4.WithLowerFloor(floor3);
                

            var result = 0;

            Assert.Equal(31, result);
            _output.WriteLine($"Day 11 problem 1: {result}");
        }

        [Fact]
        public void Problem2()
        {
            var result = 0;

            Assert.Equal(55, result);
            _output.WriteLine($"Day 11 problem 2: {result}");
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

        public class Assembly 
        {
            public Assembly()
            {
                Generators = new HashSet<Element>();
                Chips = new HashSet<Element>();
            }

            public Assembly WithChip(Element chip)
            {
                Assembly assembly = new Assembly();
                assembly.Chips.Add(chip);
                return assembly;
            }

            public Assembly WithGenerator(Element chip)
            {
                Assembly assembly = new Assembly();
                assembly.Generators.Add(chip);
                return assembly;
            }

            public Assembly WithMatchingChipAndGenerator(Element element)
            {
                Assembly assembly = new Assembly();
                assembly.Chips.Add(element);
                assembly.Generators.Add(element);
                return assembly;
            }

            public Assembly WithUpperFloor(Assembly upperFloor)
            {
                UpperFloor = upperFloor;
                return this;
            }

            public Assembly WithLowerFloor(Assembly lowerFloor)
            {
                LowerFloor = lowerFloor;
                return this;
            }

            public HashSet<Element> Generators { get; }
            public HashSet<Element> Chips { get; }

            public Assembly UpperFloor { get; private set; }
            public Assembly LowerFloor { get; private set; }

            public IEnumerable<Assembly> MovableAssemblies()
            {
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
}