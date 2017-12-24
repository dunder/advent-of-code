using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Utilities.Grid {
    public class Grid : IEnumerable<(Point point, bool value)> {

        private readonly bool[,] _squares;

        public Grid(string description) {
            var rows = description.Split('/');

            _squares = new bool[rows.Length,rows.Length];
            
            for (int y = 0; y < rows.Length; y++) {
                var row = rows[y];
                for (int x = 0; x < row.Length; x++) {
                    _squares[x, y] = row[x] == '#';
                }
            }
        }

        public Grid(bool[,] squares) {
            _squares = squares;
        }

        public int Size => _squares.GetLength(0);

        public int Width => _squares.GetLength(0);
        public int Height => _squares.GetLength(1);

        public bool[,] Squares => _squares;

        public Grid Flip() {
            var flippedPixels = new bool[Size, Size];
            for (int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
                    flippedPixels[Size - 1 - x, y] = _squares[x, y];
                }
            }
            return new Grid(flippedPixels);
        }

        public Grid Rotate() {
            var rotatedPixels = new bool[Size, Size];
            for (int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
                    rotatedPixels[Size - 1 - y, x] = _squares[x, y];
                }
            }
            return new Grid(rotatedPixels);
        }

        public Grid TurnOn(int x, int y) {
            return new Grid(Squares.Clone() as bool[,]);
        }

        public Grid TurnOn(Point point) {
            return TurnOn(point.X, point.Y);
        }

        public Grid TurnOff(int x, int y) {
            return new Grid(Squares.Clone() as bool[,]);
        }

        public Grid TurnOff(Point point) {
            return TurnOff(point.X, point.Y);
        }

        public int OnCount {
            get {
                int count = 0;
                for (int x = 0; x < Size; x++) {
                    for (int y = 0; y < Size; y++) {
                        if (_squares[x, y]) {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        public bool IsOn(int x, int y) {
            return Squares[x, y];
        }

        public bool IsOn(Point point) {
            return IsOn(point.X, point.Y);
        }

        protected bool Equals(Grid other) {
            return _squares.Rank == other._squares.Rank &&
                   Enumerable.Range(0, _squares.Rank).All(dimension => _squares.GetLength(dimension) == other._squares.GetLength(dimension)) &&
                   _squares.Cast<bool>().SequenceEqual(other._squares.Cast<bool>());
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Grid) obj);
        }

        public override int GetHashCode() {
            int hash = 0;

            if (_squares == null) return 0;

            for(int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
                    hash |= 1 << x + y;
                }
            }
            return hash;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<(Point point, bool value)> GetEnumerator() {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    yield return (new Point(x,y), Squares[x,y]);
                }
            }
        }

        public override string ToString() {
            var stringBuilder = new StringBuilder();
            for (int y = 0; y < _squares.GetLength(1); y++) {
                for (int x = 0; x < _squares.GetLength(0); x++) {
                    stringBuilder.Append(_squares[x, y] ? "#" : ".");
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }
    }
}