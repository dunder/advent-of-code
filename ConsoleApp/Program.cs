using System;
using System.Collections.Generic;
using Y2017.Day25;

namespace ConsoleApp {
    public class Program {
        static void Main(string[] args) {


            var states = new Dictionary<string, Func<StateContext, StateContext>>();

            StateContext StateA(StateContext stateContext) {
                int nextPosition;
                string nextState;

                if (stateContext.Tape[stateContext.Position]) {
                    stateContext.Tape[stateContext.Position] = false;
                    nextPosition = stateContext.Position - 1;
                    nextState = "B";

                } else {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position + 1;
                    nextState = "B";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            StateContext StateB(StateContext stateContext) {
                int nextPosition;
                string nextState;

                if (stateContext.Tape[stateContext.Position]) {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position + 1;
                    nextState = "A";

                } else {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position - 1;
                    nextState = "A";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            states.Add("A", StateA);
            states.Add("B", StateB);

            var result = TheHaltingProblem.DiagnosticChecksum("A", 6, states);
            Console.ReadKey();
        }
    }
}
