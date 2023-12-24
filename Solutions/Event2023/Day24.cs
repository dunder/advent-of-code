using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 24: Never Tell Me The Odds ---
    public class Day24
    {
        private readonly ITestOutputHelper output;

        public Day24(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Position(long X, long Y, long Z);
        private record Velocity(long X, long Y, long Z);

        private record Hailstone(Position Position, Velocity Velocity);

        private List<Hailstone> Parse(IList<string> input)
        {
            return input.Select(line =>
            {
                var parts = line.Split(" @ ");
                var ps = parts[0].Split(", ").Select(long.Parse).ToList();
                var vs = parts[1].Split(", ").Select(long.Parse).ToList();

                return new Hailstone(new Position(ps[0], ps[1], ps[2]), new Velocity(vs[0], vs[1], vs[2]));

            }).ToList();
        }
        
        private (double, double)? LineIntersection(Hailstone hailstone1, Hailstone hailstone2)
        {
            (long x1, long y1, long _) = hailstone1.Position;
            (long vx1, long vy1, long _) = hailstone1.Velocity;

            (long x2, long y2, long _) = hailstone2.Position;
            (long vx2, long vy2, long _) = hailstone2.Velocity;
            
            double determinant = (vx1 * vy2) - (vy1 * vx2);

            if (determinant == 0)
            {
                return null;
            }

            var n1 = ((x2 - x1) * vy2) - ((y2 - y1) * vx2);

            double x = (n1 / determinant) * vx1 + x1;
            double y = (n1 / determinant) * vy1 + y1;

            return (x, y);
        }

        private bool IntersectionWithinTestArea(long testFrom, long testTo, (double, double) intersection)
        {
            (double x, double y) = intersection;
            return x >= testFrom && x <= testTo && y >= testFrom && y <= testTo;
        }

        private bool IsInFuture(Hailstone hailstone, double x, double y)
        {
            var xfuture = hailstone.Velocity.X > 0 ? x > hailstone.Position.X : x < hailstone.Position.X;
            var yfuture = hailstone.Velocity.Y > 0 ? y > hailstone.Position.Y : y < hailstone.Position.Y;

            return xfuture && yfuture;
        }

        public int Run1(IList<string> input, long testFrom, long testTo)
        {
            var hailstones = Parse(input);

            int counter = 0;

            for (int i = 0; i < hailstones.Count-1; i++)
            {
                for (int j = i + 1; j < hailstones.Count; j++)
                {
                    var hailstone1 = hailstones[i];
                    var hailstone2 = hailstones[j];

                    var intersection = LineIntersection(hailstone1, hailstone2);

                    if (intersection == null)
                    {
                        // parallell
                        continue;
                    }

                    (double x, double y) = intersection.Value;

                    var future1 = IsInFuture(hailstone1, x, y);
                    var future2 = IsInFuture(hailstone2, x, y);

                    if (!(future1 && future2))
                    {
                        continue;
                    }

                    if (IntersectionWithinTestArea(testFrom, testTo, intersection.Value))
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Run1(input, 200000000000000, 400000000000000);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(13149, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "19, 13, 30 @ -2,  1, -2",
                "18, 19, 22 @ -1, -1, -2",
                "20, 25, 34 @ -2, -2, -4",
                "12, 31, 28 @ -1, -2, -1",
                "20, 19, 15 @  1, -5, -3"
            };

            Assert.Equal(2, Run1(example, 7, 27));
        }

        [Fact]
        public void SecondStarExample()
        {
            
        }
    }
}
