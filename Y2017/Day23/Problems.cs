using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using Y2017.Day18;

namespace Y2017.Day23 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day23\input.txt");

            var result = Coprocessor.Run(input);

            Assert.Equal(3025, result);
            _output.WriteLine($"Day 23 problem 1: {result}"); // inte 1,2
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day23\input.txt");

            var result = Coprocessor.Run2(input);

            Assert.Equal(915, result);
            _output.WriteLine($"Day 23 problem 2: {result}"); // inte 0, inte -1, inte 1001, inte 501, inte 11425
        }
    }

    

    public class Coprocessor {
        public static int Run(string[] input) {

            int mulCount = 0;
            Dictionary<string, long> registers = new Dictionary<string, long>();

            for (long instructionIndex = 0; instructionIndex < input.Length; instructionIndex++) {
                var instruction = input[instructionIndex];

                var multiInstruction = new Regex(@"(set|sub|mul|jnz) ([a-z]|-?\d+) ([a-z]|-?\d+)");

                var multiMatch = multiInstruction.Match(instruction);
                var command = multiMatch.Groups[1].Value;
                var setRegister = multiMatch.Groups[2].Value;
                var getValueDescription = multiMatch.Groups[3].Value;
                long value;
                if (!Int64.TryParse(getValueDescription, out value)) {
                    value = registers.GetValue(getValueDescription);
                }
                switch (command) {
                    case "set":
                        registers.SetValue(setRegister, value);
                        break;
                    case "sub":
                        registers.SetValue(setRegister, registers.GetValue(setRegister) - value);
                        break;
                    case "mul":
                        registers.SetValue(setRegister, registers.GetValue(setRegister) * value);
                        mulCount++;
                        break;
                    case "jnz":
                        long jumpCheck;
                        if (!Int64.TryParse(setRegister, out jumpCheck)) {
                            jumpCheck = registers.GetValue(setRegister);
                        }
                        if (jumpCheck != 0) {
                            instructionIndex += value - 1;
                        }
                        break;
                    default: 
                        throw new InvalidOperationException($"Unknown instruction: {command}");
                }
            }

            return mulCount;
        }

        public static int Run2(string[] input, bool console = false) {

            var h = 0;
            for (var b = 105700; b <= 122700; b += 17) {
                if (!Coprocessor.IsPrime(b)) {
                    h += 1;
                }
            }

            return h;
        }

        private static bool IsPrime(int number) {
            if (number == 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2) {
                if (number % i == 0) return false;
            }

            return true;
        }
    }
}
