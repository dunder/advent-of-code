using System;

namespace Y2016 {
    public static class TurnExtensions {
        public static Turn TurnFromString(this string turn) {
            switch (turn) {
                case "R":
                    return Turn.Right;
                case "L":
                    return Turn.Left;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}