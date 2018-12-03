using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = new AssembunnyInterpreter();
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }
    }

    public class AssembunnyInterpreterState
    {
        public int ExecutionPoint { get; set; }
        public RegisterSet Registers { get; }
        
    }

    public class RegisterSet
    {
        private readonly Dictionary<char, int> _registers = new Dictionary<char, int>();

        public int Read(char registerKey)
        {
            if (!_registers.ContainsKey(registerKey))
            {
                CreateRegister(registerKey);
            }

            return _registers[registerKey];
        }

        public void Write(char registerKey, int value)
        {
            if (!_registers.ContainsKey(registerKey))
            {
                CreateRegister(registerKey);
            }

            _registers[registerKey] = value;
        }

        private void CreateRegister(char registerKey)
        {
            _registers.Add(registerKey, 0);
        }
    }

    public class InstructionSet
    {
        private List<string> _instructions;

        public InstructionSet(List<string> instructions)
        {
            _instructions = new List<string>(instructions);
        }

        public void Execute(AssembunnyInterpreterState state)
        {
            var instruction = _instructions[state.ExecutionPoint];
        }
    }

    public interface IInstruction
    {
        AssembunnyInterpreterState Execute(string instruction, AssembunnyInterpreterState state);
    }

    public class CopyValue : IInstruction
    {
        private static readonly Regex InstructionExpression = new Regex(@"\w+\s(\d)\s(\w)");

        private int _value;
        private char _register;

        private CopyValue(int value, char register)
        {
            _value = value;
            _register = register;
        }
        public static CopyValue Parse(string instruction)
        {
            var match = InstructionExpression.Match(instruction);
            var value = int.Parse(match.Groups[1].Value);
            var register = match.Groups[2].Value.First();

            return new CopyValue(value, register);
        }

        public AssembunnyInterpreterState Execute(string instruction, AssembunnyInterpreterState state)
        {
            var match = InstructionExpression.Match(instruction);
            var value = int.Parse(match.Groups[1].Value);
            var register = match.Groups[2].Value.First();

            state.Registers.Write(register, value);

            return state;
        }
    }

    public class CopyRegister : IInstruction
    {
        private static readonly Regex InstructionExpression = new Regex(@"\w+\s(\w+)\s(\w)");

        private readonly char _fromRegister;
        private readonly char _toRegister;

        private CopyRegister(char fromRegister, char toRegister)
        {
            _fromRegister = fromRegister;
            _toRegister = toRegister;
        }

        public static CopyRegister Parse(string instruction)
        {
            var match = InstructionExpression.Match(instruction);
            var fromRegister = match.Groups[1].Value.First();
            var toRegister = match.Groups[2].Value.First();

            return new CopyRegister(fromRegister, toRegister);
        }
        public AssembunnyInterpreterState Execute(string instruction, AssembunnyInterpreterState state)
        {
            var match = InstructionExpression.Match(instruction);
            var copyFromRegister = match.Groups[1].Value.First();
            var register = match.Groups[2].Value.First();

            var value = state.Registers.Read(copyFromRegister);
            state.Registers.Write(register, value);

            return state;
        }
    }

    public class Increment : IInstruction
    {
        private static readonly Regex InstructionExpression = new Regex(@"\w+\s(\w+)");

        public AssembunnyInterpreterState Execute(string instruction, AssembunnyInterpreterState state)
        {
            var match = InstructionExpression.Match(instruction);
            var register = match.Groups[1].Value.First();

            state.Registers.Write(register, state.Registers.Read(register) + 1);

            return state;
        }
    }

    public class Decrement : IInstruction
    {
        private static readonly Regex InstructionExpression = new Regex(@"\w+\s(\w+)");

        public AssembunnyInterpreterState Execute(string instruction, AssembunnyInterpreterState state)
        {
            var match = InstructionExpression.Match(instruction);
            var register = match.Groups[1].Value.First();

            state.Registers.Write(register, state.Registers.Read(register) - 1);

            return state;
        }
    }

    public class JumpBase
    {
        public AssembunnyInterpreterState Execute(int check, int jump, AssembunnyInterpreterState state)
        {
            if (check != 0)
            {
                state.ExecutionPoint = state.ExecutionPoint + jump - 1;
            }

            return state;
        }
    }

    public class JumpValue : JumpBase, IInstruction
    {
        private static readonly Regex InstructionExpression = new Regex(@"\w+\s(\w+|\d)\s(\-?\d)");

        public AssembunnyInterpreterState Execute(string instruction, AssembunnyInterpreterState state)
        {
            var match = InstructionExpression.Match(instruction);
            var check = int.Parse(match.Groups[1].Value);
            var jump = int.Parse(match.Groups[2].Value);

            return Execute(check, jump, state);
        }
    }


    public class JumpRegister : JumpBase, IInstruction
    {
        private static readonly Regex InstructionExpression = new Regex(@"\w+\s(\w+|\d)\s(\-?\d)");

        public AssembunnyInterpreterState Execute(string instruction, AssembunnyInterpreterState state)
        {
            var match = InstructionExpression.Match(instruction);
            var register = match.Groups[1].Value.First();
            var jump = int.Parse(match.Groups[2].Value);

            int check = state.Registers.Read(register);

            return Execute(check, jump, state);
        }
    }

    public class AssembunnyInterpreter
    {
        private static readonly Regex Cpy = new Regex(@"\w+\s(\w+|\d)\s(\w)");
        private static readonly Regex Dec = new Regex(@"\w+\s(\w+)");
        private static readonly Regex Inc = Dec;
        private static readonly Regex Jnz = new Regex(@"\w+\s(\w+|\d)\s(\-?\d)");

        private readonly RegisterSet _registerSet = new RegisterSet();

        public void ExecuteInstructions(IList<string> instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                var instruction = instructions[i];
                switch (instruction.Substring(0, 3))
                {
                    case "cpy":
                        Copy(instruction);
                        break;
                    case "inc":
                        Increment(instruction);
                        break;
                    case "dec":
                        Decrement(instruction);
                        break;
                    case "jnz":
                        i = Jump(instruction, i);
                        break;
                }
            }
        }
        

        private void Copy(string instruction)
        {
            var match = Cpy.Match(instruction);
            var valueOrRegister = match.Groups[1];
            var register = match.Groups[2].Value.First();
            if (int.TryParse(valueOrRegister.Value, out int value))
            {
                _registerSet.Write(register, value);
            }
            else
            {
                var copyFromRegister = valueOrRegister.Value.First();
                value = _registerSet.Read(copyFromRegister);
                _registerSet.Write(register, value);
            }
        }

        private void Increment(string instruction)
        {
            var match = Inc.Match(instruction);
            var register = match.Groups[1].Value.First();

            _registerSet.Write(register, _registerSet.Read(register) + 1);
        }

        private void Decrement(string instruction)
        {
            var match = Dec.Match(instruction);
            var register = match.Groups[1].Value.First();

            _registerSet.Write(register, _registerSet.Read(register) - 1);
        }

        /// <returns>The index of the position before the next instruction (compensated for loop variable increment)</returns>
        private int Jump(string instruction, int i)
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
                var register = registerOrValue.First();
                check = _registerSet.Read(register);
            }

            if (check != 0)
            {
                return i + jump - 1;
            }

            return i;
        }
    }
}