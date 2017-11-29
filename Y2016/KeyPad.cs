using System;

namespace Y2016 {
    public class KeyPad {
        public static string Crack(string[] input) {
            var key = "5";
            var code = "";
            foreach (var line in input) {
                foreach (var movement in line) {
                    key = Move(key, movement);
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