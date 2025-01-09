using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2021
{
    // --- Day 23: Amphipod ---
    public class Day23
    {
        private readonly ITestOutputHelper output;

        public Day23(ITestOutputHelper output)
        {
            this.output = output;
        }

        // #####################################
        // # 0 1      2      3      4      5 6 #
        // ######  8 ### 10 ### 12 ### 14 ######
        //      #  7 # #  9 # # 11 # # 13 #
        //      ###### ###### ###### ######

        private readonly static int RoomOffset = 7;
        private readonly static List<(int from, int to)> Gaps = [(1, 2), (2, 3), (3, 4), (4, 5)];
        private readonly static List<char> PodTypes = ['A', 'B', 'C', 'D'];

        private enum Moving { Left, Right }

        private readonly static Dictionary<(int, Moving), List<int>> OutPaths = new();
        private readonly static Dictionary<(char, int), List<int>> HomePaths = new();

        private readonly static HashSet<(int, int)> HiddenSteps = new();

        private static bool IsInHallway(int position) => position < RoomOffset;

        private static (int start, int end) RoomIndeces(bool extended, char pod) => pod switch
        {
            'A' => extended ? (7, 10) : (7, 8),
            'B' => extended ? (11, 14) : (9, 10),
            'C' => extended ? (15, 18) : (11, 12),
            'D' => extended ? (19, 22) : (13, 14),
            _ => throw new ArgumentOutOfRangeException(nameof(pod)),
        };

        private static int EnergyCost(char pod) => pod switch
        {
            'A' => 1,
            'B' => 10,
            'C' => 100,
            'D' => 1000,
            _ => throw new ArgumentOutOfRangeException(nameof(pod)),
        };
        private static int StepEnergyCost(char pod, int from, int to)
        {
            if (HiddenSteps.Contains((from, to)))
            {
                return 2 * EnergyCost(pod);
            }

            return EnergyCost(pod);
        }

        private static void InitializeHiddenSteps(bool extended)
        {
            (int, int) Swap((int, int) tuple) => (tuple.Item2, tuple.Item1);

            foreach (var gap in Gaps)
            {
                HiddenSteps.Add(gap);
                HiddenSteps.Add(Swap(gap));
            }

            List<(char, int left, int right)> pods = PodTypes.Zip(Gaps, ((pod, gap) => (pod, gap.from, gap.to))).ToList();

            foreach ((char pod, int left, int right) cnx in pods)
            {
                var left = (RoomIndeces(extended, cnx.pod).end, cnx.left);
                var right = (RoomIndeces(extended, cnx.pod).end, cnx.right);

                HiddenSteps.Add(left);
                HiddenSteps.Add(Swap(left));
                HiddenSteps.Add(right);
                HiddenSteps.Add(Swap(right));
            }
        }

        private static void InitializePaths(bool extended)
        {
            HomePaths.Clear();

            foreach ((char pod, int i) in PodTypes.Select((pod, i) => (pod, i)))
            {
                for (int startPosition = 0; startPosition < RoomOffset; startPosition++)
                {
                    var gap = Gaps[i];

                    List<int> path = new();

                    HomePaths.Add((pod, startPosition), path);

                    if (startPosition < gap.to)
                    {
                        // walking right in hallway
                        for (int pos = startPosition + 1; pos < gap.to; pos++)
                        {
                            path.Add(pos);
                        }

                        // walking down in room
                        var r = RoomIndeces(extended, pod);

                        for (int room = r.end; room >= r.start; room--)
                        {
                            path.Add(room);
                        }
                    }
                    else
                    {
                        // walking left in hallway
                        for (int pos = startPosition - 1; pos > gap.from; pos--)
                        {
                            path.Add(pos);
                        }

                        // walking down in room
                        var r = RoomIndeces(extended, pod);

                        for (int room = r.end; room >= r.start; room--)
                        {
                            path.Add(room);
                        }
                    }
                }
            }

            OutPaths.Clear();

            foreach ((char pod, int i) in PodTypes.Select((pod, i) => (pod, i)))
            {
                var r = RoomIndeces(extended, pod);

                var gap = Gaps[i];

                for (int room = r.start; room <= r.end; room++)
                {

                    List<int> leftPath = new();
                    List<int> rightPath = new();

                    OutPaths.Add((room, Moving.Left), leftPath);
                    OutPaths.Add((room, Moving.Right), rightPath);

                    // walk up in room
                    for (int rnext = room + 1; rnext <= r.end; rnext++)
                    {
                        leftPath.Add(rnext);
                        rightPath.Add(rnext);
                    }

                    // walking left from room
                    for (int left = gap.from; left >= 0; left--)
                    {
                        leftPath.Add(left);
                    }

                    // walking right from room
                    for (int right = gap.to; right < RoomOffset; right++)
                    {
                        rightPath.Add(right);
                    }
                }
            }
        }

        private class State : IEquatable<State>
        {
            private string _encoded;

            public State(int a1, int a2, int b1, int b2, int c1, int c2, int d1, int d2)
            {
                StringBuilder sb = new("...............");

                sb[a1] = 'A';
                sb[a2] = 'A';
                sb[b1] = 'B';
                sb[b2] = 'B';
                sb[c1] = 'C';
                sb[c2] = 'C';
                sb[d1] = 'D';
                sb[d2] = 'D';

                _encoded = sb.ToString();
            }

            private State(string encoded)
            {
                _encoded = encoded;
            }

            public static State Decode(string encoded)
            {
                return new State(encoded);
            }

            // problem 2 extension
            public State Extend()
            {
                StringBuilder sb = new StringBuilder(_encoded);

                sb.Insert(8, 'D');
                sb.Insert(9, 'D');
                sb.Insert(12, 'B');
                sb.Insert(13, 'C');
                sb.Insert(16, 'A');
                sb.Insert(17, 'B');
                sb.Insert(20, 'C');
                sb.Insert(21, 'A');

                return new State(sb.ToString());
            }

            public bool Equals(State other)
            {
                if (other is null) { return false; }
                if (ReferenceEquals(this, other)) { return true; }
                if (GetType() != other.GetType()) { return false; }

                return _encoded.Equals(other._encoded);
            }

            public override bool Equals(object obj) => Equals(obj as State);

            public override int GetHashCode() => _encoded.GetHashCode();

            public override string ToString()
            {
                return _encoded;
            }
            public bool IsComplete => Extended ?
                _encoded == ".......AAAABBBBCCCCDDDD" :
                _encoded == ".......AABBCCDD";

            public bool IsInHomePosition(char pod, int position)
            {
                (int start, int end) = RoomIndeces(Extended, pod);

                if (position < start || position > end)
                {
                    return false;
                }

                int roomsBehind = position - start;

                return Rooms(pod).Substring(0, roomsBehind).All(c => pod == c);
            }

            public bool IsOccupied(int position) => _encoded[position] != '.';

            public List<(State, int)> Neighbors
            {
                get
                {
                    List<(State, int)> ns = new();

                    void AddPaths(char pod, int position, int podnr)
                    {
                        if (IsInHallway(position))
                        {
                            if (CanMoveHome(pod))
                            {
                                var path = HomePaths[(pod, position)];
                                var energy = 0;
                                int previous = position;
                                while (path.Count > 0 && !IsOccupied(path.First()))
                                {
                                    int next = path.First();

                                    energy += StepEnergyCost(pod, previous, next);

                                    if (IsInHomePosition(pod, next))
                                    {
                                        ns.Add((MoveTo(pod, podnr, next), energy));
                                    }

                                    path = path[1..];
                                    previous = next;
                                }
                            }
                        }
                        else if (!IsInHomePosition(pod, position))
                        {
                            var leftPath = OutPaths[(position, Moving.Left)];
                            var energy = 0;
                            int previous = position;
                            while (leftPath.Count > 0 && !IsOccupied(leftPath.First()))
                            {
                                int next = leftPath.First();

                                energy += StepEnergyCost(pod, previous, next);

                                if (IsInHallway(next))
                                {
                                    ns.Add((MoveTo(pod, podnr, next), energy));
                                }

                                leftPath = leftPath[1..];
                                previous = next;
                            }

                            var rightPath = OutPaths[(position, Moving.Right)];
                            energy = 0;
                            previous = position;
                            while (rightPath.Count > 0 && !IsOccupied(rightPath.First()))
                            {
                                int next = rightPath.First();

                                energy += StepEnergyCost(pod, previous, next);

                                if (IsInHallway(next))
                                {
                                    ns.Add((MoveTo(pod, podnr, next), energy));
                                };

                                rightPath = rightPath[1..];
                                previous = next;
                            }
                        }
                    }

                    foreach ((int i, char pod, int position) in Pods.Select((state, i) => (i, state.pod, state.position)))
                    {
                        AddPaths(pod, position, i % RoomCount);
                    }

                    return ns;
                }
            }

            private bool Extended => _encoded.Length != 15;

            private List<(char pod, int position)> Pods
            {
                get
                {
                    List<(char pod, int position)> pods = [];

                    for (int i = 0; i < _encoded.Length; i++)
                    {
                        if (_encoded[i] == '.') { continue; }

                        pods.Add((_encoded[i], i));
                    }

                    return pods.OrderBy(p => p.pod).ThenBy(p => p.position).ToList();
                }
            }

            private int RoomCount => Extended ? 4 : 2;

            private string Rooms(char pod) => _encoded.Substring(RoomIndeces(Extended, pod).start, RoomCount);

            private bool CanMoveHome(char pod)
            {
                for (int i = 0; i < RoomCount; i++)
                {
                    string pattern = new string(pod, i) + new string('.', RoomCount - i);

                    if (Rooms(pod) == pattern)
                    {
                        return true;
                    }
                }
                return false;
            }

            private State MoveTo(char pod, int podnr, int position)
            {
                StringBuilder encoded = new StringBuilder(_encoded);

                int current = -1;

                for (int i = 0; i <= podnr; i++)
                {
                    current = _encoded.IndexOf(pod, current + 1);
                }

                encoded[current] = '.';
                encoded[position] = Convert.ToChar(pod);

                return new State(encoded.ToString());
            }
        }

        private static State Parse(IList<string> input)
        {
            List<(int position, char pod)> initialState =
                [
                    (8, input[2][3]),
                    (7, input[3][3]),
                    (10, input[2][5]),
                    (9, input[3][5]),
                    (12, input[2][7]),
                    (11, input[3][7]),
                    (14, input[2][9]),
                    (13, input[3][9]),
                ];

            List<int> s = initialState.OrderBy(x => x.pod).Select(x => x.position).ToList();

            return new State(s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7]);
        }

        private static int Solve(State startState, bool extended = false)
        {
            InitializeHiddenSteps(extended);
            InitializePaths(extended);

            var queue = new PriorityQueue<State, int>();

            queue.Enqueue(startState, 0);

            var distances = new Dictionary<State, int>
            {
                { startState, 0 }
            };

            var prev = new Dictionary<State, State>();

            int Distance(State state)
            {
                if (distances.TryGetValue(state, out int distance))
                {
                    return distances[state];
                }
                else
                {
                    return int.MaxValue;
                }
            }

            // djikstra https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var neighbor in current.Neighbors)
                {
                    (var pos, int cost) = neighbor;

                    var alt = Distance(current) + cost;

                    if (alt < Distance(pos))
                    {
                        prev[pos] = current;
                        distances[pos] = alt;
                        queue.Enqueue(pos, alt);
                    }
                }
            }

            return distances.Where(kvp => kvp.Key.IsComplete).Select(kvp => kvp.Value).Min();
        }

        private static int Problem1(IList<string> input)
        {
            State startState = Parse(input);

            return Solve(startState);
        }

        private static int Problem2(IList<string> input)
        {
            State startState = Parse(input);

            startState = startState.Extend();

            return Solve(startState, extended: true);
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(14460, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(41366, Problem2(input));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(12521, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(44169, Problem2(exampleInput));
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(5, false)]
        [InlineData(6, false)]
        [InlineData(7, true)]
        [InlineData(8, true)]
        [InlineData(9, true)]
        [InlineData(10, true)]
        [InlineData(11, true)]
        [InlineData(12, true)]
        [InlineData(13, true)]
        [InlineData(14, true)]
        [Trait("Event", "2021")]
        public void IsOccupiedTest(int position, bool expected)
        {
            var exampleInput = ReadExampleLineInput("Example");

            var startState = Parse(exampleInput);

            Assert.Equal(expected, startState.IsOccupied(position));
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(5, true)]
        [InlineData(6, true)]
        [InlineData(7, false)]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, false)]
        [InlineData(11, false)]
        [InlineData(12, false)]
        [InlineData(13, false)]
        [InlineData(14, false)]
        [Trait("Event", "2021")]
        public void IsInHallwayTest(int position, bool expected)
        {
            var exampleInput = ReadExampleLineInput("Example");

            var startState = Parse(exampleInput);

            Assert.Equal(expected, IsInHallway(position));
        }

        [Theory]
        [InlineData(".......AA.......", 'A', 7, true)]
        [InlineData(".......AA.......", 'A', 8, true)]
        [InlineData(".......AB.......", 'A', 7, true)]
        [InlineData(".......BA.......", 'A', 8, false)]
        [Trait("Event", "2021")]
        public void IsInCorrectRoomTest(string encodedState, char pod, int position, bool expected)
        {
            var state = State.Decode(encodedState);

            Assert.Equal(expected, state.IsInHomePosition(pod, position));
        }
    }
}
