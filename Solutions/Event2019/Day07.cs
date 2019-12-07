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

        public int SignalToThrust(List<int> code, IList<int> phaseSetting)
        {
            var code1 = new List<int>(code);

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

            return 0;
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
    }
}
