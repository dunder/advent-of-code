using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        

        public static long DualSendCount(string[] input) {
            Dictionary<string, long> registersProgram0 = new Dictionary<string, long>();
            Dictionary<string, long> registersProgram1 = new Dictionary<string, long> {{"p", 1}};

            Queue<long> messagesFrom0To1 = new Queue<long>();
            Queue<long> messagesFrom1To0 = new Queue<long>();
            long[] sendCounters = new long[2];
            long[] instructionCounters = new long[2];

            do {
                DualInterpreter(0, input, instructionCounters, registersProgram0, messagesFrom0To1, messagesFrom1To0, sendCounters);
                DualInterpreter(1, input, instructionCounters, registersProgram1, messagesFrom1To0, messagesFrom0To1, sendCounters);
            } while (messagesFrom1To0.Any() || messagesFrom0To1.Any());

            return sendCounters[1];
        }

        private static void DualInterpreter(int id, 
                string[] instructions, 
                long[] instructionCounters, 
                Dictionary<string, long> registers,
                Queue<long> sendQueue, 
                Queue<long> receiveQueue, 
                long[] sendCounters) {
            while (instructionCounters[id] >= 0 && instructionCounters[id] <instructions.Length ) {
                var instruction = instructions[instructionCounters[id]];

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
                                sendCounters[id] += 1;
                                sendQueue.Enqueue(singleRegisterValue);
                                break;
                            case "rcv":
                                if (!receiveQueue.Any()) {
                                    return;
                                }
                                var receivedValue = receiveQueue.Dequeue();
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
                                    instructionCounters[id] += value - 1;
                                }
                                break;
                        }
                        break;
                }
                instructionCounters[id] += 1;
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

        public static int GetIntValue(this Dictionary<string, int> registers, string register) {
            if (!registers.ContainsKey(register)) {
                registers.Add(register, 0);
            }
            return registers[register];
        }

        public static void SetIntValue(this Dictionary<string, int> registers, string register, int value) {
            if (!registers.ContainsKey(register)) {
                registers.Add(register, value);
            }
            else {
                registers[register] = value;
            }
        }

        public static int GetByteValue(this Dictionary<string, byte> registers, string register) {
            if (!registers.ContainsKey(register)) {
                registers.Add(register, 0);
            }
            
            return registers[register];
        }

        public static void SetByteValue(this Dictionary<string, byte> registers, string register, byte value) {
            if (!registers.ContainsKey(register)) {
                registers.Add(register, value);
            }
            else {
                registers[register] = value;
            }
        }

        public static sbyte GetSbyteValue(this Dictionary<string, sbyte> registers, string register) {
            if (!registers.ContainsKey(register)) {
                registers.Add(register, 0);
            }
            
            return registers[register];
        }

        public static void SetSbyteValue(this Dictionary<string, sbyte> registers, string register, sbyte value) {
            if (!registers.ContainsKey(register)) {
                registers.Add(register, value);
            }
            else {
                registers[register] = value;
            }
        }
    }
}