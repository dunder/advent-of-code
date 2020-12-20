using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // 

    public class Day20
    {
        private readonly ITestOutputHelper output;

        public Day20(ITestOutputHelper output)
        {
            this.output = output;
        }

        private class Grid : IEnumerable<(Point point, bool value)>
        {
            private readonly bool[,] _squares;

            public Grid(string description)
            {
                var rows = description.Split('/');

                _squares = new bool[rows.Length, rows.Length];

                for (int y = 0; y < rows.Length; y++)
                {
                    var row = rows[y];
                    for (int x = 0; x < row.Length; x++)
                    {
                        _squares[x, y] = row[x] == '#';
                    }
                }
            }

            public Grid(bool[,] squares)
            {
                _squares = squares;
            }

            public int Size => _squares.GetLength(0);
            public int Width => _squares.GetLength(0);
            public int Height => _squares.GetLength(1);

            public bool[,] Squares => _squares;

            public Grid RemoveBorder()
            {
                var borderLessPixels = new bool[Size-2, Size-2];
                for (int y = 1; y < Size-1; y++)
                {
                    for (int x = 1; x < Size-1; x++)
                    {
                        borderLessPixels[x-1,y-1] = _squares[x, y];
                    }
                }

                return new Grid(borderLessPixels);
            }

            public Grid FlipHorizontal()
            {
                var flippedPixels = new bool[Size, Size];
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        flippedPixels[Size - 1 - x, y] = _squares[x, y];
                    }
                }

                return new Grid(flippedPixels);
            }

            public Grid FlipVertical()
            {
                var flippedPixels = new bool[Size, Size];
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        flippedPixels[x, Size - 1 - y] = _squares[x, y];
                    }
                }

                return new Grid(flippedPixels);
            }

            public Grid RotateRight()
            {
                var rotatedPixels = new bool[Size, Size];
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        rotatedPixels[y, Size - 1 - x] = _squares[x, y];
                    }
                }

                return new Grid(rotatedPixels);
            }

            public Grid TurnOn(int x, int y)
            {
                return new Grid(Squares.Clone() as bool[,]);
            }

            public Grid TurnOn(Point point)
            {
                return TurnOn(point.X, point.Y);
            }

            public Grid TurnOff(int x, int y)
            {
                return new Grid(Squares.Clone() as bool[,]);
            }

            public Grid TurnOff(Point point)
            {
                return TurnOff(point.X, point.Y);
            }

            public int OnCount
            {
                get
                {
                    int count = 0;
                    for (int x = 0; x < Size; x++)
                    {
                        for (int y = 0; y < Size; y++)
                        {
                            if (_squares[x, y])
                            {
                                count++;
                            }
                        }
                    }

                    return count;
                }
            }

            public bool IsOn(int x, int y)
            {
                return Squares[x, y];
            }

            public bool IsOn(Point point)
            {
                return IsOn(point.X, point.Y);
            }

            protected bool Equals(Grid other)
            {
                return _squares.Rank == other._squares.Rank &&
                       Enumerable.Range(0, _squares.Rank).All(dimension =>
                           _squares.GetLength(dimension) == other._squares.GetLength(dimension)) &&
                       _squares.Cast<bool>().SequenceEqual(other._squares.Cast<bool>());
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Grid)obj);
            }

            public override int GetHashCode()
            {
                int hash = 0;

                if (_squares == null) return 0;

                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        hash |= 1 << x + y;
                    }
                }

                return hash;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<(Point point, bool value)> GetEnumerator()
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        yield return (new Point(x, y), Squares[x, y]);
                    }
                }
            }

            public override string ToString()
            {
                var stringBuilder = new StringBuilder();
                for (int y = 0; y < _squares.GetLength(1); y++)
                {
                    for (int x = 0; x < _squares.GetLength(0); x++)
                    {
                        stringBuilder.Append(_squares[x, y] ? "#" : ".");
                    }

                    stringBuilder.AppendLine();
                }

                return stringBuilder.ToString();
            }
        }

        private static class GridParser
        {

            public static Grid Parse(IList<string> input)
            {

                bool[,] grid = new bool[input[0].Length, input.Count];

                for (int y = 0; y < input.Count; y++)
                {
                    var line = input[y];
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (line[x] == '#')
                        {
                            grid[x, y] = true;
                        }
                    }
                }

                return new Grid(grid);
            }
        }

        private static IDictionary<int, Grid> Parse(List<string> lines)
        {
            var grids = new Dictionary<int, Grid>();
            var gridIdExpression = new Regex(@"Tile (\d+):");
            int currentId = -1;
            var currentGrid = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.Contains(":"))
                {
                    var m = gridIdExpression.Match(line);
                    var nextId = int.Parse(m.Groups[1].Value);

                    if (currentGrid.Any())
                    {
                        var grid = GridParser.Parse(currentGrid);
                        grids.Add(currentId, grid);

                        currentId = nextId;
                        currentGrid = new List<string>();
                    }
                    else
                    {
                        currentId = nextId;
                    }

                }
                else
                {
                    currentGrid.Add(line);
                }
            }

            var lastGrid = GridParser.Parse(currentGrid);
            grids.Add(currentId, lastGrid);

            return grids;
        }

        private static List<Grid> FlipRotate(Grid grid1)
        {
            var flippedHorizontally = grid1.FlipHorizontal();
            var flippedVertically = grid1.FlipVertical();

            var rotated1 = grid1.RotateRight();
            var rotated1FlippedHorizontally = rotated1.FlipHorizontal();
            var rotated1FlippedVertically = rotated1.FlipVertical();

            var rotated2 = rotated1.RotateRight();
            var rotated2FlippedHorizontally = rotated2.FlipHorizontal();
            var rotated2FlippedVertically = rotated2.FlipVertical();

            var rotated3 = rotated2.RotateRight();
            var rotated3FlippedHorizontally = rotated3.FlipHorizontal();
            var rotated3FlippedVertically = rotated3.FlipVertical();

            return new List<Grid> { 
                grid1, flippedHorizontally, flippedVertically,
                rotated1, rotated1FlippedHorizontally, rotated1FlippedVertically,
                rotated2,  rotated2FlippedHorizontally, rotated2FlippedVertically,
                rotated3, rotated3FlippedHorizontally, rotated3FlippedVertically
            };
        }

        private static bool MatchTop(Grid grid1, Grid grid2)
        {
            for (int x = 0; x < grid1.Width; x++)
            {
                
                if (grid1.IsOn(x, 0) != grid2.IsOn(x, grid2.Height - 1))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool MatchRight(Grid grid1, Grid grid2)
        {
            for (int y = 0; y < grid1.Height; y++)
            {
                if (grid1.IsOn(grid1.Width - 1, y) != grid2.IsOn(0, y))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool MatchBottom(Grid grid1, Grid grid2)
        {
            for (int x = 0; x < grid1.Width; x++)
            {
                if (grid1.IsOn(x, grid1.Height - 1) != grid2.IsOn(x, 0))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool MatchLeft(Grid grid1, Grid grid2)
        {
            for (int y = 0; y < grid1.Height; y++)
            {
                if (grid1.IsOn(0, y) != grid2.IsOn(grid2.Width - 1, y))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<int> FindCornerGrids(IDictionary<int, Grid> grids)
        {
            var cornerGrids = new List<int>();

            foreach (var gridId in grids.Keys)
            {
                var grid = grids[gridId];
                var variations = FlipRotate(grid);

                var top = false;
                var right = false;
                var bottom = false;
                var left = false;

                foreach (var otherGridId in grids.Keys.Where(k => k != gridId))
                {
                    var otherGrid = grids[otherGridId];
                    var otherVariations = FlipRotate(otherGrid);

                    foreach (var variation in variations)
                    {
                        foreach (var otherVariation in otherVariations)
                        {
                            if (MatchTop(variation, otherVariation))
                            {
                                top = true;
                                break;
                            }

                            if (MatchRight(variation, otherVariation))
                            {
                                right = true;
                                break;
                            }

                            if (MatchBottom(variation, otherVariation))
                            {
                                bottom = true;
                                break;
                            }

                            if (MatchLeft(variation, otherVariation))
                            {
                                left = true;
                                break;
                            }
                        }

                        if (top || right || bottom || left)
                        {
                            break;
                        }
                    }
                }

                // top left corner
                if (!top && right && bottom && !left)
                {
                    cornerGrids.Add(gridId);
                }

                // top right corner
                if (!top && !right && bottom && left)
                {
                    cornerGrids.Add(gridId);
                }

                // bottom left corner
                if (top && right && !bottom && !left)
                {
                    cornerGrids.Add(gridId);
                }

                // bottom right corner
                if (top && !right && !bottom && left)
                {
                    cornerGrids.Add(gridId);
                }
            }

            return cornerGrids;
        }

        private static MatchingTiles MatchGrids(Grid grid1, int grid2Id, Grid grid2, MatchingTiles matchingTiles)
        {
            var grid2Variations = FlipRotate(grid2);

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(grid1, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = grid1;
                    return matchingTiles;
                }

                if (MatchRight(grid1, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = grid1;

                    return matchingTiles;
                }

                if (MatchBottom(grid1, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = grid1;
                    return matchingTiles;
                }

                if (MatchLeft(grid1, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = grid1;
                    return matchingTiles;
                }
            }

            var flippedHorizontally = grid1.FlipHorizontal();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(flippedHorizontally, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = flippedHorizontally;
                    return matchingTiles;
                }

                if (MatchRight(flippedHorizontally, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = flippedHorizontally;
                    return matchingTiles;
                }

                if (MatchBottom(flippedHorizontally, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = flippedHorizontally;
                    return matchingTiles;
                }

                if (MatchLeft(flippedHorizontally, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = flippedHorizontally;
                    return matchingTiles;
                }
            }

            var flippedVertically = grid1.FlipVertical();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(grid1, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = flippedVertically;
                    return matchingTiles;
                }

                if (MatchRight(grid1, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = flippedVertically;
                    return matchingTiles;
                }

                if (MatchBottom(grid1, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = flippedVertically;
                    return matchingTiles;
                }

                if (MatchLeft(grid1, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = flippedVertically;
                    return matchingTiles;
                }
            }

            var rotated1 = grid1.RotateRight();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated1, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated1;
                    return matchingTiles;
                }

                if (MatchRight(rotated1, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated1;
                    return matchingTiles;
                }

                if (MatchBottom(rotated1, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated1;
                    return matchingTiles;
                }

                if (MatchLeft(rotated1, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated1;
                    return matchingTiles;
                }
            }

            var rotated1FlippedHorizontally = rotated1.FlipHorizontal();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated1FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated1FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchRight(rotated1FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated1FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchBottom(rotated1FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated1FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchLeft(rotated1FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated1FlippedHorizontally;
                    return matchingTiles;
                }
            }

            var rotated1FlippedVertically = rotated1.FlipVertical();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated1FlippedVertically, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated1FlippedVertically;
                    return matchingTiles;
                }

                if (MatchRight(rotated1FlippedVertically, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated1FlippedVertically;
                    return matchingTiles;
                }

                if (MatchBottom(rotated1FlippedVertically, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated1FlippedVertically;
                    return matchingTiles;
                }

                if (MatchLeft(rotated1FlippedVertically, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated1FlippedVertically;
                    return matchingTiles;
                }
            }

            var rotated2 = rotated1.RotateRight();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated2, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated2;
                    return matchingTiles;
                }

                if (MatchRight(rotated2, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated2;
                    return matchingTiles;
                }

                if (MatchBottom(rotated2, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated2;
                    return matchingTiles;
                }

                if (MatchLeft(rotated2, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated2;
                    return matchingTiles;
                }
            }

            var rotated2FlippedHorizontally = rotated2.FlipHorizontal();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated2FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated2FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchRight(rotated2FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated2FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchBottom(rotated2FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated2FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchLeft(rotated2FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated2FlippedHorizontally;
                    return matchingTiles;
                }
            }

            var rotated2FlippedVertically = rotated2.FlipVertical();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated2FlippedVertically, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated2FlippedVertically;
                    return matchingTiles;
                }

                if (MatchRight(rotated2FlippedVertically, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated2FlippedVertically;
                    return matchingTiles;
                }

                if (MatchBottom(rotated2FlippedVertically, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated2FlippedVertically;
                    return matchingTiles;
                }

                if (MatchLeft(rotated2FlippedVertically, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated2FlippedVertically;
                    return matchingTiles;
                }
            }

            var rotated3 = rotated2.RotateRight();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated3, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated3;
                    return matchingTiles;
                }

                if (MatchRight(rotated3, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated3;
                    return matchingTiles;
                }

                if (MatchBottom(rotated3, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated3;
                    return matchingTiles;
                }

                if (MatchLeft(rotated3, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated3;
                    return matchingTiles;
                }
            }

            var rotated3FlippedHorizontally = rotated3.FlipHorizontal();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated3FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated3FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchRight(rotated3FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated3FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchBottom(rotated3FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated3FlippedHorizontally;
                    return matchingTiles;
                }

                if (MatchLeft(rotated3FlippedHorizontally, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated3FlippedHorizontally;
                    return matchingTiles;
                }
            }

            var rotated3FlippedVertically = rotated3.FlipVertical();

            foreach (var grid2Variation in grid2Variations)
            {
                if (MatchTop(rotated3FlippedVertically, grid2Variation))
                {
                    matchingTiles.Left = grid2Id;
                    matchingTiles.Variation = rotated3FlippedVertically;
                    return matchingTiles;
                }

                if (MatchRight(rotated3FlippedVertically, grid2Variation))
                {
                    matchingTiles.Bottom = grid2Id;
                    matchingTiles.Variation = rotated3FlippedVertically;
                    return matchingTiles;
                }

                if (MatchBottom(rotated3FlippedVertically, grid2Variation))
                {
                    matchingTiles.Right = grid2Id;
                    matchingTiles.Variation = rotated3FlippedVertically;
                    return matchingTiles;
                }

                if (MatchLeft(rotated3FlippedVertically, grid2Variation))
                {
                    matchingTiles.Top = grid2Id;
                    matchingTiles.Variation = rotated3FlippedVertically;
                    return matchingTiles;
                }
            }

            return matchingTiles;
        }
        
        private class MatchingTiles
        {
            public MatchingTiles(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
            public Grid Variation { get; set; }
            public int? Top { get; set; }
            public int? Right { get; set; }
            public int? Bottom { get; set; }
            public int? Left { get; set; }

            public override string ToString()
            {
                var top = Top.HasValue ? Top.Value.ToString() : "-";
                var right = Right.HasValue ? Right.Value.ToString() : "-";
                var bottom = Bottom.HasValue ? Bottom.Value.ToString() : "-";
                var left = Left.HasValue ? Left.Value.ToString() : "-";

                return $"({top},{right},{bottom},{left})";
            }
        }

        private static IDictionary<int, MatchingTiles> FindPositions(IDictionary<int, Grid> grids)
        {
            var positions = new Dictionary<int, MatchingTiles>();

            foreach (var gridId in grids.Keys)
            {
                var grid = grids[gridId];
                var variations = FlipRotate(grid);

                var matchingTiles = new MatchingTiles(gridId);

                foreach (var otherGridId in grids.Keys.Where(k => k != gridId))
                {
                    var otherGrid = grids[otherGridId];

                    matchingTiles = MatchGrids(grid, otherGridId, otherGrid, matchingTiles);
                }

                positions.Add(gridId, matchingTiles);
            }

            return positions;
        }

        public long FirstStar()
        {
            var input = ReadLineInput().ToList();
            var grids = Parse(input);

            var corners = FindCornerGrids(grids);

            var result = corners.Aggregate(1L, (x, y) => x * y);
            return result;
        }

        public long SecondStar()
        {
            var input = ReadLineInput().ToList();

            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(22878471088273, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
                "Tile 1951:",
                "#.##...##.",
                "#.####...#",
                ".....#..##",
                "#...######",
                ".##.#....#",
                ".###.#####",
                "###.##.##.",
                ".###....#.",
                "..#.#..#.#",
                "#...##.#..",
                "          ",
                "Tile 1171:",
                "####...##.",
                "#..##.#..#",
                "##.#..#.#.",
                ".###.####.",
                "..###.####",
                ".##....##.",
                ".#...####.",
                "#.##.####.",
                "####..#...",
                ".....##...",
                "          ",
                "Tile 1427:",
                "###.##.#..",
                ".#..#.##..",
                ".#.##.#..#",
                "#.#.#.##.#",
                "....#...##",
                "...##..##.",
                "...#.#####",
                ".#.####.#.",
                "..#..###.#",
                "..##.#..#.",
                "          ",
                "Tile 1489:",
                "##.#.#....",
                "..##...#..",
                ".##..##...",
                "..#...#...",
                "#####...#.",
                "#..#.#.#.#",
                "...#.#.#..",
                "##.#...##.",
                "..##.##.##",
                "###.##.#..",
                "          ",
                "Tile 2473:",
                "#....####.",
                "#..#.##...",
                "#.##..#...",
                "######.#.#",
                ".#...#.#.#",
                ".#########",
                ".###.#..#.",
                "########.#",
                "##...##.#.",
                "..###.#.#.",
                "          ",
                "Tile 2971:",
                "..#.#....#",
                "#...###...",
                "#.#.###...",
                "##.##..#..",
                ".#####..##",
                ".#..####.#",
                "#..#.#..#.",
                "..####.###",
                "..#.#.###.",
                "...#.#.#.#",
                "          ",
                "Tile 2729:",
                "...#.#.#.#",
                "####.#....",
                "..#.#.....",
                "....#..#.#",
                ".##..##.#.",
                ".#.####...",
                "####.#.#..",
                "##.####...",
                "##..#.##..",
                "#.##...##.",
                "          ",
                "Tile 3079:",
                "#.#.#####.",
                ".#..######",
                "..#.......",
                "######....",
                "####.#..#.",
                ".#...#.##.",
                "#.#####.##",
                "..#.###...",
                "..#.......",
                "..#.###..."
            };

            var grids = Parse(input);

            var corners = FindCornerGrids(grids);

            var result = corners.Aggregate(1L, (x, y) => x * y);

            Assert.Equal(20899048083289, result);
        }

        [Fact]
        public void SecondStarRemoveBorderExample()
        {
            var input = new List<string>
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
                "Tile 1951:",
                "#.##...##.",
                "#.####...#",
                ".....#..##",
                "#...######",
                ".##.#....#",
                ".###.#####",
                "###.##.##.",
                ".###....#.",
                "..#.#..#.#",
                "#...##.#..",
                "          ",
                "Tile 1171:",
                "####...##.",
                "#..##.#..#",
                "##.#..#.#.",
                ".###.####.",
                "..###.####",
                ".##....##.",
                ".#...####.",
                "#.##.####.",
                "####..#...",
                ".....##...",
                "          ",
                "Tile 1427:",
                "###.##.#..",
                ".#..#.##..",
                ".#.##.#..#",
                "#.#.#.##.#",
                "....#...##",
                "...##..##.",
                "...#.#####",
                ".#.####.#.",
                "..#..###.#",
                "..##.#..#.",
                "          ",
                "Tile 1489:",
                "##.#.#....",
                "..##...#..",
                ".##..##...",
                "..#...#...",
                "#####...#.",
                "#..#.#.#.#",
                "...#.#.#..",
                "##.#...##.",
                "..##.##.##",
                "###.##.#..",
                "          ",
                "Tile 2473:",
                "#....####.",
                "#..#.##...",
                "#.##..#...",
                "######.#.#",
                ".#...#.#.#",
                ".#########",
                ".###.#..#.",
                "########.#",
                "##...##.#.",
                "..###.#.#.",
                "          ",
                "Tile 2971:",
                "..#.#....#",
                "#...###...",
                "#.#.###...",
                "##.##..#..",
                ".#####..##",
                ".#..####.#",
                "#..#.#..#.",
                "..####.###",
                "..#.#.###.",
                "...#.#.#.#",
                "          ",
                "Tile 2729:",
                "...#.#.#.#",
                "####.#....",
                "..#.#.....",
                "....#..#.#",
                ".##..##.#.",
                ".#.####...",
                "####.#.#..",
                "##.####...",
                "##..#.##..",
                "#.##...##.",
                "          ",
                "Tile 3079:",
                "#.#.#####.",
                ".#..######",
                "..#.......",
                "######....",
                "####.#..#.",
                ".#...#.##.",
                "#.#####.##",
                "..#.###...",
                "..#.......",
                "..#.###..."
            };

            var grids = Parse(input);

            // 3079 is not flippted or rotated in the example
            var withoutBorder = grids[3079].RemoveBorder();

            var expectedLines = new List<string>
            {
                "#..#####",
                ".#......",
                "#####...",
                "###.#..#",
                "#...#.##",
                ".#####.#",
                ".#.###..",
                ".#......"
            };

            var expectedGrid = GridParser.Parse(expectedLines);

            Assert.Equal(expectedGrid, withoutBorder);
        }

        [Fact]
        public void SecondStarFindPositionsExample()
        {
            var input = new List<string>
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
                "Tile 1951:",
                "#.##...##.",
                "#.####...#",
                ".....#..##",
                "#...######",
                ".##.#....#",
                ".###.#####",
                "###.##.##.",
                ".###....#.",
                "..#.#..#.#",
                "#...##.#..",
                "          ",
                "Tile 1171:",
                "####...##.",
                "#..##.#..#",
                "##.#..#.#.",
                ".###.####.",
                "..###.####",
                ".##....##.",
                ".#...####.",
                "#.##.####.",
                "####..#...",
                ".....##...",
                "          ",
                "Tile 1427:",
                "###.##.#..",
                ".#..#.##..",
                ".#.##.#..#",
                "#.#.#.##.#",
                "....#...##",
                "...##..##.",
                "...#.#####",
                ".#.####.#.",
                "..#..###.#",
                "..##.#..#.",
                "          ",
                "Tile 1489:",
                "##.#.#....",
                "..##...#..",
                ".##..##...",
                "..#...#...",
                "#####...#.",
                "#..#.#.#.#",
                "...#.#.#..",
                "##.#...##.",
                "..##.##.##",
                "###.##.#..",
                "          ",
                "Tile 2473:",
                "#....####.",
                "#..#.##...",
                "#.##..#...",
                "######.#.#",
                ".#...#.#.#",
                ".#########",
                ".###.#..#.",
                "########.#",
                "##...##.#.",
                "..###.#.#.",
                "          ",
                "Tile 2971:",
                "..#.#....#",
                "#...###...",
                "#.#.###...",
                "##.##..#..",
                ".#####..##",
                ".#..####.#",
                "#..#.#..#.",
                "..####.###",
                "..#.#.###.",
                "...#.#.#.#",
                "          ",
                "Tile 2729:",
                "...#.#.#.#",
                "####.#....",
                "..#.#.....",
                "....#..#.#",
                ".##..##.#.",
                ".#.####...",
                "####.#.#..",
                "##.####...",
                "##..#.##..",
                "#.##...##.",
                "          ",
                "Tile 3079:",
                "#.#.#####.",
                ".#..######",
                "..#.......",
                "######....",
                "####.#..#.",
                ".#...#.##.",
                "#.#####.##",
                "..#.###...",
                "..#.......",
                "..#.###..."
            };

            var grids = Parse(input);


            var positions = FindPositions(grids);

            var grid3079 = positions[3079];

            Assert.False(grid3079.Top.HasValue);
            Assert.False(grid3079.Right.HasValue);
            Assert.Equal(2311, grid3079.Left);
            Assert.Equal(2473, grid3079.Bottom);

            var grid1427 = positions[1427];

            Assert.Equal(2311, grid1427.Top);
            Assert.Equal(2473, grid1427.Right);
            Assert.Equal(1489, grid1427.Bottom);
            Assert.Equal(2729, grid1427.Left);
        }
    }
}
