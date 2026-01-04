using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private List<(string a, string b)> ToConnectionList(Dictionary<string, List<string>> connections)
        {
            HashSet<(string a, string b)> connectionSet = [];

            foreach (var kvp in connections)
            {
                foreach (var v in kvp.Value)
                {
                    if (!connectionSet.Contains((v, kvp.Key)))
                    {
                        connectionSet.Add((kvp.Key, v));
                    }
                }
            }

            return connectionSet.ToList();
        }

        //private HashSet<string> BronKerbosch(HashSet<string> R, HashSet<string> P, HashSet<string> X)
        //{
        //    if (P.Count == 0 && X.Count == 0)
        //    {
        //        return R;
        //    }
        //    else
        //    {
        //        foreach (var v in P)
        //        {
        //            HashSet<string> N = Connections[v].ToHashSet();

        //            R = BronKerbosch(R.Union([v]).ToHashSet(), P.Intersect(N).ToHashSet(), X.Intersect(N).ToHashSet());

        //            if (R is not null)
        //            {
        //                return R;
        //            }

        //            P.Remove(v);
        //            X.Add(v);
        //        }

        //        return null;
        //    }
        //}

        private HashSet<string> Reachable(string from, Dictionary<string, List<string>> connectionLookup)
        {
            Queue<string> queue = [];
            queue.Enqueue(from);

            HashSet<string> visited = [];

            while (queue.Count > 0)
            {
                string next = queue.Dequeue();

                if (!visited.Add(next))
                {
                    continue;
                }

                foreach (var n in connectionLookup[next])
                {
                    queue.Enqueue(n);
                }
            }

            return visited;
        }

        private record Node(string Name, string Parent);

        private HashSet<Node> Paths(string from, string to, Dictionary<string, List<string>> connectionLookup)
        {
            Stack<Node> stack = [];
            stack.Push(new Node(from, null));

            HashSet<Node> visited = [];

            while (stack.Count > 0)
            {
                Node next = stack.Pop();

                if (!visited.Add(next))
                {
                    continue;
                }

                if (to.Equals(next))
                {
                    return 
                }

                foreach (var n in connectionLookup[next.Name])
                {
                    stack.Push(new Node(n, next.Name));
                }
            }

            return visited;
        }

        private static Dictionary<string, List<string>> Remove(
            (string from, string to) a,
            (string from, string to) b,
            (string from, string to) c,
            Dictionary<string, List<string>> connectionLookup)
        {
            var copy = connectionLookup.Select(kvp => (kvp.Key, new List<string>(kvp.Value))).ToDictionary();

            copy[a.from].Remove(a.to);
            copy[a.to].Remove(a.from);
            copy[b.from].Remove(b.to);
            copy[b.to].Remove(b.from);
            copy[c.from].Remove(c.to);
            copy[c.to].Remove(c.from);

            return copy;
        }


        private bool ConnectionMatch((string a, string b) expected, (string a, string b) actual)
        {
            return actual == expected || (actual.b, actual.a) == expected;
        }

        public int FirstStar(IList<string> input)
        {
            var connectionLookup = ParseConnections(input);
            var connections = ToConnectionList(connectionLookup);


            IEnumerable<(string a, string b)[]> perms3 =
                from i in Enumerable.Range(0, connections.Count)
                from j in Enumerable.Range(0, connections.Count)
                where j != i
                from k in Enumerable.Range(0, connections.Count)
                where k != i && k != j
                select new[] { connections[i], connections[j], connections[k] };

            int counter = 0;

            foreach (var p in perms3)
            {
                counter++;

                var adjusted = Remove(p[0], p[1], p[2], connectionLookup);

                var first = adjusted.First().Key;

                var firstReachable = Reachable(first, adjusted);

                if (firstReachable.Count == connectionLookup.Count) { continue; }

                var second = adjusted.Keys.FirstOrDefault(key => !firstReachable.Contains(key));

                if (second == null) { continue; }

                var secondReachable = Reachable(second, adjusted);

                var isSplit = firstReachable.Count != connectionLookup.Count;
                var isGroup = firstReachable.Count > 1;
                var isExactlyTwoGroups = firstReachable.Count + secondReachable.Count == connectionLookup.Count;

                if (isSplit && isGroup && isExactlyTwoGroups)
                {
                    return firstReachable.Count * secondReachable.Count;
                }
            }

            return 0;
        }

        [Fact]
        [Trait("Event", "2023")]
        public void FirstStarTest()
        {
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
