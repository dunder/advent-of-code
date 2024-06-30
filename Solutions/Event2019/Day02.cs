using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 2: 1202 Program Alarm ---
    public class Day02
    {
        public static List<int> Parse(string input)
        {
            return input.Split(',').Select(int.Parse).ToList();
        }

        private static void Execute(List<int> code)
        {
            for (int i = 0; i < code.Count;i = i + 4)
            {
                var op = code[i];
                switch (op)
                {
                    case 1:
                    {
                        var arg1 = code[i + 1];
                        var arg2 = code[i + 2];
                        var writeTo = code[i + 3];
                        var result = code[arg1] + code[arg2];
                        code[writeTo] = result;
                    }
                        break;
                    case 2:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];
                            var result = code[arg1] * code[arg2];
                            code[writeTo] = result;
                        }
                        break;
                    case 99:
                    {
                        return;
                    }
                    default:
                        throw new InvalidOperationException($"Unknown op '{op}' code at i = {i}");
                }
            }
        }

        public int RunLoop(List<int> code)
        {
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    var localCode = new List<int>(code) {[1] = noun, [2] = verb};
                    Execute(localCode);
                    if (localCode[0] == 19690720)
                    {
                        return 100 * noun + verb;
                    }
                }
            }

            return 0;
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var code = Parse(input);
            code[1] = 12;
            code[2] = 2;
            Execute(code);
            return code[0];
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var code = Parse(input);
            return RunLoop(code);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(5434663, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(4559, SecondStar());
        }

        [Fact]
        public void FirstStartExample1()
        {
            const string input = "1,9,10,3,2,3,11,0,99,30,40,50";
            var code = Parse(input);
            Execute(code);
            Assert.Collection(code,
                x => Assert.Equal(3500,x),
                x => Assert.Equal(9, x),
                x => Assert.Equal(10, x),
                x => Assert.Equal(70, x),
                x => Assert.Equal(2, x),
                x => Assert.Equal(3, x),
                x => Assert.Equal(11, x),
                x => Assert.Equal(0, x),
                x => Assert.Equal(99, x),
                x => Assert.Equal(30, x),
                x => Assert.Equal(40, x),
                x => Assert.Equal(50, x));
        }
        
        [Fact]
        public void FirstStartExample2()
        {
            const string input = "1,0,0,0,99";
            var code = Parse(input);
            Execute(code);
            Assert.Collection(code,
                x => Assert.Equal(2, x),
                x => Assert.Equal(0, x),
                x => Assert.Equal(0, x),
                x => Assert.Equal(0, x),
                x => Assert.Equal(99, x));
        }
        
        [Fact]
        public void FirstStartExample3()
        {
            const string input = "2,3,0,3,99";
            var code = Parse(input);
            Execute(code);
            Assert.Collection(code,
                x => Assert.Equal(2, x),
                x => Assert.Equal(3, x),
                x => Assert.Equal(0, x),
                x => Assert.Equal(6, x),
                x => Assert.Equal(99, x));
        }
        
        [Fact]
        public void FirstStartExample4()
        {
            const string input = "2,4,4,5,99,0";
            var code = Parse(input);
            Execute(code);
            Assert.Collection(code,
                x => Assert.Equal(2, x),
                x => Assert.Equal(4, x),
                x => Assert.Equal(4, x),
                x => Assert.Equal(5, x),
                x => Assert.Equal(99, x),
                x => Assert.Equal(9801, x));
        }
        
        [Fact]
        public void FirstStartExample5()
        {
            const string input = "1,1,1,4,99,5,6,0,99";
            var code = Parse(input);
            Execute(code);
            Assert.Collection(code,
                x => Assert.Equal(30, x),
                x => Assert.Equal(1, x),
                x => Assert.Equal(1, x),
                x => Assert.Equal(4, x),
                x => Assert.Equal(2, x),
                x => Assert.Equal(5, x),
                x => Assert.Equal(6, x),
                x => Assert.Equal(0, x),
                x => Assert.Equal(99, x));
        }
    }
}
