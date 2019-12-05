﻿using System;
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

        private enum Mode
        {
            Position = 0,
            Immediate
        }

        public static int Execute(List<int> code, int input)
        {
            var output = new List<int>();

            for (int i = 0; i < code.Count;)
            {
                // 0 - position mode
                // 1 - immediate mode

                var instruction = code[i];
                var op = instruction % 100;
                var p1 = (instruction / 100) % 10;
                var p2 = (instruction / 1000) % 10;

                switch (op)
                {
                    case 1:
                        {
                            var arg1 = code[i + 1];
                            var arg2 = code[i + 2];
                            var writeTo = code[i + 3];
                            arg1 = p1 == 0 ? code[arg1] : arg1;
                            arg2 = p2 == 0 ? code[arg2] : arg2;
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
                            code[arg1] = input;
                            i = i + 2;
                        }
                        break;
                    case 4:
                        {
                            var arg1 = code[i + 1];
                            arg1 = p1 == 0 ? code[arg1] : arg1;
                            output.Add(arg1);
                            i = i + 2;
                        }
                        break;
                    case 99:
                        {
                            return output.Last();
                        }
                    default:
                        throw new InvalidOperationException($"Unknown op '{op}' code at i = {i}");
                }
            }
            throw new InvalidOperationException("Unexpected program exit");
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var code = Parse(input);
            return Execute(code, 1);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(7265618, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(7731427, SecondStar());
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
