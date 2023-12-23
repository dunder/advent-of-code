using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 22: Sand Slabs ---
    public class Day22
    {
        private readonly ITestOutputHelper output;

        public Day22(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<Brick> Parse(IList<string> input)
        {
            return input.Select((line, i) =>
            {
                var parts = line.Split("~");
                var start = parts[0].Split(",").Select(int.Parse).ToArray();
                var end = parts[1].Split(",").Select(int.Parse).ToArray();
                var brickStart = new Coordinate(start[0], start[1], start[2]);
                var brickEnd = new Coordinate(end[0], end[1], end[2]);
                return new Brick(i, brickStart, brickEnd);
            }).ToList();
        }

        private record Range(int From, int To)
        {
            public int Length = To - From + 1;

            public bool IsWithin(int value)
            {
                return value >= From && value <= To;
            }

            public bool Intersects(Range other)
            {
                return From <= other.To && other.From <= To;
            }

            public List<int> Values => Enumerable.Range(From, Length).ToList();
        }

        private record Coordinate(int X, int Y, int Z);
        private record Brick(int Id, Coordinate Start, Coordinate End)
        {
            public Range X => new Range(Start.X, End.X);
            public Range Y => new Range(Start.Y, End.Y);
            public Range Z => new Range(Start.Z, End.Z);

            public List<Brick> Supports { get; set; } = new();
            public List<Brick> SupportedBy { get; set; } = new();

            public bool Intersects(Brick other)
            {
                return X.Intersects(other.X) && Y.Intersects(other.Y);
            }
        }

        private List<Brick> PutBricksToRest(IList<string> input)
        {
            var bricks = Parse(input).OrderBy(brick => brick.Z.From).ToList();

            for (int i = 0; i < bricks.Count; i++)
            {
                var brick = bricks[i];

                var z = brick.Z.From;

                while (z > 0)
                {
                    var bricksSupporting = bricks.Where(b => b.Z.To == z - 1 && brick.Intersects(b)).ToList();

                    if (bricksSupporting.Any() || z == 1)
                    {
                        foreach (var supportingBrick in bricksSupporting)
                        {
                            supportingBrick.Supports.Add(brick);
                            brick.SupportedBy.Add(supportingBrick);
                        }
                        bricks[i] = brick;
                        break;
                    }

                    z--;

                    brick = new Brick(
                        brick.Id,
                        new Coordinate(brick.X.From, brick.Y.From, z),
                        new Coordinate(brick.X.To, brick.Y.To, z + brick.Z.Length - 1));
                }

            }

            return bricks;
        }

        private int Run1(IList<string> input)
        {
            var bricks = PutBricksToRest(input);

            return bricks.Count(brick => brick.Supports.All(supporting => supporting.SupportedBy.Count > 1));
        }

        private int Run2(IList<string> input)
        {
            var bricks = PutBricksToRest(input);


            return 0;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Run1(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return Run2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(517, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            // 1491 That's not the right answer; your answer is too low.
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "1,0,1~1,2,1",
                "0,0,2~2,0,2",
                "0,2,3~2,2,3",
                "0,0,4~0,2,4",
                "2,0,5~2,2,5",
                "0,1,6~2,1,6",
                "1,1,8~1,1,9"
            };

            Assert.Equal(5, Run1(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "1,0,1~1,2,1",
                "0,0,2~2,0,2",
                "0,2,3~2,2,3",
                "0,0,4~0,2,4",
                "2,0,5~2,2,5",
                "0,1,6~2,1,6",
                "1,1,8~1,1,9"
            };

            Assert.Equal(7, Run2(example));

        }
    }
}
