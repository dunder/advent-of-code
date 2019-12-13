using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 13: Care Package ---
    public class Day13
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

                this.Output = new List<long>();

            }

            public IntCodeComputer(List<long> program, long input)
            {
                this.memory = Load(program);
                this.input = new List<long> { input };
                this.Output = new List<long>();
                this.instructionPointer = 0;
            }

            public void SetInput(long inputValue)
            {
                this.input.Add(inputValue);
            }

            public void EnterQuarters(int quarters)
            {
                memory[0] = quarters;
                instructionPointer++;
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

                                memory[arg1] = input.Last();
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

            public void ExecuteAll()
            {
                var state = ExecutionState.WaitingForInput;
                while (state != ExecutionState.Ready)
                {
                    state = Execute();
                }
            }
        }

        private class GameState
        {
            public int Score { get; set; }
            public Point BallPosition { get; set; }
            public Point PreviousBallPosition { get; set; }

            public Point PaddlePosition { get; set; }
            public int OutputConsumed { get; set; }
        }

        private static int Update(List<long> output, GameState state)
        {
            var newInstructions = output.Skip(state.OutputConsumed).ToList();

            if (newInstructions.Count % 3 != 0)
            {
                return 0;
            }
            var batches = newInstructions.Batch(3).ToList();
            foreach (var batch in batches)
            {
                var input = batch.ToList();
                var x = (int)input[0];
                var y = (int)input[1];
                var tile = (int)input[2];

                if (x == -1 && y == 0)
                {
                    state.Score = tile;
                    continue;
                }

                if (tile == 4)
                {
                    state.PreviousBallPosition = state.BallPosition;
                    state.BallPosition = new Point(x,y);
                }
                else if (tile == 3)
                {
                    state.PaddlePosition = new Point(x,y);
                }
            }

            state.OutputConsumed += newInstructions.Count;

            var ballXDiff = state.BallPosition.X - state.PreviousBallPosition.X;
            var ballYDiff = state.BallPosition.Y - state.PreviousBallPosition.Y;
            var joystickCommand = 0;
            if (ballYDiff > 0)
            {
                // try to forecast ball position 
                var guessedX = ballYDiff / ballXDiff * (state.PaddlePosition.Y - state.BallPosition.Y) + state.BallPosition.X;
                if (guessedX > state.PaddlePosition.X)
                {
                    joystickCommand = 1;
                }
                else if (guessedX < state.PaddlePosition.X)
                {
                    joystickCommand = -1;
                }
            }
            else if (ballYDiff <= 0)
            {
                // just follow ball when going up
                var paddleBallXDiff = state.BallPosition.X - state.PaddlePosition.X;
                joystickCommand = Math.Sign(paddleBallXDiff);
            }

            return joystickCommand;
        }

        private static int RunGame(List<long> program)
        {
            var computer = new IntCodeComputer(program);
            computer.ExecuteAll();
            
            var ball = computer.Output
                .Batch(3)
                .Single(b => b.Last() == 4)
                .ToList();

            var ballPosition = new Point((int) ball[0], (int) ball[1]);
            var paddle = computer.Output
                .Batch(3)
                .Single(b => b.Last() == 3)
                .ToList();
            var paddlePosition = new Point((int)paddle[0], (int)paddle[1]);

            var gameState = new GameState
            {
                Score = 0,
                BallPosition = ballPosition,
                PreviousBallPosition = ballPosition,
                PaddlePosition = paddlePosition,
                OutputConsumed = computer.Output.Count
            };

            computer.EnterQuarters(2);
            var executionState = IntCodeComputer.ExecutionState.WaitingForInput;
            while (executionState == IntCodeComputer.ExecutionState.WaitingForInput)
            {
                executionState = computer.Execute();
                int joystickSignal = Update(computer.Output, gameState);
                computer.SetInput(joystickSignal);
            }

            return gameState.Score;
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            var computer = new IntCodeComputer(program);
            computer.ExecuteAll();
            var blockTiles = computer.Output.Batch(3).Select(b => b.Last()).Count(t => t == 2);
            return blockTiles;
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var program = Parse(input);
            var score = RunGame(program);
            
            return score;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(333, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(16539, SecondStar());
        }
    }
}
