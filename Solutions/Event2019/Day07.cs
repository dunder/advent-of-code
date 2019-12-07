using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private enum Mode
        {
            Position = 0,
            Immediate
        }

        private class Instruction
        {
            public Instruction(int operation)
            {
                OperationCode = operation % 100;
                Parameter1Mode = (Mode)((operation / 100) % 10);
                Parameter2Mode = (Mode)((operation / 1000) % 10);
            }

            public int OperationCode { get; }
            public Mode Parameter1Mode { get; }
            public Mode Parameter2Mode { get; }


        }

        public static int Execute(List<int> code, int[] inputs)
        {
            var output = new List<int>();
            var inputCounter = 0;

            for (int i = 0; i < code.Count;)
            {
                var instruction = new Instruction(code[i]);

                switch (instruction.OperationCode)
                {
                    case 1:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            var result = arg1 + arg2;
                            code[writeTo] = result;
                            i = i + 4;
                            break;
                        }
                    case 2:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            var result = arg1 * arg2;
                            code[writeTo] = result;
                            i = i + 4;
                            break;
                        }
                    case 3:
                        {
                            var arg1 = code[i + 1];
                            code[arg1] = inputs[inputCounter++];
                            i = i + 2;
                            break;
                        }
                    case 4:
                        {
                            var arg1 = code[i + 1];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            output.Add(arg1);
                            i = i + 2;
                            break;
                        }
                    case 5:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            if (arg1 != 0)
                            {
                                i = arg2;
                            }
                            else
                            {
                                i = i + 3;
                            }
                            break;
                        }
                    case 6:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            if (arg1 == 0)
                            {
                                i = arg2;
                            }
                            else
                            {
                                i = i + 3;
                            }
                            break;
                        }
                    case 7:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];

                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            if (arg1 < arg2)
                            {
                                code[writeTo] = 1;
                            }
                            else
                            {
                                code[writeTo] = 0;
                            }

                            i = i + 4;
                            break;
                        }
                    case 8:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];

                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            if (arg1 == arg2)
                            {
                                code[writeTo] = 1;
                            }
                            else
                            {
                                code[writeTo] = 0;
                            }

                            i = i + 4;
                            break;
                        }
                    case 99:
                        {
                            return output.Last();
                        }
                    default:
                        throw new InvalidOperationException($"Unknown op '{instruction.OperationCode}' code at i = {i}");
                }
            }
            throw new InvalidOperationException("Unexpected program exit");
        }


        private class Amplifier
        {
            private readonly List<int> code;
            private readonly List<int> input;
            private readonly List<int> output;
            private int inputCounter;
            private int i;

            public Amplifier(List<int> code, List<int> input, List<int> output)
            {
                this.code = code;
                this.input = input;
                this.output = output;
                this.inputCounter = 0;
                this.i = 0;
            }

            public bool Execute()
            {
                while (true)
                {
                    var instruction = new Instruction(code[i]);

                    switch (instruction.OperationCode)
                    {
                        case 1:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            var result = arg1 + arg2;
                            code[writeTo] = result;
                            i = i + 4;
                            break;
                        }
                        case 2:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            var result = arg1 * arg2;
                            code[writeTo] = result;
                            i = i + 4;
                            break;
                        }
                        case 3:
                        {
                            var arg1 = code[i + 1];
                            //if (inputCounter > input.Count - 1)
                            //{
                            //    return false;
                            //}
                            code[arg1] = input[inputCounter++];
                            i = i + 2;
                            break;
                        }
                        case 4:
                        {
                            var arg1 = code[i + 1];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            output.Add(arg1);
                            i = i + 2;
                            return false;
                        }
                        case 5:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            if (arg1 != 0)
                            {
                                i = arg2;
                            }
                            else
                            {
                                i = i + 3;
                            }

                            break;
                        }
                        case 6:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            if (arg1 == 0)
                            {
                                i = arg2;
                            }
                            else
                            {
                                i = i + 3;
                            }

                            break;
                        }
                        case 7:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];

                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            if (arg1 < arg2)
                            {
                                code[writeTo] = 1;
                            }
                            else
                            {
                                code[writeTo] = 0;
                            }

                            i = i + 4;
                            break;
                        }
                        case 8:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];

                            arg1 = instruction.Parameter1Mode == Mode.Position ? code[arg1] : arg1;
                            arg2 = instruction.Parameter2Mode == Mode.Position ? code[arg2] : arg2;
                            if (arg1 == arg2)
                            {
                                code[writeTo] = 1;
                            }
                            else
                            {
                                code[writeTo] = 0;
                            }

                            i = i + 4;
                            break;
                        }
                        case 99:
                        {
                            return true;
                        }
                        default:
                            throw new InvalidOperationException(
                                $"Unknown op '{instruction.OperationCode}' code at i = {i}");
                    }
                }

                throw new InvalidOperationException("Unexpected program exit");
            }
        }

        public int SignalToThrust(List<int> code, IList<int> phaseSetting)
        {
            int input2 = Execute(code, new [] {phaseSetting[0],0});
            int input3 = Execute(code, new [] {phaseSetting[1],input2});
            int input4 = Execute(code, new [] {phaseSetting[2],input3});
            int input5 = Execute(code, new [] {phaseSetting[3],input4});
            int thrustInput = Execute(code, new [] {phaseSetting[4],input5});

            return thrustInput;
        }

        public int CalculateMaxSignalToThrusters(List<int> code)
        {
            var phases = new List<int> {0, 1, 2, 3, 4};
            var allPossiblePhaseSettings = phases.Permutations();
            return allPossiblePhaseSettings.Select(phaseSetting => SignalToThrust(code, phaseSetting)).Max();
        }

        public int SignalToThrust2(List<int> code, IList<int> phaseSetting)
        {
            var code1 = new List<int>(code);
            var code2 = new List<int>(code);
            var code3 = new List<int>(code);
            var code4 = new List<int>(code);
            var code5 = new List<int>(code);

            var amplifier1Input = new List<int> {phaseSetting[0], 0};
            var amplifier1Output = new List<int> {phaseSetting[1]};
            var amplifier2Output = new List<int> {phaseSetting[2]};
            var amplifier3Output = new List<int> {phaseSetting[3]};
            var amplifier4Output = new List<int> {phaseSetting[4]};

            var amplifier1 = new Amplifier(code1, amplifier1Input, amplifier1Output);
            var amplifier2 = new Amplifier(code2, amplifier1Output, amplifier2Output);
            var amplifier3 = new Amplifier(code3, amplifier2Output, amplifier3Output);
            var amplifier4 = new Amplifier(code4, amplifier3Output, amplifier4Output);
            var amplifier5 = new Amplifier(code5, amplifier4Output, amplifier1Input);

            bool ready;
            do
            {
                amplifier1.Execute();
                amplifier2.Execute();
                amplifier3.Execute();
                amplifier4.Execute();
                ready = amplifier5.Execute();
            } while (!ready);

            return amplifier1Input.Last();
        }

        public int CalculateMaxSignalToThrusters2(List<int> code)
        {
            var phases = new List<int> { 5, 6, 7, 8, 9 };
            var allPossiblePhaseSettings = phases.Permutations();
            return allPossiblePhaseSettings.Select(phaseSetting => SignalToThrust2(code, phaseSetting)).Max();
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var code = Parse(input);
            return CalculateMaxSignalToThrusters(code);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var code = Parse(input);
            return CalculateMaxSignalToThrusters2(code);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(929800, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Theory]
        [InlineData("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", "4,3,2,1,0", 43210)]
        [InlineData("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", "0,1,2,3,4", 54321)]
        [InlineData("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", "1,0,4,3,2", 65210)]
        public void FirstStarExamples(string input, string phaseSetting, int expectedMax)
        {
            var code = Parse(input);
            var signal = SignalToThrust(code, phaseSetting.Split(',').Select(int.Parse).ToList());
            Assert.Equal(expectedMax, signal);
        }        
        
        [Theory]
        [InlineData("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5", "9,8,7,6,5", 139629729)]
        [InlineData("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10", "9,7,8,5,6", 18216)]
        public void SecondStarExamples(string input, string phaseSetting, int expectedMax)
        {
            var code = Parse(input);
            var signal = SignalToThrust2(code, phaseSetting.Split(',').Select(int.Parse).ToList());
            Assert.Equal(expectedMax, signal);
        }
    }
}
