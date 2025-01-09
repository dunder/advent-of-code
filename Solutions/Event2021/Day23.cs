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

        // ###########################################
        // # H1 H2     H3      H4      H5      H6 H7 #
        // ####### A2 #### B2 #### C2 #### D2 ########
        //       # A1 #  # B1 #  # C1 #  # D1 #
        //       ######  ######  ######  ######

        // #####################################
        // # 0 1      2      3      4      5 6 #
        // ######  8 ### 10 ### 12 ### 14 ######
        //      #  7 # #  9 # # 11 # # 13 #
        //      ###### ###### ###### ######

        private class State : IEquatable<State>
        {
            private readonly static int RoomOffset = 7;
            private readonly static List<(int from, int to)> Gaps = [(1, 2), (2, 3), (3, 4), (4, 5)];
            private readonly static List<char> PodTypes = ['A', 'B', 'C', 'D'];

            public Dictionary<(int, Moving), List<int>> outPaths = new();
            public Dictionary<(char, int), List<int>> homePaths = new();

            private readonly HashSet<(int, int)> hiddenSteps = new();

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

                InitializeHiddenSteps();
                InitializePaths();
            }

            private State(string encoded)
            {
                _encoded = encoded;

                InitializeHiddenSteps();
                InitializePaths();
            }

            private void InitializeHiddenSteps()
            {
                (int, int) Swap((int, int) tuple) => (tuple.Item2, tuple.Item1);

                foreach (var gap in Gaps)
                {
                    hiddenSteps.Add(gap);
                    hiddenSteps.Add(Swap(gap));
                }

                List<(char, int left, int right)> pods = PodTypes.Zip(Gaps, ((pod, gap) => (pod, gap.from, gap.to))).ToList();

                foreach ((char pod, int left, int right) cnx in pods)
                {
                    var left = (RoomIndeces(cnx.pod).end, cnx.left);
                    var right = (RoomIndeces(cnx.pod).end, cnx.right);

                    hiddenSteps.Add(left);
                    hiddenSteps.Add(Swap(left));
                    hiddenSteps.Add(right);
                    hiddenSteps.Add(Swap(right));
                }
            }

            private void InitializePaths()
            {
                foreach ((char pod, int i) in PodTypes.Select((pod, i) => (pod, i)))
                {
                    for (int startPosition = 0; startPosition < RoomOffset; startPosition++)
                    {
                        var gap = Gaps[i];

                        List<int> path = new();

                        homePaths.Add((pod, startPosition), path);

                        if (startPosition < gap.to)
                        {
                            // walking right in hallway
                            for (int pos = startPosition + 1; pos < gap.to; pos++)
                            {
                                path.Add(pos);
                            }

                            // walking down in room
                            var r = RoomIndeces(pod);

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
                            var r = RoomIndeces(pod);

                            for (int room = r.end; room >= r.start; room--)
                            {
                                path.Add(room);
                            }
                        }
                    }
                }

                foreach ((char pod, int i) in PodTypes.Select((pod, i) => (pod, i)))
                {
                    var r = RoomIndeces(pod);

                    var gap = Gaps[i];

                    for (int room = r.start; room <= r.end; room++)
                    {
                        
                        List<int> leftPath = new();
                        List<int> rightPath = new();

                        outPaths.Add((room, Moving.Left), leftPath);
                        outPaths.Add((room, Moving.Right), rightPath);

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

            public static State Decode(string encoded)
            {
                return new State(encoded);
            }

            public bool Extended => _encoded.Length != 15;

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

            public bool IsOccupied(int position) => _encoded[position] != '.';

            public static bool IsInHallway(int position) => position < RoomOffset;

            public bool CanMoveHome(char pod)
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

            public bool IsInCorrectRoom(char pod, int position)
            {
                (int start, int end) = RoomIndeces(pod);

                if (position < start || position > end)
                {
                    return false;
                }

                int roomsBehind = position - start;

                return Rooms(pod).Substring(0, roomsBehind).All(c => pod == c);
            }

            public State MoveTo(char pod, int podnr, int position)
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

            public List<(char pod, int position)> Pods
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

            public int StepEnergyCost(char pod, int from, int to)
            {
                if (hiddenSteps.Contains((from, to)))
                {
                    return 2 * EnergyCost(pod);
                }

                return EnergyCost(pod);
            }

            public int RoomCount => Extended ? 4 : 2;


            private (int start, int end) RoomIndeces(char pod) => pod switch
            {
                'A' => Extended ? (7, 10) : (7, 8),
                'B' => Extended ? (11, 14) : (9, 10),
                'C' => Extended ? (15, 18) : (11, 12),
                'D' => Extended ? (19, 22) : (13, 14),
                _ => throw new ArgumentOutOfRangeException(nameof(pod)),
            };

            private string Rooms(char pod) => _encoded.Substring(RoomIndeces(pod).start, RoomCount);

            public bool IsComplete => Extended ?
                _encoded == ".......AAAABBBBCCCCDDDD" :
                _encoded == ".......AABBCCDD";
        }

        private enum Moving { Left, Right }

        public enum Pod { A, B, C, D }

        //private static Dictionary<(char, int), List<int>> homePaths = new()
        //{
        //    { ('A', 0), [ 1, 8, 7 ] },
        //    { ('A', 1), [ 8, 7 ] },
        //    { ('A', 2), [ 8, 7 ] },
        //    { ('A', 3), [ 2, 8, 7 ] },
        //    { ('A', 4), [ 3, 2, 8, 7 ] },
        //    { ('A', 5), [ 4, 3, 2, 8, 7 ] },
        //    { ('A', 6), [ 5, 4, 3, 2, 8, 7 ] },

        //    { ('B', 0), [ 1, 2, 10, 9 ] },
        //    { ('B', 1), [ 2, 10, 9 ] },
        //    { ('B', 2), [ 10, 9 ] },
        //    { ('B', 3), [ 10, 9 ] },
        //    { ('B', 4), [ 3, 10, 9 ] },
        //    { ('B', 5), [ 4, 3, 10, 9 ] },
        //    { ('B', 6), [ 5, 4, 3, 10, 9 ] },

        //    { ('C', 0), [ 1, 2, 3, 12, 11 ] },
        //    { ('C', 1), [ 2, 3, 12, 11 ] },
        //    { ('C', 2), [ 3, 12, 11 ] },
        //    { ('C', 3), [ 12, 11 ] },
        //    { ('C', 4), [ 12, 11 ] },
        //    { ('C', 5), [ 4, 12, 11 ] },
        //    { ('C', 6), [ 5, 4, 12, 11 ] },

        //    { ('D', 0), [ 1, 2, 3, 4, 14, 13 ] },
        //    { ('D', 1), [ 2, 3, 4, 14, 13 ] },
        //    { ('D', 2), [ 3, 4, 14, 13 ] },
        //    { ('D', 3), [ 4, 14, 13 ] },
        //    { ('D', 4), [ 14, 13 ] },
        //    { ('D', 5), [ 14, 13 ] },
        //    { ('D', 6), [ 5, 14, 13 ] },
        //};

        //private static Dictionary<(char, int), List<int>> extendedHomePaths = new()
        //{
        //    { ('A', 0), [ 1, 10, 9, 8, 7 ] },
        //    { ('A', 1), [ 10, 9, 8, 7 ] },
        //    { ('A', 2), [ 10, 9, 8, 7 ] },
        //    { ('A', 3), [ 2, 10, 9, 8, 7 ] },
        //    { ('A', 4), [ 3, 2, 10, 9, 8, 7 ] },
        //    { ('A', 5), [ 4, 3, 2, 10, 9, 8, 7 ] },
        //    { ('A', 6), [ 5, 4, 3, 2, 10, 9, 8, 7 ] },

        //    { ('B', 0), [ 1, 2, 14, 13, 12, 11 ] },
        //    { ('B', 1), [ 2, 14, 13, 12, 11 ] },
        //    { ('B', 2), [ 14, 13, 12, 11 ] },
        //    { ('B', 3), [ 14, 13, 12, 11 ] },
        //    { ('B', 4), [ 3, 14, 13, 12, 11 ] },
        //    { ('B', 5), [ 4, 3, 14, 13, 12, 11 ] },
        //    { ('B', 6), [ 5, 4, 3, 14, 13, 12, 11 ] },

        //    { ('C', 0), [ 1, 2, 3, 18, 17, 16, 15 ] },
        //    { ('C', 1), [ 2, 3, 18, 17, 16, 15 ] },
        //    { ('C', 2), [ 3, 18, 17, 16, 15 ] },
        //    { ('C', 3), [ 18, 17, 16, 15 ] },
        //    { ('C', 4), [ 18, 17, 16, 15 ] },
        //    { ('C', 5), [ 4, 18, 17, 16, 15 ] },
        //    { ('C', 6), [ 5, 4, 18, 17, 16, 15 ] },

        //    { ('D', 0), [ 1, 2, 3, 4, 22, 21, 20, 19 ] },
        //    { ('D', 1), [ 2, 3, 4, 22, 21, 20, 19 ] },
        //    { ('D', 2), [ 3, 4, 22, 21, 20, 19 ] },
        //    { ('D', 3), [ 4, 22, 21, 20, 19 ] },
        //    { ('D', 4), [ 22, 21, 20, 19 ] },
        //    { ('D', 5), [ 22, 21, 20, 19 ] },
        //    { ('D', 6), [ 5, 22, 21, 20, 19 ] },
        //};

        //private static Dictionary<(int, Moving), List<int>> outPaths = new()
        //{
        //    { (7, Moving.Left), [ 8, 1, 0] },
        //    { (8, Moving.Left), [ 1, 0 ] },
        //    { (7, Moving.Right), [ 8, 2, 3, 4, 5, 6 ] },
        //    { (8, Moving.Right), [ 2, 3, 4, 5, 6 ] },

        //    { (9, Moving.Left), [ 10, 2, 1, 0] },
        //    { (10, Moving.Left), [ 2, 1, 0] },
        //    { (9, Moving.Right), [ 10, 3, 4, 5, 6 ] },
        //    { (10, Moving.Right), [ 3, 4, 5, 6 ] },

        //    { (11, Moving.Left), [ 12, 3, 2, 1, 0] },
        //    { (12, Moving.Left), [ 3, 2, 1, 0] },
        //    { (11, Moving.Right), [ 12, 4, 5, 6 ] },
        //    { (12, Moving.Right), [ 4, 5, 6 ] },

        //    { (13, Moving.Left), [ 14, 4, 3, 2, 1, 0] },
        //    { (14, Moving.Left), [ 4, 3, 2, 1, 0] },
        //    { (13, Moving.Right), [ 14, 5, 6 ] },
        //    { (14, Moving.Right), [ 5, 6 ] },
        //};

        //private static Dictionary<(int, Moving), List<int>> extendedOutPaths = new()
        //{
        //    { (7, Moving.Left), [ 8, 9, 10, 1, 0] },
        //    { (8, Moving.Left), [ 9, 10, 1, 0 ] },
        //    { (9, Moving.Left), [ 10, 1, 0 ] },
        //    { (10, Moving.Left), [ 1, 0 ] },
        //    { (7, Moving.Right), [ 8, 9, 10, 2, 3, 4, 5, 6 ] },
        //    { (8, Moving.Right), [ 9, 10, 2, 3, 4, 5, 6 ] },
        //    { (9, Moving.Right), [ 10, 2, 3, 4, 5, 6 ] },
        //    { (10, Moving.Right), [ 2, 3, 4, 5, 6 ] },

        //    { (11, Moving.Left), [ 12, 13, 14, 2, 1, 0] },
        //    { (12, Moving.Left), [ 13, 14, 2, 1, 0] },
        //    { (13, Moving.Left), [ 14, 2, 1, 0] },
        //    { (14, Moving.Left), [ 2, 1, 0] },
        //    { (11, Moving.Right), [ 12, 13, 14, 3, 4, 5, 6 ] },
        //    { (12, Moving.Right), [ 13, 14, 3, 4, 5, 6 ] },
        //    { (13, Moving.Right), [ 14, 3, 4, 5, 6 ] },
        //    { (14, Moving.Right), [ 3, 4, 5, 6 ] },

        //    { (15, Moving.Left), [ 16, 17, 18, 3, 2, 1, 0] },
        //    { (16, Moving.Left), [ 17, 18, 3, 2, 1, 0] },
        //    { (17, Moving.Left), [ 17, 3, 2, 1, 0] },
        //    { (18, Moving.Left), [ 3, 2, 1, 0] },
        //    { (15, Moving.Right), [ 16, 17, 18, 4, 5, 6 ] },
        //    { (16, Moving.Right), [ 17, 18, 4, 5, 6 ] },
        //    { (17, Moving.Right), [ 18, 4, 5, 6 ] },
        //    { (18, Moving.Right), [ 4, 5, 6 ] },

        //    { (19, Moving.Left), [ 20, 21, 22, 4, 3, 2, 1, 0] },
        //    { (20, Moving.Left), [ 21, 22, 4, 3, 2, 1, 0] },
        //    { (21, Moving.Left), [ 22, 4, 3, 2, 1, 0] },
        //    { (22, Moving.Left), [ 4, 3, 2, 1, 0] },
        //    { (19, Moving.Right), [ 20, 21, 22, 5, 6 ] },
        //    { (20, Moving.Right), [ 21, 22, 5, 6 ] },
        //    { (21, Moving.Right), [ 22, 5, 6 ] },
        //    { (22, Moving.Right), [ 5, 6 ] },
        //};

        private static int EnergyCost(char pod) => pod switch
        {
            'A' => 1,
            'B' => 10,
            'C' => 100,
            'D' => 1000,
            _ => throw new ArgumentOutOfRangeException(nameof(pod)),
        };

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

        private static List<(State, int)> Neighbors(State state)
        {
            List<(State, int)> ns = new();

            void AddPaths(char pod, int position, int podnr)
            {
                if (State.IsInHallway(position))
                {
                    if (state.CanMoveHome(pod))
                    {
                        //var path = state.Extended ? extendedHomePaths[(pod, position)] : homePaths[(pod, position)];
                        var path = state.homePaths[(pod, position)];
                        var energy = 0;
                        int previous = position;
                        while (path.Count > 0 && !state.IsOccupied(path.First()))
                        {
                            int next = path.First();

                            energy += state.StepEnergyCost(pod, previous, next);
                            
                            if (state.IsInCorrectRoom(pod, next))
                            {
                                ns.Add((state.MoveTo(pod, podnr, next), energy));
                            }

                            path = path[1..];
                            previous = next;
                        }
                    }
                }
                else if (!state.IsInCorrectRoom(pod, position))
                {
                    //var leftPath = state.Extended ? extendedOutPaths[(position, Moving.Left)] : outPaths[(position, Moving.Left)];
                    var leftPath = state.outPaths[(position, Moving.Left)];
                    var energy = 0;
                    int previous = position;
                    while (leftPath.Count > 0 && !state.IsOccupied(leftPath.First()))
                    {
                        int next = leftPath.First();

                        energy += state.StepEnergyCost(pod, previous, next);

                        if (State.IsInHallway(next))
                        {
                            ns.Add((state.MoveTo(pod, podnr, next), energy));
                        }
                        
                        leftPath = leftPath[1..];
                        previous = next;
                    }

                    //var rightPath = state.Extended ? extendedOutPaths[(position, Moving.Right)] : outPaths[(position, Moving.Right)];
                    var rightPath = state.outPaths[(position, Moving.Right)];
                    energy = 0;
                    previous = position;
                    while (rightPath.Count > 0 && !state.IsOccupied(rightPath.First()))
                    {
                        int next = rightPath.First();

                        energy += state.StepEnergyCost(pod, previous, next);

                        if (State.IsInHallway(next))
                        {
                            ns.Add((state.MoveTo(pod, podnr, next), energy));
                        };
                        
                        rightPath = rightPath[1..];
                        previous = next;
                    }
                }
            }

            foreach ((int i, char pod, int position) in state.Pods.Select((state, i) => (i, state.pod, state.position)))
            {
                AddPaths(pod, position, i % state.RoomCount);
            }

            return ns;
        }

        private static int Solve(State startState)
        {
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

                foreach (var neighbor in Neighbors(current))
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

            return Solve(startState);
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

            Assert.Equal(-1, Problem2(input));
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

            Assert.Equal(expected, State.IsInHallway(position));
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

            Assert.Equal(expected, state.IsInCorrectRoom(pod, position));
        }
    }
}
