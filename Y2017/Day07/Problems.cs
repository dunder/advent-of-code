using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day7 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day7\input.txt");

            var result = Towers.BottomDisc(input);

            _output.WriteLine($"Day 7 problem 1: {result.Name}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day7\input.txt");

            var result = Towers.FindUnbalanced(input);

            _output.WriteLine($"Day 7 problem 2: {result}");
        }
    }

    public class Towers {
 

        private static (Dictionary<string, Disc> allDiscs, HashSet<Disc> allParents) AllDiscs(string[] input) {
            var discs = new Dictionary<string, Disc>();
            var parents = new HashSet<Disc>();
            foreach (var row in input) {
                var discName = row.Substring(0, row.IndexOf("(", StringComparison.InvariantCulture)).Trim();
                int weight = int.Parse(Regex.Match(row, @"\d+").Value);
                var disc = new Disc { Name = discName, Weight = weight };
                discs.Add(disc.Name, disc);

                
            }
            foreach (var row in input) {
                var disc = discs[row.Substring(0, row.IndexOf("(", StringComparison.InvariantCulture)).Trim()];

                if (row.Contains("->")) {
                    var result = Regex.Split(row, "->");
                    var discParents = result[1].Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
                    disc.Parents.AddRange(discParents.Select(p => discs[p]));
                    foreach (var discParent in disc.Parents) {
                        parents.Add(discParent);
                    }
                }
            }
            return (discs, parents);
        }

        public static Disc BottomDisc(string[] input) {

            (var discs, var parents) = AllDiscs(input);

            return BottomDisc(discs, parents);
        }

        public static int FindUnbalanced(string[] input) {

            (var discs, var parents) = AllDiscs(input);

            var bottomDisc = BottomDisc(discs, parents);

            Disc unbalancedCandidate = bottomDisc;
            int targetWeight = 0;
            while(true) { 

                var weightGroups = unbalancedCandidate.Parents.GroupBy(TowerWeight).ToList();

                if (weightGroups.Count == 1) {
                    break;
                }

                var odd = weightGroups.Single(g => g.Count() == 1);
                targetWeight = weightGroups.Single(g => g.Count() > 1).Key;

                unbalancedCandidate = odd.Single();
            } 
         

            return unbalancedCandidate.Weight - (TowerWeight(unbalancedCandidate) - targetWeight);
        }

        private static Disc BottomDisc(Dictionary<string, Disc> discs, HashSet<Disc> parents) {
            return discs.Values.Single(d => !parents.Contains(d));
        }

        private static int TowerWeight(Disc start) {
            (_, var visited) = start.DepthFirstWithVisited(n => n.Parents);
            return visited.Sum(p => p.Weight);
        }

    }

    public class Disc : IGraph<Disc> {
        public string Name { get; set; }
        public int Weight { get; set; }
        public List<Disc> Parents { get; set; } = new List<Disc>();
        public IEnumerable<Disc> GetNeighbours(Disc vertex) {
            return Parents;
        }

        protected bool Equals(Disc other) {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Disc) obj);
        }

        public override int GetHashCode() {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString() {
            return $"{Name} ({Weight})";
        }
    }

    public interface IGraph<T> {
        IEnumerable<T> GetNeighbours(T vertex);
    }

    public static class GraphExtensions {

        public static IEnumerable<T> TraverseAll<T>(this T start, Func<T, IEnumerable<T>> neighbourFetcher)  {
            if (!neighbourFetcher(start).Any()) {
                yield return start;
            } else {
                foreach (var neighbour in neighbourFetcher(start)) {
                    foreach (var neighboursNeighbour in neighbour.TraverseAll(neighbourFetcher)) {
                        yield return neighboursNeighbour;
                    }
                }
            }
        }

        public static IEnumerable<T> DepthFirst<T>(this T start, Func<T,IEnumerable<T>> neighbourFetcher) {
            var visited = new HashSet<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count != 0) {
                var current = stack.Pop();

                if (!visited.Add(current)) {
                    continue;
                }

                yield return current;

                var neighbours = neighbourFetcher(current)
                    .Where(n => !visited.Contains(n));

                // If you don't care about the left-to-right order, remove the Reverse
                foreach (var neighbour in neighbours.Reverse()) {
                    stack.Push(neighbour);
                }
            }
        }

        public static (IEnumerable<T> depthFirst, ISet<T> visited) DepthFirstWithVisited<T>(this T start, Func<T, IEnumerable<T>> neighbourFetcher) {
            var visited = new HashSet<T>();
            var depthFirst = new List<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count != 0) {
                var current = stack.Pop();

                if (!visited.Add(current)) {
                    continue;
                }

                depthFirst.Add(current);

                var neighbours = neighbourFetcher(current)
                    .Where(n => !visited.Contains(n));

                // If you don't care about the left-to-right order, remove the Reverse
                foreach (var neighbour in neighbours.Reverse()) {
                    stack.Push(neighbour);
                }
            }
            return (depthFirst, visited);
        }

        public static IEnumerable<T> DepthFirst<T>(this IGraph<T> graph, T start) {
            var visited = new HashSet<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count != 0) {
                var current = stack.Pop();

                if (!visited.Add(current)) {
                    continue;
                }

                yield return current;

                var neighbours = graph.GetNeighbours(current)
                    .Where(n => !visited.Contains(n));

                // If you don't care about the left-to-right order, remove the Reverse
                foreach (var neighbour in neighbours.Reverse()) {
                    stack.Push(neighbour);
                }
            }
        }
    }

}
