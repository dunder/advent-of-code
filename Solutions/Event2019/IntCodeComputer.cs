using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Extensions;

namespace Solutions.Event2019
{
    public class IntCodeComputer
    {
        public enum ExecutionState
        {
            WaitingForInput,
            Ready
        }

        private enum Mode
        {
            Position = 0,
            Immediate,
            Relative
        }

        private enum ParameterType
        {
            Read,
            Write
        }

        private class Instruction
        {
            public Instruction(long operation)
            {
                OperationCode = operation % 100;
                ParameterModes = new[]
                {
                    (Mode) ((operation / 100) % 10),
                    (Mode) ((operation / 1000) % 10),
                    (Mode) ((operation / 10000) % 10),
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

        private IntCodeComputer(List<long> program)
        {
            this.memory = Load(program);
            this.Input = new Queue<long>();
            this.instructionPointer = 0;

            this.Output = new Queue<long>();
        }

        public static IntCodeComputer Load(string textProgram)
        {
            return new IntCodeComputer(textProgram.Split(',').Select(long.Parse).ToList());
        }

        public Queue<long> Output { get; }
        public Queue<long> Input { get; }

        public void MemoryOverride(long address, long value)
        {
            memory[address] = value;
        }
        
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
            Output.Clear();

            while (true)
            {
                instruction = new Instruction(ReadMemory(instructionPointer));

                switch (instruction.OperationCode)
                {
                    case 1:
                    {
                        var (arg1, arg2, writeTo) =
                            LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

                        memory[writeTo] = arg1 + arg2;

                        instructionPointer = instructionPointer + 4;

                        break;
                    }
                    case 2:
                    {
                        var (arg1, arg2, writeTo) =
                            LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

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
                        var (arg1, arg2, writeTo) =
                            LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

                        memory[writeTo] = arg1 < arg2 ? 1 : 0;

                        instructionPointer = instructionPointer + 4;
                        break;
                    }
                    case 8:
                    {
                        var (arg1, arg2, writeTo) =
                            LoadParameters(ParameterType.Read, ParameterType.Read, ParameterType.Write);

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

        public void Execute(params int[] input)
        {
            foreach (var i in input)
            {
                Input.Enqueue(i);
            }

            Execute();
        }

        public void ExecuteAscii(string asciiInstructions)
        {
            ExecuteAscii(asciiInstructions.Yield().ToList());
        }

        public void ExecuteAscii(List<string> asciiInstructions)
        {
            Output.Clear();

            var fullProgram = string.Join("", asciiInstructions);

            foreach (var i in fullProgram.Select(c => (long) c))
            {
                Input.Enqueue(i);
            }

            Execute();
        }

        public string OutputAscii => string.Join("", Output.Select(x => (char) x));
    }
}