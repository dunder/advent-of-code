using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2021
{
    // --- Day 24: Arithmetic Logic Unit ---
    public class Day24
    {
        private readonly ITestOutputHelper output;

        public Day24(ITestOutputHelper output)
        {
            this.output = output;
        }

        // I liked this implementation and used it to try to analyze the problem initially but it is not needed in
        // any of the two solutions I wrote
        private static void RunALU(List<string> program, List<long> variables, List<int> input)
        {
            int Index(string variable) => variable switch { "w" => 0, "x" => 1, "y" => 2, "z" => 3, _ => throw new ArgumentOutOfRangeException(nameof(variable)) };

            long Read(string variable)
            {
                switch (variable)
                {
                    case "w":
                    case "x":
                    case "y":
                    case "z":
                        return variables[Index(variable)];
                    default:
                        return int.Parse(variable);
                }
                
            }

            int i = 0;

            void Write(string variable, long value)
            {
                variables[Index(variable)] = value;
            }

            foreach (string instruction in program)
            {
                List<string> parts = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                switch (parts)
                {
                    case ["inp", string a]:
                        Write(a, input[i++]);
                        break;
                    case ["add", string a, string b]:
                        Write(a, Read(a) + Read(b));
                        break;
                    case ["mul", string a, string b]:
                        Write(a, Read(a) * Read(b));
                        break;
                    case ["div", string a, string b]:
                        Write(a, Read(a) / Read(b));
                        break;
                    case ["mod", string a, string b]:
                        Write(a, Read(a) % Read(b));
                        break;
                    case ["eql", string a, string b]:
                        if (Read(a) == Read(b))
                        {
                            Write(a, 1);
                        }
                        else
                        {
                            Write(a, 0);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown instruction: {instruction}");
                }
            }
        }

        // use this method to get an overview of the sub routines for each input
        // it prints each sub routine in its own column making it easier to get an idea of how the program works
        private static string SubroutinesToColumns(List<List<string>> subroutines)
        {
            StringBuilder sb = new StringBuilder();

            for (int row = 0; row < subroutines[0].Count; row++)
            {
                for (int chunk = 0; chunk < subroutines.Count; chunk++)
                {
                    sb.Append(subroutines[chunk][row]);
                    sb.Append("\t\t");
                }
                sb.AppendLine();
            }

            var output = sb.ToString();

            return output;
        }

        private static long RunCompressedALU(int subroutine, int w, long z, List<(int i, int b, int c)> variables)
        {
            int b = variables[subroutine].b;
            int c = variables[subroutine].c;
            int a = b > 0 ? 1 : 26;

            long x = z % 26 + b == w ? 0 : 1;

            return z / a * (25 * x + 1) + (w + c) * x;
        }

        // alternative recursive solution that works well for first problem but takes 20 seconds for second
        private static long SolveRecursive(IList<string> input, bool solveForMax = true)
        {
            List<List<string>> subroutines = Subroutines(input);

            List<(int i, int b, int c)> variables = SubroutineVariables(subroutines);

            HashSet<(int subroutine, long z)> invalid = [];

            IEnumerable<int> wRange = Enumerable.Range(1, 9).ToList();

            if (solveForMax)
            {
                wRange = wRange.Reverse();
            }

            long? Solve(int subroutine, long modelNumber, long z)
            {
                if (invalid.Contains((subroutine, z)) || subroutine == subroutines.Count)
                {
                    return null;
                }

                modelNumber = modelNumber * 10;

                foreach (var w in wRange)
                {
                    long zOut = RunCompressedALU(subroutine, w, z, variables);

                    if (subroutine == subroutines.Count - 1 && zOut == 0)
                    {
                        return modelNumber + w;
                    }

                    var next = Solve(subroutine + 1, modelNumber + w, zOut);

                    if (next.HasValue)
                    {
                        return next.Value;
                    }
                }

                invalid.Add((subroutine, z));

                return null;
            }


            return Solve(0, 0, 0).Value;
        }

        private static List<List<string>> Subroutines(IList<string> program)
        {
            return program.Chunk(program.Count / 14).Select(chunk => chunk.ToList()).ToList();
        }

        private static List<(int i, int b, int c)> SubroutineVariables(List<List<string>> subroutines)
        {
            int ReadArgument(List<string> subroutine, int instruction)
            {
                return int.Parse(subroutine[instruction].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
            }

            return subroutines.Select((s, i) => (i, ReadArgument(s, 5), ReadArgument(s, 15))).ToList();
        }

        private static double Solve(IList<string> input, bool solveForMax = true)
        {
            // looking at the code it is quite obvious that the code is divided into 14 subroutines each 
            // reading one digit of the 14 digit input

            List<List<string>> subroutines = Subroutines(input);

            // use this to illustrate the subroutines next to each other to see common patterns between the
            // different subroutines

            var displaySubroutines = SubroutinesToColumns(subroutines);

            // each subroutine has the following structure

            // inp w
            // mul x 0
            // add x z
            // mod x 26
            // div z a
            // add x b
            // eql x w
            // eql x 0
            // mul y 0
            // add y 25
            // mul y x
            // add y 1
            // mul z y
            // mul y 0
            // add y w
            // add y c
            // mul y x
            // add z y

            // there are only three variables between the different subroutines, a, b and c. The variable a
            // either have the value 1 or the value 26. For all subroutines where a is 1 the value of b is
            // positive, for all subroutines where a is 26 its value of b is negative.

            // the code can be expressed and simplified like this

            // w = input
            // x = x * 0             |
            // x = x + z             |
            // x = x % 26            |-> x = z % 26

            // z = z / a

            // x = x + b             | -> x = z % 26 + b

            // x = x == w ? 1 : 0    |
            // x = x == 0 ? 1 : 0    | -> x = z % 26 + b == w ? 0 : 1

            // y = 0                 |
            // y = 25                |
            // y = y * x             |
            // y = y + 1             | -> y = 25*x + 1 where x is 0 or 1

            // z = z * y             | -> z = z / a * (25*x + 1)

            // y = 0                 |
            // y = y + w             |
            // y = y + c             |
            // y = y * x             | -> y = (w + c) * x

            // z = z + y             | -> z = z / a * (25*x + 1) + (w + c) * x

            // thus there is a condition that will determine the value of x in the end:

            // x = z % 26 + b == w ? 0 : 1

            // this means that the z function can take two shapes:

            // x = 0: z = z / a

            // x = 1: z = z * 26 / a + w + c

            // another observation is that for those subroutines where b is positive b is always > 9 which 
            // means that z % 26 + b > w (the eq condition will never be fulfilled) since w is a digit between
            // 1 and 9 so this subroutine will always have x = 1 and thus z is always:

            // z = z * 26 + w + c and with c always > 0 this is always increasing z

            // for a subroutine with negative b to reduce z we must meet the condition

            // z % 26 + b == w for z to be reduced:

            // z = z / 26

            // how do we balance those subroutines so that z = 0 after the last subroutine is executed?

            // here is the key insight that I did not figure out myself:

            // the subroutines with a = 26 and b > 9 are basically pushing base 26 values of (w + c) onto a 
            // stack and subroutines with a = 1 and b < 0 are poping from the stack, the stack is empty when
            // z = 0 and thus we must manage to pop all pushed values for a valid model number
            // we must match pushed and poped values to end up with a valid model number

            // the pushed w + c must match the poped w - b
            
            List<(int i, int b, int c)> subroutineVariables = SubroutineVariables(subroutines);

            Stack<(int i, int b, int c)> stack = [];

            List<(int i, int z)> output = [];

            foreach ((int i, int b, int c) in subroutineVariables)
            {
                if (b > 0)
                {
                    stack.Push((i, b, c));
                }
                else
                {
                    (int i2, int b2, int c2) = stack.Pop();

                    bool found = false;

                    IEnumerable<int> w2Range = Enumerable.Range(1, 9);

                    if (solveForMax)
                    {
                        w2Range = w2Range.Reverse();
                    }

                    foreach (int w2 in w2Range)
                    {
                        for (int w = 1; w < 10; w++)
                        {
                            if (w2 + c2 + b == w)
                            {
                                output.Add((i, w));
                                output.Add((i2, w2));
                                found = true;
                            }
                            if (found) break;
                        }
                        if (found) break;
                    }
                }
            }

            return (long)output.OrderBy(x => x.i).Reverse().Select((x, i) => x.z * Math.Pow(10, i)).Sum();
        }

        private static double Problem1(IList<string> input)
        {
            return Solve(input);
        }

        private static double Problem2(IList<string> input)
        {
            return Solve(input, false);
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(99298993199873, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(73181221197111, Problem2(input));
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(2, -2)]
        [InlineData(3, -3)]
        [Trait("Event", "2021")]
        public void FirstStarExample1(int input, int expected)
        {
            var exampleInput = ReadExampleLineInput("Example1");

            List<long> variables = [0, 0, 0, 0];

            RunALU(exampleInput.ToList(), variables, [input]);


            Assert.Equal(expected, variables[1]);
        }

        [Theory]
        [InlineData(1, 3, 1)]
        [InlineData(1, 2, 0)]
        [InlineData(2, 6, 1)]
        [InlineData(2, 7, 0)]
        [InlineData(1000, 3000, 1)]
        [InlineData(999, 7, 0)]
        [Trait("Event", "2021")]
        public void FirstStarExample2(int input1, int input2, int expectedZ)
        {
            var exampleInput = ReadExampleLineInput("Example2");

            List<long> variables = [0, 0, 0, 0];

            RunALU(exampleInput.ToList(), variables, [input1, input2]);

            Assert.Equal(expectedZ, variables[3]);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 0, 1)]
        [InlineData(2, 0, 0, 1, 0)]
        [InlineData(3, 0, 0, 1, 1)]
        [InlineData(4, 0, 1, 0, 0)]
        [InlineData(5, 0, 1, 0, 1)]
        [InlineData(6, 0, 1, 1, 0)]
        [InlineData(7, 0, 1, 1, 1)]
        [InlineData(8, 1, 0, 0, 0)]
        [InlineData(15, 1, 1, 1, 1)]
        [Trait("Event", "2021")]
        public void FirstStarExample3(int input, int expectedW, int expectedX, int expectedY, int expectedZ)
        {
            var exampleInput = ReadExampleLineInput("Example3");

            List<long> variables = [0, 0, 0, 0];

            RunALU(exampleInput.ToList(), variables, [input]);

            Assert.Equal(expectedW, variables[0]);
            Assert.Equal(expectedX, variables[1]);
            Assert.Equal(expectedY, variables[2]);
            Assert.Equal(expectedZ, variables[3]);
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem2(exampleInput));
        }
    }
}
