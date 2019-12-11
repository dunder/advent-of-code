using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using Shared.MapGeometry;
using Solutions.Event2016.Day01;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // 
    public class Day11
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
                        (IntCodeComputer.Mode)((operation / 100) % 10),
                        (IntCodeComputer.Mode)((operation / 1000) % 10),
                        (IntCodeComputer.Mode)((operation / 10000) % 10),
                    };
                }

                public long OperationCode { get; }

                // mode for parameter 1 at index 0
                // mode for parameter 2 at index 1 
                // mode for parameter 3 at index 2
                public IntCodeComputer.Mode[] ParameterModes { get; }
            }

            private readonly Dictionary<long, long> memory;

            private readonly List<long> input;

            private long instructionPointer;
            private int inputPosition;
            private long relativeBase;
            private IntCodeComputer.Instruction instruction;

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

            public void SetInput(long input)
            {
                this.input.Add(input);
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
                    case IntCodeComputer.Mode.Position:
                        {
                            return ReadMemory(value);
                        }
                    case IntCodeComputer.Mode.Immediate:
                        {
                            return value;
                        }
                    case IntCodeComputer.Mode.Relative:
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
                    case IntCodeComputer.Mode.Position:
                        {
                            return value;
                        }
                    case IntCodeComputer.Mode.Immediate:
                        {
                            throw new InvalidOperationException("Write parameters cannot use immediate mode");
                        }
                    case IntCodeComputer.Mode.Relative:
                        {
                            return value + relativeBase;
                        }
                    default:
                        throw new ArgumentOutOfRangeException($"Unexpected mode parameter {mode}");
                }
            }

            private long Parameter(long position, IntCodeComputer.Mode mode, IntCodeComputer.ParameterType type)
            {
                var value = ReadMemory(instructionPointer + position);

                switch (mode)
                {
                    case IntCodeComputer.Mode.Position:
                        {
                            return type == IntCodeComputer.ParameterType.Read ? ReadMemory(value) : value;
                        }
                    case IntCodeComputer.Mode.Immediate:
                        {
                            return value;
                        }
                    case IntCodeComputer.Mode.Relative:
                        {
                            return type == IntCodeComputer.ParameterType.Read ? ReadMemory(value + relativeBase) : value + relativeBase;
                        }
                    default:
                        throw new ArgumentOutOfRangeException($"Unexpected mode parameter {mode}");
                }
            }

            private (long, long, long) LoadParameters(params IntCodeComputer.ParameterType[] parameterTypes)
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

            public IntCodeComputer.ExecutionState Execute()
            {
                while (true)
                {
                    instruction = new IntCodeComputer.Instruction(ReadMemory(instructionPointer));

                    switch (instruction.OperationCode)
                    {
                        case 1:
                            {
                                // 3 parametrar
                                var (arg1, arg2, writeTo) = LoadParameters(IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Write);

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
                                var (arg1, arg2, writeTo) = LoadParameters(IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Write);

                                memory[writeTo] = arg1 * arg2;
                                instructionPointer = instructionPointer + 4;
                                break;
                            }
                        case 3:
                            {
                                var (arg1, _, _) = LoadParameters(IntCodeComputer.ParameterType.Write);

                                memory[arg1] = input[inputPosition++];
                                instructionPointer = instructionPointer + 2;
                                break;
                            }
                        case 4:
                            {
                                var (arg1, _, _) = LoadParameters(IntCodeComputer.ParameterType.Read);

                                Output.Add(arg1);
                                instructionPointer = instructionPointer + 2;
                                return IntCodeComputer.ExecutionState.WaitingForInput;
                            }
                        case 5:
                            {
                                var (arg1, arg2, _) = LoadParameters(IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Read);

                                instructionPointer = arg1 != 0 ? arg2 : instructionPointer + 3;
                                break;
                            }
                        case 6:
                            {
                                var (arg1, arg2, _) = LoadParameters(IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Read);

                                instructionPointer = arg1 == 0 ? arg2 : instructionPointer + 3;
                                break;
                            }
                        case 7:
                            {
                                var (arg1, arg2, writeTo) = LoadParameters(IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Write);

                                memory[writeTo] = arg1 < arg2 ? 1 : 0;

                                instructionPointer = instructionPointer + 4;
                                break;
                            }
                        case 8:
                            {
                                var (arg1, arg2, writeTo) = LoadParameters(IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Read, IntCodeComputer.ParameterType.Write);

                                memory[writeTo] = arg1 == arg2 ? 1 : 0;

                                instructionPointer = instructionPointer + 4;
                                break;
                            }
                        case 9:
                            {
                                var (arg1, _, _) = LoadParameters(IntCodeComputer.ParameterType.Read);

                                relativeBase += arg1;

                                instructionPointer = instructionPointer + 2;
                                break;
                            }
                        case 99:
                            {
                                return IntCodeComputer.ExecutionState.Ready;
                            }
                        default:
                            throw new InvalidOperationException(
                                $"Unknown op code '{instruction.OperationCode}' at address = {instructionPointer}");
                    }
                }
            }

            public List<long> ExecuteAll()
            {
                var state = IntCodeComputer.ExecutionState.WaitingForInput;
                while (state != IntCodeComputer.ExecutionState.Ready)
                {
                    state = Execute();
                }

                return Output;
            }
        }

        private Direction MakeTurn(int code, Direction currentDirection)
        {
            if (code > 1 || code < 0)
            {
                throw new ArgumentOutOfRangeException($"Invalid turn: {code}");
            }
            return code == 0 ? currentDirection.Turn(Turn.Left) : currentDirection.Turn(Turn.Right);
        }

        private static int Read(Dictionary<Point, int> painted, Point p)
        {
            if (!painted.ContainsKey(p))
            {
                painted.Add(p, 0);
            }

            return painted[p];
        }

        private static void Paint(Dictionary<Point, int> painted, Point p, int color)
        {
            if (!painted.ContainsKey(p))
            {
                painted.Add(p, 0);
            }

            painted[p] = color;
        }

        public int Run(List<long> program)
        {
            // all initial black = 0 (1 = white)
            // input from camera color below robot
            // output 1 = color to paint the panel it is over
            // output 2 = direction 0 is left, 1 is right
            // after turn move forward one panel

            var startingPoint = new Point(0, 0);
            var painted = new Dictionary<Point, int>();
            var currentDirection = Direction.North;
            var currentLocation = startingPoint;

            var computer = new IntCodeComputer(program);
            var path = new List<Point> {startingPoint};

            var exitCode = IntCodeComputer.ExecutionState.WaitingForInput;
            while (exitCode != IntCodeComputer.ExecutionState.Ready)
            {
                computer.SetInput(Read(painted, currentLocation));

                exitCode = computer.Execute();
                if (exitCode == IntCodeComputer.ExecutionState.Ready)
                {
                    continue;
                }
                var color = computer.Output.Last();
                exitCode = computer.Execute();
                if (exitCode == IntCodeComputer.ExecutionState.Ready)
                {
                    continue;
                }

                int turnCode = (int) computer.Output.Last();
                currentDirection = MakeTurn(turnCode, currentDirection);
                Paint(painted, currentLocation, (int)color);
                currentLocation = currentLocation.Move(currentDirection);
                path.Add(currentLocation);
            }


            return painted.Keys.Count;
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            var count = Run(program);
            return count;
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            var count = Run(program);
            return count;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(2184, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

    }
}
