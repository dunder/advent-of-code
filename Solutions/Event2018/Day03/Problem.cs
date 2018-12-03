using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solutions.Event2018.Day03
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day03;

        public override string FirstStar()
        {
            var claimDescriptions = ReadLineInput();
            var claims = Parse(claimDescriptions);
            var result = CountOverlaps(claims);
            return result.ToString();
        }

        public override string SecondStar()
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
}