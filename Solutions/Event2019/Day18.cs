using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.MapGeometry;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // 
    public class Day18
    {
        public Dictionary<Point, char> Parse(List<string> input)
        {
            var map = new Dictionary<Point, char>();
            for (int y = 0; y < input.Count(); y++)
            {
                var line = input[y];
                for (int x = 0; x < line.Length; x++)
                {
                    map.Add(new Point(x,y), line[x]);
                }
            }

            return map;
        }

        public class Node<T>
        {
            public T Data { get; }
            public int Depth { get; }
            public Node<T> Parent { get; }

            public Node<T> Start
            {
                get
                {
                    var walkTo = this;
                    while (walkTo.Parent != null)
                    {
                        walkTo = walkTo.Parent;
                    }

                    return walkTo;
                }
            }
            public IList<T> Path
            {
                get
                {
                    var path = new List<T>();
                    var node = this;
                    path.Add(node.Data);
                    while (node.Parent != null)
                    {
                        node = node.Parent;
                        path.Add(node.Data);
                    }

                    path.Reverse();

                    return path;
                }
            }

            public int DepthKeyComplete(Func<T, T, bool> predicate)
            {
                    var node = this;
                    while (node.Parent != null)
                    {
                        if (predicate(node.Data, node.Parent.Data))
                        {
                            return Depth;
                        }
                        node = node.Parent;
                    }

                    return 0;
            }

            public Node(T data, int depth, Node<T> parent = null)
            {
                Data = data;
                Depth = depth;
                Parent = parent;
            }

            public override string ToString()
            {
                return Data.ToString();
            }
        }

        private class LocationWithKey
        {
            public Point Location { get; set; }
            public HashSet<string> Keys { get; set; } = new HashSet<string>();


            public override string ToString()
            {
                var key = Keys.Any() ? string.Join(",", Keys) : "(no keys)";
                return $"{Location} {key}";
            }

            protected bool Equals(LocationWithKey other)
            {
                return Location.Equals(other.Location) && Keys.SetEquals(other.Keys);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((LocationWithKey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    // TODO: Possible performance problem
                    return (Location.GetHashCode() * 397) ^ (Keys != null ? string.Join("", Keys.OrderBy(k => k)).GetHashCode() : 0);
                }
            }
        }


        private List<LocationWithKey> Neighbors(Node<LocationWithKey> node, Dictionary<Point, char> map)
        {
            if (node.Data.Location.X == 14 && node.Data.Location.Y == 1)
            {
                var stop = "stop";
            }
            var neighbors = new List<LocationWithKey>();
            var potentialNeighbors = node.Data.Location.AdjacentInMainDirections();
            foreach (var potentialNeighbor in potentialNeighbors)
            {
                if (map.TryGetValue(potentialNeighbor, out char mapItem))
                {
                    switch (mapItem.ToString())
                    {
                        case "#":
                            continue;
                        case "@":
                        case ".":
                            neighbors.Add(new LocationWithKey {Location = potentialNeighbor, Keys = new HashSet<string>(node.Data.Keys)});
                            break;
                        case var _ when keyPattern.IsMatch(mapItem.ToString()):
                            var neighbor = new LocationWithKey {Location = potentialNeighbor, Keys = new HashSet<string>(node.Data.Keys)};
                            neighbor.Keys.Add(mapItem.ToString());
                            neighbors.Add(neighbor);
                            break;
                        case var x when doorPattern.IsMatch(mapItem.ToString()):
                            if (node.Data.Keys.Contains(x.ToLowerInvariant()))
                            {
                                neighbors.Add(new LocationWithKey {Location = potentialNeighbor, Keys = new HashSet<string>(node.Data.Keys)});
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unknown map item");

                    }
                }
            }
            return neighbors;
        }
        private static Regex doorPattern = new Regex(@"[A-Z]");
        private static Regex keyPattern = new Regex(@"[a-z]");

        private static (IEnumerable<Node<LocationWithKey>> depthFirst, ISet<LocationWithKey> visited) DepthFirst(
            LocationWithKey start,
            Func<Node<LocationWithKey>, Dictionary<Point, char>, IEnumerable<LocationWithKey>> neighborFetcher,
            Dictionary<Point, char> map, 
            int keyCount)
        {
            var visited = new HashSet<LocationWithKey>();
            var depthFirst = new List<Node<LocationWithKey>>();
            var stack = new Stack<Node<LocationWithKey>>();

            stack.Push(new Node<LocationWithKey>(start, 0));

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current.Data))
                {
                    continue;
                }

                if (current.Data.Keys.Count == 1 && current.Data.Keys.Contains("a"))
                {
                    var test = "test";
                }

                depthFirst.Add(current);

                var neighbors = neighborFetcher(current, map).Where(n => !visited.Contains(n));

                foreach (var neighbor in neighbors.Reverse())
                {
                    stack.Push(new Node<LocationWithKey>(neighbor, current.Depth + 1, current));
                }
            }

            return (depthFirst, visited);
        }

        private int StepsInShortestPath(Dictionary<Point, char> map)
        {
            var keys = new HashSet<string>();

            var start = map.Single(p => p.Value == '@').Key;
            var keyCount = map.Values.Count(char.IsLower);
            var (depthFirst, _) = DepthFirst(new LocationWithKey {Location = start}, Neighbors, map, keyCount);

            var withAllKeys = depthFirst.Where(n => n.Data.Keys.Count == keyCount).OrderBy(n => n.Depth).ToList();
            var depths = withAllKeys.Select(x =>
                x.DepthKeyComplete((current, previous) => !current.Keys.SetEquals(previous.Keys))).OrderBy(x => x);

            return withAllKeys.Min(n => n.Depth);
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            var map = Parse(input.ToList());

            return StepsInShortestPath(map);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
            //Assert.Equal(7980, FirstStar()); // too high
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample1()
        {
            var input = new[]
            {
                "########################",
                "#f.D.E.e.C.b.A.@.a.B.c.#",
                "######################.#",
                "#d.....................#",
                "########################"
            };
            var map = Parse(input.ToList());
            var steps = StepsInShortestPath(map);

            Assert.Equal(86, steps);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = new[]
            {
                "########################",
                "#...............b.C.D.f#",
                "#.######################",
                "#.....@.a.B.c.d.A.e.F.g#",
                "########################"
            };
            var map = Parse(input.ToList());
            var steps = StepsInShortestPath(map);

            Assert.Equal(132, steps);
        }

        [Fact]
        public void FirstStarExample3()
        {
            var input = new[]
            {
                "#################",
                "#i.G..c...e..H.p#",
                "########.########",
                "#j.A..b...f..D.o#",
                "########@########",
                "#k.E..a...g..B.n#",
                "########.########",
                "#l.F..d...h..C.m#",
                "#################"
            };
            var map = Parse(input.ToList());
            var steps = StepsInShortestPath(map);

            Assert.Equal(136, steps);
        }

        [Fact]
        public void FirstStarExample4()
        {
            var input = new[]
            {
                "########################",
                "#@..............ac.GI.b#",
                "###d#e#f################",
                "###A#B#C################",
                "###g#h#i################",
                "########################"
            };
            var map = Parse(input.ToList());
            var steps = StepsInShortestPath(map);

            Assert.Equal(81, steps);
        }
    }
}
