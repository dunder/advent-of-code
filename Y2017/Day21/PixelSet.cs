using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Grid;

namespace Y2017.Day21 {
    public class PixelSet {
        private Grid Grid { get; }

        public PixelSet(Grid grid) {
            Grid = grid;
        }

        public PixelSet(List<PixelSet> pixelSets) {
            if (pixelSets.Any(p => p == null)) {
                throw new ArgumentNullException(nameof(pixelSets), "All Grid isntances in the list must be non null");
            }
            if (!pixelSets.Any()) {
                throw new ArgumentOutOfRangeException(nameof(pixelSets), "There must be at least one Grid instance in the list");
            }
            if (pixelSets.Any(p => p.Grid.Size != pixelSets.First().Grid.Size)) {
                throw new ArgumentOutOfRangeException($"All Grid instances in the list must have the same size");
            }

            double squaredCount = Math.Sqrt(pixelSets.Count);
            int squaredIntCount = (int)Math.Sqrt(pixelSets.Count);
            // ReSharper disable once CompareOfFloatsByEqualityOperator the whole point is to 
            if (squaredIntCount != squaredCount) {
                throw new ArgumentOutOfRangeException(nameof(pixelSets), $"The sqrt of the number of elements in the list must be an int, was: {pixelSets.Count}");
            }

            int oldSize = pixelSets.First().Grid.Size;
            int pixelSetsPerNewRow = (int)Math.Sqrt(pixelSets.Count);
            int newSize = oldSize * pixelSetsPerNewRow;

            bool[,] pixels = new bool[newSize, newSize];
            int pixelSetPositionX = 0;
            int pixelSetPositionY = 0;
            foreach (var pixelSet in pixelSets) {
                for (int py = 0; py < oldSize; py++) {
                    for (int px = 0; px < oldSize; px++) {
                        pixels[pixelSetPositionX * oldSize + px, pixelSetPositionY * oldSize + py] = pixelSet.Grid.Squares[px, py];
                    }
                }
                pixelSetPositionX += 1;
                if (pixelSetPositionX % pixelSetsPerNewRow == 0) {
                    pixelSetPositionX = 0;
                    pixelSetPositionY += 1;
                }
            }
            Grid = new Grid(pixels);
        }

        public PixelSet Flip() {
            return new PixelSet(Grid.Flip());
        }
        public PixelSet Rotate() {
            return new PixelSet(Grid.Rotate());
        }

        public int OnCount => Grid.OnCount;

        public PixelSet Expand(Dictionary<PixelSet, PixelSet> rules) {
            if (Grid.Size % 2 == 0) {
                var pixelSets = new List<PixelSet>();

                int squareY = 0;
                for (int squareX = 0; squareX < Grid.Size / 2 && squareY < Grid.Size / 2;) {

                    bool[,] square = new bool[2, 2];
                    for (int y = 0; y < 2; y++) {
                        for (int x = 0; x < 2; x++) {
                            square[x, y] = Grid.Squares[x + 2 * squareX, y + 2 * squareY];
                        }
                    }

                    var expandedSquare = new PixelSet(new Grid(square)).ApplyRules(rules);
                    pixelSets.Add(expandedSquare);
                    squareX++;
                    if (squareX % (Grid.Size / 2) == 0) {
                        squareX = 0;
                        squareY += 1;
                    }
                }
                return new PixelSet(pixelSets);
            } else {
                var pixelSets = new List<PixelSet>();

                int squareY = 0;
                for (int squareX = 0; squareX < Grid.Size / 3 && squareY < Grid.Size / 3;) {

                    bool[,] square = new bool[3, 3];
                    for (int y = 0; y < 3; y++) {
                        for (int x = 0; x < 3; x++) {
                            square[x, y] = Grid.Squares[x + 3 * squareX, y + 3 * squareY];
                        }
                    }

                    var expandedSquare = new PixelSet(new Grid(square)).ApplyRules(rules);
                    pixelSets.Add(expandedSquare);
                    squareX++;
                    if (squareX % (Grid.Size / 3) == 0) {
                        squareX = 0;
                        squareY += 1;
                    }
                }
                return new PixelSet(pixelSets);
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

        public override string ToString() {
            return Grid.ToString();
        }

        protected bool Equals(PixelSet other) {
            return Equals(Grid, other.Grid);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PixelSet) obj);
        }

        public override int GetHashCode() {
            return (Grid != null ? Grid.GetHashCode() : 0);
        }
    }
}
