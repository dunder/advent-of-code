using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2016.Day02 {
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day02;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = KeyPad.NumericKeyPad.Sequence(input);
            return result;
        }

        public override string SecondStar() {
            var input = ReadLineInput();
            var result = KeyPad.AlphaNumericKeyPad.Sequence(input);
            return result;
        }
    }

    public class KeyPad {
        private Dictionary<string, Key> Keys { get; }

        public static KeyPad NumericKeyPad => new KeyPad(new List<Key>() {
            new Key("1", new Dictionary<char, Func<string>> {
                {'U', () => "1" },
                {'R', () => "2" },
                {'D', () => "4" },
                {'L', () => "1" },
            }),
            new Key("2", new Dictionary<char, Func<string>> {
                {'U', () => "2" },
                {'R', () => "3" },
                {'D', () => "5" },
                {'L', () => "1" },
            }),
            new Key("3", new Dictionary<char, Func<string>> {
                {'U', () => "3" },
                {'R', () => "3" },
                {'D', () => "6" },
                {'L', () => "2" },
            }),
            new Key("4", new Dictionary<char, Func<string>> {
                {'U', () => "1" },
                {'R', () => "5" },
                {'D', () => "7" },
                {'L', () => "4" },
            }),
            new Key("5", new Dictionary<char, Func<string>> {
                {'U', () => "2" },
                {'R', () => "6" },
                {'D', () => "8" },
                {'L', () => "4" },
            }),
            new Key("6", new Dictionary<char, Func<string>> {
                {'U', () => "3" },
                {'R', () => "6" },
                {'D', () => "9" },
                {'L', () => "5" },
            }),
            new Key("7", new Dictionary<char, Func<string>> {
                {'U', () => "4" },
                {'R', () => "8" },
                {'D', () => "7" },
                {'L', () => "7" },
            }),
            new Key("8", new Dictionary<char, Func<string>> {
                {'U', () => "5" },
                {'R', () => "9" },
                {'D', () => "8" },
                {'L', () => "7" },
            }),
            new Key("9", new Dictionary<char, Func<string>> {
                {'U', () => "6" },
                {'R', () => "9" },
                {'D', () => "9" },
                {'L', () => "8" },
            }),
        });

        public static KeyPad AlphaNumericKeyPad => new KeyPad(new List<Key> {
            new Key("1", new Dictionary<char, Func<string>> {
                {'U', () => "1" },
                {'R', () => "1" },
                {'D', () => "3" },
                {'L', () => "1" }
            }),
            new Key("2", new Dictionary<char, Func<string>> {
                {'U', () => "2" },
                {'R', () => "3" },
                {'D', () => "6" },
                {'L', () => "2" }
            }),
            new Key("3", new Dictionary<char, Func<string>> {
                {'U', () => "1" },
                {'R', () => "4" },
                {'D', () => "7" },
                {'L', () => "2" }
            }),
            new Key("4", new Dictionary<char, Func<string>> {
                {'U', () => "4" },
                {'R', () => "4" },
                {'D', () => "8" },
                {'L', () => "3" }
            }),
            new Key("5", new Dictionary<char, Func<string>> {
                {'U', () => "5" },
                {'R', () => "6" },
                {'D', () => "5" },
                {'L', () => "5" }
            }),
            new Key("6", new Dictionary<char, Func<string>> {
                {'U', () => "2" },
                {'R', () => "7" },
                {'D', () => "A" },
                {'L', () => "5" }
            }),
            new Key("7", new Dictionary<char, Func<string>> {
                {'U', () => "3" },
                {'R', () => "8" },
                {'D', () => "B" },
                {'L', () => "6" }
            }),
            new Key("8", new Dictionary<char, Func<string>> {
                {'U', () => "4" },
                {'R', () => "9" },
                {'D', () => "C" },
                {'L', () => "7" }
            }),
            new Key("9", new Dictionary<char, Func<string>> {
                {'U', () => "9" },
                {'R', () => "9" },
                {'D', () => "9" },
                {'L', () => "8" }
            }),
            new Key("A", new Dictionary<char, Func<string>> {
                {'U', () => "6" },
                {'R', () => "B" },
                {'D', () => "A" },
                {'L', () => "A" }
            }),
            new Key("B", new Dictionary<char, Func<string>> {
                {'U', () => "7" },
                {'R', () => "C" },
                {'D', () => "D" },
                {'L', () => "A" }
            }),
            new Key("C", new Dictionary<char, Func<string>> {
                {'U', () => "8" },
                {'R', () => "C" },
                {'D', () => "C" },
                {'L', () => "B" }
            }),
            new Key("D", new Dictionary<char, Func<string>> {
                {'U', () => "B" },
                {'R', () => "D" },
                {'D', () => "D" },
                {'L', () => "D" }
            }),
        });

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

        public KeyPad(List<Key> keys) {
            Keys = keys.ToDictionary(k => k.Name);
        }

        public string Sequence(IList<string> input) {
            var key = "5";
            var code = "";
            foreach (var line in input) {
                foreach (var movement in line) {
                    key = Keys[key].Adjacent(movement);
                }
                code = code + key;
            }
            return code;
        }

        public static string Move(string key, char movement) {
            switch (movement) {
                case 'U': {
                        switch (key) {
                            case "1":
                            case "2":
                            case "3":
                                return key;
                            case "4":
                                return "1";
                            case "5":
                                return "2";
                            case "6":
                                return "3";
                            case "7":
                                return "4";
                            case "8":
                                return "5";
                            case "9":
                                return "6";
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                case 'R': {
                        switch (key) {
                            case "3":
                            case "6":
                            case "9":
                                return key;
                            case "1":
                                return "2";
                            case "2":
                                return "3";
                            case "4":
                                return "5";
                            case "5":
                                return "6";
                            case "7":
                                return "8";
                            case "8":
                                return "9";
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                case 'D': {
                        switch (key) {
                            case "7":
                            case "8":
                            case "9":
                                return key;
                            case "1":
                                return "4";
                            case "2":
                                return "5";
                            case "3":
                                return "6";
                            case "4":
                                return "7";
                            case "5":
                                return "8";
                            case "6":
                                return "9";
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                case 'L': {
                        switch (key) {
                            case "1":
                            case "4":
                            case "7":
                                return key;
                            case "2":
                                return "1";
                            case "3":
                                return "2";
                            case "5":
                                return "4";
                            case "6":
                                return "5";
                            case "8":
                                return "7";
                            case "9":
                                return "8";
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                default:
                    throw new InvalidOperationException();
            }
        }
    }

}
