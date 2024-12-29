using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 23: LAN Party ---
    public class Day23
    {
        private readonly ITestOutputHelper output;

        public Day23(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static Dictionary<string , HashSet<string>> Parse(IList<string> input)
        {
            Dictionary<string, HashSet<string>> connections = new();

            foreach (var connection in input)
            {
                var parts = connection.Split('-');

                var from = parts[0];
                var to = parts[1];

                if (connections.ContainsKey(from))
                {
                    connections[from].Add(to);
                }
                else
                {
                    connections[from] = new HashSet<string> { to };
                }

                if (connections.ContainsKey(to))
                {
                    connections[to].Add(from);
                }
                else
                {
                    connections[to] = new HashSet<string> { from };
                }
            }

            return connections;
        }

        private static int Problem1(IList<string> input)
        {
            Dictionary<string, HashSet<string>> connections = Parse(input);

            HashSet<string> groups = new HashSet<string>();

            foreach (var from in connections.Keys)
            {
                HashSet<string> tos = connections[from];

                foreach (var to in tos)
                {

                    var overlap = tos.Except([to]).Where(t => connections[t].Contains(to));

                    foreach (var computer in overlap)
                    {
                        List<string> computers = [from, to, computer];
                        computers.Sort();
                        groups.Add(string.Join(",", computers));
                    }
                }
            }

            return groups.Count(group => group.Split(",").Any(computer => computer.StartsWith("t")));
        }

        // https://en.wikipedia.org/wiki/Bron%E2%80%93Kerbosch_algorithm
        private static void BronKerbosch(
            HashSet<string> R, 
            HashSet<string> P, 
            HashSet<string> X, 
            List<HashSet<string>> output, 
            Dictionary<string, HashSet<string>> connections)
        {
            if (!P.Any() && !X.Any())
            {
                output.Add(R);
            }

            foreach (var computer in P)
            {
                HashSet<string> N = connections[computer];

                BronKerbosch(R.Union([computer]).ToHashSet(),
                    P.Intersect(N).ToHashSet(),
                    X.Intersect(N).ToHashSet(),
                    output,
                    connections);

                P.Remove(computer);
                X.Add(computer);
            }
        }

        private static string Problem2(IList<string> input)
        {
            Dictionary<string, HashSet<string>> connections = Parse(input);

            List<HashSet<string>> lans = new();

            BronKerbosch([], connections.Keys.ToHashSet(), [], lans, connections);
            
            return string.Join(",", lans.MaxBy(lan => lan.Count).OrderBy(x => x));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1077, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal("bc,bf,do,dw,dx,ll,ol,qd,sc,ua,xc,yu,zt", Problem2(input)); 
        }

        private List<string> exampleInput =
            [
                "kh-tc",
                "qp-kh",
                "de-cg",
                "ka-co",
                "yn-aq",
                "qp-ub",
                "cg-tb",
                "vc-aq",
                "tb-ka",
                "wh-tc",
                "yn-cg",
                "kh-ub",
                "ta-co",
                "de-co",
                "tc-td",
                "tb-wq",
                "wh-td",
                "ta-ka",
                "td-qp",
                "aq-cg",
                "wq-ub",
                "ub-vc",
                "de-ta",
                "wq-aq",
                "wq-vc",
                "wh-yn",
                "ka-de",
                "kh-ta",
                "co-tc",
                "wh-qp",
                "tb-vc",
                "td-yn",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(7, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal("co,de,ka,ta", Problem2(exampleInput));
        }
    }
}
