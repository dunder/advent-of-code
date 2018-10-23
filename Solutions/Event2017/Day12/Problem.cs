using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Extensions;
using Shared.Tree;

namespace Solutions.Event2017.Day12
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day12;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = MemoryBank.CountConnectedTo0(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = MemoryBank.CountGroups(input);
            return result.ToString();
        }

    }

    public static class MemoryBank {
        public static int CountConnectedTo0(IList<string> input) {
            var connections = ReadConnections(input);

            return 1 + connections.Count(kvp =>
                       !kvp.Key.Equals("0") &&
                       kvp.Key.DepthFirstWithVisited(n => connections[n]).visited.Contains("0"));
        }

        private static Dictionary<string, List<string>> ReadConnections(IList<string> input) {
            var connections = (from line in input
                    let parts = Regex.Split(line, @" <-> ")
                    select new { Id = parts[0], Connections = parts[1].SplitOnCommaSpaceSeparated().ToList() })
                .ToDictionary(y => y.Id, z => z.Connections);
            return connections;
        }

        public static int CountGroups(IList<string> input) {
            var connections = ReadConnections(input);

            int counter = 0;

            HashSet<string> groupedSet = new HashSet<string>();

            var visitedSets = connections.Select(kvp =>
                new HashSet<string>(kvp.Key.DepthFirstWithVisited(c => connections[c]).visited));

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