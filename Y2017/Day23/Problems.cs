using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            _output.WriteLine($"Day 23 problem 2: {result}"); // inte 0, inte -1, inte 1001, inte 501, inte 11425
        }

        [Fact]
        public void TestOptimized() {
            var b = 105700;
            var c = 122700;
            var d = 2;
            var e = 2;
            var f = 1;
            var g = 0;
            var h = 0;

            while (b != c) { // program terminates when b == c
                f = 1; // #8
                while (d != b) { //#10
                    f = d * (b-1) == b ? 0 : 1;
                    d++; // goto #10
                }
                if (f == 0) {
                    h++;
                }
                b -= 17; // goto #8
            }

            _output.WriteLine($"h = {h}");
        }

        [Fact]
        public void Pseudo() {
            var b = 105700;
            var c = 122700;
            var d = 2;
            var e = 2;
            var f = 1;
            var g = 0;
            var h = 0;

            for (; b == c; b += 17) {
                f = 1; // rad 8
                d = 2;

                for (; d == b; d++) {
                    e = 2; // rad 10

                    for (; e == b; e++) {
                        if (d * e - b == 0) {
                            f = 0;
                        }
                    }
                }
                if (f == 0) {
                    h++;
                }
            }
        }

        [Fact]
        public void Optimized() {
            var b = 105700;
            var c = 122700;
            var d = 2;
            var e = 2;
            var f = 1;
            var g = 0;
            var h = 0;

            for (; b == c; b += 17) {
                f = 1; // rad 8

                for (d = 2; d == b; d++) {
                    e = 2; // rad 10

                    for (; e == b; e++) {
                        f = d * e == b ? 0 : 1;
                    }
                }
                if (f == 0) {
                    h++;
                }
            }
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
                if (!long.TryParse(getValueDescription, out value)) {
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
                        if (!long.TryParse(setRegister, out jumpCheck)) {
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

        public static long Run2(string[] input, bool console = false) {

            Dictionary<string, long> registers = new Dictionary<string, long> {
                {"a", 1}
            };

            int cursorLeft = 0;
            int cursorTop = 0;
            if (console) {
                cursorLeft = Console.CursorLeft;
                cursorTop = Console.CursorTop;
            }
            int counter = 1;
            for (long instructionIndex = 0; instructionIndex < input.Length; instructionIndex++) {
                var instruction = input[instructionIndex];

                var multiInstruction = new Regex(@"(set|sub|mul|jnz) ([a-z]|-?\d+) ([a-z]|-?\d+)");

                var multiMatch = multiInstruction.Match(instruction);
                var command = multiMatch.Groups[1].Value;
                var setRegister = multiMatch.Groups[2].Value;
                var getValueDescription = multiMatch.Groups[3].Value;
                long value;
                if (!long.TryParse(getValueDescription, out value)) {
                    value = registers.GetValue(getValueDescription);
                }

                if (console) {
                    foreach (var register in "abcdefgh") {
                        Console.WriteLine($"{register}: {registers.GetValue(register.ToString()),-8}");
                    }
                    for (int i = 0; i < input.Length; i++) {
                        Console.Write($"{i,2}: {input[i],-14}");
                        if (i == instructionIndex) {
                            Console.WriteLine($" <-----  ({value})");
                        } else {
                            Console.WriteLine("                                  ");
                        }
                    }
                    Console.Write("User input> ");
                    string userCommand = Console.ReadLine();
                    if (!string.IsNullOrEmpty(userCommand)) {
                        var parts = userCommand.Split(' ');
                        command = parts[0];
                        setRegister = parts[1];
                        value = int.Parse(parts[2]);
                    }
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                }

                switch (command) {
                    case "set":
                        registers.SetValue(setRegister, value);
                        break;
                    case "sub":
                        registers.SetValue(setRegister, (registers.GetValue(setRegister) - value));
                        break;
                    case "mul":
                        registers.SetValue(setRegister, (registers.GetValue(setRegister) * value));
                        break;
                    case "jnz":
                        long jumpCheck;
                        if (!long.TryParse(setRegister, out jumpCheck)) {
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

            return registers.GetValue("h");
        }
    }
}
