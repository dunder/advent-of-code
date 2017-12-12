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

            _output.WriteLine($"Day 12 problem 2: {result}"); // not 2000
        }
    }

    public static class MemoryBank {
        public static int CountConnectedTo0(string[] input) {

            var connections = new Dictionary<string, List<string>>();

            foreach (var line in input) {
                var parts = Regex.Split(line, @" <-> ");
                var id = parts[0];
                connections.Add(id, parts[1].SplitOnCommaSpaceSeparated().ToList());
            }

            int connection0Count = 0;
            foreach (var connection in connections) {
                if (connection.Key == "0") {
                    connection0Count++;
                }
                else {
                    (_, var visited) = connection.Key.DepthFirstWithVisited(c => connections[c]);
                    if (visited.Contains("0")) {
                        connection0Count++;
                    }
                }
            }

            return connection0Count;
        }

        public static int CountGroups(string[] input) {
            var connections = new Dictionary<string, List<string>>();

            foreach (var line in input) {
                var parts = Regex.Split(line, @" <-> ");
                var id = parts[0];
                connections.Add(id, parts[1].SplitOnCommaSpaceSeparated().ToList());
            }

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
