using System;

namespace Y2016 {
    public static class MovementExtensions {
        public static Movement ToMovement(this char movementDescription) {
            switch (movementDescription) {
                case 'U':
                    return Movement.Up;
                case 'R':
                    return Movement.Right;
                case 'D':
                    return Movement.Down;
                case 'L':
                    return Movement.Left;
                default:
                    throw new ArgumentOutOfRangeException(nameof(movementDescription));
            }
        }
    }
}