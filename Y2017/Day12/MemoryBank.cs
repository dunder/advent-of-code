using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities;

namespace Y2017.Day12 {
    public static class MemoryBank {
        public static int CountConnectedTo0(string[] input) {
            var connections = ReadConnections(input);

            return 1 + connections.Count(kvp => !kvp.Key.Equals("0") && GraphExtensions.DepthFirstWithVisited<string>(kvp.Key, n => connections[n]).visited.Contains("0"));
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

            HashSet<string> groupedSet = new HashSet<string>();

            var visitedSets = connections.Select(kvp => new HashSet<string>(kvp.Key.DepthFirstWithVisited(c => connections[c]).visited));

            foreach (var visited in visitedSets) {
                if (!visited.IsSubsetOf(groupedSet)) {
                    groupedSet.UnionWith(visited);
                    counter++;
                }
            }
            
            return counter;
        }
    }
}