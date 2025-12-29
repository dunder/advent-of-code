using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 12: Christmas Tree Farm ---
    public class Day12
    {
        private readonly ITestOutputHelper output;

        public Day12(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Region(int Width, int Length, List<int> Quantities);

        private static int[,] ParseShape(List<string> lines)
        {
            var shape = new int[lines[0].Length,lines.Count];

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    shape[x,y] = lines[y][x] == '#' ? 1 : 0;
                }
            }

            return shape;
        }

        private class Shape
        {
            private int[,] _shape;

            // This shape:
            //
            // #..
            // ##.
            // ###
            //
            // layed out as e.g 100 110 111 and represented as int
            private int _key;

            // This shape:
            //
            // #..
            // ##.
            // ###
            //
            // layed out as lines of longs [100, 110, 111]
            private List<ulong> _long;

            private string _string;

            private int _size;

            public Shape(int[,] shape)
            {
                _shape = shape;
                _key = CreateKey(shape);
                _long = CreateLongArray(shape);
                _string = CreateString(shape);
                _size = CalculateSize(shape);
            }

            public int Int => _key;

            public int[,] As2DArray => _shape;
            public List<ulong> AsLongArray => _long;

            public int Size => _size;

            public Shape Rotate()
            {
                var rotatedPixels = new int[_shape.GetLength(0), _shape.GetLength(1)];

                for (int y = 0; y < _shape.GetLength(1); y++)
                {
                    for (int x = 0; x < _shape.GetLength(0); x++)
                    {
                        rotatedPixels[_shape.GetLength(0) - 1 - y, x] = _shape[x, y];
                    }
                }

                return new Shape(rotatedPixels);
            }

            public IEnumerable<Shape> Variations
            {
                get
                {
                    List<Shape> flips = [this, FlipX(), FlipY()];

                    HashSet<int> yielded = [];

                    foreach (var flip in flips)
                    {
                        var rotated = flip;

                        if (yielded.Add(rotated.Int))
                        {
                            yield return rotated;
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            rotated = rotated.Rotate();

                            if (yielded.Add(rotated.Int))
                            {
                                yield return rotated;
                            }
                        }
                    }
                }
            }

            public Shape FlipX()
            {
                var flippedPixels = new int[_shape.GetLength(0), _shape.GetLength(1)];

                for (int y = 0; y < _shape.GetLength(1); y++)
                {
                    for (int x = 0; x < _shape.GetLength(0); x++)
                    {
                        flippedPixels[x, _shape.GetLength(0) - 1 - y] = _shape[x, y];
                    }
                }
                return new Shape(flippedPixels);
            }

            public Shape FlipY()
            {
                var flippedPixels = new int[_shape.GetLength(0), _shape.GetLength(1)];

                for (int y = 0; y < _shape.GetLength(1); y++)
                {
                    for (int x = 0; x < _shape.GetLength(0); x++)
                    {
                        flippedPixels[_shape.GetLength(0) - 1 - x, y] = _shape[x, y];
                    }
                }
                return new Shape(flippedPixels);
            }

            public override string ToString()
            {
                return _string;
            }

            private static int CreateKey(int[,] shape)
            {
                int result = 0;

                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    string row = "";
                    for (int x = 0; x < shape.GetLength(0); x++) {
                        row += shape[x, y];
                    }
                    var tmp = Convert.ToInt32(row, 2);
                    var tmp2 = tmp << (2 - y) * 3;
                    result +=  tmp2;
                }

                return result;
            }

            private static List<ulong> CreateLongArray(int[,] shape)
            {
                List<ulong> result = [];

                for (int y = 0; y < shape.GetLength(1);y++)
                {
                    string row = "";
                    for (int x = 0; x < shape.GetLength(0); x++) {
                        row += shape[x, y];
                    }
                    result.Add((ulong)Convert.ToInt32(row, 2));
                }

                return result;
            }

            private static string CreateString(int[,] shape)
            {
                StringBuilder sb = new();

                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    string row = "";
                    for (int x = 0; x < shape.GetLength(0); x++)
                    {
                        row += shape[x, y] == 1 ? "#" : " ";
                    }
                    sb.AppendLine(row);
                }

                return sb.ToString();
            }

            private int CalculateSize(int[,] shape)
            {
                int size = 0;

                for (int y = 0; y < shape.GetLength(1); y++)
                {
                    for (int x = 0; x < shape.GetLength(0); x++)
                    {

                        if (shape[x, y] == 1)
                        {
                            size++;
                        }
                    }
                }

                return size;
            }
        }

        private static Region ParseRegion(string line)
        {
            var parts = line.Split(": ");
            var dimensions = parts[0].Split("x").Select(int.Parse).ToList();
            var quantities = parts[1..][0].Split(" ").Select(int.Parse).ToList();

            return new Region(dimensions[0], dimensions[1], quantities);
        }

        private static (List<Shape> shapes, List<Region> regions) Parse(IList<string> input)
        {
            List<List<string>> groups = input.Split("").Select(g => g.ToList()).ToList();
            List<Shape> shapes = groups[..^1].Select(lines => ParseShape(lines[1..])).Select(s => new Shape(s)).ToList();
            List<Region> regions = groups[^1].Select(ParseRegion).ToList();

            return (shapes, regions);
        }

        private static List<List<Shape>> CreateShapeLookup(List<Shape> shapes)
        {
            List<List<Shape>> lookup = [];

            foreach (var shape in shapes)
            {
                lookup.Add(shape.Variations.ToList());
            }

            return lookup;
        }

        private static List<ulong> ToCompactRegion(Region region)
        {
            List<ulong> compact = [];

            ulong filledLine = (ulong)Convert.ToInt64(string.Join("", Enumerable.Repeat("1", 64)), 2);

            var filledRegion = Enumerable.Repeat(filledLine, region.Length).ToList();

            var openRegion = filledRegion.Select(line => line << region.Width).ToList();

            return openRegion;
        }

        private static List<ulong> Merge(List<ulong> region, List<ulong> shape, int x, int y)
        {
            var before = y > 0 ? region[0..y] : [];

            var shapXShifted = shape.Select(line => line << x);

            List<ulong> merged =
                [
                    ..before,
                    ..region.Skip(y).Zip(shapXShifted).Select(pair => pair.First | pair.Second),
                    ..region[(y+3)..]
                ];

            return merged;
        }

        // check if shape overlaps region when shape placed at (x,y) where (0,0) is top right corner
        // shape is assumed to always be 3 rows
        private static bool Overlaps(List<ulong> region, List<ulong> shape, int x, int y)
        {
            int yMax = region.Count - 3;

            if (y > yMax)
            {
                throw new ArgumentOutOfRangeException(nameof(y), $"The shape must be fully located within the region, y = {y}, yMax = {yMax}");
            }

            // ulong is 64 bits
            int xMax = 63 - 2;

            if (x > xMax)
            {
                throw new ArgumentOutOfRangeException(nameof(x), $"The shape must be fully located within the region, x = {x}, xMax = {xMax}");
            }

            var matched = region[y..(y + 3)];

            var shapeXShifted = shape.Select(line => line << x);

            var overlap = matched.Zip(shapeXShifted).Select(line => line.First & line.Second);

            return overlap.Any(x => x > 0);
        }

        private static bool TryGetCached(
            List<ulong> region,
            List<(int, int)> positions,
            List<int> quantities,
            out bool value)
        {
            value = default;

            if (Cache.TryGetValue(region, out var r) && r.TryGetValue(positions, out var p) && p.TryGetValue(quantities, out var q))
            {
                value = q;
                return true;
            }
            return false;
        }

        private static void UpdateCache(
            List<ulong> region,
            List<(int,int)> positions,
            List<int> quantities,
            bool result)
        {
            if (!Cache.ContainsKey(region))
            {
                Cache.Add(region, new Dictionary<List<(int, int)>, Dictionary<List<int>, bool>>(SequenceComparer<(int, int)>.Default));
            }

            if (!Cache[region].ContainsKey(positions))
            {
                Cache[region].Add(positions, new Dictionary<List<int>, bool>(SequenceComparer<int>.Default));
            }

            Cache[region][positions].TryAdd(quantities, result);
        }

        private static bool PresentSizesFits(List<ulong> region, List<int> quantities)
        {
            int requiredSpace = quantities.Zip(ShapeSizes).Select(p => p.First * p.Second).Sum();

            int remainingSpace = region.Select(line => 64 - BitOperations.PopCount(line)).Sum();

            return remainingSpace >= requiredSpace;
        }

        private static List<List<Shape>> ShapeLookup { get; set; }
        private static List<int> ShapeSizes { get; set; }
        private static Dictionary<List<ulong>, Dictionary<List<(int, int)>, Dictionary<List<int>, bool>>> Cache { get; } = new(SequenceComparer<ulong>.Default);

        private static bool PresentsFitsRegion(
            List<ulong> region,
            List<(int x, int y)> positions,
            List<int> quantities)
        {
            if (TryGetCached(region, positions, quantities, out bool value))
            {
                return value;
            }

            if (quantities.All(q => q == 0))
            {
                return true;
            }

            if (positions.Count == 0)
            {
                return false;
            }

            if (!PresentSizesFits(region, quantities))
            {
                return false;
            }

            bool result = false;

            (int x, int y) = positions.First();

            for (int i = 0; i < ShapeLookup.Count; i++)
            {
                if (quantities[i] == 0)
                {
                    continue;
                }

                List<Shape> shapes = ShapeLookup[i];

                foreach (var variation in shapes)
                {
                    List<ulong> shape = variation.AsLongArray;

                    List<int> newQuantities = [.. quantities];

                    var newPositions = positions.Skip(1).ToList();

                    if (!Overlaps(region, shape, x, y))
                    {
                        newQuantities[i] -= 1;
                        var newRegion = Merge(region, shape, x, y);

                        result = result || PresentsFitsRegion(newRegion, newPositions, newQuantities);
                    }
                    else
                    {
                        result = result || PresentsFitsRegion(region, newPositions, newQuantities);
                    }

                    UpdateCache(region, positions, quantities, result);

                    if (result)
                    {

                        return true;
                    }
                }
            }

            return result;
        }

        private static List<(int x, int y)> Positions(Region region)
        {
            List<(int x, int y)> positions = [];

            for (int x = 0; x < region.Width - 2; x++)
            {
                for (int y = 0; y < region.Length - 2; y++)
                {
                    positions.Add((x, y));
                }
            }

            return positions;
        }

        private class SequenceComparer<T> : IEqualityComparer<IReadOnlyList<T>>
        {
            public static readonly SequenceComparer<T> Default = new(EqualityComparer<T>.Default);

            private readonly IEqualityComparer<T> _elementComparer;

            public SequenceComparer(IEqualityComparer<T> elementComparer)
                => _elementComparer = elementComparer ?? EqualityComparer<T>.Default;

            public bool Equals(IReadOnlyList<T>? x, IReadOnlyList<T>? y)
            {
                if (ReferenceEquals(x, y)) { return true; };
                if (x is null || y is null) { return false; };
                if (x.Count != y.Count) { return false; };

                for (int i = 0; i < x.Count; i++)
                {
                    if (!_elementComparer.Equals(x[i], y[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(IReadOnlyList<T> obj)
            {
                var hc = new HashCode();

                hc.Add(obj.Count);

                for (int i = 0; i < obj.Count; i++)
                {
                    hc.Add(obj[i], _elementComparer);
                }

                return hc.ToHashCode();
            }
        }

        private static int Problem1(IList<string> input)
        {
            (List<Shape> shapes, List<Region> regions) = Parse(input);

            ShapeLookup = CreateShapeLookup(shapes);
            ShapeSizes = shapes.Select(shape => shape.Size).ToList();

            return regions.Count(region => PresentsFitsRegion(ToCompactRegion(region), Positions(region), region.Quantities));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(510, Problem1(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(2, Problem1(exampleInput));
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(1,0)]
        [InlineData(2,0)]
        [InlineData(0,1)]
        [InlineData(1,1)]
        [InlineData(2,1)]
        [InlineData(0,2)]
        [InlineData(1,2)]
        [InlineData(2,2)]
        [Trait("Example", "2025")]
        public void OverlapWhenOverlappingTest(int x, int y)
        {
            // ...###
            // ...###
            // ...###
            // ......
            // ......
            // ......

            List<ulong> region = [7,7,7,0,0,0];

            // ..X
            // ...
            // ...

            List<ulong> shape = [1,0,0];

            // should overlap within the top 3x3 region
            Assert.True(Overlaps(region, shape, x, y));
        }

        [Theory]
        [InlineData(0,3)]
        [InlineData(1,3)]
        [InlineData(2,3)]
        [InlineData(3,0)]
        [InlineData(3,1)]
        [InlineData(3,2)]
        [InlineData(3,3)]
        [Trait("Example", "2025")]
        public void OverlapWhenNotOverlappingTest(int x, int y)
        {
            // ...###
            // ...###
            // ...###
            // ......
            // ......
            // ......

            List<ulong> region = [7,7,7,0,0,0];

            // ..X
            // ...
            // ...

            List<ulong> shape = [1,0,0];

            // should not overlap outside of the top 3x3 region
            Assert.False(Overlaps(region, shape, x, y));
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(0,1)]
        [InlineData(1,1)]
        [InlineData(0,2)]
        [InlineData(1,2)]
        [InlineData(0,3)]
        [InlineData(1,3)]
        [InlineData(2,3)]
        [Trait("Example", "2025")]
        public void MergeWithNoOverlapTest(int x, int y)
        {
            // ......
            // ......
            // ......
            // ......
            // ......
            // ......

            List<ulong> region = [0,0,0,0,0,0];

            // ###
            // ##.
            // ##.

            List<ulong> shape1 = [7,6,6];

            // .XX
            // XXX
            // XX.

            List<ulong> shape2 = [3,7,6];


            // ......
            // ###...
            // ##....
            // ##....
            // ......
            // ......

            var merged = Merge(region, shape1, 3, 1);

            // reference for no overlap (0,0)

            // ....XX
            // ###XXX
            // ##.XX.
            // ##....
            // ......
            // ......

            Assert.False(Overlaps(merged, shape2, x, y));
        }

        [Theory]
        [InlineData(1,0)]
        [InlineData(2,0)]
        [InlineData(3,0)]
        [InlineData(2,1)]
        [InlineData(3,1)]
        [InlineData(2,2)]
        [InlineData(3,2)]
        [Trait("Example", "2025")]
        public void MergeWithOverlapTest(int x, int y)
        {
            // ......
            // ......
            // ......
            // ......
            // ......
            // ......

            List<ulong> region = [0,0,0,0,0,0];

            // ###
            // ##.
            // ##.

            List<ulong> shape1 = [7,6,6];

            // .XX
            // XXX
            // XX.

            List<ulong> shape2 = [3,7,6];


            // ......
            // ###...
            // ##....
            // ##....
            // ......
            // ......

            var merged = Merge(region, shape1, 3, 1);

            // reference for overlap (1,0)

            // ...XX.
            // ##OXX.
            // ##XX..
            // ##....
            // ......
            // ......

            Assert.True(Overlaps(merged, shape2, x, y));
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 6, 0)]
        [InlineData(2, 2, 2)]
        [InlineData(6, 1, 0)]
        [Trait("Example", "2025")]
        public void PresentSizesFitsTests(int x, int y, int z)
        {
            // empty region of 6x6 = 36
            // ......
            // ......
            // ......
            // ......
            // ......
            // ......

            List<ulong> region = [
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
            ];


            ShapeSizes = [5, 6, 7];

            List<int> quantities = [x, y, z];

            Assert.True(PresentSizesFits(region, quantities));
        }

        [Theory]
        [InlineData(0, 7, 0)]
        [InlineData(6, 1, 1)]
        [Trait("Example", "2025")]
        public void PresentSizesFitsNoFitTests(int x, int y, int z)
        {
            // empty region of 6x6 = 36
            // ......
            // ......
            // ......
            // ......
            // ......
            // ......

            List<ulong> region = [
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
                ulong.MaxValue << 6,
            ];

            ShapeSizes = [5, 6, 7];

            List<int> quantities = [x, y, z];

            Assert.False(PresentSizesFits(region, quantities));
        }
    }
}