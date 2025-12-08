using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 8: Playground ---
    public class Day08
    {
        private readonly ITestOutputHelper output;

        public Day08(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record JunctionBox(int x, int y, int z);

        private class JunctionBoxRig
        {
            public JunctionBoxRig(List<JunctionBox> junctionBoxes)
            {
                JunctionBoxes = junctionBoxes;
                Distances = [];

                for (int i = 0; i < JunctionBoxes.Count; i++)
                {
                    JunctionBox a = JunctionBoxes[i];

                    for (int j = i + 1; j < JunctionBoxes.Count; j++)
                    {
                        JunctionBox b = JunctionBoxes[j];

                        Distances.Add(Distance(a, b), (a, b));
                    }
                }
            }

            public List<HashSet<JunctionBox>> Circuits { get; } = [];

            public List<JunctionBox> JunctionBoxes { get; private set; }
            public Dictionary<double, (JunctionBox, JunctionBox)> Distances { get; private set; }
            public IEnumerable<(JunctionBox a, JunctionBox b)> JunctionBoxPairsByDistance => Distances.OrderBy(d => d.Key).Select(kvp => kvp.Value);

            public void Connect(JunctionBox a, JunctionBox b)
            {
                int circuitAIndex = Circuits.FindIndex(circuit => circuit.Contains(a));
                int circuitBIndex = Circuits.FindIndex(circuit => circuit.Contains(b));

                if (circuitAIndex == -1 && circuitBIndex == -1)
                {
                    Circuits.Add([a, b]);
                }
                else if (circuitAIndex > -1 && circuitBIndex > -1 && circuitAIndex != circuitBIndex)
                {
                    Circuits[circuitAIndex].UnionWith(Circuits[circuitBIndex]);
                    Circuits.RemoveAt(circuitBIndex);
                }
                else if (circuitAIndex > -1)
                {
                    Circuits[circuitAIndex].Add(b);
                }
                else if (circuitBIndex > -1)
                {
                    Circuits[circuitBIndex].Add(a);
                }
            }
        }

        private static JunctionBoxRig Parse(IList<string> input)
        {
            List<JunctionBox> junctionBoxes = input
                .Select(line =>
                {
                    var parts = line.Split(",").Select(int.Parse).ToList();
                    return new JunctionBox(parts[0], parts[1], parts[2]);
                })
                .ToList();

            return new JunctionBoxRig(junctionBoxes);
        }

        private static double Distance(JunctionBox p1, JunctionBox p2)
        {
            return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2) + Math.Pow(p2.z - p1.z, 2));
        }

        private static int CountLargest(IList<string> input, int count)
        {
            JunctionBoxRig rig = Parse(input);

            foreach (var pair in rig.JunctionBoxPairsByDistance.Take(count).ToList())
            {
                (JunctionBox a, JunctionBox b) = pair;

                rig.Connect(a, b);
            }

            var largest = rig.Circuits.Select(v => v.Count).OrderByDescending(x => x).Take(3);

            return largest.Aggregate(1, (a, v) => a * v);
        }

        private static long ConnectAll(IList<string> input)
        {
            JunctionBoxRig rig = Parse(input);

            (JunctionBox a, JunctionBox b)? lastPair = null;

            foreach (var pair in rig.JunctionBoxPairsByDistance)
            {
                (JunctionBox a, JunctionBox b) = pair;

                rig.Connect(a, b);

                lastPair = pair;

                if (rig.Circuits.Any(c => c.Count == rig.JunctionBoxes.Count))
                {
                    break;
                }
            }

            var largest = rig.Circuits.Select(v => v.Count).OrderByDescending(x => x).Take(3);

            return (long)lastPair.Value.a.x * lastPair.Value.b.x;
        }

        private static int Problem1(IList<string> input)
        {
            return CountLargest(input, 1000);
        }

        private static long Problem2(IList<string> input)
        {
            return ConnectAll(input);
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

            Assert.Equal(8995844880, Problem2(input));
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

            Assert.Equal(25272, ConnectAll(exampleInput));
        }
    }
}