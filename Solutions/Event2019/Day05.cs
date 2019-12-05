using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 5: Sunny with a Chance of Asteroids ---
    public class Day05
    {
        public static List<int> Parse(string input)
        {
            return input.Split(',').Select(int.Parse).ToList();
        }

        public static void Execute(List<int> code)
        {
            for (int i = 0; i < code.Count;)
            {
                // 0 - position mode
                // 1 - immediate mode

                var instruction = code[i];
                var op = instruction % 100;
                var p1 = (instruction / 100) % 10;
                var p2 = (instruction / 1000) % 10;
                var p3 = (instruction / 1000) % 10;

                switch (op)
                {
                    case 1:
                    {
                        var arg1 = code[i + 1];
                        var arg2 = code[i + 2];
                        var writeTo = code[i + 3];
                        arg1 = p1 == 0 ? code[arg1] : arg1;
                        arg2 = p1 == 0 ? code[arg2] : arg2;
                        var result = arg1 + arg2;
                        code[writeTo] = result;
                        i = i + 4;
                    }
                        break;
                    case 2:
                    {
                        var arg1 = code[i + 1];
                        var arg2 = code[i + 2];
                        var writeTo = code[i + 3];
                        arg1 = p1 == 0 ? code[arg1] : arg1;
                        arg2 = p2 == 0 ? code[arg2] : arg2;
                        var result = arg1 * arg2;
                        code[writeTo] = result;
                        i = i + 4;
                    }
                        break;
                    case 3:
                    {
                        var arg1 = code[i + 1];
                        code[arg1] = 0;
                        i = i + 2;
                    }
                        break;
                    case 4:
                    {
                        var arg1 = code[i + 1];
                        code[arg1] = 0;
                        i = i + 2;
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

        public int FirstStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void ReadOpCodeTest()
        {
            int x = 12345;

            int op = x % 100;

            Assert.Equal(45, op);
        }

        [Fact]
        public void ReadFirstParameterTest()
        {
            int x = 12345;

            int p1 = (x / 100) % 10;

            Assert.Equal(3, p1);
        }
    }
}
