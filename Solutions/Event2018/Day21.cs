using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day21 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day21;
        public string Name => "Chronal Conversion";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = RunProgram1(input, new[] { 0, 0, 0, 0, 0, 0 });
            return result.ToString();
        }

        public string FirstStarConsole()
        {
            var input = ReadLineInput();
            var result = RunProgram2(input, new[] { 0, 0, 0, 0, 0, 0 }, true);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = RunProgram2(input, new[] { 0, 0, 0, 0, 0, 0 });
            return result.ToString();
        }

        public int RunProgram1(IList<string> program, int[] registry, bool print = false)
        {
            var instructionPointerRegistry = int.Parse(program[0].Substring(4, 1));

            var programInstructions = program.Skip(1).ToList();

            if (print)
            {
                Print(programInstructions);
            }

            for (var i = 0; i < programInstructions.Count; i++)
            {
                var programInstruction = programInstructions[(int) i];

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

                // The natural end condition for the program is the eqrr 1 0 5 (i == 28) expression.
                // The program will terminate when register 0 is equal to register 1 at this execution point.
                // The program never updates register 0 so register 0 should be set to the value encountered 
                // in register 1 when the eqrr 1 0 5 expression is reached for the first time
                if (programInstruction == "eqrr 1 0 5")
                {
                    return registry[1];
                }

                registry = Instructions[op](registry, 0, a, b, c);

                i = registry[instructionPointerRegistry];
            }

            return -1;
        }

        public int RunProgram2(IList<string> program, int[] registry, bool print = false)
        {
            var instructionPointerRegistry = int.Parse(program[0].Substring(4, 1));

            var programInstructions = program.Skip(1).ToList();

            if (print)
            {
                Print(programInstructions);
            }

            var valuesOfR1 = new HashSet<int>();
            var repeatingSequence = new List<int>();
            var repeatingSequenceValues = new HashSet<int>();

            for (var i = 0; i < programInstructions.Count; i++)
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

                // Optimize the loop between instructions i == 17 and i == 25
                if (programInstruction == "addi 5 1 5")
                {
                    var r2 = registry[2];
                    var x = r2 / 256;
                    while ((x + 1) * 256 <= r2) { x++; }

                    registry[5] = x;
                }
                else
                {
                    // Assume that the values for register 1 sooner or later repeats 
                    // The first time a repeated value is found twice the sequence starts
                    // to repeat itself and then the last value in the repeated sequence
                    // must be the answer
                    if (programInstruction == "eqrr 1 0 5")
                    { 
                        var r1 = registry[1];
                        if (valuesOfR1.Contains(r1))
                        {
                            if (repeatingSequenceValues.Contains(r1))
                            {
                                return repeatingSequence.Last();
                            }

                            repeatingSequence.Add(r1);
                            repeatingSequenceValues.Add(r1);
                        }

                        valuesOfR1.Add(r1);
                    }

                    registry = Instructions[op](registry, 0, a, b, c);
                }

                i = registry[instructionPointerRegistry];
            }

            return -1;
        }

        private void Print(IList<string> instructions)
        {
            Console.WindowHeight = 50;
            Console.WindowWidth = 25;

            foreach (var instruction in instructions)
            {
                Console.WriteLine($" {instruction}");
            }
        }

        private void Print(long i, string instruction, int[] registry)
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
            Console.WriteLine($"i: {i.ToString().PadLeft(2, ' ')}");
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
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("6483199", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("13338900", actual);
        }

        private static readonly Dictionary<string, Func<int[], int, int, int, int, int[]>> Instructions =
            new Dictionary<string, Func<int[], int, int, int, int, int[]>>
        {
            {"addr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

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