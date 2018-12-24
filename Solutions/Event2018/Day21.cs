﻿using System;
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
            var result = RunProgram(input, 0, 0, new []{ 6483199, 0,0,0,0,0});
            return result.ToString();
        }

        public string FirstStarConsole()
        {
            // i = 18
            // x, 9784884, 65536, 0, 17, 255 (counter = 23 + 7*(255-1) = 1801)
            // x, 9784884, 6548735, 0, 17, 25579
            // x, 1508639, 15202945, 0, 17, 59385
            var input = ReadLineInput();
            var result = RunProgram(input, 18, 1801, new[] { 0, 9784884, 65536, 0, 17, 255 }, true);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public int RunProgram(IList<string> program, int i, int instructionCounter, int[] registry, bool print = false)
        {
            var instructionPointerRegistry = int.Parse(program[0].Substring(4, 1));

            var programInstructions = program.Skip(1).ToList();

            if (print)
            {
                Print(programInstructions);
            }

            for (; i < programInstructions.Count; i++)
            {
                var programInstruction = programInstructions[(int) i];

                if (print)
                {
                    Print(i, programInstruction, registry, instructionCounter);
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

                instructionCounter++;
            }

            return instructionCounter;
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

        public void Print(long i, string instruction, int[] registry, int instructionCount)
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
            Console.WriteLine($"i: {i.ToString().PadLeft(2, ' ')} ({instructionCount.ToString().PadLeft(14, ' ')})");
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
            Assert.Equal("", actual); // register 0 input = 6483199
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual); // register 0 input = ?
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