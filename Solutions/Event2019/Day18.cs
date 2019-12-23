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
    // --- Day 18: Many-Worlds Interpretation ---
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
            public HashSet<char> KeySet { get; private set; } = new HashSet<char>();

            public LocationWithKey NewLocation(Point newLocation)
            {
                return new LocationWithKey {Location = newLocation, KeySet = new HashSet<char>(KeySet)};
            }

            public void AddKey(char c)
            {
                KeySet.Add(c);
            }

            public bool HasKey(char c)
            {
                return KeySet.Contains(c);
            }

            public override string ToString()
            {
                var keys = KeySet.Any() ? string.Join(",", KeySet.OrderBy(k => k)) : "(no keys)";
                return $"{Location} {keys}";
            }

            private bool Equals(LocationWithKey other)
            {
                return Location.Equals(other.Location) && KeySet.SetEquals(other.KeySet);

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
                    var hash = Location.GetHashCode() * 397;
                    foreach (var key in KeySet)
                    {
                        hash ^= key.GetHashCode();
                    }
                    return hash;
                }
            }
        }

        private List<LocationWithKey> Neighbors(Node<LocationWithKey> node, Dictionary<Point, char> map)
        {
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
                            neighbors.Add(node.Data.NewLocation(potentialNeighbor));

                            break;
                        case var _ when KeyPattern.IsMatch(mapItem.ToString()):
                            var neighbor = node.Data.NewLocation(potentialNeighbor);
                            neighbor.AddKey(mapItem);
                            neighbors.Add(neighbor);
                            break;
                        case var x when DoorPattern.IsMatch(mapItem.ToString()):
                            if (node.Data.HasKey(char.Parse(x.ToLowerInvariant())))
                            {
                                neighbors.Add(node.Data.NewLocation(potentialNeighbor));
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unknown map item");

                    }
                }
            }
            return neighbors;
        }

        private static readonly Regex DoorPattern = new Regex(@"[A-Z]");
        private static readonly Regex KeyPattern = new Regex(@"[a-z]");

        private static Node<LocationWithKey> BreadthFirst(
            Node<LocationWithKey> start,
            Func<Node<LocationWithKey>, Dictionary<Point, char>, IEnumerable<LocationWithKey>> neighborFetcher,
            Dictionary<Point, char> map,
            int keyCount)
        {
            var visited = new HashSet<LocationWithKey>();

            var queue = new Queue<Node<LocationWithKey>>();
            Node<LocationWithKey> terminationNode = null;
            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();

                if (!visited.Add(current.Data))
                {
                    continue;
                }

                if (current.Data.KeySet.Count == keyCount)
                {
                    terminationNode = current;
                    break;
                }


                var neighbors = neighborFetcher(current, map).Where(n => !visited.Contains(n)).ToList();

                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(new Node<LocationWithKey>(neighbor, current.Depth + 1, current));
                }
            }

            return terminationNode;
        }

        private int StepsInShortestPath(Dictionary<Point, char> map)
        {
            var keys = new HashSet<string>();

            var start = map.Single(p => p.Value == '@').Key;
            var keyCount = map.Values.Count(char.IsLower);
            var startLocation = new LocationWithKey {Location = start};
            var startNode = new Node<LocationWithKey>(startLocation, 0);
            var terminationNode = BreadthFirst(startNode, Neighbors, map, keyCount);
             return terminationNode.Depth;
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
            Assert.Equal(4270, FirstStar());
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
