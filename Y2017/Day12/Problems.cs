using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day12 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day12\input.txt");

            var result = MemoryBank.CountConnectedTo0(input);

            _output.WriteLine($"Day 12 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day12\input.txt");

            var result = MemoryBank.CountGroups(input);

            _output.WriteLine($"Day 12 problem 2: {result}");
        }
    }

    public static class MemoryBank {
        public static int CountConnectedTo0(string[] input) {
            var connections = ReadConnections(input);

            return 1 + connections.Count(kvp => !kvp.Key.Equals("0") && kvp.Key.DepthFirstWithVisited(n => connections[n]).visited.Contains("0"));
        }

        private static Dictionary<string, List<string>> ReadConnections(string[] input) {
            var connections = (from line in input
                    let parts = Regex.Split(line, @" <-> ")
                    select new {Id = parts[0], Connections = parts[1].SplitOnCommaSpaceSeparated().ToList()})
                .ToDictionary(y => y.Id, z => z.Connections);
            return connections;
        }

        public static int CountGroups(string[] input) {
            var connections = ReadConnections(input);

            int counter = 0;
            var grouped = new HashSet<string>();
            foreach (var kvp in connections) {
                (_, var visited) = kvp.Key.DepthFirstWithVisited(c => connections[c]);
                var alreadyVisited = new HashSet<string>(visited);
                alreadyVisited.IntersectWith(grouped);
                if (!alreadyVisited.Any()) {
                    grouped.UnionWith(visited);
                    counter++;
                }
            }
            
            return counter;
        }
    }

}
