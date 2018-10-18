using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.Tree;

namespace Solutions.Event2017.Day07
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day07;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = Towers.BottomDisc(input).Name;
            return result;
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = Towers.FindUnbalanced(input);
            return result.ToString();
        }
    }

    public class Towers {
        private static (Dictionary<string, Disc> allDiscs, HashSet<Disc> allParents) AllDiscs(IList<string> input) {
            var discs = new Dictionary<string, Disc>();
            var parents = new HashSet<Disc>();

            foreach (var row in input) {
                var discName = row.Substring(0, row.IndexOf("(", StringComparison.InvariantCulture)).Trim();
                int weight = int.Parse(Regex.Match(row, @"\d+").Value);
                var disc = new Disc(discName, weight);
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

        public static Disc BottomDisc(IList<string> input) {
            (var discs, var parents) = AllDiscs(input);

            return BottomDisc(discs, parents);
        }

        public static int FindUnbalanced(IList<string> input) {

            (var discs, var parents) = AllDiscs(input);

            var bottomDisc = BottomDisc(discs, parents);

            Disc unbalancedCandidate = bottomDisc;
            int targetWeight = 0;
            while (true) {

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
        public string Name { get; }
        public int Weight { get; }
        public List<Disc> Parents { get; set; } = new List<Disc>();

        public Disc(string name, int weight) {
            Name = name;
            Weight = weight;
        }

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
            return Equals((Disc)obj);
        }

        public override int GetHashCode() {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString() {
            return $"{Name} ({Weight})";
        }
    }
}