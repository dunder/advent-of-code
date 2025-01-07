using System;
using System.Collections.Generic;
using System.IO;
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

        public enum P
        {
            H1 = 0,
            H2,
            H3,
            H4,
            H5,
            H6,
            H7,
            A1,
            A2,
            B1,
            B2,
            C1,
            C2,
            D1,
            D2,
        }

        private class State : IEquatable<State>
        {
            private string _encoded;

            public State(P a1, P a2, P b1, P b2, P c1, P c2, P d1, P d2)
            {
                StringBuilder sb = new("...............");

                sb[(int)a1] = 'A';
                sb[(int)a2] = 'A';
                sb[(int)b1] = 'B';
                sb[(int)b2] = 'B';
                sb[(int)c1] = 'C';
                sb[(int)c2] = 'C';
                sb[(int)d1] = 'D';
                sb[(int)d2] = 'D';

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

            public bool IsOccupied(P position) => _encoded[(int)position] != '.';

            public static bool IsInHallway(P position) => (int)position < 7;

            // only checks if it is feasable to stop at the position when moving home, does not consider if 
            // there are any occupied locations in the path, just checks if the position is a home position
            // so A1 is always a valid home for A, but A2 is only valid if A1 is occupied by an A
            public bool IsPossibleHomeMove(Pod pod, P position) => pod switch
            {
                Pod.A => (int)position == 8 && _encoded[7] == 'A' || (int)position == 7,
                Pod.B => (int)position == 10 && _encoded[9] == 'B' || (int)position == 9,
                Pod.C => (int)position == 12 && _encoded[11] == 'C' || (int)position == 11,
                Pod.D => (int)position == 14 && _encoded[13] == 'D' || (int)position == 13,
                _ => throw new ArgumentOutOfRangeException(nameof(position)),
            };

            public bool CanMoveHome(Pod pod) => Rooms(pod) == ".." || Rooms(pod) == $"{Letter(pod)}.";

            public bool IsInCorrectRoom(Pod pod, P position) => pod switch
            {
                Pod.A => (int)position == 7 || ((int)position == 8 && _encoded[7] == 'A'),
                Pod.B => (int)position == 9 || ((int)position == 10 && _encoded[9] == 'B'),
                Pod.C => (int)position == 11 || ((int)position == 12 && _encoded[11] == 'C'),
                Pod.D => (int)position == 13 || ((int)position == 14 && _encoded[13] == 'D'),
                _ => throw new ArgumentOutOfRangeException(nameof(position)),
            };

            public State MoveTo(Pod pod, bool first, P position)
            {
                StringBuilder encoded = new StringBuilder(_encoded);

                int current = first ? _encoded.IndexOf(Letter(pod)) : _encoded.LastIndexOf(Letter(pod));

                encoded[current] = '.';
                encoded[(int)position] = Convert.ToChar(Letter(pod));

                return new State(encoded.ToString());
            }

            public List<(Pod pod, P position)> Pods => 
                [
                    (Pod.A, (P)_encoded.IndexOf("A")),
                    (Pod.A, (P)_encoded.LastIndexOf("A")),
                    (Pod.B, (P)_encoded.IndexOf("B")),
                    (Pod.B, (P)_encoded.LastIndexOf("B")),
                    (Pod.C, (P)_encoded.IndexOf("C")),
                    (Pod.C, (P)_encoded.LastIndexOf("C")),
                    (Pod.D, (P)_encoded.IndexOf("D")),
                    (Pod.D, (P)_encoded.LastIndexOf("D")),
                ];

            private string Letter(Pod pod) => pod switch
            {
                Pod.A => "A",
                Pod.B => "B",
                Pod.C => "C",
                Pod.D => "D",
                _ => throw new ArgumentOutOfRangeException(nameof(pod)),
            };

            private string Rooms(Pod pod) => pod switch
            {
                Pod.A => _encoded.Substring(7, 2),
                Pod.B => _encoded.Substring(9, 2),
                Pod.C => _encoded.Substring(11, 2),
                Pod.D => _encoded.Substring(13, 2),
                _ => throw new ArgumentOutOfRangeException(nameof(pod)),
            };

            public bool IsComplete => _encoded == ".......AABBCCDD";
        }

        private enum Moving { Left, Right }

        public enum Pod { A, B, C, D }

        private static Dictionary<(Pod, P), List<P>> homePaths = new()
        {
            { (Pod.A, P.H1), [ P.H2, P.A2, P.A1 ] },
            { (Pod.A, P.H2), [ P.A2, P.A1 ] },
            { (Pod.A, P.H3), [ P.A2, P.A1 ] },
            { (Pod.A, P.H4), [ P.H3, P.A2, P.A1 ] },
            { (Pod.A, P.H5), [ P.H4, P.H3, P.A2, P.A1 ] },
            { (Pod.A, P.H6), [ P.H5, P.H4, P.H3, P.A2, P.A1 ] },
            { (Pod.A, P.H7), [ P.H6, P.H5, P.H4, P.H3, P.A2, P.A1 ] },

            { (Pod.B, P.H1), [ P.H2, P.H3, P.B2, P.B1 ] },
            { (Pod.B, P.H2), [ P.H3, P.B2, P.B1 ] },
            { (Pod.B, P.H3), [ P.B2, P.B1 ] },
            { (Pod.B, P.H4), [ P.B2, P.B1 ] },
            { (Pod.B, P.H5), [ P.H4, P.B2, P.B1 ] },
            { (Pod.B, P.H6), [ P.H5, P.H4, P.B2, P.B1 ] },
            { (Pod.B, P.H7), [ P.H6, P.H5, P.H4, P.B2, P.B1 ] },

            { (Pod.C, P.H1), [ P.H2, P.H3, P.H4, P.C2, P.C1 ] },
            { (Pod.C, P.H2), [ P.H3, P.H4, P.C2, P.C1 ] },
            { (Pod.C, P.H3), [ P.H4, P.C2, P.C1 ] },
            { (Pod.C, P.H4), [ P.C2, P.C1 ] },
            { (Pod.C, P.H5), [ P.C2, P.C1 ] },
            { (Pod.C, P.H6), [ P.H5, P.C2, P.C1 ] },
            { (Pod.C, P.H7), [ P.H6, P.H5, P.C2, P.C1 ] },

            { (Pod.D, P.H1), [ P.H2, P.H3, P.H4, P.H5, P.D2, P.D1 ] },
            { (Pod.D, P.H2), [ P.H3, P.H4, P.H5, P.D2, P.D1 ] },
            { (Pod.D, P.H3), [ P.H4, P.H5, P.D2, P.D1 ] },
            { (Pod.D, P.H4), [ P.H5, P.D2, P.D1 ] },
            { (Pod.D, P.H5), [ P.D2, P.D1 ] },
            { (Pod.D, P.H6), [ P.D2, P.D1 ] },
            { (Pod.D, P.H7), [ P.H6, P.D2, P.D1 ] },
        };

        private static Dictionary<(P, Moving), List<P>> outPaths = new()
        {
            { (P.A1, Moving.Left), [ P.A2, P.H2, P.H1] },
            { (P.A2, Moving.Left), [ P.H2, P.H1 ] },
            { (P.A1, Moving.Right), [ P.A2, P.H3, P.H4, P.H5, P.H6, P.H7 ] },
            { (P.A2, Moving.Right), [ P.H3, P.H4, P.H5, P.H6, P.H7 ] },

            { (P.B1, Moving.Left), [ P.B2, P.H3, P.H2, P.H1] },
            { (P.B2, Moving.Left), [ P.H3, P.H2, P.H1] },
            { (P.B1, Moving.Right), [ P.B2, P.H4, P.H5, P.H6, P.H7 ] },
            { (P.B2, Moving.Right), [ P.H4, P.H5, P.H6, P.H7 ] },

            { (P.C1, Moving.Left), [ P.C2, P.H4, P.H3, P.H2, P.H1] },
            { (P.C2, Moving.Left), [ P.H4, P.H3, P.H2, P.H1] },
            { (P.C1, Moving.Right), [ P.C2, P.H5, P.H6, P.H7 ] },
            { (P.C2, Moving.Right), [ P.H5, P.H6, P.H7 ] },

            { (P.D1, Moving.Left), [ P.D2, P.H5, P.H4, P.H3, P.H2, P.H1] },
            { (P.D2, Moving.Left), [ P.H5, P.H4, P.H3, P.H2, P.H1] },
            { (P.D1, Moving.Right), [ P.D2, P.H6, P.H7 ] },
            { (P.D2, Moving.Right), [ P.H6, P.H7 ] },
        };

        private static int EnergyCost(Pod pod) => pod switch
        {
            Pod.A => 1,
            Pod.B => 10,
            Pod.C => 100,
            Pod.D => 1000,
            _ => throw new ArgumentOutOfRangeException(nameof(pod)),
        };

        private static State Parse(IList<string> input)
        {
            List<(P position, char pod)> initialState =
                [
                    (P.A2, input[2][3]),
                    (P.A1, input[3][3]),
                    (P.B2, input[2][5]),
                    (P.B1, input[3][5]),
                    (P.C2, input[2][7]),
                    (P.C1, input[3][7]),
                    (P.D2, input[2][9]),
                    (P.D1, input[3][9]),
                ];

            List<P> s = initialState.OrderBy(x => x.pod).Select(x => x.position).ToList();

            return new State(s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7]);
        }

        private static int StepEnergyCost(Pod pod, P from, P to) => (from, to) switch
        {
            (P.A2, P.H2) => EnergyCost(pod) * 2,
            (P.H2, P.A2) => EnergyCost(pod) * 2,
            (P.A2, P.H3) => EnergyCost(pod) * 2,
            (P.H3, P.A2) => EnergyCost(pod) * 2,
            (P.B2, P.H3) => EnergyCost(pod) * 2,
            (P.H3, P.B2) => EnergyCost(pod) * 2,
            (P.B2, P.H4) => EnergyCost(pod) * 2,
            (P.H4, P.B2) => EnergyCost(pod) * 2,
            (P.C2, P.H4) => EnergyCost(pod) * 2,
            (P.H4, P.C2) => EnergyCost(pod) * 2,
            (P.C2, P.H5) => EnergyCost(pod) * 2,
            (P.H5, P.C2) => EnergyCost(pod) * 2,
            (P.D2, P.H5) => EnergyCost(pod) * 2,
            (P.H5, P.D2) => EnergyCost(pod) * 2,
            (P.D2, P.H6) => EnergyCost(pod) * 2,
            (P.H6, P.D2) => EnergyCost(pod) * 2,
            (P.H2, P.H3) => EnergyCost(pod) * 2,
            (P.H3, P.H2) => EnergyCost(pod) * 2,
            (P.H3, P.H4) => EnergyCost(pod) * 2,
            (P.H4, P.H3) => EnergyCost(pod) * 2,
            (P.H4, P.H5) => EnergyCost(pod) * 2,
            (P.H5, P.H4) => EnergyCost(pod) * 2,
            (P.H5, P.H6) => EnergyCost(pod) * 2,
            (P.H6, P.H5) => EnergyCost(pod) * 2,
            _ => EnergyCost(pod)
        };

        private static List<(State, int)> Neighbors(State state)
        {
            List<(State, int)> ns = new();

            void AddPaths(Pod pod, P position, bool first)
            {
                if (State.IsInHallway(position))
                {
                    if (state.CanMoveHome(pod))
                    {
                        var path = homePaths[(pod, position)];
                        var energy = 0;
                        P previous = position;
                        while (path.Count > 0 && !state.IsOccupied(path.First()))
                        {
                            P next = path.First();

                            energy += StepEnergyCost(pod, previous, next);
                            
                            if (state.IsPossibleHomeMove(pod, next))
                            {
                                ns.Add((state.MoveTo(pod, first, next), energy));
                            }

                            path = path[1..];
                            previous = next;
                        }
                    }
                }
                else if (!state.IsInCorrectRoom(pod, position))
                {
                    var leftPath = outPaths[(position, Moving.Left)];
                    var energy = 0;
                    P previous = position;
                    while (leftPath.Count > 0 && !state.IsOccupied(leftPath.First()))
                    {
                        P next = leftPath.First();

                        energy += StepEnergyCost(pod, previous, next);

                        if (State.IsInHallway(next))
                        {
                            ns.Add((state.MoveTo(pod, first, next), energy));
                        }
                        
                        leftPath = leftPath[1..];
                        previous = next;
                    }

                    var rightPath = outPaths[(position, Moving.Right)];
                    energy = 0;
                    previous = position;
                    while (rightPath.Count > 0 && !state.IsOccupied(rightPath.First()))
                    {
                        P next = rightPath.First();

                        energy += StepEnergyCost(pod, previous, next);

                        if (State.IsInHallway(next))
                        {
                            ns.Add((state.MoveTo(pod, first, next), energy));
                        };
                        
                        rightPath = rightPath[1..];
                        previous = next;
                    }
                }
            }

            foreach ((int i, Pod pod, P position) in state.Pods.Select((state, i) => (i, state.pod, state.position)))
            {
                AddPaths(pod, position, i % 2 == 0);
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
            return 0;
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

        [Theory]
        [InlineData(P.H1, false)]
        [InlineData(P.H2, false)]
        [InlineData(P.H3, false)]
        [InlineData(P.H4, false)]
        [InlineData(P.H5, false)]
        [InlineData(P.H6, false)]
        [InlineData(P.H7, false)]
        [InlineData(P.A1, true)]
        [InlineData(P.A2, true)]
        [InlineData(P.B1, true)]
        [InlineData(P.B2, true)]
        [InlineData(P.C1, true)]
        [InlineData(P.C2, true)]
        [InlineData(P.D1, true)]
        [InlineData(P.D2, true)]
        [Trait("Event", "2021")]
        public void IsOccupiedTest(P position, bool expected)
        {
            var exampleInput = ReadExampleLineInput("Example");

            var startState = Parse(exampleInput);

            Assert.Equal(expected, startState.IsOccupied(position));
        }

        [Theory]
        [InlineData(P.H1, true)]
        [InlineData(P.H2, true)]
        [InlineData(P.H3, true)]
        [InlineData(P.H4, true)]
        [InlineData(P.H5, true)]
        [InlineData(P.H6, true)]
        [InlineData(P.H7, true)]
        [InlineData(P.A1, false)]
        [InlineData(P.A2, false)]
        [InlineData(P.B1, false)]
        [InlineData(P.B2, false)]
        [InlineData(P.C1, false)]
        [InlineData(P.C2, false)]
        [InlineData(P.D1, false)]
        [InlineData(P.D2, false)]
        [Trait("Event", "2021")]
        public void IsInHallwayTest(P position, bool expected)
        {
            var exampleInput = ReadExampleLineInput("Example");

            var startState = Parse(exampleInput);

            Assert.Equal(expected, State.IsInHallway(position));
        }

        [Theory]
        [InlineData("A......A.......", Pod.A, P.A1, true)]
        [InlineData("A......A.......", Pod.A, P.A2, true)]
        [InlineData("A..............", Pod.A, P.A1, true)]
        [InlineData("A..............", Pod.A, P.A2, false)]
        [InlineData("B........B.....", Pod.B, P.B1, true)]
        [InlineData("B........B.....", Pod.B, P.B2, true)]
        [InlineData("B..............", Pod.B, P.B1, true)]
        [InlineData("B..............", Pod.B, P.B2, false)]
        [InlineData("C..........C...", Pod.C, P.C1, true)]
        [InlineData("C..........C...", Pod.C, P.C2, true)]
        [InlineData("C..............", Pod.C, P.C1, true)]
        [InlineData("C..............", Pod.C, P.C2, false)]
        [InlineData("D............D.", Pod.D, P.D1, true)]
        [InlineData("D............D.", Pod.D, P.D2, true)]
        [InlineData("D..............", Pod.D, P.D1, true)]
        [InlineData("D..............", Pod.D, P.D2, false)]
        [Trait("Event", "2021")]
        public void IsPossibleHomeMoveTest(string encodedState, Pod pod, P position, bool expected)
        {
            var exampleInput = ReadExampleLineInput("Example");

            var state = State.Decode(encodedState);

            Assert.Equal(expected, state.IsPossibleHomeMove(pod, position));
        }

        [Theory]
        [InlineData(".......AA.......", Pod.A, P.A1, true)]
        [InlineData(".......AA.......", Pod.A, P.A2, true)]
        [InlineData(".......AB.......", Pod.A, P.A1, true)]
        [InlineData(".......BA.......", Pod.A, P.A2, false)]
        [Trait("Event", "2021")]
        public void IsInCorrectRoomTest(string encodedState, Pod pod, P position, bool expected)
        {
            var exampleInput = ReadExampleLineInput("Example");

            var state = State.Decode(encodedState);

            Assert.Equal(expected, state.IsInCorrectRoom(pod, position));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void NeighborsTest()
        {
            var exampleInput = ReadExampleLineInput("Example");

            var startState = Parse(exampleInput);

            var ns = Neighbors(startState);


            Assert.Equal(-1, ns.Count);
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem2(exampleInput));
        }
    }
}
