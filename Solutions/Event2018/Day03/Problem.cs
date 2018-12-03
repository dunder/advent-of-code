using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Facet.Combinatorics;

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

        public static int FindOnlyNonOverlapping(List<Claim> claims)
        {
            for (int i = 0; i < claims.Count; i++)
            {
                var c1 = claims[i];
                if (claims.Where(c => c.Id != c1.Id).All(c => c1.Overlaps(c).Count == 0))
                {
                    return c1.Id;
                }

            }

            throw new Exception("Could not find any claim that does not overlap any other claim");
        }

        public static int CountOverlaps(IList<Claim> claims)
        {
            var combinations = new Combinations<Claim>(claims, 2);

            var all = new HashSet<Point>();

            foreach (var combination in combinations)
            {
                var c1 = combination.First();
                var c2 = combination.Last();

                foreach (var p in c1.Overlaps(c2))
                {
                    all.Add(p);

                }
            }
            return all.Count;
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
                var claim = new Claim(id, new Point(left, top), width, height);
                claims.Add(claim);
            }

            return claims;
        }
    }


    public class Claim
    {
        public int Id { get; set; }
        public Point UpperLeft { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public HashSet<Point> Points { get; set; }

        public Claim(int id, Point upperLeft, int width, int height)
        {
            Id = id;
            UpperLeft = upperLeft;
            Width = width;
            Height = height;

            Points = new HashSet<Point>();

            for (int x = UpperLeft.X; x < UpperLeft.X + Width; x++)
            {
                for (int y = UpperLeft.Y; y < UpperLeft.Y + Height; y++)
                {
                    Points.Add(new Point(x, y));
                }
            }
        }

        public HashSet<Point> Overlaps(Claim claim)
        {
            var mine = new HashSet<Point>(Points);
            var others = new HashSet<Point>(claim.Points);

            mine.IntersectWith(others);

            return mine;
        }

        public override string ToString()
        {
            return $"#{Id} @ {UpperLeft.X},{UpperLeft.Y}: {Width}x{Height}";
        }

        protected bool Equals(Claim other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Claim) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}