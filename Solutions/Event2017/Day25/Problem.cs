using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2017.Day25
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day25;

        public override string FirstStar()
        {
            var states = TheHaltingProblem.BuildStates();
            var result = TheHaltingProblem.DiagnosticChecksum("A", 12317297, states);
            return result.ToString();
        }

        public override string SecondStar()
        {
            return "No problem to solve";
        }
    }

    public class TheHaltingProblem {
        public static int DiagnosticChecksum(string startState, int iterations, Dictionary<string, Func<StateContext, StateContext>> input) {

            var stateContext = new StateContext(iterations / 2 + 1, new bool[2 * iterations], startState);
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

            StateContext StateB(StateContext stateContext) {
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

            StateContext StateC(StateContext stateContext) {
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

            StateContext StateD(StateContext stateContext) {
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

            StateContext StateE(StateContext stateContext) {
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

            StateContext StateF(StateContext stateContext) {
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
                } else {
                    Console.Write(" ");
                }

                if (stateContext.Tape[x]) {
                    Console.Write("1");
                } else {
                    Console.Write("0");
                }

                if (x == stateContext.Position) {
                    Console.Write("]");
                } else {
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