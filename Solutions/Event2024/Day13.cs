﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 13: Phrase ---
    public class Day13
    {
        private readonly ITestOutputHelper output;


        public record Machine(int AX, int AY, int BX, int BY, long PX, long PY);

        private static List<Machine> Parse(IList<string> input, long offset = 0)
        {
            List<Machine> machines = new List<Machine>();

            int AX = 0;
            int AY = 0;

            int BX = 0;
            int BY = 0;

            int PX = 0;
            int PY = 0;

            var aRegex = new Regex(@"Button A: X\+(\d+), Y\+(\d+)");
            var bRegex = new Regex(@"Button B: X\+(\d+), Y\+(\d+)");
            var pRegex = new Regex(@"Prize: X=(\d+), Y=(\d+)");

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (aRegex.IsMatch(line))
                {
                    var m = aRegex.Match(line);
                    AX = int.Parse(m.Groups[1].Value);
                    AY = int.Parse(m.Groups[2].Value);
                }

                if (bRegex.IsMatch(line))
                {
                    var m = bRegex.Match(line);
                    BX = int.Parse(m.Groups[1].Value);
                    BY = int.Parse(m.Groups[2].Value);
                }
                if (pRegex.IsMatch(line))
                {
                    var m = pRegex.Match(line);
                    PX = int.Parse(m.Groups[1].Value);
                    PY = int.Parse(m.Groups[2].Value);
                    machines.Add(new Machine(AX, AY, BX, BY, PX + offset, PY + offset));
                }
            }
            return machines;
        }

        private static List<Machine> Parse(IList<string> input)
        {
            List<Machine> machines = new List<Machine>();

            int AX = 0;
            int AY = 0;

            int BX = 0;
            int BY = 0;

            int PX = 0;
            int PY = 0;

            var aRegex = new Regex(@"Button A: X\+(\d+), Y\+(\d+)");
            var bRegex = new Regex(@"Button B: X\+(\d+), Y\+(\d+)");
            var pRegex = new Regex(@"Prize: X=(\d+), Y=(\d+)");

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (aRegex.IsMatch(line))
                {
                    var m = aRegex.Match(line);
                    AX = int.Parse(m.Groups[1].Value);
                    AY = int.Parse(m.Groups[2].Value);
                }

                if (bRegex.IsMatch(line))
                {
                    var m = bRegex.Match(line);
                    BX = int.Parse(m.Groups[1].Value);
                    BY = int.Parse(m.Groups[2].Value);
                }
                if (pRegex.IsMatch(line))
                {
                    var m = pRegex.Match(line);
                    PX = int.Parse(m.Groups[1].Value);
                    PY = int.Parse(m.Groups[2].Value);
                    machines.Add(new Machine(AX, AY, BX, BY, PX, PY));
                }
            }
            return machines;
        }

        public Day13(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static int Problem1(IList<string> input)
        {
            var machines = Parse(input);

            var total = 0;

            foreach (var machine in machines)
            {
                int tokens = int.MaxValue;

                for (int a = 1; a <= 100; a++)
                {
                    for (int b = 1; b <= 100; b++)
                    {
                        var x = a * machine.AX + b * machine.BX;
                        var y = a * machine.AY + b * machine.BY;

                        if (x == machine.PX && y == machine.PY)
                        {
                            tokens = Math.Min(3 * a + b, tokens);
                        }
                    }

                }

                if (tokens < int.MaxValue)
                {
                    total += tokens;
                }
            }

            return total;
        }

        private static long Problem2(IList<string> input)
        {
            var machines = Parse(input, 10000000000000);

            long total = 0;

            
            foreach (var machine in machines)
            {
                // cramers rule

                long a1 = machine.AX;
                long b1 = machine.BX;
                long a2 = machine.AY;
                long b2 = machine.BY;
                long c1 = machine.PX;
                long c2 = machine.PY;

                long xd = c1 * b2 - b1 * c2;
                long xn = a1 * b2 - b1 * a2;

                bool xIsInt = xd % xn == 0;
                if (!xIsInt)
                {
                    continue;
                }

                long yd = a1 * c2 - c1 * a2;
                long yn = a1 * b2 - b1 * a2;

                bool yIsInt = yd % xn == 0;
                if (!yIsInt)
                {
                    continue;
                }

                var x = xd / xn;
                var y = yd / yn;

                total += 3 * (xd / xn) + (yd / yn);
            }

            return total; // 60841592630030 too low
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(32026, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(89013607072065, Problem2(input));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "Button A: X+94, Y+34",
                "Button B: X+22, Y+67",
                "Prize: X=8400, Y=5400",
                "",
                "Button A: X+26, Y+66",
                "Button B: X+67, Y+21",
                "Prize: X=12748, Y=12176",
                "",
                "Button A: X+17, Y+86",
                "Button B: X+84, Y+37",
                "Prize: X=7870, Y=6450",
                "",
                "Button A: X+69, Y+23",
                "Button B: X+27, Y+71",
                "Prize: X=18641, Y=10279",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(480, Problem1(exampleInput));
        }
    }
}
