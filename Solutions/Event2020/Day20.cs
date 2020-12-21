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

            public Grid(bool[,] squares, int id)
            {
                _squares = squares;
                Id = id;
            }

            public int Size => _squares.GetLength(0);
            public int Width => _squares.GetLength(0);
            public int Height => _squares.GetLength(1);

            public bool[,] Squares => _squares;

            public int Id { get; private set; }
            public int? Top { get; set; }
            public int? Right { get; set; }
            public int? Bottom { get; set; }
            public int? Left { get; set; }

            public List<int?> Adjacent => new List<int?> { Top, Right, Bottom, Left };

            public bool IsCorner => Adjacent.Where(x => x.HasValue).Count() == 2;
                

            public Grid RemoveBorder()
            {
                var borderLessPixels = new bool[Size - 2, Size - 2];
                for (int y = 1; y < Size - 1; y++)
                {
                    for (int x = 1; x < Size - 1; x++)
                    {
                        borderLessPixels[x - 1, y - 1] = _squares[x, y];
                    }
                }

                return new Grid(borderLessPixels, Id);
            }

            public Grid MergeRight(Grid rightGrid)
            {
                return rightGrid;
            }

            public Grid MergeBottom(Grid bottomGrid)
            {
                return bottomGrid;
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

                var grid = new Grid(flippedPixels, Id);

                grid.Top = Top;
                grid.Right = Left;
                grid.Bottom = Bottom;
                grid.Left = Right;

                return grid;
            }

            public Grid RotateRight()
            {
                var rotatedPixels = new bool[Size, Size];
                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        rotatedPixels[Size - 1 - y, x] = _squares[x, y];
                    }
                }

                var grid = new Grid(rotatedPixels, Id);

                grid.Top = Left;
                grid.Right = Top;
                grid.Bottom = Right;
                grid.Left = Bottom;

                return grid;
            }

            public List<Grid> Orientations()
            {
                var orientations = new List<Grid>();

                orientations.Add(this);

                var flippedHorizontally = FlipHorizontal();
                orientations.Add(flippedHorizontally);

                var rotated1 = RotateRight();
                orientations.Add(rotated1);

                var rotated1FlippedHorizontally = rotated1.FlipHorizontal();
                orientations.Add(rotated1FlippedHorizontally);

                var rotated2 = rotated1.RotateRight();
                orientations.Add(rotated2);

                var rotated2FlippedHorizontally = rotated2.FlipHorizontal();
                orientations.Add(rotated2FlippedHorizontally);

                var rotated3 = rotated2.RotateRight();
                orientations.Add(rotated3);

                var rotated3FlippedHorizontally = rotated3.FlipHorizontal();
                orientations.Add(rotated3FlippedHorizontally);

                return orientations;
            }

            public Grid TurnOn(int x, int y)
            {
                return new Grid(Squares.Clone() as bool[,], Id);
            }

            public Grid TurnOn(Point point)
            {
                return TurnOn(point.X, point.Y);
            }

            public Grid TurnOff(int x, int y)
            {
                return new Grid(Squares.Clone() as bool[,], Id);
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

            public static Grid Parse(IList<string> input, int id)
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

                return new Grid(grid, id);
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
                        var grid = GridParser.Parse(currentGrid, currentId);
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

            var lastGrid = GridParser.Parse(currentGrid, currentId);
            grids.Add(currentId, lastGrid);

            return grids;
        }

        private static List<Grid> FlipRotate(Grid grid1)
        {
            var flippedHorizontally = grid1.FlipHorizontal();

            var rotated1 = grid1.RotateRight();
            var rotated1FlippedHorizontally = rotated1.FlipHorizontal();

            var rotated2 = rotated1.RotateRight();
            var rotated2FlippedHorizontally = rotated2.FlipHorizontal();

            var rotated3 = rotated2.RotateRight();
            var rotated3FlippedHorizontally = rotated3.FlipHorizontal();

            return new List<Grid> {
                grid1, flippedHorizontally,
                rotated1, rotated1FlippedHorizontally,
                rotated2,  rotated2FlippedHorizontally,
                rotated3, rotated3FlippedHorizontally
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

        private static Grid MatchGrids(Grid grid1, int grid2Id, Grid grid2)
        {
            var otherVariations = Orientations(grid2);

            foreach (var otherVariation in otherVariations)
            {
                if (MatchTop(grid1, otherVariation.Variation))
                {
                    grid1.Top = grid2Id;
                    break;
                }
                else if (MatchRight(grid1, otherVariation.Variation))
                {
                    grid1.Right = grid2Id;
                    break;
                }
                else if (MatchBottom(grid1, otherVariation.Variation))
                {
                    grid1.Bottom = grid2Id;
                    break;
                }
                else if (MatchLeft(grid1, otherVariation.Variation))
                {
                    grid1.Left = grid2Id;
                    break;
                }
            }

            return grid1;
        }

        private class TileMatch
        {
            public TileMatch(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }

            public List<int> Matches { get; set; } = new List<int>();

            public override string ToString()
            {
                return $"{Id}: " + string.Join(",", Matches.OrderBy(x => x));
            }
        }

        private class GridVariation
        {
            public GridVariation(Grid variation, int rotates, int flip)
            {
                Variation = variation;
                Rotates = rotates;
                Flip = flip;
            }

            public int Rotates { get; private set; }
            public int Flip { get; private set; }

            public Grid Variation { get; private set; }

            public override string ToString()
            {
                return $"R: {Rotates}, F: {Flip}";
            }
        }

        private static List<GridVariation> Orientations(Grid grid)
        {
            var variations = new List<GridVariation>();

            variations.Add(new GridVariation(grid, 0, 0));

            var flippedHorizontally = grid.FlipHorizontal();
            variations.Add(new GridVariation(flippedHorizontally, 0, 1));

            var rotated1 = grid.RotateRight();
            variations.Add(new GridVariation(rotated1, 1, 0));

            var rotated1FlippedHorizontally = rotated1.FlipHorizontal();
            variations.Add(new GridVariation(rotated1FlippedHorizontally, 1, 1));

            var rotated2 = rotated1.RotateRight();
            variations.Add(new GridVariation(rotated2, 2, 0));

            var rotated2FlippedHorizontally = rotated2.FlipHorizontal();
            variations.Add(new GridVariation(rotated2FlippedHorizontally, 2, 1));

            var rotated3 = rotated2.RotateRight();
            variations.Add(new GridVariation(rotated3, 3, 0));

            var rotated3FlippedHorizontally = rotated3.FlipHorizontal();
            variations.Add(new GridVariation(rotated3FlippedHorizontally, 3, 1));

            return variations;
        }

        private static List<List<Grid>> FindPositions(IDictionary<int, Grid> grids)
        {
            foreach (var gridId in grids.Keys)
            {
                var grid = grids[gridId];

                var matchingTiles = new TileMatch(gridId);

                foreach (var otherGridId in grids.Keys.Where(k => k != gridId))
                {
                    var otherGrid = grids[otherGridId];

                    grid = MatchGrids(grid, otherGridId, otherGrid);
                }
            }

            // just take a corner and assign it to 0,0
            var aCorner = grids.Values.First(grid => grid.IsCorner);

            var rotatedCorner = aCorner
                .Orientations()
                .First(orientation => !orientation.Top.HasValue && !orientation.Left.HasValue);

            // "square arrangement" according to problem description; calculate size by square root
            int size = (int)System.Math.Sqrt(grids.Count);

            var arrangement = new List<List<Grid>>();

            Grid Above(List<List<Grid>> arrangement, int x, int y)
            {
                if (y - 1 < 0)
                {
                    return null;
                }

                return arrangement[y-1][x];
            }

            for (int y = 0; y < size; y++)
            {
                var row = new List<Grid>();

                for (int x = 0; x < size; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        row.Add(rotatedCorner);
                    }
                    else if (x < size)
                    {
                        Grid previous = x == 0 ? null : row.Last();
                        Grid above = Above(arrangement, x, y);
                        Grid current = above == null ? grids[previous.Right.Value] : grids[above.Bottom.Value];
                        Grid currentArranged = current
                            .Orientations()
                            .Single(o => o.Top == above?.Id && o.Left == previous?.Id);

                        row.Add(currentArranged);
                    }
                }

                arrangement.Add(row);
            }

            return arrangement;
        }

        private static List<List<Grid>> RemoveBorders(List<List<Grid>> arrangement)
        {
            return arrangement.Select(row => row.Select(grid => grid.RemoveBorder()).ToList()).ToList();
        }

        private static Grid Merge(List<List<Grid>> arrangement)
        {
            var mergeBottom = new List<Grid>();

            foreach (var row in arrangement)
            {
                Grid leftGrid = null;
                
                foreach (var rightGrid in row)
                {
                    if (leftGrid == null)
                    {
                        leftGrid = rightGrid;
                    }
                    else
                    {
                        leftGrid = leftGrid.MergeRight(rightGrid);
                    }
                }

                mergeBottom.Add(leftGrid);
            }

            Grid aboveGrid = null;
            foreach (var bottomGrid in mergeBottom)
            {
                if (aboveGrid == null)
                {
                    aboveGrid = bottomGrid;
                }

                aboveGrid = aboveGrid.MergeBottom(bottomGrid);
            }

            return aboveGrid;
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
            var grids = Parse(input);

            var positions = FindPositions(grids);
            positions = RemoveBorders(positions);

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

            // 3079 is not flipped or rotated in the example
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

            var expectedGrid = GridParser.Parse(expectedLines, 3079);

            Assert.Equal(expectedGrid, withoutBorder);
        }

        [Fact]
        public void SecondStarRemoveBordersExample()
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
            positions = RemoveBorders(positions);

            Assert.All(positions.SelectMany(x => x), x => Assert.Equal(8, x.Size));
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

            var grid3079 = positions.SelectMany(x => x).Single(x => x.Id == 3079);

            Assert.Null(grid3079.Top);
            Assert.Null(grid3079.Right);
            Assert.Equal(2473, grid3079.Bottom);
            Assert.Equal(2311, grid3079.Left);

            var grid1427 = positions.SelectMany(x => x).Single(x => x.Id == 1427);

            Assert.Equal(1489, grid1427.Top);
            Assert.Equal(2473, grid1427.Right);
            Assert.Equal(2311, grid1427.Bottom);
            Assert.Equal(2729, grid1427.Left);
        }

        [Fact]
        public void SecondStarRotateExample()
        {
            var input = new List<string>
            {
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
            };

            var grid = GridParser.Parse(input, 0);


            var rotated = grid.RotateRight();

            Assert.NotEqual(grid, rotated);
        }
    }
}
