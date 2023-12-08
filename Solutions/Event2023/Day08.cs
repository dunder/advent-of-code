using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 8: Haunted Wasteland ---
    public class Day08
    {
        private readonly ITestOutputHelper output;

        public Day08(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Node(string Left, string Right);
        private record Network(char[] Instructions, Dictionary<string, Node> Nodes);

        private Network Parse(IList<string> input)
        {
            var instructionsInput = input.First();

            var instructions = instructionsInput.ToCharArray();

            return new Network(instructions, input.Skip(2).Select(line => {
                var start = line[..3];
                var left = line[7..10];
                var right = line[12..15];

                return (start, new Node(left, right));
            }).ToDictionary(x => x.Item1, x => x.Item2));
        } 

        public long StepsToTarget(IList<string> input)
        {
            Network network = Parse(input);
            
            return StepsBetween(network, "AAA", "ZZZ".Equals);
        }

        private long StepsBetween(Network network, string start, Func<string, bool> endPredicate)
        {
            int steps = 0;
            string node = start;

            (char[] instructions, Dictionary<string, Node> nodes) = network;

            while (!endPredicate(node))
            {
                node = instructions[steps++ % instructions.Length] == 'L' ? nodes[node].Left : nodes[node].Right;
            }

            return steps;
        }

        public long GhostStepsToTarget(IList<string> input)
        {
            var network = Parse(input);

            var ghostPaths = network.Nodes.Keys
                .Where(node => node.EndsWith("A"))
                .Select(startNode => StepsBetween(network, startNode, node => node.EndsWith("Z")))
                .ToArray();

            // added method in Maths for calculating least common multiple for more than two numers according to
            // https://stackoverflow.com/a/29717490/15366690
            return Maths.LCM(ghostPaths);
        }

        public long FirstStar()
        {
            var input = ReadLineInput();
            return StepsToTarget(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();
            return GhostStepsToTarget(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(21797, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(23977527174353, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "RL",
                "",
                "AAA = (BBB, CCC)",
                "BBB = (DDD, EEE)",
                "CCC = (ZZZ, GGG)",
                "DDD = (DDD, DDD)",
                "EEE = (EEE, EEE)",
                "GGG = (GGG, GGG)",
                "ZZZ = (ZZZ, ZZZ)"
            };

            Assert.Equal(2, StepsToTarget(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "LR",
                "",
                "11A = (11B, XXX)",
                "11B = (XXX, 11Z)",
                "11Z = (11B, XXX)",
                "22A = (22B, XXX)",
                "22B = (22C, 22C)",
                "22C = (22Z, 22Z)",
                "22Z = (22B, 22B)",
                "XXX = (XXX, XXX)",
            };

            Assert.Equal(6, GhostStepsToTarget(example));
        }
    }
}
