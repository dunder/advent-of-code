using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day8 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day8\input.txt");

            (var highestFinal, _) = Interpreter.LargestRegisterCount(input);

            _output.WriteLine($"Day 8 problem 1: {highestFinal}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day8\input.txt");

            (_, var highestStored) = Interpreter.LargestRegisterCount(input);

            _output.WriteLine($"Day 8 problem 2: {highestStored}");
        }

        
    }
    public class Interpreter {

        public static (int,int) LargestRegisterCount(string[] input) {

            var registers = new Dictionary<string, int>();
            int highestStored = 0;

            foreach (var instruction in input) {
                var instructionParts = instruction.Split(' ');
                var register = instructionParts[0];
                var adjustment = instructionParts[1];
                var adjustmentValue = int.Parse(instructionParts[2]);
                var checkRegister = instructionParts[4];
                var comparison = instructionParts[5];
                var comparisonValue = int.Parse(instructionParts[6]);

                if (!registers.ContainsKey(register)) {
                    registers.Add(register, 0);
                }
                if (!registers.ContainsKey(checkRegister)) {
                    registers.Add(checkRegister, 0);
                }

                if (Compare(comparison, registers[checkRegister], comparisonValue)) {
                    var newRegisterValue = NewRegisterValue(adjustment, registers[register], adjustmentValue);
                    highestStored = Math.Max(highestStored, newRegisterValue);
                    registers[register] = newRegisterValue;
                }
            }
            return (registers.Values.Max(), highestStored);
        }

        private static int NewRegisterValue(string adjustmentType, int registerValue, int adjustmentValue) {
            if (adjustmentType == "inc") {
                return registerValue + adjustmentValue;
            }
            return registerValue - adjustmentValue;
        }

        private static bool Compare(string comparison, int registerValue, int value) {
            switch (comparison) {
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
