using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 8: Handheld Halting ---
    public class Day08
    {
        private readonly ITestOutputHelper output;

        public Day08(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record ProgramState(int accumulator, int instructionPointer);

        private delegate ProgramState Instruction(ProgramState inputState);

        private static readonly Regex AccRegEx = new Regex(@"^acc ([\+|-]\d+)$");
        private static readonly Regex JmpRegEx = new Regex(@"^jmp ([\+|-]\d+)$");

        private List<Instruction> Parse(IList<string> input, int? swapIndex = null)
        {
            bool NoOp(string instruction)
            {
                return instruction.StartsWith("nop");
            }

            bool IntParameterInstruction(Regex regex, string instruction, out int parameter)
            {
                Match match = regex.Match(instruction);

                if (match.Success)
                {
                    parameter = int.Parse(match.Groups[1].Value);
                    return true;
                }
                else
                {
                    parameter = 0;
                    return false;
                }
            }

            var instructions = new List<Instruction>();

            for (int i = 0; i < input.Count; i++)
            {
                string instruction = input[i];

                if (i == swapIndex)
                {
                    if (instruction.StartsWith("jmp"))
                    {
                        instruction = instruction.Replace("jmp", "nop");
                    }
                    else if (instruction.StartsWith("nop"))
                    {
                        instruction = instruction.Replace("nop", "jmp");
                    }
                }

                switch (instruction)
                {
                    case var x when NoOp(instruction):
                        instructions.Add(state => state with { 
                            instructionPointer = state.instructionPointer + 1 
                        });
                        break;
                    case var x when IntParameterInstruction(AccRegEx, instruction, out int parameter):
                        instructions.Add(state => state with { 
                            instructionPointer = state.instructionPointer + 1,
                            accumulator = state.accumulator + parameter
                        });
                        break;
                    case var x when IntParameterInstruction(JmpRegEx, instruction, out int parameter):
                        instructions.Add(state => state with { 
                            instructionPointer = state.instructionPointer + parameter
                        });
                        break;
                    default:
                        throw new Exception($"Unrecognized instruction: {instruction}");
                }
            }

            return instructions;
        }

        private ProgramState Run(IList<string> input, int? swapIndex = null)
        {
            var instructions = Parse(input, swapIndex);

            HashSet<int> instructionsExecuted = new HashSet<int>();

            ProgramState state = new ProgramState(0, 0);

            while (true)
            {
                if (instructionsExecuted.Add(state.instructionPointer))
                {
                    if (state.instructionPointer >= instructions.Count)
                    {
                        break;
                    }
                    var nextInstruction = instructions[state.instructionPointer];
                    state = nextInstruction(state);
                }
                else
                {
                    break;
                }
            }

            return state;
        }

        public int RunUntilNotCorrupted(IList<string> input) 
        {
            var indexOfPossiblyCorrupted = input
                .Select((line, i) => (line, i))
                .Where(p => p.line.StartsWith("nop") || p.line.StartsWith("jmp"))
                .Select(possiblyCorrupted => possiblyCorrupted.i);

            ProgramState state = new ProgramState(0, 0);

            foreach (var i in indexOfPossiblyCorrupted)
            {
                state = Run(input, i);

                if (state.instructionPointer == input.Count)
                {
                    break;
                }
            }

            return state.accumulator;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return Run(input).accumulator;
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return RunUntilNotCorrupted(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(1782, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(797, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };

            var result = Run(example).accumulator;

            Assert.Equal(5, result);
        }

        [Fact]
        public void SecondStarExample()
        {

            var example = new List<string>
            {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };

            var result = RunUntilNotCorrupted(example);

            Assert.Equal(8, result);
        }
    }
}
