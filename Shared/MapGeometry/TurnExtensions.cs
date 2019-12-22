using System;

namespace Shared.MapGeometry
{
    public static class TurnExtensions
    {
        public static Turn TurnFromString(this string turn)
        {
            switch (turn)
            {
                case "R":
                    return Turn.Right;
                case "L":
                    return Turn.Left;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected turn string: {turn} (only 'R' and 'L' are valid)");
            }
        }

        public static string ToShortString(this Turn turn)
        {
            switch (turn)
            {
                case Turn.Right:
                    return "R";
                case Turn.Left:
                    return "L";
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected turn: {(int)turn}");
            }
        }
    }

}
