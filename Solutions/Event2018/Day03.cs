using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day03 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day03;
        public string Name => "No Matter How You Slice It";

        public string FirstStar()
        {
            var claimDescriptions = ReadLineInput();
            var claims = Parse(claimDescriptions);
            var result = CountOverlaps(claims);
            return result.ToString();
        }

        public string SecondStar()
        {
            var claimDescriptions = ReadLineInput();
            var claims = Parse(claimDescriptions);
            var result = FindOnlyNonOverlapping(claims);
            return result.ToString();
        }

        public static int CountOverlaps(IList<Claim> claims)
        {
            var counts = new Dictionary<Point, int>();
            foreach (var claim in claims)
            {
                var points = claim.Points;
                foreach (var point in points)
                {
                    if (counts.ContainsKey(point))
                    {
                        counts[point]++;
                    }
                    else
                    {
                        counts[point] = 1;
                    }
                }
            }
            return counts.Values.Count(i => i > 1);
        }

        public static int FindOnlyNonOverlapping(List<Claim> claims)
        {
            for (int i = 0; i < claims.Count; i++)
            {
                var c1 = claims[i];
                if (claims.Where(c => c.Id != c1.Id).All(c => !c1.Overlaps(c)))
                {
                    return c1.Id;
                }

            }

            throw new Exception("Could not find any claim that does not overlap any other claim");
        }

        public static List<Claim> Parse(IList<string> claimDescriptions)
        {
            var claims = new List<Claim>();

            var claimExpression = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");

            foreach (var claimDescription in claimDescriptions)
            {
                var match = claimExpression.Match(claimDescription);

                int id = int.Parse(match.Groups[1].Value);
                int left = int.Parse(match.Groups[2].Value);
                int top = int.Parse(match.Groups[3].Value);
                int width = int.Parse(match.Groups[4].Value);
                int height = int.Parse(match.Groups[5].Value);
                var claim = new Claim(id, left, top, width, height);
                claims.Add(claim);
            }

            return claims;
        }

        public class Claim
        {
            public int Id { get; set; }
            public Rectangle Rectangle { get; }
            public HashSet<Point> Points { get; }

            public Claim(int id, int x, int y, int width, int height)
            {
                Id = id;
                Rectangle = new Rectangle(x, y, width, height);
                Points = new HashSet<Point>();
                for (int ix = x; ix < x + width; ix++)
                {
                    for (int iy = y; iy < y + height; iy++)
                    {
                        Points.Add(new Point(ix, iy));
                    }
                }
            }

            public bool Overlaps(Claim claim)
            {
                return Rectangle.IntersectsWith(claim.Rectangle);
            }

            public override string ToString()
            {
                return $"#{Id} @ {Rectangle.X},{Rectangle.Y}: {Rectangle.Width}x{Rectangle.Height}";
            }
        }

        [Fact]
        public void ParseTest()
        {
            var descriptions = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            };

            var claims = Parse(descriptions);

            Assert.Equal(descriptions[0], claims[0].ToString());
            Assert.Equal(descriptions[1], claims[1].ToString());
            Assert.Equal(descriptions[2], claims[2].ToString());
        }

        [Fact]
        public void Count()
        {
            var descriptions = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            };

            var claims = Parse(descriptions);

            var count = CountOverlaps(claims);

            Assert.Equal(4, count);
        }

        [Fact]
        public void FindNonOverlapping()
        {
            var descriptions = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2"
            };

            var claims = Parse(descriptions);

            var id = FindOnlyNonOverlapping(claims);

            Assert.Equal(3, id);
        }

        [Fact]
        public void FirstStarTests()
        {
            var actual = FirstStar();
            Assert.Equal("104241", actual);
        }

        [Fact]
        public void SecondStarTests()
        {
            var actual = SecondStar();
            Assert.Equal("806", actual);
        }

    }
}
