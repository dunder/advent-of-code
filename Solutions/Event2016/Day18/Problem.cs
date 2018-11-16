using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solutions.Event2016.Day18
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day18;

        private const string Input =
            ".^^.^^^..^.^..^.^^.^^^^.^^.^^...^..^...^^^..^^...^..^^^^^^..^.^^^..^.^^^^.^^^.^...^^^.^^.^^^.^.^^.^.";

        public override string FirstStar()
        {
            var safeTiles = CountSafeTiles(Input, 40);
            return safeTiles.ToString();
        }

        public override string SecondStar()
        {
            var safeTiles = CountSafeTiles(Input, 400_000);
            return safeTiles.ToString();
        }

        private static readonly HashSet<string> TrapCombos = new HashSet<string>
        {
            "^^.",
            ".^^",
            "^..",
            "..^"
        };
        public static bool IsTrap(char left, char center, char right)
        {
            var combo = new String(new[] {left, center, right});

            return TrapCombos.Contains(combo);
        }

        public static string NextRow(string row)
        {
            StringBuilder nextRow = new StringBuilder();
            for (int i = 0; i < row.Length; i++)
            {
                var left = i == 0 ? '.' : row[i - 1];
                var center = row[i];
                var right = i == row.Length - 1 ? '.' : row[i + 1];

                var nextRowForPosition = IsTrap(left, center, right) ? "^" : ".";

                nextRow.Append(nextRowForPosition);
            }
            return nextRow.ToString();
        }

        public static List<string> CreateMap(string firstRow, int totalRows)
        {
            var map = new List<string> {firstRow};
            var currentRow = firstRow;
            foreach (var _ in Enumerable.Range(0, totalRows - 1))
            {
                currentRow = NextRow(currentRow);
                map.Add(currentRow);
            }

            return map;
        }

        public static int CountSafeTiles(string firstRow, int totalRows)
        {
            var map = CreateMap(firstRow, totalRows);

            return map.Aggregate(0, (a, row) => a + row.Count(t => t == '.'));
        }
    }
}