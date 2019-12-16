using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Shared.Tree;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 15: Oxygen System ---
    public class Day15
    {
        private readonly ITestOutputHelper output;

        public Day15(ITestOutputHelper output)
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
            //private int inputPosition;
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
                //this.inputPosition = 0;

                this.Output = new List<long>();

            }

            public IntCodeComputer(List<long> program, long input)
            {
                this.memory = Load(program);
                this.input = new List<long> { input };
                this.Output = new List<long>();
                this.instructionPointer = 0;
                //this.inputPosition = 0;
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

            public ExecutionState Execute(int input)
            {
                this.input.Add(input);
                ExecutionState state = Execute();
                //if (Output.Last() == 2)
                //{
                //    throw new Exception("Found oxygen");
                //}

                if (state == ExecutionState.Ready)
                {
                    throw new Exception("Terminated");
                }
                return state;
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

                                memory[arg1] = input.Last();//input[inputPosition++];
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

        private static int OppositeDirection(int code)
        {
            switch (code)
            {
                case 1:
                    return 2;
                case 2:
                    return 1;
                case 3:
                    return 4;
                case 4:
                    return 3;
                default:
                    throw new ArgumentOutOfRangeException($"Not a valid direction: {code}");
            }
        }

        private static Direction ToDirection(int code)
        {
            switch (code)
            {
                case 1:
                    return Direction.North;
                case 2:
                    return Direction.South;
                case 3:
                    return Direction.West;
                case 4:
                    return Direction.East;
                default:
                    throw new ArgumentOutOfRangeException($"Not a valid direction: {code}");
            }
        }

        private static int MovementToGetHere(Point from, Point to)
        {
            for (int d = 1; d <= 4; d++)
            {
                var direction = ToDirection(d);
                var next = from.Move(direction);
                if (next == to)
                {
                    return d;
                }
            }
            throw new ArgumentOutOfRangeException($"Impossible to get from {from} to {to} in one step");
        }

        private List<Node<Point>> Explore(Point start, IntCodeComputer computer, Dictionary<Point, long> known, bool stopWhenOxygenFound = true)
        {
            var visited = new HashSet<Point>();
            var depthFirst = new List<Node<Point>>();
            var stack = new Stack<Node<Point>>();
            //var known = new Dictionary<Point, long>();
            stack.Push(new Node<Point>(start, 0));

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current.Data))
                {
                    continue;
                }

                depthFirst.Add(current);
                if (current.Parent != null)
                {
                    var movement = MovementToGetHere(current.Parent.Data, current.Data);
                    computer.Execute(movement);
                }

                if (stopWhenOxygenFound && computer.Output.Any() && computer.Output.Last() == 2)
                {
                    break;
                }

                List<int> possibleMoves = PossibleMoves(computer, current.Data, known);
                IEnumerable<Point> neighbors = possibleMoves.Select(m => current.Data.Move(ToDirection(m)))
                    .Where(n => !visited.Contains(n));
                if (!neighbors.Any() && stack.Any())
                {
                    // must backtrack computer here
                    var backTo = stack.Peek();

                    var next = current;
                    
                    while (!next.Data.AdjacentInMainDirections().Contains(backTo.Data))
                    {
                        var movement = MovementToGetHere(next.Data, next.Parent.Data);
                        computer.Execute(movement);
                        next = next.Parent;
                    }
                }

                foreach (var neighbor in neighbors.Reverse())
                {
                    stack.Push(new Node<Point>(neighbor, current.Depth + 1, current));
                }
            }

            return depthFirst;
        }


        private static List<int> PossibleMoves(IntCodeComputer computer, 
            Point currentPoint,
            Dictionary<Point, long> knowns)
        {
            var possibleMoves = new List<int>();

            for (int d = 1; d <= 4; d++)
            {
                var direction = ToDirection(d);
                var next = currentPoint.Move(direction);

                if (!knowns.ContainsKey(next))
                {
                    var executionState = computer.Execute(d);
                    var output = computer.Output.Last();
                    knowns.Add(next, output);

                    if (output > 0)
                    {
                        computer.Execute(OppositeDirection(d));
                        if (computer.Output.Last() == 2)
                        {
                            throw new Exception("Found oxygen");
                        } 
                    }
                }

                if (knowns[next] > 0)
                {
                    possibleMoves.Add(d);
                }
            }

            return possibleMoves;
        }

        private char GetPrintChar(long code)
        {
            switch (code)
            {
                case 0:
                    return '#';
                case 1:
                    return '.';
                case 2:
                    return 'X';
            }

            return ' ';
        }

        private void PrintWindow(Dictionary<Point, long> visited, Node<Point> current, HashSet<Point> printed)
        {
            Console.SetCursorPosition(current.Data.X + 25, current.Data.Y + 25);
            Console.Write("D");
            if (current.Parent != null)
            {
                Console.SetCursorPosition(current.Parent.Data.X + 25, current.Parent.Data.Y + 25);
                var c = ' ';
                if (visited.ContainsKey(current.Parent.Data))
                {
                    c = GetPrintChar(visited[current.Parent.Data]);
                }
                Console.Write(c);
            }

            foreach (var print in visited.Keys.Where(p => !printed.Contains(p)))
            {
                Console.SetCursorPosition(print.X + 25, print.Y + 25);
                Console.Write(GetPrintChar(visited[print]));
                printed.Add(print);
            }

            Console.SetCursorPosition(0, 41);
            Console.WriteLine($"Current: {current.Data}                            ");
        }

        private void Print(Dictionary<Point, long> knowns)
        {
            var minx = knowns.Keys.Min(p => p.X);
            var miny = knowns.Keys.Min(p => p.Y);
            var maxx = knowns.Keys.Max(p => p.X);
            var maxy = knowns.Keys.Max(p => p.Y);

            for (int y = miny; y <= maxy; y++)
            {
                var line = new StringBuilder();
                for (int x = minx; x <= maxx; x++)
                {
                    var p = new Point(x, y);
                    if (knowns.ContainsKey(p))
                    {
                        line.Append(knowns[p]);
                    }
                    else
                    {
                        line.Append("?");
                    }
                }
                output.WriteLine(line.ToString());
            }
        }

        private int FewestMovementCommandsToOxygenSystem(List<long> program)
        {
            var computer = new IntCodeComputer(program);

            var depthFirstPath = Explore(new Point(0,0), computer, new Dictionary<Point, long>());

            return depthFirstPath.Last().Depth;
        }

        private int TimeToFillWithOxygen(List<long> program)
        {
            var computer = new IntCodeComputer(program);

            var map = new Dictionary<Point, long>();
            var depthFirstPath = Explore(new Point(0, 0), computer, map, false);

            // try to use solution 1 to create the map then depth first from location of oxygen source
            // and take max depth from there

            var start = depthFirstPath.Last().Data;

            List<Point> Neighbors(Point p)
            {
                return p.AdjacentInMainDirections().Where(a => map[a] == 1).ToList();
            }

            var (depthFirst, _) = start.DepthFirst(Neighbors);

            return depthFirst.Max(p => p.Depth);
        }

        public static void ExplorerMode()
        {
            var input = ReadInput();
            var program = Parse(input);
            var computer = new IntCodeComputer(program);
            var visited = new Dictionary<Point, int>();
            var currentLocation = new Point(0,0);
            PrintSurrounding(currentLocation, visited);
            while (true)
            {
                var key = Console.ReadKey(true);
                Point nextLocation;
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        computer.Execute(1);
                        nextLocation = currentLocation.Move(Direction.North);
                        break;
                    case ConsoleKey.DownArrow:
                        computer.Execute(2);
                        nextLocation = currentLocation.Move(Direction.South);
                        break;
                    case ConsoleKey.LeftArrow:
                        computer.Execute(3);
                        nextLocation = currentLocation.Move(Direction.West);
                        break;
                    case ConsoleKey.RightArrow:
                        computer.Execute(4);
                        nextLocation = currentLocation.Move(Direction.East);
                        break;
                    default:
                        continue;
                }

                if (!visited.ContainsKey(nextLocation))
                {
                    visited.Add(nextLocation, (int)computer.Output.Last());
                }

                if (computer.Output.Last() > 0)
                {
                    currentLocation = nextLocation;
                }

                PrintSurrounding(currentLocation, visited);
            }
        }

        private static void PrintSurrounding(Point current, Dictionary<Point, int> visited)
        {
            Console.SetCursorPosition(0,0);
            for (int y = -20; y <= 20; y++)
            {
                var line = new StringBuilder();
                for (int x = -20; x <= 20; x++)
                {
                    var point = new Point(current.X + x, current.Y + y);
                    if (point == current)
                    {
                        line.Append("D");
                    }
                    else
                    {
                        if (!visited.ContainsKey(point))
                        {
                            line.Append(" ");
                        }
                        else
                        {
                            var type = visited[point];
                            line.Append(type == 0 ? "#" : type == 1 ? "." : "¤");
                        }
                    }
                }
                Console.WriteLine(line.ToString());
            }
            Console.Write($"Current position: {current}");
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            return FewestMovementCommandsToOxygenSystem(program);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            return TimeToFillWithOxygen(program);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(244, FirstStar());
            // Assert.Equal(88, FirstStar()); // not correct
            // Assert.Equal(373, FirstStar()); // too high
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
            //Assert.Equal(442, SecondStar()); // too high
            // Assert.Equal(514, SecondStar()); // too high
        }
    }
}
