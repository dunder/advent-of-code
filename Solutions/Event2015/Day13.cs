using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Combinatorics;
using Xunit;

using static Solutions.InputReader;


namespace Solutions.Event2015
{
    // --- Day 13: Knights of the Dinner Table ---
    public class Day13
    {
        public Dictionary<string, Dictionary<string, int>> Parse(IEnumerable<string> input)
        {
            var expression = new Regex(@"([A-Z][a-z]+) would (lose|gain) (\d+) happiness units by sitting next to ([A-Z][a-z]+).");

            var pairs = new Dictionary<string, Dictionary<string, int>>();

            foreach (var line in input)
            {
                var match = expression.Match(line);

                var name1 = match.Groups[1].Value;
                var loseOrGain = match.Groups[2].Value;
                var happiness = int.Parse(match.Groups[3].Value);
                var name2 = match.Groups[4].Value;

                if (!pairs.ContainsKey(name1))
                {
                    pairs.Add(name1, new Dictionary<string, int>());
                }

                pairs[name1][name2] = loseOrGain == "gain" ? happiness : -happiness;
            }

            return pairs;
        }

        public static IEnumerable<int> EnumerateHappiness(Dictionary<string, Dictionary<string, int>> pairs)
        {
            List<IEnumerable<string>> allArrangements = pairs.Keys.Permutations().ToList();
            
            foreach (var arrangement in allArrangements)
            {
                var seating = arrangement.ToList();
                int happiness = 0;
                for (int i = 0; i < seating.Count; i++)
                {
                    string left = i == 0 ? seating[seating.Count - 1] : seating[i - 1];
                    string current = seating[i];
                    string right = i == seating.Count - 1 ? seating[0] : seating[i + 1];

                    happiness = happiness + pairs[current][left] + pairs[current][right];
                }

                yield return happiness;
            }
        }


        public int FirstStar()
        {
            var input = ReadLineInput();
            var pairs = Parse(input);

            return EnumerateHappiness(pairs).Max();
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            var pairs = Parse(input);

            pairs = AddMe(pairs);

            return EnumerateHappiness(pairs).Max();
        }

        private Dictionary<string, Dictionary<string, int>> AddMe(Dictionary<string, Dictionary<string, int>> pairs)
        {
            foreach (var dict in pairs.Values)
            {
                dict.Add("Matias", 0);
            }

            var keys = pairs.Keys.ToList();

            var myFriends = new Dictionary<string, int>();

            foreach (var key in keys)
            {
                myFriends.Add(key, 0);
            }

            pairs.Add("Matias", myFriends);

            return pairs;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(709, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(668, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new[]
            {
                "Alice would gain 54 happiness units by sitting next to Bob.",
                "Alice would lose 79 happiness units by sitting next to Carol.",
                "Alice would lose 2 happiness units by sitting next to David.",
                "Bob would gain 83 happiness units by sitting next to Alice.",
                "Bob would lose 7 happiness units by sitting next to Carol.",
                "Bob would lose 63 happiness units by sitting next to David.",
                "Carol would lose 62 happiness units by sitting next to Alice.",
                "Carol would gain 60 happiness units by sitting next to Bob.",
                "Carol would gain 55 happiness units by sitting next to David.",
                "David would gain 46 happiness units by sitting next to Alice.",
                "David would lose 7 happiness units by sitting next to Bob.",
                "David would gain 41 happiness units by sitting next to Carol."
            };

            var pairs = Parse(input);
            var optimalHappiness = EnumerateHappiness(pairs).Max();

            Assert.Equal(330, optimalHappiness);

        }
    }
}
