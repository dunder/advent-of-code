using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Combinatorics;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 9: All in a Single Night ---
    public class Day09
    {
        public static Dictionary<string, Dictionary<string, int>> Parse(IEnumerable<string> input)
        {
            var distances = new Dictionary<string, Dictionary<string, int>>();

            var pattern = new Regex(@"([A-Za-z]+) to ([A-Za-z]+) = (\d+)");

            foreach (var line in input)
            {
                var match = pattern.Match(line);
                var from = match.Groups[1].Value;
                var to = match.Groups[2].Value;
                var distance = int.Parse(match.Groups[3].Value);

                if (!distances.ContainsKey(from))
                {
                    distances.Add(from, new Dictionary<string, int>());
                }

                distances[from].Add(to, distance);

                if (!distances.ContainsKey(to))
                {
                    distances.Add(to, new Dictionary<string, int>());
                }

                distances[to].Add(from, distance);
            }

            return distances;
        }

        public static IEnumerable<int> CalculateRouteDistances(Dictionary<string, Dictionary<string,int>> distances)
        {
            var permutationsOfAllLocations = distances.Keys.Permutations(distances.Keys.Count);

            var pathDistances = permutationsOfAllLocations.Select(path =>
            {
                var (distance, _) = path.Aggregate<string, (int Distance, string Previous)>(
                    (0, null), (state, current) =>
                    {
                        if (state.Previous == null)
                        {
                            return (state.Distance, current);
                        }

                        return (state.Distance + distances[state.Previous][current], current);
                    });
                return distance;
            });

            return pathDistances;
        }

        public static int FirstStar()
        {
            var input = ReadLineInput();

            var distances = Parse(input);
            var routeDistances = CalculateRouteDistances(distances);

            return routeDistances.Min();
        }

        public static int SecondStar()
        {
            var input = ReadLineInput();

            var distances = Parse(input);
            var routeDistances = CalculateRouteDistances(distances);

            return routeDistances.Max();
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(117, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(909, result);
        }

        [Fact]
        public void ParseTest()
        {
            var input = new[]
            {
                "London to Dublin = 464",
                "London to Belfast = 518",
                "Dublin to Belfast = 141"
            };

            var distances = Parse(input);

            Assert.Equal(3, distances.Count);

            Assert.Equal(464, distances["London"]["Dublin"]);
            Assert.Equal(464, distances["Dublin"]["London"]);
            Assert.Equal(518, distances["London"]["Belfast"]);
            Assert.Equal(518, distances["Belfast"]["London"]);
            Assert.Equal(141, distances["Dublin"]["Belfast"]);
            Assert.Equal(141, distances["Belfast"]["Dublin"]);
        }
    }
}
