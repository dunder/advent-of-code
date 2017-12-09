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

        [Fact]
        public void Problem2Stolen () { 
            string[] input = File.ReadAllLines(@".\Day7\input.txt");

            (var bottom, var newUnbalancedValue) = Towers.Stolen(input);

            _output.WriteLine($"Day 7 problem 2: {bottom}");
        }
    }

    public class Towers {
        public static Disc BottomDisc(string[] input) {

            (var discs, var parents) = AllDiscs(input);

            return BottomDisc(discs, parents);
        }

        private static Disc BottomDisc(Dictionary<string, Disc> discs, HashSet<Disc> parents) {
            return discs.Values.Single(d => !parents.Contains(d));
        } 

        private static (Dictionary<string, Disc>, HashSet<Disc>) AllDiscs(string[] input) {
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
        public static int FindUnbalanced(string[] input) {

            (var discs, var parents) = AllDiscs(input);

            var bottomDisc = BottomDisc(discs, parents);

            

            var parentTowerWeights = new Dictionary<Disc, int>();

            foreach (var parent in bottomDisc.Parents) {
                (_, var visited) = parent.DepthFirstWithVisited(n => n.Parents);
                bool parentIncluded = visited.Contains(parent);
                int towerWeight = visited.Sum(p => p.Weight);
                parentTowerWeights.Add(parent, towerWeight);
            }

            var groupedByWeight = parentTowerWeights.GroupBy(p => p.Value).ToList();

            var odd = groupedByWeight.Where(g => g.Count() == 1).Select(g => g.Single()).Single();
            var targetWeight = groupedByWeight.Single(g => g.Count() > 1).First().Value;

            var unbalancedDisc = FindUnbalanced(odd.Key);

            var unbalancedNode = odd.Key.Weight;

            var adjustment = targetWeight - odd.Value;

            return unbalancedDisc.Weight + adjustment;
        }

        public static (string bottom, int newUnbalancedValue) Stolen(string[] input) {
            var children = new Dictionary<string, List<string>>();
            var ownWeights = new Dictionary<string, int>();
            var totalWeights = new Dictionary<string, int>();

            foreach (var line in input) {
                var name = line.Substring(0, line.IndexOf("(", StringComparison.InvariantCulture)).Trim();
                int weight = int.Parse(Regex.Match(line, @"\d+").Value);
                ownWeights.Add(name, weight);
                if (line.Contains("->")) {
                    var myChildren = Regex.Split(line, "->")[1].Trim().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                    children.Add(name, myChildren);
                } else {
                    children.Add(name, new List<string>());
                }
            }
            var all = new HashSet<string>(ownWeights.Keys);
            var allThatAreChildren = new HashSet<string>(children.SelectMany(c => c.Value));

            all.ExceptWith(allThatAreChildren);

            var bottom = all.Single();

            RecursiveWeight(bottom, children, ownWeights, totalWeights);

            var at = bottom;
            //int previousWeight = 0;
            //do {
                
            //} while ()

            return (bottom, 0);

        }

        private static void RecursiveWeight(string name, Dictionary<string, List<string>> children, Dictionary<string, int> ownHeights, Dictionary<string, int> totalWeights) {
            //int newWeight = ownHeights[name] + children[name].Select(c => RecursiveWeight(c, children, ownHeights, totalWeights)).Sum();
            //if (totalWeights.ContainsKey(name)) {
            //    totalWeights[name] = newWeight;
            //} else {
            //    totalWeights.Add(name, newWeight);
            //}
        }


        private static Disc FindUnbalancedDisc(Disc start, Dictionary<string, Disc> discs) {
            var allDiscs = discs.Values;

            

            var count = start.TraverseAll(n => n.Parents).Count(disc => disc.Parents.Any(d => d.Weight != disc.Parents.First().Weight));
            return allDiscs.First(disc => disc.Parents.All(d => d.Weight == disc.Parents.First().Weight));
        }

        private static Disc FindUnbalanced(Disc start) {
            if (start.Parents.Any(p => p.Weight != start.Parents.First().Weight)) {
                foreach (var startParent in start.Parents) {
                    return FindUnbalanced(startParent);
                }
                
            }
            return start;
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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Disc) obj);
        }

        public override int GetHashCode() {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString() {
            return Name;
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

        public static (IEnumerable<T>, ISet<T>) DepthFirstWithVisited<T>(this T start, Func<T, IEnumerable<T>> neighbourFetcher) {
            var visited = new HashSet<T>();
            var shortest = new List<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count != 0) {
                var current = stack.Pop();

                if (!visited.Add(current)) {
                    continue;
                }

                shortest.Add(current);

                var neighbours = neighbourFetcher(current)
                    .Where(n => !visited.Contains(n));

                // If you don't care about the left-to-right order, remove the Reverse
                foreach (var neighbour in neighbours.Reverse()) {
                    stack.Push(neighbour);
                }
            }
            return (shortest, visited);
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
