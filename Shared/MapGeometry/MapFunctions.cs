using System;

namespace Shared.MapGeometry
{
    public static class MapFunctions
    {
        public static int NowFacing(int facing, char turn) => (facing, turn) switch
        {
            (0, 'R') => 1,
            (0, 'L') => 3,
            (1, 'R') => 2,
            (1, 'L') => 0,
            (2, 'R') => 3,
            (2, 'L') => 1,
            (3, 'R') => 0,
            (3, 'L') => 2,
            _ => throw new NotImplementedException()
        };

        public static (int x, int y) Move(int x, int y, int facing, int steps) => facing switch
        {
            0 => (x, y + steps),
            1 => (x + steps, y),
            2 => (x, y - steps),
            3 => (x - steps, y),
            _ => throw new NotImplementedException()
        };

        public static (int x, int y) Move((int x, int y) pos, int facing, int steps) => Move(pos.x, pos.y, facing, steps);
    }
}