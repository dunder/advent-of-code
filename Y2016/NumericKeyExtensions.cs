using System;

namespace Y2016 {
    public static class NumericKeyExtensions {
        public static NumericKey Move(this NumericKey key, Movement movement) {
            switch (movement) {
                case Movement.Up: {
                    switch (key) {
                        case NumericKey.K1:
                        case NumericKey.K2:
                        case NumericKey.K3:
                            return key;
                        case NumericKey.K4:
                            return NumericKey.K1;
                        case NumericKey.K5:
                            return NumericKey.K2;
                        case NumericKey.K6:
                            return NumericKey.K3;
                        case NumericKey.K7:
                            return NumericKey.K4;
                        case NumericKey.K8:
                            return NumericKey.K5;
                        case NumericKey.K9:
                            return NumericKey.K6;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                case Movement.Right: {
                    switch (key) {
                        case NumericKey.K3:
                        case NumericKey.K6:
                        case NumericKey.K9:
                            return key;
                        case NumericKey.K1:
                            return NumericKey.K2;
                        case NumericKey.K2:
                            return NumericKey.K3;
                        case NumericKey.K4:
                            return NumericKey.K5;
                        case NumericKey.K5:
                            return NumericKey.K6;
                        case NumericKey.K7:
                            return NumericKey.K8;
                        case NumericKey.K8:
                            return NumericKey.K9;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                case Movement.Down: {
                    switch (key) {
                        case NumericKey.K7:
                        case NumericKey.K8:
                        case NumericKey.K9:
                            return key;
                        case NumericKey.K1:
                            return NumericKey.K4;
                        case NumericKey.K2:
                            return NumericKey.K5;
                        case NumericKey.K3:
                            return NumericKey.K6;
                        case NumericKey.K4:
                            return NumericKey.K7;
                        case NumericKey.K5:
                            return NumericKey.K8;
                        case NumericKey.K6:
                            return NumericKey.K9;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                case Movement.Left: {
                    switch (key) {
                        case NumericKey.K1:
                        case NumericKey.K4:
                        case NumericKey.K7:
                            return key;
                        case NumericKey.K2:
                            return NumericKey.K1;
                        case NumericKey.K3:
                            return NumericKey.K2;
                        case NumericKey.K5:
                            return NumericKey.K4;
                        case NumericKey.K6:
                            return NumericKey.K5;
                        case NumericKey.K8:
                            return NumericKey.K7;
                        case NumericKey.K9:
                            return NumericKey.K8;
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