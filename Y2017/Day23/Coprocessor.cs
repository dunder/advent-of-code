using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Utilities;
using Y2017.Day18;

namespace Y2017.Day23 {
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
                if (!Prime.IsPrime(b)) {
                    h += 1;
                }
            }

            return h;
        }
    }
}