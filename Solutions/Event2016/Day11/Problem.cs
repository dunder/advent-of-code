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
            var floor1Assembly = new Assembly();
            var floor2Assembly = new Assembly();
            var floor3Assembly = new Assembly();
            var floor4Assembly = new Assembly();

            floor1Assembly
                .WithChip(Element.Hydrogen)
                .WithChip(Element.Lithium);

            floor2Assembly.WithGenerator(Element.Hydrogen);

            floor3Assembly.WithGenerator(Element.Lithium);


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

        public IList<Floor> ValidAlternatives()
        {
            var validForElevator = Assembly.SelectValidAssembliesForElevator();
            var safeToRelease = validForElevator.Where(a => Assembly.Release(a).IsSafe()).ToList();

            var alternatives = new List<Floor>();

            if (Lower != null)
            {
                var safeToReceive = safeToRelease.Where(a => Lower.Assembly.Merge(a).IsSafe());

                foreach (var assembly in safeToReceive)
                {
                    var newThis = new Floor(Level, Assembly.Release(assembly));
                    var newLower = new Floor(Lower.Level, Assembly.Merge(assembly));

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

                foreach (var assembly in safeToReceive) {
                    var newThis = new Floor(Level, Assembly.Release(assembly));
                    var newUpper = new Floor(Upper.Level, Assembly.Merge(assembly));

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
        

        public HashSet<Element> Generators { get; }
        public HashSet<Element> Chips { get; }
        
        public bool IsSafe()
        {
            if (!Chips.Any()) return true;
            if (!Generators.Any()) return true;
            return Chips.All(c => Generators.Contains(c));
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

        public IEnumerable<Assembly> SelectValidAssembliesForElevator() {
            var singleChips = Chips.Select(c => new Assembly().WithChip(c));
            var singleGenerators = Generators.Select(g => new Assembly().WithGenerator(g));
            var chipCombinations = new Combinations<Element>(Chips.ToList(), 2)
                .Select(pair => new Assembly()
                    .WithChip(pair.First())
                    .WithChip(pair.Last()));
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
                return Generators.Aggregate(397, (a, e) => a * (int)e) ^ Chips.Aggregate(1, (a, e) => a * (int)e);
                //return ((Generators != null ? Generators.GetHashCode() : 0) * 397) ^ (Chips != null ? Chips.GetHashCode() : 0);
            }
        }
    }

}