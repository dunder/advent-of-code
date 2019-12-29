using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 19: Tractor Beam ---
    public class Day19
    {
        private readonly ITestOutputHelper output;

        public Day19(ITestOutputHelper output)
        {
            this.output = output;
        }

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

            public List<long> Execute(int x, int y)
            {
                input.Add(x);
                input.Add(y);
                return ExecuteAll();
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

        private Dictionary<Point, int> Scan(List<long> program, Point p, int xMax, int yMax)
        {
            var map = new Dictionary<Point, int>();
            for (int y = p.Y; y < p.Y + yMax; y++)
            {
                for (int x = p.X; x < p.X + xMax; x++)
                {
                    var computer = new IntCodeComputer(program);

                    computer.Execute(x, y);
                    map.Add(new Point(x,y), (int) computer.Output.Last());
                }
            }
            //PrintMap(map, p, xMax, yMax);
            return map;
        }

        private bool ExecuteOne(List<long> program, int x, int y)
        {
            var computer = new IntCodeComputer(program);
            computer.Execute(x,y);
            return computer.Output.Last() == 1;
        }

        private int FindNew(List<long> program, int width, int height, Point start)
        {
            var xStep = start.X;
            var yStep = start.Y;
            var x = xStep;
            var y = yStep;
            var found = false;
            while (!found)
            {
                y++;
                while (!ExecuteOne(program, x, y))
                {
                    x++;
                }

                int x2 = x;
                while (ExecuteOne(program, x2, y))
                {
                    x2++;
                    if (CoversSimple(program, x2, y, width, height))
                    {
                        x = x2;
                        found = true;
                    }
                }
            }

            var beam = new Point(0, 0);
            var closest = Enumerable.Range(x, width).Select(xx => new Point(xx, y)).OrderBy(p => p.ManhattanDistance(beam)).First();

            return closest.X * 10000 + closest.Y;
        }
        
        private bool CoversSimple(List<long> program, int x, int y, int width, int height)
        {
            var computer = new IntCodeComputer(program);
            computer.Execute(x + width - 1, y);
            
            if (computer.Output.Last() == 0)
            {
                return false;
            }

            computer = new IntCodeComputer(program);
            computer.Execute(x, y + height - 1);

            return computer.Output.Last() == 1;
        }


        private void PrintMap(Dictionary<Point, int> map, Point p, int xMax, int yMax)
        {
            for (int y = p.Y; y < p.Y + yMax; y++)
            {
                var s = new StringBuilder();
                for (int x = p.X; x < p.X + xMax; x++)
                {
                    s.Append(map[new Point(x, y)]);
                }
                output.WriteLine(s.ToString());

            }
        }
        public int FirstStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            var map = Scan(program, new Point(0,0), 50, 50);
            return map.Values.Count(v => v == 1);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            var value = FindNew(program, 100, 100, new Point(6, 5));
            return value;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(118, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(18651593, SecondStar());
        }
    }
}
