using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared;
using Solutions.Event2017.Day18;

namespace Solutions.Event2017.Day23
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day23;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = Coprocessor.Run(input.ToArray());
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = Coprocessor.Run2(input);
            return result.ToString();
        }
    }

    public class Coprocessor
    {
        public static int Run(string[] input)
        {
            int mulCount = 0;
            Dictionary<string, long> registers = new Dictionary<string, long>();

            for (long instructionIndex = 0; instructionIndex < input.Length; instructionIndex++)
            {
                var instruction = input[instructionIndex];

                var multiInstruction = new Regex(@"(set|sub|mul|jnz) ([a-z]|-?\d+) ([a-z]|-?\d+)");

                var multiMatch = multiInstruction.Match(instruction);
                var command = multiMatch.Groups[1].Value;
                var setRegister = multiMatch.Groups[2].Value;
                var getValueDescription = multiMatch.Groups[3].Value;
                long value;
                if (!Int64.TryParse(getValueDescription, out value))
                {
                    value = registers.GetValue(getValueDescription);
                }

                switch (command)
                {
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
                        if (!Int64.TryParse(setRegister, out jumpCheck))
                        {
                            jumpCheck = registers.GetValue(setRegister);
                        }

                        if (jumpCheck != 0)
                        {
                            instructionIndex += value - 1;
                        }

                        break;
                    default:
                        throw new InvalidOperationException($"Unknown instruction: {command}");
                }
            }

            return mulCount;
        }

        public static int Run2(IList<string> input, bool console = false)
        {
            var h = 0;
            for (var b = 105700; b <= 122700; b += 17)
            {
                if (!Prime.IsPrime(b))
                {
                    h += 1;
                }
            }

            return h;
        }
    }
}