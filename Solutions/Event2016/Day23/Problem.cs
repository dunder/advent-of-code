using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Solutions.Event2016.Day23
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day23;

        public override string FirstStar()
        {
            var instructions = ReadLineInput();
            var registers = new Dictionary<char, int>
            {
                { 'a', 7 },
                { 'b', 0 },
                { 'c', 0 },
                { 'd', 0 },
            };
            var result = RegisterAfterInstructions(instructions, registers, 'a');
            return result.ToString();
        }

        public override string SecondStar()
        {
            var instructions = ReadLineInput();
            var registers = new Dictionary<char, int>
            {
                { 'a', 12 },
                { 'b', 0 },
                { 'c', 0 },
                { 'd', 0 },
            };
            var result = RegisterAfterInstructions(instructions, registers, 'a');
            return result.ToString();
        }

        public static int RegisterAfterInstructions(IList<string> instructions, Dictionary<char, int> registers, char register)
        {
            //PrintAll(instructions, registers);
            for (int i = 0; i < instructions.Count; i++)
            {
                //PrintUpdate(i, instructions.Count, registers);

                var instruction = instructions[i];
                switch (instruction.Substring(0, 3))
                {
                    case "cpy":
                        Copy(instruction, registers);
                        break;
                    case "inc":
                        Increment(instruction, registers);
                        break;
                    case "dec":
                        Decrement(instruction, registers);
                        break;
                    case "jnz":
                        i = Jump(instruction, registers, i);
                        break;
                    case "tgl":
                        Toggle(instruction, instructions, registers, i);
                        //PrintAll(instructions, registers);
                        break;
                }

            }

            return registers[register];
        }

        private static void PrintAll(IList<string> instructions, Dictionary<char, int> registers)
        {
            Console.Clear();
            foreach (var keyValuePair in registers.OrderBy(r => r.Key))
            {
                Console.WriteLine($"{keyValuePair.Key}: {keyValuePair.Value.ToString().PadLeft(8, '0')}");
            }
            Console.WriteLine();
            for (int i = 0; i < instructions.Count; i++)
            {
                var executionPoint = i == 0 ? "->" : "  ";
                Console.WriteLine($"{i.ToString().PadLeft(2, '0')} {executionPoint} {instructions[i]}");
            }
        }

        private static void PrintUpdate(int executionAt, int instructions, Dictionary<char, int> registers)
        {
            Console.SetCursorPosition(4, 0);
            Console.Write($"{registers['a'].ToString().PadLeft(8, '0')}");
            Console.SetCursorPosition(4, 1);
            Console.Write($"{registers['b'].ToString().PadLeft(8, '0')}");
            Console.SetCursorPosition(4, 2);
            Console.Write($"{registers['c'].ToString().PadLeft(8, '0')}");
            Console.SetCursorPosition(4, 3);
            Console.Write($"{registers['d'].ToString().PadLeft(8, '0')}");
            int topOffset = 5;
            for (int i = 0; i < instructions; i++)
            {
                Console.SetCursorPosition(3, i + topOffset);
                var executionPoint = i == executionAt ? "->" : "  ";
                Console.Write(executionPoint);
            }
        }

        private static readonly Regex Cpy = new Regex(@"\w+\s(\w+|\-?\d)\s(\w)");
        private static readonly Regex Dec = new Regex(@"\w+\s(\w+)");
        private static readonly Regex Inc = Dec;
        private static readonly Regex Jnz = new Regex(@"\w+\s(\w+|\d)\s(\-?\d)");
        private static readonly Regex Tgl = new Regex(@"\w+\s(\w+)");

        private static void Copy(string instruction, Dictionary<char, int> registers)
        {
            if (!Cpy.IsMatch(instruction))
            {
                return;
            }
            var match = Cpy.Match(instruction);
            var valueOrRegister = match.Groups[1];
            var register = match.Groups[2].Value.First();
            if (int.TryParse(valueOrRegister.Value, out int value))
            {
                registers[register] = value;
            }
            else
            {
                registers[register] = registers[valueOrRegister.Value.First()];
            }
        }

        private static void Increment(string instruction, Dictionary<char, int> registers)
        {
            var match = Inc.Match(instruction);
            var register = match.Groups[1].Value.First();

            registers[register]++;
        }

        private static void Decrement(string instruction, Dictionary<char, int> registers)
        {
            var match = Dec.Match(instruction);
            var register = match.Groups[1].Value.First();

            registers[register]--;
        }

        /// <returns>The index of the position before the next instruction (compensated for loop variable increment)</returns>
        private static int Jump(string instruction, Dictionary<char, int> registers, int i)
        {
            if (!Jnz.IsMatch(instruction))
            {
                return i;
            }
            var match = Jnz.Match(instruction);
            var registerOrValue = match.Groups[1].Value;
            var jump = int.Parse(match.Groups[2].Value);

            int check;
            if (int.TryParse(registerOrValue, out int value))
            {
                check = value;
            }
            else
            {
                check = registers[registerOrValue.First()];
            }
            if (check != 0)
            {
                return i + jump - 1;
            }

            return i;
        }

        private static void Toggle(string instruction, IList<string> instructions, Dictionary<char, int> registers, int i)
        {
            // add check to all other instructions to skip if invalid (eg cpy 1 2)

            var match = Tgl.Match(instruction);
            var jumpRegister = match.Groups[1].Value.First();

            var jump = registers[jumpRegister] + i;

            if (jump < 0 || jump >= instructions.Count)
            {
                return;
            }

            var toggledInstruction = instructions[jump];

            switch (toggledInstruction.Substring(0,3))
            {
                case "inc":
                    instructions[jump] = instructions[jump].Replace("inc", "dec");
                    break;
                case "dec":
                    instructions[jump] = instructions[jump].Replace("dec", "inc");
                    break;
                case "jnz":
                    instructions[jump] = instructions[jump].Replace("jnz", "cpy");
                    break;
                case "cpy":
                case "tgl":
                    instructions[jump] = instructions[jump].Replace("cpy", "jnz");
                    break;
                default:
                    throw new InvalidOperationException($"Do not know how to toggle {toggledInstruction}");
            }
        }
    }
}