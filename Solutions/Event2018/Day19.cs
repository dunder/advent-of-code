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
            var input = ReadLineInput();
            var result = RunProgram(input, 1);
            return result.ToString();
        }

        public void SecondStarConsole()
        {
            var input = ReadLineInput();
            RunProgram(input, 1, true);
        }

        public long RunProgram(IList<string> program, int register0Value = 0, bool print = false)
        {
            var instructionPointerRegistry = int.Parse(program[0].Substring(4, 1));

            // original i = 0
            var registry = new[] { 1L, 0L, 0L, 0L, 0L, 0L };

            // optimization 1 start with i = 1
            //var registry = new[] { 0, 10550400L, 0L, 34L, 0L, 10551381L };

            // optimization 2 start with i = 9
            //var registry = new[] { 0, 1L, 10551382L, 8L, 1L, 10551381L };


            // optimization 3 start with i = 9
            //var registry = new[] { 0, 1L, 10551382L, 8L, 2L, 10551381L };

            // optimization 4 start with i = 9
            //var registry = new[] { 0, 1L, 10551382L, 8L, 10551381L, 10551381L };

            var programInstructions = program.Skip(1).ToList();

            if (print)
            {
                Print(programInstructions);
            }

            for (long i = 0; i < programInstructions.Count; i++)
            {
                var programInstruction = programInstructions[(int)i];

                if (print)
                {
                    Print(i, programInstruction, registry);
                }

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

        public void Print(IList<string> instructions)
        {
            Console.WindowHeight = 50;
            Console.WindowWidth = 25;

            foreach (var instruction in instructions)
            {
                Console.WriteLine($" {instruction}");
            }
        }

        public void Print(long i, string instruction, long[] registry)
        {
            var registry0 = registry[0].ToString();
            var registry1 = registry[1].ToString();
            var registry2 = registry[2].ToString();
            var registry3 = registry[3].ToString();
            var registry4 = registry[4].ToString();
            var registry5 = registry[5].ToString();

            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            for (int y = 0; y < 36; y++)
            {
                Console.Write(y == i ? '>' : ' ');
                Console.CursorLeft--;
                Console.CursorTop++;
            }

            Console.CursorTop = 38;
            Console.CursorLeft = 0;

            Console.WriteLine();
            Console.WriteLine($"0: {registry0.PadLeft(20, ' ')}");
            Console.WriteLine($"1: {registry1.PadLeft(20, ' ')}");
            Console.WriteLine($"2: {registry2.PadLeft(20, ' ')}");
            Console.WriteLine($"3: {registry3.PadLeft(20, ' ')}");
            Console.WriteLine($"4: {registry4.PadLeft(20, ' ')}");
            Console.WriteLine($"5: {registry5.PadLeft(20, ' ')}");

            Console.ReadKey(true);
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

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("1430", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual);
        }

        private static readonly Dictionary<string, Func<long[], int, int, int, int, long[]>> Instructions =
            new Dictionary<string, Func<long[], int, int, int, int, long[]>>
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