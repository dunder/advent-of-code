using System.Collections.Generic;
using System.Linq;
using Shared.Tree;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 6: Universal Orbit Map ---
    public class Day06
    {
        private Dictionary<string, string> Parse(IEnumerable<string> input)
        {
            return input.Select(line => line.Split(')')).ToDictionary(pair => pair[1], pair => pair[0]);
        }

        public int CountTotalOrbits(Dictionary<string, string> orbitMap)
        {
            int count = 0;

            foreach (var pair in orbitMap)
            {
                count++;
                var around = pair.Value;
                while (orbitMap.ContainsKey(around))
                {
                    count++;
                    around = orbitMap[around];
                }
            }
            return count;
        }

        public int CountTotalTransfers(Dictionary<string, string> orbitMap)
        {
            var from = orbitMap["YOU"];
            var to = orbitMap["SAN"];

            var reversedOrbitMap = new Dictionary<string, List<string>>();

            foreach (var map in orbitMap)
            {
                if (!reversedOrbitMap.ContainsKey(map.Value))
                {
                    reversedOrbitMap.Add(map.Value, new List<string>());
                }
                reversedOrbitMap[map.Value].Add(map.Key);
            }

            IEnumerable<string> Neighbours(string o)
            {
                var ns = new List<string>();

                if (orbitMap.ContainsKey(o))
                {
                    ns.Add(orbitMap[o]);
                }

                if (reversedOrbitMap.ContainsKey(o))
                {
                    ns.AddRange(reversedOrbitMap[o]);
                }

                return ns;
            }

            var result = from.DepthFirst(Neighbours);
            return result.depthFirst.Single(x => x.Data == to).Depth;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            var orbitMap = Parse(input);
            return CountTotalOrbits(orbitMap);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            var orbitMap = Parse(input);
            return CountTotalTransfers(orbitMap);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(122782, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(271, SecondStar());
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new[]
            {
                "COM)B",
                "B)C",
                "C)D",
                "D)E",
                "E)F",
                "B)G",
                "G)H",
                "D)I",
                "E)J",
                "J)K",
                "K)L",
                "K)YOU",
                "I)SAN"
            };
            var orbitMap = Parse(input);
            var transfers = CountTotalTransfers(orbitMap);

            Assert.Equal(4, transfers);
        }
    }
}
