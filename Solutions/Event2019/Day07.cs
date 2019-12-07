using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 7: Amplification Circuit ---
    public class Day07
    {
        public static List<int> Parse(string input)
        {
            return input.Split(',').Select(int.Parse).ToList();
        }

        public class IntCodeComputer
        {
            private enum Mode { Position = 0, Immediate }
            public enum ExecutionState { WaitingForInput, Ready }

            private class Instruction
            {
                public Instruction(int operation)
                {
                    OperationCode = operation % 100;
                    Parameter1Mode = (Mode)((operation / 100) % 10);
                    Parameter2Mode = (Mode)((operation / 1000) % 10);
                    ParameterModes = new[] {Parameter1Mode, Parameter2Mode};
                }

                public int OperationCode { get; }
                public Mode Parameter1Mode { get; }
                public Mode Parameter2Mode { get; }
                public Mode[] ParameterModes { get; }
            }

            private class ReadParameter
            {
                private readonly int position;
                private readonly IntCodeComputer computer;

                public ReadParameter(int position, IntCodeComputer computer)
                {
                    this.position = position;
                    this.computer = computer;
                }

                public int Get(Instruction instruction)
                {
                    var value = computer.memory[computer.address + position];
                    return instruction.ParameterModes[position] == Mode.Position ? computer.memory[value] : value;
                }
            }



            private readonly List<int> memory;
            private readonly List<int> input;

            private List<int> output;
            private int address;
            private int inputPosition;


            public IntCodeComputer(List<int> program, int input, int phase)
            {
                this.memory = new List<int>(program);
                this.input = new List<int> {phase, input};
                this.output = new List<int>();
                this.address = 0;
                this.inputPosition = 0;
            }

            public IntCodeComputer(IntCodeComputer inputProvider, int phase)
            {
                this.memory = new List<int>(inputProvider.memory);
                this.input = inputProvider.output;
                this.input.Add(phase);
                this.output = new List<int>();
                this.address = 0;
                this.inputPosition = 0;
            }


            public int Output => this.output.Last();

            public void ConnectOutputTo(IntCodeComputer other)
            {
                this.output = other.input;
            }

            private int GetReadParameter(int position, Mode mode)
            {
                var value = memory[address + position];
                return mode == Mode.Position ? memory[value] : value;
            }

            private int GetWriteParameter(int position)
            {
                return GetReadParameter(position, Mode.Immediate);
            }

            public ExecutionState Execute()
            {
                while (true)
                {
                    var instruction = new Instruction(memory[address]);

                    switch (instruction.OperationCode)
                    {
                        case 1:
                            {
                                var arg1 = GetReadParameter(1, instruction.ParameterModes[0]);
                                var arg2 = GetReadParameter(2, instruction.ParameterModes[1]);
                                var writeTo = GetWriteParameter(3);

                                var result = arg1 + arg2;
                                memory[writeTo] = result;
                                address = address + 4;
                                break;
                            }
                        case 2:
                            {
                                var arg1 = GetReadParameter(1, instruction.ParameterModes[0]);
                                var arg2 = GetReadParameter(2, instruction.ParameterModes[1]);
                                var writeTo = GetWriteParameter(3);

                                var result = arg1 * arg2;
                                memory[writeTo] = result;
                                address = address + 4;
                                break;
                            }
                        case 3:
                            {
                                var arg1 = GetWriteParameter(1);
                                memory[arg1] = input[inputPosition++];
                                address = address + 2;
                                break;
                            }
                        case 4:
                            {
                                var arg1 = GetReadParameter(1, instruction.ParameterModes[0]);

                                output.Add(arg1);
                                address = address + 2;
                                return ExecutionState.WaitingForInput;
                            }
                        case 5:
                            {
                                var arg1 = GetReadParameter(1, instruction.ParameterModes[0]);
                                var arg2 = GetReadParameter(2, instruction.ParameterModes[1]);

                                if (arg1 != 0)
                                {
                                    address = arg2;
                                }
                                else
                                {
                                    address = address + 3;
                                }

                                break;
                            }
                        case 6:
                            {
                                var arg1 = GetReadParameter(1, instruction.ParameterModes[0]);
                                var arg2 = GetReadParameter(2, instruction.ParameterModes[1]);

                                if (arg1 == 0)
                                {
                                    address = arg2;
                                }
                                else
                                {
                                    address = address + 3;
                                }

                                break;
                            }
                        case 7:
                            {
                                var arg1 = GetReadParameter(1, instruction.ParameterModes[0]);
                                var arg2 = GetReadParameter(2, instruction.ParameterModes[1]);
                                var writeTo = GetWriteParameter(3);

                                if (arg1 < arg2)
                                {
                                    memory[writeTo] = 1;
                                }
                                else
                                {
                                    memory[writeTo] = 0;
                                }

                                address = address + 4;
                                break;
                            }
                        case 8:
                            {
                                var arg1 = GetReadParameter(1, instruction.ParameterModes[0]);
                                var arg2 = GetReadParameter(2, instruction.ParameterModes[1]);
                                var writeTo = GetWriteParameter(3);

                                if (arg1 == arg2)
                                {
                                    memory[writeTo] = 1;
                                }
                                else
                                {
                                    memory[writeTo] = 0;
                                }

                                address = address + 4;
                                break;
                            }
                        case 99:
                            {
                                return ExecutionState.Ready;
                            }
                        default:
                            throw new InvalidOperationException(
                                $"Unknown op '{instruction.OperationCode}' code at address = {address}");
                    }
                }
            }
        }

        public int SignalToThrusters(List<int> code, IList<int> phaseSetting)
        {
            var computer1 = new IntCodeComputer(code, 0, phaseSetting[0]);
            var computer2 = new IntCodeComputer(computer1, phaseSetting[1]);
            var computer3 = new IntCodeComputer(computer2, phaseSetting[2]);
            var computer4 = new IntCodeComputer(computer3, phaseSetting[3]);
            var computer5 = new IntCodeComputer(computer4, phaseSetting[4]);

            computer1.Execute();
            computer2.Execute();
            computer3.Execute();
            computer4.Execute();
            computer5.Execute();

            return computer5.Output;
        }

        public int MaxSignalToThrusters(List<int> code)
        {
            var phases = new List<int> {0, 1, 2, 3, 4};
            var allPossiblePhaseSettings = phases.Permutations();
            return allPossiblePhaseSettings.Select(phaseSetting => SignalToThrusters(code, phaseSetting)).Max();
        }

        public int SignalToThrustersWithFeedbackLoop(List<int> code, IList<int> phaseSetting)
        {
            var computer1 = new IntCodeComputer(code, 0, phaseSetting[0]);
            var computer2 = new IntCodeComputer(computer1, phaseSetting[1]);
            var computer3 = new IntCodeComputer(computer2, phaseSetting[2]);
            var computer4 = new IntCodeComputer(computer3, phaseSetting[3]);
            var computer5 = new IntCodeComputer(computer4, phaseSetting[4]);
            computer5.ConnectOutputTo(computer1);

            IntCodeComputer.ExecutionState computer5State;
            do
            {
                computer1.Execute();
                computer2.Execute();
                computer3.Execute();
                computer4.Execute();
                computer5State = computer5.Execute();
            } while (computer5State != IntCodeComputer.ExecutionState.Ready);

            return computer5.Output;
        }

        public int MaxSignalToThrustersWithFeedbackLoop(List<int> code)
        {
            var phases = new List<int> { 5, 6, 7, 8, 9 };
            var allPossiblePhaseSettings = phases.Permutations();
            return allPossiblePhaseSettings.Select(phaseSetting => SignalToThrustersWithFeedbackLoop(code, phaseSetting)).Max();
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var code = Parse(input);
            return MaxSignalToThrusters(code);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var code = Parse(input);
            return MaxSignalToThrustersWithFeedbackLoop(code);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(929800, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(15432220, SecondStar());
        }

        [Theory]
        [InlineData("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", "4,3,2,1,0", 43210)]
        [InlineData("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", "0,1,2,3,4", 54321)]
        [InlineData("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", "1,0,4,3,2", 65210)]
        public void FirstStarExamples(string input, string phaseSetting, int expectedMax)
        {
            var code = Parse(input);
            var signal = SignalToThrusters(code, phaseSetting.Split(',').Select(int.Parse).ToList());
            Assert.Equal(expectedMax, signal);
        }        
        
        [Theory]
        [InlineData("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5", "9,8,7,6,5", 139629729)]
        [InlineData("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10", "9,7,8,5,6", 18216)]
        public void SecondStarExamples(string input, string phaseSetting, int expectedMax)
        {
            var code = Parse(input);
            var signal = SignalToThrustersWithFeedbackLoop(code, phaseSetting.Split(',').Select(int.Parse).ToList());
            Assert.Equal(expectedMax, signal);
        }
    }
}
