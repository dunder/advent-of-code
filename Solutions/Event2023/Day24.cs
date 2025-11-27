using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public int FirstStar(IList<string> input, long testStart, long testEnd)
        {

            var hailstones = Parse(input);

            int counter = 0;

            for (int i = 0; i < hailstones.Count - 1; i++)
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

                    if (IntersectionWithinTestArea(testStart, testEnd, intersection.Value))
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }


        private long Answer(Position position)
        {
            return position.X + position.Y + position.Z;
        }

        public BigInteger SecondStar(IList<string> input)
        {
            // Did not really solve this by myself. Found a nice solution by reddit user mynt
            // https://www.reddit.com/r/adventofcode/comments/18pnycy/2023_day_24_solutions/

            // If standing on a hail we will see the rock travel in a straight line, pass through us and two
            // other points on two other pieces of hail that zip by. There must be two vectors from our hail
            // to the other two collisions (v1 and v2) such that v1 = m * v2 where m is some unknown scalar
            // multiplier. We can make v1 = v2 by dividing one of the x, y or z components by itself to ensure
            // it is equal to 1. Then solve.

            List<Hailstone> hailstones = Parse(input);

            // start by finding three hailstones with equal x, y or z velocities, group them by their
            // velocities

            var gvx = hailstones.GroupBy(hailstone => hailstone.Velocity.X).ToList();
            var gvz = hailstones.GroupBy(hailstone => hailstone.Velocity.Z).ToList();
            var gvy = hailstones.GroupBy(hailstone => hailstone.Velocity.Y).ToList();

            // then find the first group with at least 3 hail stones

            IGrouping<long, Hailstone> matchingX = gvx.First(g => g.Count() > 2);
            IGrouping<long, Hailstone> matchingY = gvy.First(g => g.Count() > 2);
            IGrouping<long, Hailstone> matchingZ = gvy.First(g => g.Count() > 2);

            // the first match on vy gives us three hail stones, lets take them

            List<Hailstone> selectedHailStones = matchingY.ToList();

            Hailstone h0 = selectedHailStones[0];
            Hailstone h1 = selectedHailStones[1];
            Hailstone h2 = selectedHailStones[2];

            BigInteger vx0 = h0.Velocity.X;
            BigInteger vy0 = h0.Velocity.Y;
            BigInteger vz0 = h0.Velocity.Z;

            BigInteger vx1 = h1.Velocity.X;
            BigInteger vy1 = h1.Velocity.Y;
            BigInteger vz1 = h1.Velocity.Z;

            BigInteger vx2 = h2.Velocity.X;
            BigInteger vy2 = h2.Velocity.Y;
            BigInteger vz2 = h2.Velocity.Z;

            BigInteger x0 = h0.Position.X;
            BigInteger y0 = h0.Position.Y;
            BigInteger z0 = h0.Position.Z;

            BigInteger x1 = h1.Position.X;
            BigInteger y1 = h1.Position.Y;
            BigInteger z1 = h1.Position.Z;

            BigInteger x2 = h2.Position.X;
            BigInteger y2 = h2.Position.Y;
            BigInteger z2 = h2.Position.Z;

            // calculate relative velocities of hail 1 and 2 to hail 0 the y component is zero due to selection of hail

            BigInteger vxr1 = vx1 - vx0;
            BigInteger vzr1 = vz1 - vz0;
            BigInteger vxr2 = vx2 - vx0;
            BigInteger vzr2 = vz2 - vz0;

            // relative initial position of hail 1
            BigInteger xr1 = x1 - x0;
            BigInteger yr1 = y1 - y0;
            BigInteger zr1 = z1 - z0;

            // relative initial position of hail 2
            BigInteger xr2 = x2 - x0;
            BigInteger yr2 = y2 - y0;
            BigInteger zr2 = z2 - z0;

            // first set of hail equations
            // x = xr1 + vxr1*t1
            // y = yr1
            // z = zr1 + vzr1*t1

            // second set of hail equations
            // x = xr2 + vxr2*t2
            // y = yr2
            // z = zr2 + vzr2*t2

            // divide all equations by the y component to make y = 1 and ensure both vectors are the same

            // first set results
            // x = (xr1+vxr1*t1)/yr1
            // y = 1
            // z = (zr1+vzr1*t1)/yr1

            // second set results
            // x = (xr2+vxr2*t2)/yr2
            // y = 1
            // z = (zr2+vzr2*t2)/yr2

            // solve set of two linear equations x = x and z = z
            BigInteger num = (yr2 * xr1 * vzr1) - (vxr1 * yr2 * zr1) + (yr1 * zr2 * vxr1) - (yr1 * xr2 * vzr1);
            BigInteger den = yr1 * ((vzr1 * vxr2) - (vxr1 * vzr2));
            BigInteger t2 = num / den;

            // substitute t2 into a t1 equation
            num = (yr1 * xr2) + (yr1 * vxr2 * t2) - (yr2 * xr1);
            den = yr2 * vxr1;

            BigInteger t1 = num / den;

            // calculate collision position at t1 and t2 of hail 1 and 2 in normal frame of reference
            BigInteger cx1 = x1 + (t1 * vx1);
            BigInteger cy1 = y1 + (t1 * vy1);
            BigInteger cz1 = z1 + (t1 * vz1);

            BigInteger cx2 = x2 + (t2 * vx2);
            BigInteger cy2 = y2 + (t2 * vy2);
            BigInteger cz2 = z2 + (t2 * vz2);

            // calculate the vector the rock travelled between those two collisions
            BigInteger xm = (cx2 - cx1) / (t2 - t1);
            BigInteger ym = (cy2 - cy1) / (t2 - t1);
            BigInteger zm = (cz2 - cz1) / (t2 - t1);

            // calculate the initial position of the rock based on its vector
            BigInteger xc = cx1 - (xm * t1);
            BigInteger yc = cy1 - (ym * t1);
            BigInteger zc = cz1 - (zm * t1);

            return xc + yc + zc;
        }

        [Fact]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(13149, FirstStar(input, 200000000000000, 400000000000000));
        }

        [Fact]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1033770143421619, SecondStar(input));
        }

        [Fact]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(2, FirstStar(exampleInput, 7, 27));
        }

        [Fact]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(47, SecondStar(exampleInput));

        }
    }
}
