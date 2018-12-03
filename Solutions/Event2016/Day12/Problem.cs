using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solutions.Event2016.Day12
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day12;

        public override string FirstStar()
        {
            var allInstructions = ReadLineInput();
            var registers = new Dictionary<char, int>
            {
                {'a', 0},
                {'b', 0},
                {'c', 0},
                {'d', 0}
            };

            var result = RegisterAfterInstructions(allInstructions, registers, 'a');
            return result.ToString();
        }


        public override string SecondStar()
        {
            var allInstructions = ReadLineInput();
            var registers = new Dictionary<char, int>
            {
                {'a', 0},
                {'b', 0},
                {'c', 1},
                {'d', 0}
            };

            var result = RegisterAfterInstructions(allInstructions, registers, 'a');
            return result.ToString();
        }

        public static int RegisterAfterInstructions(IList<string> instructions, Dictionary<char, int> registers, char register)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
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
                }
            }

            return registers[register];
        }

        private static readonly Regex Cpy = new Regex(@"\w+\s(\w+|\d)\s(\w)");
        private static readonly Regex Dec = new Regex(@"\w+\s(\w+)");
        private static readonly Regex Inc = Dec;
        private static readonly Regex Jnz = new Regex(@"\w+\s(\w+|\d)\s(\-?\d)");

        private static void Copy(string instruction, Dictionary<char, int> registers)
        {
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
    }
}