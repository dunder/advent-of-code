using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 3: Gear Ratios ---
    public class Day03
    {
        private record Position(int X, int Y);
        private record PartNumber(int Id, HashSet<Position> Positions) { }
        private record Symbol(char symbol, Position Position);

        private class EngineSchematic
        {
            private List<List<PartNumber>> partsByRow;

            public EngineSchematic(int Rows, int Columns, List<PartNumber> Parts, List<Symbol> Symbols)
            {
                this.Rows = Rows;
                this.Columns = Columns;
                this.Parts = Parts;
                this.Symbols = Symbols;


                var partsByRow = Parts
                    .GroupBy(grouping => grouping.Positions.First().Y)
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.Select(d => d).ToList());

                this.partsByRow = Enumerable.Range(0, Rows).Select(x => new List<PartNumber>()).ToList();

                foreach (var part in partsByRow)
                {
                    this.partsByRow[part.Key] = part.Value;
                }
            }

            public int Rows { get; }
            public int Columns { get; }
            public List<PartNumber> Parts { get; }
            public List<Symbol> Symbols { get; }

            public int SumPartNumbers()
            {
                HashSet<Position> symbolPositions = Symbols.Select(symbol => symbol.Position).ToHashSet();

                bool IsAdjacentToSymbol(PartNumber part)
                {
                    return PositionsAdjacentTo(part).Any(symbolPositions.Contains);
                }

                return Parts
                    .Where(IsAdjacentToSymbol)
                    .Select(x => x.Id)
                    .Sum();
            }

            public int GearRatio()
            {
                return Gears
                    .Select(PartsAdjacentTo)
                    .Where(adjacent => adjacent.Count() == 2)
                    .Select(parts => parts.Aggregate(1, (ratio, part) => ratio * part.Id))
                    .Sum();
            }

            private List<PartNumber> PartsAdjacentTo(Symbol symbol)
            {
                return Parts
                    .Where(part => part.Positions.Intersect(PositionsAdjacentTo(symbol.Position)).Any())
                    .ToList();
            }

            private HashSet<Position> PositionsAdjacentTo(PartNumber part)
            {
                return part.Positions.Select(PositionsAdjacentTo).SelectMany(x => x).ToHashSet();
            }

            private HashSet<Position> PositionsAdjacentTo(Position position)
            {
                var adjacent = new List<Position>
                {
                    new Position(position.X, position.Y - 1),
                    new Position(position.X + 1, position.Y - 1),
                    new Position(position.X + 1, position.Y),
                    new Position(position.X + 1, position.Y + 1),
                    new Position(position.X, position.Y + 1),
                    new Position(position.X - 1, position.Y + 1),
                    new Position(position.X - 1, position.Y),
                    new Position(position.X - 1, position.Y - 1)
                };

                return new HashSet<Position>(
                    adjacent.Where(
                        position => position.X >= 0 &&
                        position.X < Columns &&
                        position.Y >= 0 && position.X < Rows
                    ));
            }

            private IEnumerable<Symbol> Gears => Symbols.Where(symbol => symbol.symbol == '*');

        }

        private EngineSchematic Parse(IList<string> input)
        {
            int rows = input.Count;
            int columns = input.First().Length;

            var parts = new List<PartNumber>();
            var symbols = new List<Symbol>();

            for (int y = 0; y < rows; y++)
            {
                var row = input[y];

                for (int x = 0; x < columns; x++)
                {
                    char column = row[x];

                    if (column == '.') continue;

                    PartNumber ParsePartNumber() {
                        var number = "";
                        var positions = new HashSet<Position>();
                        do {
                            positions.Add(new Position(x, y));
                            var digit = row[x];
                            number += digit;
                            x++;
                        } while (x < columns && char.IsDigit(row[x]));
                        x--;
                        return new PartNumber(int.Parse(number), positions);
                    }

                    if (char.IsDigit(column))
                    {
                        parts.Add(ParsePartNumber());
                    }
                    else
                    {
                        symbols.Add(new Symbol(column, new Position(x, y)));
                    }
                }
            }

            return new EngineSchematic(rows, columns, parts, symbols);
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return Parse(input).SumPartNumbers();
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return Parse(input).GearRatio();
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(532331, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(82301120, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598..",
            };

            Assert.Equal(4361, Parse(example).SumPartNumbers());
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598..",
            };

            Assert.Equal(467835, Parse(example).GearRatio());
        }
    }
}
