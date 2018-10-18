using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2017.Day08
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day08;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            (var highestFinal, _) = Interpreter.LargestRegisterCount(input);
            return highestFinal.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            (_, var highestStored) = Interpreter.LargestRegisterCount(input);
            return highestStored.ToString();
        }
    }

    public class Interpreter
    {
        public static (int, int) LargestRegisterCount(IList<string> input)
        {
            var registers = new Dictionary<string, int>();
            int highestStored = 0;

            foreach (var instruction in input)
            {
                var instructionParts = instruction.Split(' ');
                var register = instructionParts[0];
                var adjustment = instructionParts[1];
                var adjustmentValue = int.Parse(instructionParts[2]);
                var checkRegister = instructionParts[4];
                var comparison = instructionParts[5];
                var comparisonValue = int.Parse(instructionParts[6]);

                if (!registers.ContainsKey(register))
                {
                    registers.Add(register, 0);
                }

                if (!registers.ContainsKey(checkRegister))
                {
                    registers.Add(checkRegister, 0);
                }

                if (Compare(comparison, registers[checkRegister], comparisonValue))
                {
                    var newRegisterValue = NewRegisterValue(adjustment, registers[register], adjustmentValue);
                    highestStored = Math.Max(highestStored, newRegisterValue);
                    registers[register] = newRegisterValue;
                }
            }

            return (registers.Values.Max(), highestStored);
        }

        private static int NewRegisterValue(string adjustmentType, int registerValue, int adjustmentValue)
        {
            if (adjustmentType == "inc")
            {
                return registerValue + adjustmentValue;
            }

            return registerValue - adjustmentValue;
        }

        private static bool Compare(string comparison, int registerValue, int value)
        {
            switch (comparison)
            {
                case ">":
                    return registerValue > value;
                case ">=":
                    return registerValue >= value;
                case "<":
                    return registerValue < value;
                case "<=":
                    return registerValue <= value;
                case "==":
                    return registerValue == value;
                case "!=":
                    return registerValue != value;
                default:
                    throw new InvalidOperationException($"Unknown comparison operator: {comparison}");
            }
        }
    }
}