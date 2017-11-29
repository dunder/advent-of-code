using System;
using System.Linq;

namespace Y2016 {
    public class AlphaNumericKeyPad {

        public static string Crack(string[] input) {
            var key = "5";
            var code = "";
            foreach (var line in input) {
                var movements = line.Select(c => c.ToMovement());
                foreach (var movement in movements) {
                    key = Move(key, movement);
                }
                code = code + key;
            }
            return code;
        }

        public static string Move(string key, Movement movement) {
            switch (movement) {
                case Movement.Up:
                    switch (key) {
                        case "1":
                        case "2":
                        case "4":
                        case "5":
                        case "9":
                            return key;
                        case "3":
                            return "1";
                        case "6":
                            return "2";
                        case "7":
                            return "3";
                        case "8":
                            return "4";
                        case "A":
                            return "6";
                        case "B":
                            return "7";
                        case "C":
                            return "8";
                        case "D":
                            return "B";
                        default:
                            throw new InvalidOperationException();
                    }

                case Movement.Right:
                    switch (key) {
                        case "1":
                        case "4":
                        case "9":
                        case "C":
                        case "D":
                            return key;
                        case "2":
                            return "3";
                        case "3":
                            return "4";
                        case "5":
                            return "6";
                        case "6":
                            return "7";
                        case "7":
                            return "8";
                        case "8":
                            return "9";
                        case "A":
                            return "B";
                        case "B":
                            return "C";
                        default:
                            throw new InvalidOperationException();
                    }
                case Movement.Down:
                    switch (key) {
                        case "5":
                        case "9":
                        case "A":
                        case "C":
                        case "D":
                            return key;
                        case "1":
                            return "3";
                        case "2":
                            return "6";
                        case "3":
                            return "7";
                        case "4":
                            return "8";
                        case "6":
                            return "A";
                        case "7":
                            return "B";
                        case "8":
                            return "C";
                        case "B":
                            return "D";
                        default:
                            throw new InvalidOperationException();
                    }
                case Movement.Left:
                    switch (key) {
                        case "1":
                        case "2":
                        case "5":
                        case "A":
                        case "D":
                            return key;
                        case "3":
                            return "2";
                        case "4":
                            return "3";
                        case "6":
                            return "5";
                        case "7":
                            return "6";
                        case "8":
                            return "7";
                        case "9":
                            return "8";
                        case "B":
                            return "A";
                        case "C":
                            return "B";
                        default:
                            throw new InvalidOperationException();
                    }
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
