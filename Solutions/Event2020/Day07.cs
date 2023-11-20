
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 7: Handy Haversacks ---
    public class Day07
    {
        private readonly ITestOutputHelper output;

        public Day07(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Bag(string color, Dictionary<string, int> compartment);


        private Bag ParseBag(string line)
        {
            var parts = line.TrimEnd('.').Split(" bags contain ");

            var colorPart = parts[0];
            var compartmentPart = parts[1];

            var compartment = new Dictionary<string, int>();

            if (compartmentPart.Contains("no other bags"))
            {
                return new Bag(colorPart, compartment);
            }

            var compartmentParts = compartmentPart.Split(", ");

            foreach (var part in compartmentParts)
            {
                int count = int.Parse(part.Substring(0,1));
                int strip = count == 1 ? " bag".Length : " bags".Length;
                string color = part.Substring(2, part.Length - 2 - strip);
                compartment.Add(color, count);
            }

            return new Bag(colorPart, compartment);
        }

        private Dictionary<string, Bag> ParseBags(IList<string> input)
        {
            return input.Select(ParseBag).ToDictionary(bag => bag.color, bag => bag);
        }

        private bool CanBagHold(Bag bag, string color, Dictionary<string, Bag> allBags, Dictionary<string, bool> canHold)
        {
            if (canHold.ContainsKey(bag.color))
            {
                return canHold[bag.color];
            }

            if (bag.compartment.ContainsKey(color))
            {
                canHold.Add(bag.color, true);
                return true;
            }
            else if (!bag.compartment.Any())
            {
                canHold.Add(bag.color, false);
                return false;
            }
            else
            {
                var containedBags = bag.compartment.Keys.Select(color => allBags[color]).ToList();

                return containedBags.Any(containedBag => CanBagHold(containedBag, color, allBags, canHold));
            }
        }

        private int CountBagsHolding(string color, IList<string> input) 
        {
            var allBags = ParseBags(input);

            return allBags.Values.Where(bag => CanBagHold(bag, "shiny gold", allBags, new Dictionary<string, bool>())).Count();
        }

        private int CountContainedBags(Bag bag, Dictionary<string, Bag> allBags, Dictionary<string, int> cache)
        {
            if (cache.ContainsKey(bag.color))
            {
                return cache[bag.color];
            }

            var containedBags = bag.compartment
                .Select(colorCountPair => (allBags[colorCountPair.Key], colorCountPair.Value))
                .ToList();

            if (!containedBags.Any()) 
            {
                return 0;
            }

            var containedTotal = 0;

            foreach (var (containedBag, count) in containedBags)
            {
                var containedCount = CountContainedBags(containedBag, allBags, cache);

                cache[containedBag.color] = containedCount;

                containedTotal += count + count * containedCount;
            }

            return containedTotal;
        }

        private int CountTotalBags(string color, IList<string> input)
        {
            var allBags = ParseBags(input);

            return CountContainedBags(allBags[color], allBags, new Dictionary<string, int>());
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return CountBagsHolding("shiny gold", input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return CountTotalBags("shiny gold", input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(257, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1038, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                "bright white bags contain 1 shiny gold bag.",
                "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                "faded blue bags contain no other bags.",
                "dotted black bags contain no other bags."
            };

            Assert.Equal(4, CountBagsHolding("shiny gold", example));
        }

        [Fact]
        public void FirstStarParseMany()
        {
            var input = "light red bags contain 1 bright white bag, 2 muted yellow bags.";

            var bag = ParseBag(input);

            Assert.Equal("light red", bag.color);
            Assert.Equal(2, bag.compartment.Count);
            Assert.True(bag.compartment.ContainsKey("bright white"));
            Assert.Equal(1, bag.compartment["bright white"]);
            Assert.True(bag.compartment.ContainsKey("muted yellow"));
            Assert.Equal(2, bag.compartment["muted yellow"]);
        }

        [Fact]
        public void FirstStarParseOne()
        {
            var input = "bright white bags contain 1 shiny gold bag.";

            var bag = ParseBag(input);

            Assert.Equal("bright white", bag.color);
            Assert.Single(bag.compartment);
            Assert.True(bag.compartment.ContainsKey("shiny gold"));
            Assert.Equal(1, bag.compartment["shiny gold"]);
        }

        [Fact]
        public void FirstStarParseNone()
        {
            var input = "dotted black bags contain no other bags.";

            var bag = ParseBag(input);

            Assert.Equal("dotted black", bag.color);
            Assert.Empty(bag.compartment);
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                "bright white bags contain 1 shiny gold bag.",
                "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                "faded blue bags contain no other bags.",
                "dotted black bags contain no other bags."
            };

            Assert.Equal(32, CountTotalBags("shiny gold", example));
        }

        [Fact]
        public void SecondStarExample2()
        {
            var example = new List<string>
            {
                "shiny gold bags contain 2 dark red bags.",
                "dark red bags contain 2 dark orange bags.",
                "dark orange bags contain 2 dark yellow bags.",
                "dark yellow bags contain 2 dark green bags.",
                "dark green bags contain 2 dark blue bags.",
                "dark blue bags contain 2 dark violet bags.",
                "dark violet bags contain no other bags."
            };

            Assert.Equal(126, CountTotalBags("shiny gold", example));
        }
    }
}
