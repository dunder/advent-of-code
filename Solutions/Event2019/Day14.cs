using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 14: Space Stoichiometry ---
    public class Day014
    {
        private class Reaction
        {
            public List<Amount> Input { get; set; }
            public Amount Output { get; set; }

            public bool IsOre => Input.Count == 1 && Input.Single().Chemical == "ORE";
            public bool IsFuel => Output.Chemical == "FUEL";

            public override string ToString()
            {
                return $"{string.Join(", ", Input)} => {Output}";
            }
        }

        private class Amount
        {
            public string Chemical { get; set; }
            public int Units { get; set; }

            public Amount Times(int x)
            {
                return new Amount
                {
                    Chemical = this.Chemical,
                    Units = this.Units * x
                };
            }

            public override string ToString()
            {
                return $"{Units} {Chemical}";
            }
        }
        private static Amount ParseAmount(string text)
        {
            var pair = text.Split(" ");
            return new Amount
            {
                Chemical = pair.Last(),
                Units = int.Parse(pair.First())
            };
        }
        private static List<Reaction> Parse(IEnumerable<string> input)
        {
            var reactions = new List<Reaction>();
            foreach (var line in input)
            {
                var inputAndOutput = line.Split(" => ");
                var reactionInput = inputAndOutput.First().Split(", ");
                var reactionOutput = inputAndOutput.Last();
                var reaction = new Reaction
                {
                    Input = reactionInput.Select(ParseAmount).ToList(),
                    Output = ParseAmount(reactionOutput),
                };
                reactions.Add(reaction);
            }

            return reactions;
        }

        private static void CalculateAmountOre(Dictionary<string, Reaction> reactionMap, Amount amount, List<Amount> produce, Dictionary<string, int> surplus)
        {
            var reaction = reactionMap[amount.Chemical];
            if (reaction.IsOre)
            {
                produce.Add(amount);

                return;
            }

            var leftToProduce = amount.Units;

            if (surplus.ContainsKey(amount.Chemical) && surplus[amount.Chemical] > 0)
            {
                var foundSurplus = surplus[amount.Chemical];
                if (leftToProduce >= foundSurplus)
                {
                    leftToProduce -= foundSurplus;
                    surplus[amount.Chemical] = 0;
                }
                else
                {
                    surplus[amount.Chemical] -= leftToProduce;
                    leftToProduce = 0;
                }
            }

            if (leftToProduce > 0)
            {
                var times = leftToProduce / reaction.Output.Units;
                if (leftToProduce % reaction.Output.Units > 0)
                {
                    times += 1;
                }

                var unitsSurplus = reaction.Output.Units * times - leftToProduce;
                if (!surplus.ContainsKey(amount.Chemical))
                {
                    surplus.Add(amount.Chemical, 0);
                }

                surplus[amount.Chemical] += unitsSurplus;


                foreach (var reactionAmount in reaction.Input)
                {
                    CalculateAmountOre(reactionMap, reactionAmount.Times(times), produce, surplus);
                }
            }
        }

        private static void CalculateAmountOre2(Dictionary<string, Reaction> reactionMap, Amount amount, List<Amount> produce, Dictionary<string, int> surplus)
        {
            var reaction = reactionMap[amount.Chemical];
            if (reaction.IsOre)
            {
                produce.Add(amount);

                return;
            }
            
            var times = amount.Units / reaction.Output.Units;
            var rest = amount.Units % reaction.Output.Units;

            if (rest > 0 && surplus.ContainsKey(amount.Chemical))
            {
                var availableSurplus = surplus[amount.Chemical];
                if (availableSurplus >= rest)
                {
                    surplus[amount.Chemical] -= rest;
                }
                else
                {
                    times += 1;
                }
            }

            var unitsSurplus = reaction.Output.Units * times - amount.Units;
            if (!surplus.ContainsKey(amount.Chemical))
            {
                surplus.Add(amount.Chemical, 0);
            }

            surplus[amount.Chemical] += unitsSurplus;

            foreach (var reactionAmount in reaction.Input)
            {
                CalculateAmountOre2(reactionMap, reactionAmount.Times(times), produce, surplus);
            }
        }

        private static int MinimumAmountOreForOneUnitOfFuel(List<Reaction> reactions)
        {
            var fuelReaction = reactions.Single(r => r.IsFuel);

            var reactionMap = reactions.ToDictionary(r => r.Output.Chemical);
            var requiredAmounts = new List<Amount>();
            var surplus = new Dictionary<string, int>();
            foreach (var amount in fuelReaction.Input)
            {
                CalculateAmountOre(reactionMap, amount, requiredAmounts, surplus);
            }

            var sumOfRequiredAmounts = requiredAmounts
                .GroupBy(requiredAmount => requiredAmount.Chemical)
                .Select(g => new Amount
                {
                    Chemical = g.Key,
                    Units = g.Sum(a => a.Units)
                });

            var ore = 0;
            foreach (var requiredAmount in sumOfRequiredAmounts)
            {
                var oreReaction = reactionMap[requiredAmount.Chemical];
                var x = requiredAmount.Units / oreReaction.Output.Units;
                if (requiredAmount.Units % oreReaction.Output.Units > 0)
                {
                    x += 1;
                }

                ore += oreReaction.Input.Single().Units * x;
            }

            return ore;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            var reactions = Parse(input);

            return MinimumAmountOreForOneUnitOfFuel(reactions);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            var reactions = Parse(input);
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(143173, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample1()
        {
            var input = new[]
            {
                "10 ORE => 10 A",
                "1 ORE => 1 B",
                "7 A, 1 B => 1 C",
                "7 A, 1 C => 1 D",
                "7 A, 1 D => 1 E",
                "7 A, 1 E => 1 FUEL"
            };

            var reactions = Parse(input);

            var min = MinimumAmountOreForOneUnitOfFuel(reactions);

            Assert.Equal(31, min);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = new[]
            {
                "9 ORE => 2 A",
                "8 ORE => 3 B",
                "7 ORE => 5 C",
                "3 A, 4 B => 1 AB",
                "5 B, 7 C => 1 BC",
                "4 C, 1 A => 1 CA",
                "2 AB, 3 BC, 4 CA => 1 FUEL"
            };

            var reactions = Parse(input);

            var min = MinimumAmountOreForOneUnitOfFuel(reactions);

            Assert.Equal(165, min);
        }

        [Fact]
        public void FirstStarExample3()
        {
            var input = new[]
            {
                "157 ORE => 5 NZVS",
                "165 ORE => 6 DCFZ",
                "44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL",
                "12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ",
                "179 ORE => 7 PSHF",
                "177 ORE => 5 HKGWZ",
                "7 DCFZ, 7 PSHF => 2 XJWVT",
                "165 ORE => 2 GPVTF",
                "3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"
            };

            var reactions = Parse(input);

            var min = MinimumAmountOreForOneUnitOfFuel(reactions);

            Assert.Equal(13312, min);
        }

        [Fact]
        public void FirstStarExample4()
        {
            var input = new[]
            {
                "2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG",
                "17 NVRVD, 3 JNWZP => 8 VPVL",
                "53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL",
                "22 VJHF, 37 MNCFX => 5 FWMGM",
                "139 ORE => 4 NVRVD",
                "144 ORE => 7 JNWZP",
                "5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC",
                "5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV",
                "145 ORE => 6 MNCFX",
                "1 NVRVD => 8 CXFTF",
                "1 VJHF, 6 MNCFX => 4 RFSQX",
                "176 ORE => 6 VJHF"
            };

            var reactions = Parse(input);

            var min = MinimumAmountOreForOneUnitOfFuel(reactions);

            Assert.Equal(180697, min);
        }

        [Fact]
        public void FirstStarExample5()
        {
            var input = new[]
            {
                "171 ORE => 8 CNZTR",
                "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL",
                "114 ORE => 4 BHXH",
                "14 VRPVC => 6 BMBT",
                "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL",
                "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT",
                "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW",
                "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW",
                "5 BMBT => 4 WPTQ",
                "189 ORE => 9 KTJDG",
                "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP",
                "12 VRPVC, 27 CNZTR => 2 XDBXC",
                "15 KTJDG, 12 BHXH => 5 XCVML",
                "3 BHXH, 2 VRPVC => 7 MZWV",
                "121 ORE => 7 VRPVC",
                "7 XCVML => 6 RJRHP",
                "5 BHXH, 4 VRPVC => 5 LTCX"
            };

            var reactions = Parse(input);

            var min = MinimumAmountOreForOneUnitOfFuel(reactions);

            Assert.Equal(2210736, min);
        }
    }
}