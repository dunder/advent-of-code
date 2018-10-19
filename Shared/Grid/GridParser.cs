using System.Collections.Generic;

namespace Shared.Grid {
    public static class GridParser {

        public static Grid Parse(IList<string> input) {

            bool[,] grid = new bool[input[0].Length, input.Count];

            for (int y = 0; y < input.Count; y++) {
                var line = input[y];
                for (int x = 0; x < line.Length; x++) {
                    if (line[x] == '#') {
                        grid[x, y] = true;
                    }
                }
            }

            return new Grid(grid);
        }
    }
}
