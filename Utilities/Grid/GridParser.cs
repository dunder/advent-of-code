namespace Utilities.Grid {
    public static class GridParser {

        public static Grid Parse(string[] input) {

            bool[,] grid = new bool[input[0].Length, input.Length];

            for (int y = 0; y < input.Length; y++) {
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
