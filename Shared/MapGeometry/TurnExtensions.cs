using System;

namespace Shared.MapGeometry
{
    public static class TurnExtensions
    {
        public static Turn Turn(this string turn)
        {
            switch (turn)
            {
                case "R":
                    return MapGeometry.Turn.Right;
                case "L":
                    return MapGeometry.Turn.Left;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected turn string: {turn} (only 'R' and 'L' are valid)");
            }
        }

        public static string ToShortString(this Turn turn)
        {
            switch (turn)
            {
                case MapGeometry.Turn.Right:
                    return "R";
                case MapGeometry.Turn.Left:
                    return "L";
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected turn: {(int)turn}");
            }
        }
    }
}
