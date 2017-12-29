using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day25 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            var states = TheHaltingProblem.BuildStates();

            var result = TheHaltingProblem.DiagnosticChecksum("A", 12317297, states);

            Assert.Equal(4230, result);
            _output.WriteLine($"Day 25 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day25\input.txt");

            var result = "";

            _output.WriteLine($"Day 25 problem 2: {result}");
        }
    }

    public class TheHaltingProblem {
        public static int DiagnosticChecksum(string startState, int iterations, Dictionary<string, Func<StateContext, StateContext>> input) {
            
            var stateContext = new StateContext(iterations / 2 + 1, new bool[2*iterations], startState);
            for (int i = 0; i < iterations; i++) {
                stateContext = input[stateContext.NextState](stateContext);
            }

            return stateContext.Tape.Count(b => b);
        }

        public static Dictionary<string, Func<StateContext, StateContext>> BuildStates() {
            var states = new Dictionary<string, Func<StateContext, StateContext>>();

            StateContext StateA(StateContext stateContext) {
                int nextPosition;
                string nextState;

                if (!stateContext.Tape[stateContext.Position]) {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position + 1;
                    nextState = "B";

                } else {
                    stateContext.Tape[stateContext.Position] = false;
                    nextPosition = stateContext.Position - 1;
                    nextState = "D";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            StateContext StateB (StateContext stateContext) {
                int nextPosition;
                string nextState;

                if (!stateContext.Tape[stateContext.Position]) {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position + 1;
                    nextState = "C";

                } else {
                    stateContext.Tape[stateContext.Position] = false;
                    nextPosition = stateContext.Position + 1;
                    nextState = "F";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            StateContext StateC (StateContext stateContext) {
                int nextPosition;
                string nextState;

                if (!stateContext.Tape[stateContext.Position]) {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position - 1;
                    nextState = "C";

                } else {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position - 1;
                    nextState = "A";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            StateContext StateD (StateContext stateContext) {
                int nextPosition;
                string nextState;

                if (!stateContext.Tape[stateContext.Position]) {
                    stateContext.Tape[stateContext.Position] = false;
                    nextPosition = stateContext.Position - 1;
                    nextState = "E";

                } else {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position + 1;
                    nextState = "A";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            StateContext StateE (StateContext stateContext) {
                int nextPosition;
                string nextState;

                if (!stateContext.Tape[stateContext.Position]) {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position - 1;
                    nextState = "A";

                } else {
                    stateContext.Tape[stateContext.Position] = false;
                    nextPosition = stateContext.Position + 1;
                    nextState = "B";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            StateContext StateF (StateContext stateContext) {
                int nextPosition;
                string nextState;

                if (!stateContext.Tape[stateContext.Position]) {
                    stateContext.Tape[stateContext.Position] = false;
                    nextPosition = stateContext.Position + 1;
                    nextState = "C";

                } else {
                    stateContext.Tape[stateContext.Position] = false;
                    nextPosition = stateContext.Position + 1;
                    nextState = "E";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            states.Add("A", StateA);
            states.Add("B", StateB);
            states.Add("C", StateC);
            states.Add("D", StateD);
            states.Add("E", StateE);
            states.Add("F", StateF);

            return states;
        }

        private static void Print(StateContext stateContext) {
            for (int x = 0; x < stateContext.Tape.Length; x++) {
                if (x == stateContext.Position) {
                    Console.Write("[");
                }
                else {
                    Console.Write(" ");
                }

                if (stateContext.Tape[x]) {
                    Console.Write("1");
                }
                else {
                    Console.Write("0");
                }

                if (x == stateContext.Position) {
                    Console.Write("]");
                }
                else {
                    Console.Write(" ");
                }
            }

            Console.WriteLine();
        }
    }

    public class StateContext {
        public int Position { get; }
        public bool[] Tape { get; }
        public string NextState { get; }

        public StateContext(int position, bool[] tape, string nextState) {
            Position = position;
            Tape = tape;
            NextState = nextState;
        }
    }
}
