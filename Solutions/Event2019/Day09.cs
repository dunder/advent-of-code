using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 9: Sensor Boost ---
    public class Day09
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

            private readonly List<long> input;

            private long instructionPointer;
            private int inputPosition;
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
                this.input = new List<long>();
                this.instructionPointer = 0;
                this.inputPosition = 0;

                this.Output = new List<long>();

            }

            public IntCodeComputer(List<long> program, long input)
            {
                this.memory = Load(program);
                this.input = new List<long> { input };
                this.Output = new List<long>();
                this.instructionPointer = 0;
                this.inputPosition = 0;
            }


            public List<long> Output { get; }

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

            private long ReadParameter(int position)
            {
                var value = ReadMemory(instructionPointer + position);

                var mode = instruction.ParameterModes[position - 1];
                switch (mode)
                {
                    case Mode.Position:
                    {
                        return ReadMemory(value);
                    }
                    case Mode.Immediate:
                    {
                        return value;
                    }
                    case Mode.Relative:
                    {
                        return ReadMemory(value + relativeBase);
                    }
                    default:
                        throw new ArgumentOutOfRangeException($"Unexpected mode parameter {mode}");
                }
            }
            private long WriteParameter(int position)
            {
                var value = ReadMemory(instructionPointer + position);

                var mode = instruction.ParameterModes[position - 1];
                switch (mode)
                {
                    case Mode.Position:
                    {
                        return value;
                    }
                    case Mode.Immediate:
                    {
                        throw new InvalidOperationException("Write parameters cannot use immediate mode");
                    }
                    case Mode.Relative:
                    {
                        return value + relativeBase;
                    }
                    default:
                        throw new ArgumentOutOfRangeException($"Unexpected mode parameter {mode}");
                }
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
                                // 3 parametrar
                                var (arg1, arg2, writeTo) = LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

                                // beräkna parameter 1 + parameter 2

                                // skriv till parameter 3
                                memory[writeTo] = arg1 + arg2;

                                // hoppa till nästa instruktion (op + antal parametrar)
                                instructionPointer = instructionPointer + 4;

                                // vad är tillståndet efter denna (fortsätta eller avbryta)
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
                                var (arg1, _, _) = LoadParameters(ParameterType.Write);

                                memory[arg1] = input[inputPosition++];
                                instructionPointer = instructionPointer + 2;
                                break;
                            }
                        case 4:
                            {
                                var (arg1, _, _) = LoadParameters(ParameterType.Read);

                                Output.Add(arg1);
                                instructionPointer = instructionPointer + 2;
                                return ExecutionState.WaitingForInput;
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

            public List<long> ExecuteAll()
            {
                var state = ExecutionState.WaitingForInput;
                while (state != ExecutionState.Ready)
                {
                    state = Execute();
                }

                return Output;
            }
        }

        public long FirstStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            var computer = new IntCodeComputer(program, 1);
            computer.ExecuteAll();
            return computer.Output.Last();
        }

        public long SecondStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            var computer = new IntCodeComputer(program, 2);
            computer.ExecuteAll();
            return computer.Output.Last();
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(3280416268, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(80210, SecondStar());
        }

        [Fact]
        public void FirstStarExample1()
        {
            var code = Parse("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99");
            var computer = new IntCodeComputer(code);
            Assert.Equal(code, computer.ExecuteAll());
        }

        [Fact]
        public void FirstStarExample2()
        {
            var code = Parse("1102,34915192,34915192,7,4,7,99,0");
            var computer = new IntCodeComputer(code);
            computer.Execute();
            Assert.Equal(1219070632396864, computer.Output.Last());
        }

        [Fact]
        public void FirstStarExample3()
        {
            var code = Parse("104,1125899906842624,99");
            var computer = new IntCodeComputer(code);
            computer.Execute();
            Assert.Equal(1125899906842624, computer.Output.Last());
        }
    }
}
