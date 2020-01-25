using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Combinatorics;
using Shared.Extensions;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 25: Cryostasis ---
    public class Day25
    {
        public static List<long> Parse(string input)
        {
            return input.Split(',').Select(long.Parse).ToList();
        }

        private class IntCodeComputer
        {
            public enum ExecutionState { WaitingForInput, Ready }
            private enum Mode { Position = 0, Immediate, Relative }
            private enum ParameterType { Read, Write }

            private class Instruction
            {
                public Instruction(long operation)
                {
                    OperationCode = operation % 100;
                    ParameterModes = new[]
                    {
                        (Mode)((operation / 100) % 10),
                        (Mode)((operation / 1000) % 10),
                        (Mode)((operation / 10000) % 10),
                    };
                }

                public long OperationCode { get; }

                // mode for parameter 1 at index 0
                // mode for parameter 2 at index 1 
                // mode for parameter 3 at index 2
                public Mode[] ParameterModes { get; }
            }

            private readonly Dictionary<long, long> memory;


            private long instructionPointer;
            private long relativeBase;
            private Instruction instruction;

            private Dictionary<long, long> Load(List<long> code)
            {
                var m = new Dictionary<long, long>();
                for (int i = 0; i < code.Count; i++)
                {
                    m.Add(i, code[i]);
                }

                return m;
            }
            public IntCodeComputer(List<long> program)
            {
                this.memory = Load(program);
                this.Input = new Queue<long>();
                this.instructionPointer = 0;

                this.Output = new Queue<long>();
            }

            public Queue<long> Output { get; }
            public Queue<long> Input { get; }

            private long ReadMemory(long address)
            {
                if (address < 0)
                {
                    throw new ArgumentOutOfRangeException($"Cannot access negative memory address: {address}");
                }
                if (!memory.ContainsKey(address))
                {
                    memory.Add(address, 0);
                }

                return memory[address];
            }

            private long Parameter(long position, Mode mode, ParameterType type)
            {
                var value = ReadMemory(instructionPointer + position);

                switch (mode)
                {
                    case Mode.Position:
                        {
                            return type == ParameterType.Read ? ReadMemory(value) : value;
                        }
                    case Mode.Immediate:
                        {
                            return value;
                        }
                    case Mode.Relative:
                        {
                            return type == ParameterType.Read ? ReadMemory(value + relativeBase) : value + relativeBase;
                        }
                    default:
                        throw new ArgumentOutOfRangeException($"Unexpected mode parameter {mode}");
                }
            }

            private (long, long, long) LoadParameters(params ParameterType[] parameterTypes)
            {
                if (parameterTypes.Length > 3)
                {
                    throw new ArgumentOutOfRangeException($"Maximum 3 parameters supported (got {parameterTypes.Length}");
                }

                var result = new long[3];

                for (long i = 0; i < parameterTypes.Length; i++)
                {
                    var mode = instruction.ParameterModes[i];
                    var type = parameterTypes[i];

                    result[i] = Parameter(i + 1, mode, type);
                }

                return (result[0], result[1], result[2]);
            }

            public ExecutionState Execute()
            {
                while (true)
                {
                    instruction = new Instruction(ReadMemory(instructionPointer));

                    switch (instruction.OperationCode)
                    {
                        case 1:
                            {
                                var (arg1, arg2, writeTo) = LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

                                memory[writeTo] = arg1 + arg2;

                                instructionPointer = instructionPointer + 4;

                                break;
                            }
                        case 2:
                            {
                                var (arg1, arg2, writeTo) = LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

                                memory[writeTo] = arg1 * arg2;
                                instructionPointer = instructionPointer + 4;
                                break;
                            }
                        case 3:
                            {
                                if (!Input.Any())
                                {
                                    return ExecutionState.WaitingForInput;
                                }

                                var (arg1, _, _) = LoadParameters(ParameterType.Write);

                                memory[arg1] = Input.Dequeue();

                                instructionPointer = instructionPointer + 2;
                                break;
                            }
                        case 4:
                            {
                                var (arg1, _, _) = LoadParameters(ParameterType.Read);

                                Output.Enqueue(arg1);
                                instructionPointer = instructionPointer + 2;
                                break;
                            }
                        case 5:
                            {
                                var (arg1, arg2, _) = LoadParameters(ParameterType.Read, ParameterType.Read);

                                instructionPointer = arg1 != 0 ? arg2 : instructionPointer + 3;
                                break;
                            }
                        case 6:
                            {
                                var (arg1, arg2, _) = LoadParameters(ParameterType.Read, ParameterType.Read);

                                instructionPointer = arg1 == 0 ? arg2 : instructionPointer + 3;
                                break;
                            }
                        case 7:
                            {
                                var (arg1, arg2, writeTo) = LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

                                memory[writeTo] = arg1 < arg2 ? 1 : 0;

                                instructionPointer = instructionPointer + 4;
                                break;
                            }
                        case 8:
                            {
                                var (arg1, arg2, writeTo) = LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

                                memory[writeTo] = arg1 == arg2 ? 1 : 0;

                                instructionPointer = instructionPointer + 4;
                                break;
                            }
                        case 9:
                            {
                                var (arg1, _, _) = LoadParameters(ParameterType.Read);

                                relativeBase += arg1;

                                instructionPointer = instructionPointer + 2;
                                break;
                            }
                        case 99:
                            {
                                return ExecutionState.Ready;
                            }
                        default:
                            throw new InvalidOperationException(
                                $"Unknown op code '{instruction.OperationCode}' at address = {instructionPointer}");
                    }
                }
            }

            public void ExecuteAscii(List<string> asciiInstructions)
            {
                var fullProgram = string.Join("", asciiInstructions);

                foreach (var i in fullProgram.Select(c => (long)c))
                {
                    Input.Enqueue(i);
                }

                Execute();
            }

            public string OutputAscii => string.Join("", Output.Select(x => (char)x));
        }

        public void RunLoop()
        {
            var programInput = ReadInput();
            var program = Parse(programInput);
            var computer = new IntCodeComputer(program);
            computer.Execute();

            foreach (var c in computer.Output)
            {
                Console.Write((char)c);
            }

            computer.Output.Clear();

            var collectCommands = new[]
            {
                "south\n",
                "south\n",
                "south\n",
                "take fixed point\n",
                "south\n",
                "take festive hat\n",
                "west\n",
                "west\n",
                "take jam\n",
                "south\n",
                "take easter egg\n",
                "north\n",
                "east\n",
                "east\n",
                "north\n",
                "west\n",
                "take asterisk\n",
                "east\n",
                "north\n",
                "west\n",
                "north\n",
                "north\n",
                "take tambourine\n",
                "south\n",
                "south\n",
                "east\n",
                "north\n",
                "west\n",
                "south\n",
                "take antenna\n",
                "north\n",
                "west\n",
                "west\n",
                "take space heater\n",
                "west\n",
                "drop fixed point\n",
                "drop festive hat\n",
                "drop jam\n",
                "drop easter egg\n",
                "drop asterisk\n",
                "drop tambourine\n",
                "drop antenna\n",
                "drop space heater\n"
            };
            foreach (var command in collectCommands)
            {
                computer.ExecuteAscii(command.Yield().ToList());

                foreach (var c in computer.Output)
                {
                    Console.Write((char)c);
                }
                computer.Output.Clear();
            }

            var allItems = new[] { "fixed point", "jam", "easter egg", "asterisk", "tambourine", "antenna", "festive hat", "space heater" };
            int heavier = 0;
            int lighter = 0;
            // trial and error 2 to 4, write it as a loop
            var combinationsOfX = allItems.Combinations(4).ToList();
            foreach (var items in combinationsOfX)
            {
                var listOfItems = items.ToList();
                computer.ExecuteAscii(listOfItems.Select(item => $"take {item}\n").ToList());
                computer.ExecuteAscii("west\n".Yield().ToList());
                var outputText = computer.OutputAscii;
                if (outputText.Contains("lighter") || outputText.Contains("heavier"))
                {
                    if (outputText.Contains("lighter"))
                    {
                        lighter++;
                    }
                    else if (outputText.Contains("heavier"))
                    {
                        heavier++;
                    }
                    computer.ExecuteAscii(listOfItems.Select(item => $"drop {item}\n").ToList());
                    computer.Output.Clear();
                }
                else
                {
                    break;
                }
            }
            // parse the code and return
            if (lighter + heavier < combinationsOfX.Count)
            {
                Console.Write(computer.OutputAscii);
            }
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }
    }
}
