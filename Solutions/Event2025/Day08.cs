using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 08: Phrase ---
    public class Day08
    {
        private readonly ITestOutputHelper output;

        public Day08(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record JunctionBox(int x, int y, int z)
        {

        }

        private static double Distance(JunctionBox p1, JunctionBox p2)
        {
            return Math.Sqrt(
                Math.Pow(p2.x - p1.x, 2) +
                Math.Pow(p2.y - p1.y, 2) +
                Math.Pow(p2.z - p1.z, 2)
            );
        }

        private static int CountLargest(IList<string> input, int count)
        {
            var junctionBoxes = input
                .Select(line =>
                {
                    var parts = line.Split(",").Select(int.Parse).ToList();
                    return new JunctionBox(parts[0], parts[1], parts[2]);
                })
                .ToList();


            Dictionary<double, (JunctionBox, JunctionBox)> distances = [];

            for (int i = 0; i < junctionBoxes.Count; i++)
            {
                JunctionBox a = junctionBoxes[i];

                for (int j = i + 1; j < junctionBoxes.Count; j++)
                {
                    JunctionBox b = junctionBoxes[j];

                    distances.Add(Distance(a, b), (a, b));
                }
            }

            var shortestPairs = distances.OrderBy(d => d.Key).Select(kvp => kvp.Value).Take(count).ToList();

            List<HashSet<JunctionBox>> circuits = [];

            foreach (var pair in shortestPairs)
            {
                (JunctionBox a, JunctionBox b) = pair;
                int circuitAIndex = circuits.FindIndex(circuit => circuit.Contains(a));
                int circuitBIndex = circuits.FindIndex(circuit => circuit.Contains(b));

                if (circuitAIndex == -1 && circuitBIndex == -1)
                {
                    circuits.Add([a, b]);
                }
                else if (circuitAIndex > -1 && circuitBIndex > -1 && circuitAIndex != circuitBIndex)
                {
                    circuits[circuitAIndex].UnionWith(circuits[circuitBIndex]);
                    circuits.RemoveAt(circuitBIndex);
                }
                else if (circuitAIndex > -1)
                {
                    circuits[circuitAIndex].Add(b);
                }
                else if (circuitBIndex > -1)
                {
                    circuits[circuitBIndex].Add(a);
                }
            }

            var largest = circuits.Select(v => v.Count).OrderByDescending(x => x).Take(3);

            return largest.Aggregate(1, (a, v) => a * v);
        }

        private static int Problem1(IList<string> input)
        {
            return CountLargest(input, 1000);
        }

        private static int Problem2(IList<string> input)
        {
            return 0;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(75680, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(40, CountLargest(exampleInput, 10));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem2(exampleInput));
        }
    }
}