using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day16 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day16;
        public string Name => "Chronal Classification";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = RunSamples(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = RunProgram(input);
            return result.ToString();
        }

        private static readonly Dictionary<string, Func<int[], int, int, int, int, int[]>> Instructions = new Dictionary<string, Func<int[], int, int, int, int, int[]>>
        {
            {"addr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] + rin[b];

                return rout.ToArray();
            }},
            { "addi", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] + b;

                return rout.ToArray();
            }},
            { "mulr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] * rin[b];

                return rout.ToArray();
            }},
            { "muli", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] * b;

                return rout.ToArray();
            }},
            { "banr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] & rin[b];

                return rout.ToArray();
            }},
            { "bani", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] & b;

                return rout.ToArray();
            }},
            { "borr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] | rin[b];

                return rout.ToArray();
            }},
            { "bori", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] | b;

                return rout.ToArray();
            }},
            { "setr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a];

                return rout.ToArray();
            }},
            { "seti", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = a;

                return rout.ToArray();
            }},
            { "gtir", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = a > rin[b] ? 1 : 0;

                return rout.ToArray();
            }},
            { "gtri", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] > b ? 1 : 0;

                return rout.ToArray();
            }},
            { "gtrr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] > rin[b] ? 1 : 0;

                return rout.ToArray();
            }},
            { "eqir", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = a == rin[b] ? 1 : 0;

                return rout.ToArray();
            }},
            { "eqri", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] == b ? 1 : 0;

                return rout.ToArray();
            }},
            { "eqrr", (rin, opCode, a, b, c) =>
            {
                var rout = rin.ToList();

                rout[c] = rin[a] == rin[b] ? 1 : 0;

                return rout.ToArray();
            }}
        };


        public int RunSamples(IList<string> input)
        {
            var samples = ReadSamples(input);

            var sampleCounter = 0;

            for (int i = 0; i < samples.Count - 3; i += 4)
            {
                var sampleIn = samples[i];
                var argumentsIn = samples[i + 1];
                var sampleOut = samples[i + 2];

                var registerBefore = sampleIn
                    .Substring(9, 10)
                    .Replace(" ", "")
                    .Split(",")
                    .Select(int.Parse)
                    .ToArray();

                var arguments = argumentsIn
                    .Split(" ")
                    .Select(int.Parse)
                    .ToArray();

                var registerAfter = sampleOut
                    .Substring(9, 10)
                    .Replace(" ", "")
                    .Split(",")
                    .Select(int.Parse)
                    .ToArray();

                var op = arguments[0];
                var a = arguments[1];
                var b = arguments[2];
                var c = arguments[3];

                var sampleTest = 0;

                foreach (var instructionName in Instructions.Keys)
                {
                    var instruction = Instructions[instructionName];

                    var rout = instruction(registerBefore, op, a, b, c);

                    if (registerAfter.SequenceEqual(rout))
                    {
                        sampleTest++;
                    }
                }

                if (sampleTest > 2)
                {
                    sampleCounter++;
                }
            }

            return sampleCounter;
        }

        public Dictionary<int, string> UnderstandMapping(IList<string> input)
        {
            var sampleMatches = new Dictionary<int, HashSet<string>>();
            var samples = ReadSamples(input);

            for (int i = 0; i < samples.Count - 3; i += 4)
            {
                var sampleIn = samples[i];
                var argumentsIn = samples[i + 1];
                var sampleOut = samples[i + 2];

                var registerBefore = sampleIn
                    .Substring(9, 10)
                    .Replace(" ", "")
                    .Split(",")
                    .Select(int.Parse)
                    .ToArray();

                var arguments = argumentsIn
                    .Split(" ")
                    .Select(int.Parse)
                    .ToArray();

                var registerAfter = sampleOut
                    .Substring(9, 10)
                    .Replace(" ", "")
                    .Split(",")
                    .Select(int.Parse)
                    .ToArray();

                var op = arguments[0];
                var a = arguments[1];
                var b = arguments[2];
                var c = arguments[3];

                foreach (var instructionName in Instructions.Keys)
                {
                    var instruction = Instructions[instructionName];

                    var rout = instruction(registerBefore, op, a, b, c);

                    if (registerAfter.SequenceEqual(rout))
                    {
                        if (!sampleMatches.ContainsKey(op))
                        {
                            sampleMatches[op] = new HashSet<string>();
                        }

                        sampleMatches[op].Add(instructionName);
                    }
                }
            }

            var opToInstructionMappings = new Dictionary<int, string>();

            while (opToInstructionMappings.Count < Instructions.Count)
            {
                var singleMatches = sampleMatches.Where(s => s.Value.Count == 1).ToList();

                foreach (var singleMatch in singleMatches)
                {
                    var op = singleMatch.Key;
                    var matchingInstructionName = singleMatch.Value.Single();

                    opToInstructionMappings.Add(op, matchingInstructionName);
                    sampleMatches.Remove(op);

                    foreach (var instructionSet in sampleMatches.Values)
                    {
                        instructionSet.Remove(matchingInstructionName);
                    }
                }
            }

            return opToInstructionMappings;

            //return new Dictionary<int, string>
            //{
            //  {6 , "addr"}, 
            //  {9, "addi"}, 
            //  {14, "mulr"}, 
            //  {1 , "muli"}, 
            //  {2 , "banr"}, 
            //  {3 , "bani"}, 
            //  {12, "borr"}, 
            //  {0 , "bori"}, 
            //  {5 , "setr"}, 
            //  {8 , "seti"}, 
            //  {4 , "gtir"}, 
            //  {15, "gtri"}, 
            //  {13, "gtrr"}, 
            //  {7 , "eqir"}, 
            //  {11, "eqri"},
            //  { 10, "eqrr"}, 
            //};
        }

        public int RunProgram(IList<string> input)
        {
            var instructions = ReadInstructions(input);

            var mapping = UnderstandMapping(input);

            var register = new[] {0, 0, 0, 0};

            foreach (var argumentsIn in instructions)
            {
                var arguments = argumentsIn
                    .Split(" ")
                    .Select(int.Parse)
                    .ToArray();

                var op = arguments[0];
                var a = arguments[1];
                var b = arguments[2];
                var c = arguments[3];

                var instruction = mapping[op];

                register = Instructions[instruction](register, op, a, b, c);
            }

            return register[0];
        }

        public static IList<string> ReadSamples(IList<string> input)
        {
            var samples = input.Take(3260).ToList();

            return samples;
        }


        public static IList<string> ReadInstructions(IList<string> input)
        {
            var instructions = input.Skip(3262).ToList();

            return instructions;
        }

        [Fact]
        public void Addr()
        {
            var instruction = Instructions["addr"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 1, 2, 3);

            Assert.Equal(new[] { 1, 2, 3, 5 }, rout);
        }

        [Fact]
        public void Addi()
        {
            var instruction = Instructions["addi"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 1, 7, 3);

            Assert.Equal(new[] { 1, 2, 3, 9 }, rout);
        }

        [Fact]
        public void Mulr()
        {
            var instruction = Instructions["mulr"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 1, 2, 3);

            Assert.Equal(new[] { 1, 2, 3, 6 }, rout);
        }

        [Fact]
        public void Muli()
        {
            var instruction = Instructions["muli"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 1, 7, 3);

            Assert.Equal(new[] { 1, 2, 3, 14 }, rout);
        }

        [Fact]
        public void Banr()
        {
            var instruction = Instructions["banr"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 1, 2, 3);

            Assert.Equal(new[] { 1, 2, 3, 2 }, rout);
        }

        [Fact]
        public void Bani()
        {
            var instruction = Instructions["bani"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 1, 7, 3);

            Assert.Equal(new[] { 1, 2, 3, 2 }, rout);
        }

        [Fact]
        public void Borr()
        {
            var instruction = Instructions["borr"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 1, 2, 3);

            Assert.Equal(new[] { 1, 2, 3, 3 }, rout);
        }

        [Fact]
        public void Bori()
        {
            var instruction = Instructions["bori"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 1, 7, 3);

            Assert.Equal(new[] { 1, 2, 3, 7 }, rout);
        }

        [Fact]
        public void Setr()
        {
            var instruction = Instructions["setr"];

            var rin = new[] { 1, 2, 3, 0 };

            var rout = instruction(rin, 0, 1, 2, 3);

            Assert.Equal(new[] { 1, 2, 3, 2 }, rout);
        }

        [Fact]
        public void Seti()
        {
            var instruction = Instructions["seti"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 7, 8, 3);

            Assert.Equal(new[] { 1, 2, 3, 7 }, rout);
        }

        [Fact]
        public void Gtir()
        {
            var instruction = Instructions["gtir"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 7, 3, 3);

            Assert.Equal(new[] { 1, 2, 3, 1 }, rout);
        }

        [Fact]
        public void Gtri()
        {
            var instruction = Instructions["gtri"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 3, 2, 3);

            Assert.Equal(new[] { 1, 2, 3, 1 }, rout);
        }

        [Fact]
        public void Gtrr()
        {
            var instruction = Instructions["gtrr"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 3, 2, 3);

            Assert.Equal(new[] { 1, 2, 3, 1 }, rout);
        }

        [Fact]
        public void Eqir()
        {
            var instruction = Instructions["eqir"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 4, 3, 3);

            Assert.Equal(new[] { 1, 2, 3, 1 }, rout);
        }

        [Fact]
        public void Eqri()
        {
            var instruction = Instructions["eqri"];

            var rin = new[] { 1, 2, 3, 4 };

            var rout = instruction(rin, 0, 3, 4, 3);

            Assert.Equal(new[] { 1, 2, 3, 1 }, rout);
        }

        [Fact]
        public void Eqrr()
        {
            var instruction = Instructions["eqrr"];

            var rin = new[] { 1, 2, 2, 4 };

            var rout = instruction(rin, 0, 1, 1, 3);

            Assert.Equal(new[] { 1, 2, 2, 1 }, rout);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("651", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("706", actual);
        }
    }
}