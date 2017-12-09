using System;
using System.Collections.Generic;

namespace Y2016.Day2 {
    public class Key {
        public Key(string name, IDictionary<char, Func<string>> moveCommands) {
            Name = name;
            MoveCommands = moveCommands;
        }

        public string Name { get; }
        private IDictionary<char, Func<string>> MoveCommands { get; }

        public string Adjacent(char move) {
            return MoveCommands[move]();
        }
    }
}