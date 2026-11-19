using System.Collections.Generic;
using Priority_Queue;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 25: Snowverload ---
    public class Day25
    {
        private readonly ITestOutputHelper output;

        public Day25(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static Dictionary<string, List<string>> ParseConnections(IList<string> lines)
        {
            Dictionary<string, List<string>> connections = new();

            foreach (string line in lines)
            {
                var parts = line.Split(": ");
                var left = parts[0];
                var rights = parts[1].Split(" ");

                if (connections.ContainsKey(left))
                {
                    connections[left].AddRange(rights);
                }
                else
                {
                    connections.Add(left, [.. rights]);
                }

                foreach (var right in rights)
                {
                    if (!connections.TryGetValue(right, out var connection))
                    {
                        connections.Add(right, []);
                    }

                    connections[right].Add(left);
                }
            }

            return connections;
        }

        public int FirstStar(IList<string> input)
        {
            var connectionLookup = ParseConnections(input);

            return 0;
        }

        public class Node : FastPriorityQueueNode
        {
            
        }

        [Fact]
        [Trait("Event", "2023")]
        public void FirstStarTest()
        {
            var priorityQueue = new FastPriorityQueue<Node>(1000);

            priorityQueue.Enqueue(new Node(), 1);

            var input = ReadLineInput();

            Assert.Equal(-1, FirstStar(input));
        }

        [Fact]
        [Trait("Example", "2023")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(54, FirstStar(exampleInput));
        }
    }
}
