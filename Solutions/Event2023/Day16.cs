using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 16: The Floor Will Be Lava ---
    public class Day16
    {
        private readonly ITestOutputHelper output;

        public Day16(ITestOutputHelper output)
        {
            this.output = output;
        }

        private TileMap Parse(IList<string> input)
        {
            Dictionary<(int, int), char> tiles = new();

            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input.Count; x++)
                {
                    var tile = input[y][x];
                    var position = (x, y);
                    if (@"|-/\".Contains(tile))
                    {
                        tiles.Add(position, tile);
                    }
                }
            }

            return new TileMap(tiles, input.Count, input[0].Length);
        }

        private record TileMap(Dictionary<(int X, int Y), char> Tiles, int Height, int Width)
        {
            public bool IsWithin((int, int) position)
            {
                (int X, int Y) = position;
                return X >= 0 && X < Width && Y >= 0 && Y < Height;
            }
        }

        private enum Direction { Up, Right, Down, Left }
        private record Beam(Direction Direction, (int X, int Y) Position);

        private void Release(HashSet<Beam> visited, TileMap tileMap, List<Beam> incomingBeams)
        {
            if (!incomingBeams.Any()) { return; }

            Print(visited, tileMap, incomingBeams);

            var outgoingBeams = new List<Beam>();

            foreach (var beam in incomingBeams)
            {
                if (tileMap.Tiles.TryGetValue(beam.Position, out char splitter))
                {
                    switch (splitter)
                    {
                        case '|':
                            switch (beam.Direction)
                            {
                                case Direction.Up:
                                case Direction.Down:
                                    outgoingBeams.Add(beam);
                                    break;
                                case Direction.Right:
                                case Direction.Left:
                                    outgoingBeams.Add(new Beam(Direction.Up, beam.Position));
                                    outgoingBeams.Add(new Beam(Direction.Down, beam.Position));
                                    break;
                            }
                            break;
                        case '-':
                            switch (beam.Direction)
                            {
                                case Direction.Up:
                                case Direction.Down:
                                    outgoingBeams.Add(new Beam(Direction.Right, beam.Position));
                                    outgoingBeams.Add(new Beam(Direction.Left, beam.Position));
                                    break;
                                case Direction.Right:
                                case Direction.Left:
                                    outgoingBeams.Add(beam);
                                    break;
                            }
                            break;
                        case '/':
                            switch (beam.Direction)
                            {
                                case Direction.Up:
                                    outgoingBeams.Add(new Beam(Direction.Right, beam.Position));
                                    break;
                                case Direction.Down:
                                    outgoingBeams.Add(new Beam(Direction.Left, beam.Position));
                                    break;
                                case Direction.Right:
                                    outgoingBeams.Add(new Beam(Direction.Up, beam.Position));
                                    break;
                                case Direction.Left:
                                    outgoingBeams.Add(new Beam(Direction.Down, beam.Position));
                                    break;
                            }
                            break;
                        case '\\':
                            switch (beam.Direction)
                            {
                                case Direction.Up:
                                    outgoingBeams.Add(new Beam(Direction.Left, beam.Position));
                                    break;
                                case Direction.Down:
                                    outgoingBeams.Add(new Beam(Direction.Right, beam.Position));
                                    break;
                                case Direction.Right:
                                    outgoingBeams.Add(new Beam(Direction.Down, beam.Position));
                                    break;
                                case Direction.Left:
                                    outgoingBeams.Add(new Beam(Direction.Up, beam.Position));
                                    break;
                            }
                            break;
                        default:
                            throw new Exception($"Unexpected splitter: {splitter}");
                    }
                }
                else
                {
                    outgoingBeams.Add(beam);
                }
            }

            outgoingBeams = outgoingBeams
                .Select(beam =>
                {
                    var next = beam.Direction switch
                    {
                        Direction.Up => (beam.Position.X, beam.Position.Y - 1),
                        Direction.Right => (beam.Position.X + 1, beam.Position.Y),
                        Direction.Down => (beam.Position.X, beam.Position.Y + 1),
                        Direction.Left => (beam.Position.X - 1, beam.Position.Y),
                    };

                    return beam with { Position = next };
                })
                .Where(beam => tileMap.IsWithin(beam.Position) && visited.Add(beam))
                .ToList();

            Release(visited, tileMap, outgoingBeams);
        }

        private void Print(HashSet<Beam> visited, TileMap tileMap, List<Beam> incomingBeams)
        {
            output.WriteLine("");
            
            var visitedPositions = visited.GroupBy(beam => beam.Position).ToDictionary(g => g.Key);
            var incomingPositions = incomingBeams.Select(beam => beam.Position).ToHashSet();

            for (int y = 0; y < tileMap.Height; y++)
            {
                var line = new StringBuilder();
                for (int x = 0; x < tileMap.Width; x++)
                {
                    var position = (x, y);

                    if (incomingPositions.Contains(position))
                    {
                        line.Append("¤");
                    }
                    else if (tileMap.Tiles.ContainsKey(position))
                    {
                        line.Append(tileMap.Tiles[position]);
                    }
                    else if (visitedPositions.TryGetValue(position, out var group))
                    {
                        if (group.Count() == 1)
                        {
                            switch (group.Single().Direction)
                            {
                                case Direction.Up:
                                    line.Append("^");
                                    break;
                                case Direction.Right:
                                    line.Append(">");
                                    break;
                                case Direction.Down:
                                    line.Append("v");
                                    break;
                                case Direction.Left:
                                    line.Append("<");
                                    break;
                            }
                        }
                        else
                        {
                            line.Append(group.Count());
                        }
                    }
                    else
                    {
                        line.Append(".");
                    }
                }
                output.WriteLine(line.ToString());
            }
            output.WriteLine("");
        }

        private int EnergizedTilesFrom(TileMap tileMap, Beam beam)
        {
            var beams = beam.Yield().ToList();

            HashSet<Beam> visited = new()
            {
                beams.Single()
            };

            Release(visited, tileMap, beams);

            return visited.Select(v => v.Position).ToHashSet().Count;
        }

        private int EnergizedTiles(IList<string> input)
        {
            var tileMap = Parse(input);

            return EnergizedTilesFrom(tileMap, new Beam(Direction.Right, (0, 0)));
        }

        private int MaximumEnergizedTiles(IList<string> input)
        {
            var tileMap = Parse(input);

            var startingPoints = new List<Beam>();
            
            for (int x = 0; x < tileMap.Width; x++)
            {
                startingPoints.Add(new Beam(Direction.Down, (x, 0)));
                startingPoints.Add(new Beam(Direction.Up, (x, tileMap.Height-1)));
            }
            
            for (int y = 0; y < tileMap.Width; y++)
            {
                startingPoints.Add(new Beam(Direction.Right, (0, y)));
                startingPoints.Add(new Beam(Direction.Left, (tileMap.Width-1, y)));
            }

            return startingPoints.Select(beam => EnergizedTilesFrom(tileMap, beam)).Max();
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return EnergizedTiles(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return MaximumEnergizedTiles(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(7884, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(8185, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                @".|...\....",
                @"|.-.\.....",
                @".....|-...",
                @"........|.",
                @"..........",
                @".........\",
                @"..../.\\..",
                @".-.-/..|..",
                @".|....-|.\",
                @"..//.|....",
            };

            Assert.Equal(46, EnergizedTiles(example));
        }

        [Fact]
        public void SecondStarExample()
        {

            var example = new List<string>
            {
                @".|...\....",
                @"|.-.\.....",
                @".....|-...",
                @"........|.",
                @"..........",
                @".........\",
                @"..../.\\..",
                @".-.-/..|..",
                @".|....-|.\",
                @"..//.|....",
            };

            Assert.Equal(51, MaximumEnergizedTiles(example));
        }
    }
}
