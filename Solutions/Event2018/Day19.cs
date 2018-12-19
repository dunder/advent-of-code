using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day19 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day19;
        public string Name => "Day19";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = RunProgram(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual);
        }

        public int RunProgram(IList<string> program)
        {
            var instructionPointerRegistry = int.Parse(program[0].Substring(4, 1));

            var registry = new[] { 0, 0, 0, 0, 0, 0 };

            var programInstructions = program.Skip(1).ToList();

            for(int i = 0; i < programInstructions.Count; i++)
            {
                var programInstruction = programInstructions[i];
                var op = programInstruction.Substring(0, 4);

                var arguments = programInstruction.Substring(5)
                    .Split(" ")
                    .Select(int.Parse)
                    .ToArray();

                var a = arguments[0];
                var b = arguments[1];
                var c = arguments[2];

                registry[instructionPointerRegistry] = i;

                registry = Instructions[op](registry, 0, a, b, c);

                i = registry[instructionPointerRegistry];
            }

            return registry[0];
        }

        [Fact]
        public void FirstStarExample()
        {
            var instructions = new[]
            {
                "#ip 0    ",
                "seti 5 0 1",
                "seti 6 0 2",
                "addi 0 1 0",
                "addr 1 2 3",
                "setr 1 0 0",
                "seti 8 0 4",
                "seti 9 0 5",
            };

            var result = RunProgram(instructions);

            Assert.Equal(6, result);
        }


        private static readonly Dictionary<string, Func<int[], int, int, int, int, int[]>> Instructions =
            new Dictionary<string, Func<int[], int, int, int, int, int[]>>
        {
            {"addr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                if (a == 0)
                {
                    // 
                }

                rout[c] = rin[a] + rin[b];

                return rout.ToArray();
            }},
            { "addi", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] + b;

                return rout.ToArray();
            }},
            { "mulr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] * rin[b];

                return rout.ToArray();
            }},
            { "muli", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] * b;

                return rout.ToArray();
            }},
            { "banr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] & rin[b];

                return rout.ToArray();
            }},
            { "bani", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] & b;

                return rout.ToArray();
            }},
            { "borr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] | rin[b];

                return rout.ToArray();
            }},
            { "bori", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] | b;

                return rout.ToArray();
            }},
            { "setr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a];

                return rout.ToArray();
            }},
            { "seti", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = a;

                return rout.ToArray();
            }},
            { "gtir", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = a > rin[b] ? 1 : 0;

                return rout.ToArray();
            }},
            { "gtri", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] > b ? 1 : 0;

                return rout.ToArray();
            }},
            { "gtrr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] > rin[b] ? 1 : 0;

                return rout.ToArray();
            }},
            { "eqir", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = a == rin[b] ? 1 : 0;

                return rout.ToArray();
            }},
            { "eqri", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] == b ? 1 : 0;

                return rout.ToArray();
            }},
            { "eqrr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] == rin[b] ? 1 : 0;

                return rout.ToArray();
            }}
        };
    }
}