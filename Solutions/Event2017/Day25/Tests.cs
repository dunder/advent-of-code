﻿using System;
using System.Collections.Generic;
using Xunit;

namespace Solutions.Event2017.Day25
{
    public class Tests
    {
        [Fact]
        public static void FirstStarExample()
        {
            var states = new Dictionary<string, Func<StateContext, StateContext>>();

            StateContext StateA(StateContext stateContext)
            {
                int nextPosition;
                string nextState;

                if (stateContext.Tape[stateContext.Position])
                {
                    stateContext.Tape[stateContext.Position] = false;
                    nextPosition = stateContext.Position - 1;
                    nextState = "B";
                }
                else
                {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position + 1;
                    nextState = "B";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            StateContext StateB(StateContext stateContext)
            {
                int nextPosition;
                string nextState;

                if (stateContext.Tape[stateContext.Position])
                {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position + 1;
                    nextState = "A";
                }
                else
                {
                    stateContext.Tape[stateContext.Position] = true;
                    nextPosition = stateContext.Position - 1;
                    nextState = "A";
                }

                return new StateContext(nextPosition, stateContext.Tape, nextState);
            }

            states.Add("A", StateA);
            states.Add("B", StateB);

            var result = TheHaltingProblem.DiagnosticChecksum("A", 6, states);

            Assert.Equal(3, result);
        }


        [Fact]
        public static void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("4230", actual);
        }
    }
}