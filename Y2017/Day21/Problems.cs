using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day21 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day21\input.txt");

            var result = PixelArt.CountPixelsOnAfterExpansion(input, 5);

            Assert.Equal(152, result);
            _output.WriteLine($"Day 21 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day21\input.txt");

            var result = PixelArt.CountPixelsOnAfterExpansion(input, 18);

            Assert.Equal(1956174, result);
            _output.WriteLine($"Day 21 problem 2: {result}");
        }
    }

    public class PixelArt {
        public static int CountPixelsOnAfterExpansion(string[] input, int iterations) {

            var initialPixelSet = new PixelSet(".#./..#/###");

            var rules = ReadRules(input);

            var expanded = initialPixelSet;
            for (int i = 0; i < iterations; i++) {
                expanded = expanded.Expand(rules);
            }

            return expanded.OnCount;
        }

        public static Dictionary<PixelSet, PixelSet> ReadRules(string[] input) {

            var rules = new Dictionary<PixelSet, PixelSet>();

            foreach (var rule in input) {
                var ruleSplit = Regex.Split(rule, " => ");
                var pixelInputDescription = ruleSplit[0];
                var pixelOutputDescription = ruleSplit[1];
                var pixelInput = new PixelSet(pixelInputDescription);
                var pixelOutput = new PixelSet(pixelOutputDescription);

                rules.Add(pixelInput, pixelOutput);
            }
            return rules;
        }
    }

    public class PixelSet {
        private readonly bool[,] _pixels;

        public PixelSet(string description) {
            var rows = description.Split('/');

            _pixels = new bool[rows.Length,rows.Length];
            
            for (int y = 0; y < rows.Length; y++) {
                var row = rows[y];
                for (int x = 0; x < row.Length; x++) {
                    _pixels[x, y] = row[x] == '#';
                }
            }
        }
        public PixelSet(List<PixelSet> pixelSets) {
            if (pixelSets.Any(p => p == null)) {
                throw new ArgumentNullException(nameof(pixelSets), "All PixelSet isntances in the list must be non null");
            }
            if (!pixelSets.Any()) {
                throw new ArgumentOutOfRangeException(nameof(pixelSets), "There must be at least one PixelSet instance in the list");
            }
            if (pixelSets.Any(p => p.Size != pixelSets.First().Size)) {
                throw new ArgumentOutOfRangeException($"All PixelSet instances in the list must have the same size");
            }

            double squaredCount = Math.Sqrt(pixelSets.Count);
            int squaredIntCount = (int) Math.Sqrt(pixelSets.Count);
            // ReSharper disable once CompareOfFloatsByEqualityOperator the whole point is to 
            if (squaredIntCount != squaredCount) {
                throw new ArgumentOutOfRangeException(nameof(pixelSets), $"The sqrt of the number of elements in the list must be an int, was: {pixelSets.Count}");
            }

            int oldSize = pixelSets.First().Size;
            int pixelSetsPerNewRow = (int) Math.Sqrt(pixelSets.Count);
            int newSize = oldSize*pixelSetsPerNewRow;

            bool[,] pixels = new bool[newSize, newSize];
            int pixelSetPositionX = 0;
            int pixelSetPositionY = 0;
            foreach (var pixelSet in pixelSets) {
                for (int py = 0; py < oldSize; py++) {
                    for (int px = 0; px < oldSize; px++) {
                        pixels[pixelSetPositionX*oldSize + px, pixelSetPositionY*oldSize + py] = pixelSet._pixels[px, py];
                    }
                }
                pixelSetPositionX += 1;
                if (pixelSetPositionX % pixelSetsPerNewRow == 0) {
                    pixelSetPositionX = 0;
                    pixelSetPositionY += 1;
                }
            }
            _pixels = pixels;
        }

        private PixelSet(bool[,] flippedPixels) {
            _pixels = flippedPixels;
        }



        public int Size => _pixels.GetLength(0);

        public PixelSet Flip() {
            var flippedPixels = new bool[Size, Size];
            for (int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
                    flippedPixels[Size - 1 - x, y] = _pixels[x, y];
                }
            }
            return new PixelSet(flippedPixels);
        }

        public PixelSet Rotate() {
            var rotatedPixels = new bool[Size, Size];
            for (int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
                    rotatedPixels[Size - 1 - y, x] = _pixels[x, y];
                }
            }
            return new PixelSet(rotatedPixels);
        }

        public PixelSet Expand(Dictionary<PixelSet, PixelSet> rules) {
            if (Size % 2 == 0) {
                var pixelSets = new List<PixelSet>();

                int squareY = 0;
                for (int squareX = 0; squareX < Size / 2 && squareY < Size / 2;) {

                    bool[,] square = new bool[2, 2];
                    for (int y = 0; y < 2; y++) {
                        for (int x = 0; x < 2; x++) {
                            square[x, y] = _pixels[x + 2 * squareX, y + 2 * squareY];
                        }
                    }

                    var expandedSquare = new PixelSet(square).ApplyRules(rules);
                    pixelSets.Add(expandedSquare);
                    squareX++;
                    if (squareX % (Size / 2) == 0) {
                        squareX = 0;
                        squareY += 1;
                    }
                }
                return new PixelSet(pixelSets);
            }
            else {
                var pixelSets = new List<PixelSet>();

                int squareY = 0;
                for (int squareX = 0; squareX < Size / 3 && squareY < Size / 3;) {

                    bool[,] square = new bool[3, 3];
                    for (int y = 0; y < 3; y++) {
                        for (int x = 0; x < 3; x++) {
                            square[x, y] = _pixels[x + 3 * squareX, y + 3 * squareY];
                        }
                    }

                    var expandedSquare = new PixelSet(square).ApplyRules(rules);
                    pixelSets.Add(expandedSquare);
                    squareX++;
                    if (squareX % (Size / 3) == 0) {
                        squareX = 0;
                        squareY += 1;
                    }
                }
                return new PixelSet(pixelSets);
            }
        }

        public int OnCount {
            get {
                int count = 0;
                for (int x = 0; x < Size; x++) {
                    for (int y = 0; y < Size; y++) {
                        if (_pixels[x, y]) {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        private PixelSet ApplyRules(Dictionary<PixelSet, PixelSet> rules) {
            if (rules.ContainsKey(this)) {
                return rules[this];
            }

            var rotated = Rotate();
            if (rules.ContainsKey(rotated)) {
                return rules[rotated];
            }
            rotated = rotated.Rotate();
            if (rules.ContainsKey(rotated)) {
                return rules[rotated];
            }
            rotated = rotated.Rotate();
            if (rules.ContainsKey(rotated)) {
                return rules[rotated];
            }

            var flipped = Flip();
            if (rules.ContainsKey(flipped)) {
                return rules[flipped];
            }

            var flippedRotated = Flip().Rotate();
            if (rules.ContainsKey(flippedRotated)) {
                return rules[flippedRotated];
            }
            flippedRotated = flippedRotated.Rotate();
            if (rules.ContainsKey(flippedRotated)) {
                return rules[flippedRotated];
            }
            flippedRotated = flippedRotated.Rotate();
            if (rules.ContainsKey(flippedRotated)) {
                return rules[flippedRotated];
            }
            return this;
        }

        protected bool Equals(PixelSet other) {
            return _pixels.Rank == other._pixels.Rank &&
                Enumerable.Range(0, _pixels.Rank).All(dimension => _pixels.GetLength(dimension) == other._pixels.GetLength(dimension)) &&
                _pixels.Cast<bool>().SequenceEqual(other._pixels.Cast<bool>());
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PixelSet) obj);
        }

        public override int GetHashCode() {
            int hash = 0;

            if (_pixels == null) return 0;

            for(int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
                    hash |= 1 << x + y;
                }
            }
            return hash;
        }

        public override string ToString() {
            var stringBuilder = new StringBuilder();
            for (int y = 0; y < _pixels.GetLength(1); y++) {
                for (int x = 0; x < _pixels.GetLength(0); x++) {
                    stringBuilder.Append(_pixels[x, y] ? "#" : ".");
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }
    }
}
