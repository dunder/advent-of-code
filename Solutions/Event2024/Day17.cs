using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 17: Chronospatial Computer ---
    public class Day17
    {
        private readonly ITestOutputHelper output;


        private record State(long A, long B, long C);

        public Day17(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static long ComboOperand(int operand, State state) => operand switch
        {
            < 4 => operand,
            4 => state.A,
            5 => state.B,
            6 => state.C,
            _ => throw new ArgumentOutOfRangeException($"Illegal operand: {operand}")
        };

        private static (string program, long a) Parse(IList<string> input)
        {
            var number = new Regex(@"(\d+)");

            long a =  long.Parse(number.Match(input.First()).Value);
            
            return (input.ToList()[^1].Substring("Program: ".Length), a);
        }

        private static string Problem1(IList<string> input)
        {
            (string program, long a) = Parse(input);

            return Run(program, a);
        }

        private static string Run(string program, long a)
        {
            var code = program.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            State state = new State(a, 0, 0);

            var output = new List<long>();

            for (int i = 0; i < code.Count; i = i + 2)
            {
                if (i >= code.Count)
                {
                    break;
                }

                var opCode = code[i];
                var operand = code[i + 1];

                switch (opCode)
                {
                    // adv
                    case 0:
                        {
                            long value = state.A / (long)Math.Pow(2, ComboOperand(operand, state));
                            state = state with { A = value };
                        }
                        break;
                    // bxl
                    case 1:
                        {
                            long value = state.B ^ operand;
                            state = state with { B = value };
                        }
                        break;
                    // bst
                    case 2:
                        {
                            long value = ComboOperand(operand, state) % 8;
                            state = state with { B = value };
                        }
                        break;
                    // jnz
                    case 3:
                        {
                            if (state.A != 0)
                            {
                                i = operand - 2;
                            }
                        }
                        break;
                    // bxc
                    case 4:
                        {
                            long value = state.B ^ state.C;
                            state = state with { B = value };
                        }
                        break;
                    // out
                    case 5:
                        {
                            long value = ComboOperand(operand, state) % 8;
                            output.Add(value);
                        }
                        break;
                    // bdv
                    case 6:
                        {
                            long value = state.A / (int)Math.Pow(2, ComboOperand(operand, state));
                            state = state with { B = value };
                        }
                        break;
                    // cdv
                    case 7:
                        {
                            long value = state.A / (int)Math.Pow(2, ComboOperand(operand, state));
                            state = state with { C = value };
                        }
                        break;
                }
            }

            var result = string.Join(",", output);

            return result;

        }

        public static long Problem2(IList<string> input)
        {
            (string program, long a) = Parse(input);

            string output = Run(program, a);

            List<string> results = new();

            Queue<(long, string)> q = new Queue<(long, string)>();

            q.Enqueue((0, program.Substring(program.Length-1)));

            long mina = long.MaxValue;

            // reverse engineer program and realize that each "cycle" (per output position) is independent,
            // and that the value of A is divided by 8 in each iteration (there is only one instruction that
            // writes to the a register in my program and it is A / 8). So beginning backwards from the output
            // values, find the value (or values) that generate the correct output and multiply it by 8 and
            // expand on the program, basically a BFS search among the expanded programs

            while (q.Count > 0)
            {
                (long ainit, string subprogram) = q.Dequeue();

                for (a = ainit; a < ainit + 8; a++)
                {
                    string suboutput = Run(program, a);

                    if (suboutput == program)
                    {
                        mina = a;
                        q.Clear();
                        break;
                    }

                    if (suboutput == subprogram)
                    {
                        var newSub = program.Substring(program.Length-subprogram.Length-2);
                        q.Enqueue((a * 8, newSub));
                    }
                }
            }

            return mina;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal("7,1,2,3,2,6,7,2,5", Problem1(input)); 
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(202356708354602, Problem2(input));
        }

        private List<string> exampleInput =
        [
            "Register A: 729",
            "Register B: 0",
            "Register C: 0",
            "",
            "Program: 0,1,5,4,3,0",
        ];

        private State exampleState = new State(729, 0, 0);
        private string exampleProgram = "0,1,5,4,3,0";

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal("4,6,3,5,6,3,5,2,1,0", Problem1(exampleInput));
        }
    }
}
