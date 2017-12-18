using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Y2017.Day18 {
    public class DuetAssembly {
        public static long RecoveredFrequency(string[] input) {

            long frequencyLastPlayed = 0;
            Dictionary<string, long> registers = new Dictionary<string, long>();

            for (long instructionIndex = 0; instructionIndex < input.Length; instructionIndex++) {
                var instruction = input[instructionIndex];

                var singleRegisterInstruction = new Regex(@"(snd|rcv) ([a-z])");
                var multiInstruction = new Regex(@"(set|add|mul|mod|jgz) ([a-z]|-?\d+) ([a-z]|-?\d+)");

                switch (instruction) {

                    case var single when singleRegisterInstruction.IsMatch(instruction):                        
                        var singleMatch = singleRegisterInstruction.Match(single);
                        var singleCommand = singleMatch.Groups[1].Value;
                        var singleRegister = singleMatch.Groups[2].Value;
                        var singleRegisterValue = registers.GetValue(singleRegister);
                        switch (singleCommand) {                                
                            case "snd":
                                frequencyLastPlayed = singleRegisterValue;
                                break;
                            case "rcv":
                                if (singleRegisterValue != 0) {
                                    return frequencyLastPlayed;
                                }
                                break;
                        }
                        break;
                    case var multi when multiInstruction.IsMatch(instruction):
                        var multiMatch = multiInstruction.Match(multi);
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
                            case "add":
                                registers.SetValue(setRegister, registers.GetValue(setRegister) + value);
                                break;
                            case "mul":
                                registers.SetValue(setRegister, registers.GetValue(setRegister) * value);
                                break;
                            case "mod":
                                registers.SetValue(setRegister, registers.GetValue(setRegister) % value);
                                break;
                            case "jgz":
                                long jumpCheck;
                                if (!long.TryParse(setRegister, out jumpCheck)) {
                                    jumpCheck = registers.GetValue(setRegister);
                                }
                                if (jumpCheck > 0) {
                                    instructionIndex += value - 1;
                                }
                                break;
                        }
                        break;
                    
                }
            }

            return 0;
        }

        public static long DualCount(string[] input) {

            long frequencyLastPlayed = 0;

            Dictionary<string, long> registersProgram0 = new Dictionary<string, long>();
            Dictionary<string, long> registersProgram1 = new Dictionary<string, long>();
            registersProgram0.SetValue("p", 1);

            var queueFrom0To1 = new BlockingCollection<long>(new ConcurrentQueue<long>());
            var queueFrom1To0 =new BlockingCollection<long>(new ConcurrentQueue<long>());
            ManualResetEventSlim receiving = new ManualResetEventSlim(false);

            long dualCount = 0;

            var program0 = Task.Run(() => DualInterpreter(input, registersProgram0, queueFrom0To1, queueFrom1To0, receiving, true, ref dualCount));
            var program1 = Task.Run(() => DualInterpreter(input, registersProgram1, queueFrom1To0, queueFrom0To1, receiving, false, ref dualCount));

            Task.WaitAll(program0, program1);
            return dualCount;
        }

        private static void DualInterpreter(string[] input,
                Dictionary<string, long> registers,
                BlockingCollection<long> sendQueue,
                BlockingCollection<long> receiveQueue,
                ManualResetEventSlim receiving,
                bool countSend,
                ref long sendCounter) {

            for (long instructionIndex = 0; instructionIndex < input.Length; instructionIndex++) {
                var instruction = input[instructionIndex];

                var singleRegisterInstruction = new Regex(@"(snd|rcv) ([a-z])");
                var multiInstruction = new Regex(@"(set|add|mul|mod|jgz) ([a-z]|-?\d+) ([a-z]|-?\d+)");

                switch (instruction) {
                    case var single when singleRegisterInstruction.IsMatch(instruction):
                        var singleMatch = singleRegisterInstruction.Match(single);
                        var singleCommand = singleMatch.Groups[1].Value;
                        var singleRegister = singleMatch.Groups[2].Value;
                        var singleRegisterValue = registers.GetValue(singleRegister);
                        switch (singleCommand) {
                            case "snd":
                                if (countSend) {
                                    sendCounter++;
                                }
                                sendQueue.Add(singleRegisterValue);
                                break;
                            case "rcv":
                                var receivedValue = receiveQueue.Take();
                                registers.SetValue(singleRegister, receivedValue);
                                break;
                        }
                        break;
                    case var multi when multiInstruction.IsMatch(instruction):
                        var multiMatch = multiInstruction.Match(multi);
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
                            case "add":
                                registers.SetValue(setRegister, registers.GetValue(setRegister) + value);
                                break;
                            case "mul":
                                registers.SetValue(setRegister, registers.GetValue(setRegister) * value);
                                break;
                            case "mod":
                                registers.SetValue(setRegister, registers.GetValue(setRegister) % value);
                                break;
                            case "jgz":
                                long jumpCheck;
                                if (!long.TryParse(setRegister, out jumpCheck)) {
                                    jumpCheck = registers.GetValue(setRegister);
                                }
                                if (jumpCheck > 0) {
                                    instructionIndex += value - 1;
                                }
                                break;
                        }
                        break;
                }
            }
        }
    }

    public static class RegisterExtensions {
        public static long GetValue(this Dictionary<string, long> registers, string register) {
            if (!registers.ContainsKey(register)) {
                registers.Add(register, 0);
            }
            return registers[register];
        }

        public static void SetValue(this Dictionary<string, long> registers, string register, long value) {
            if (!registers.ContainsKey(register)) {
                registers.Add(register, value);
            }
            else {
                registers[register] = value;
            }
        }
    }
}