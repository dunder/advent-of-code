using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 17: Set and Forget ---
    public class Day17
    {
        private readonly ITestOutputHelper output;

        public Day17(ITestOutputHelper output)
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

        private static string ToChar(int i)
        {
            switch (i)
            {
                case 35:
                    return "#";
                case 46:
                    return ".";
                case 10:
                    return Environment.NewLine;
                default:
                    throw new ArgumentOutOfRangeException($"No character defined for {i}");
            }
        }

        private static bool IsConjunction(Point aPoint, Dictionary<Point, long> map)
        {
            return map[aPoint] == 35 && aPoint.AdjacentInMainDirections().All(p => map.ContainsKey(p) && map[p] == 35);
        }

        private void Print(Dictionary<Point, long> map, int width)
        {
            var height = map.Count / width;
            for (int y = 0; y < height; y++)
            {
                var line = new StringBuilder();
                for (int x = 0; x < width; x++)
                {
                    if (map.ContainsKey(new Point(x, y)))
                    {
                        var mapPoint = map[new Point(x, y)];
                        if (mapPoint == 35)
                        {
                            line.Append("#");
                        }
                        else
                        {
                            line.Append(".");
                        }
                    }
                    
                }
                output.WriteLine(line.ToString());
            }
        }

        private int Run(List<long> program)
        {
            var computer = new IntCodeComputer(program);
            computer.ExecuteAll();
            var mapData = computer.Output;
            var map = new Dictionary<Point, long>();
            // 35 means #, 46 means ., 10 starts a new line
            var width = mapData.IndexOf(10);
            var y = 0;
            for (int i = 0; i < mapData.Count; i++)
            {

                var mapPoint = mapData[i];
                if (mapPoint == 10)
                {
                    y++;
                }
                var x = i - y*width - y;

                if (mapPoint != 10)
                {
                    map.Add(new Point(x,y), mapPoint);
                }
            }
            Print(map, width);
            var conjunctions = map.Keys.Where(p => IsConjunction(p, map)).ToList();
            return conjunctions.Select(p => p.X * p.Y).Sum();
        }

        private int Run2(List<long> program)
        {
            program[0] = 2;

            // ',' = 44
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            return Run(program);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var program = Parse(input);

            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(12512, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }
    }
}
