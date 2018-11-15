using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2016.Day15
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day15;

        public override string FirstStar()
        {
            var discs = new List<Disc>
            {
                new Disc(1, 11, 13),
                new Disc(2, 0, 5),
                new Disc(3, 11, 17),
                new Disc(4, 0, 3),
                new Disc(5, 2, 7),
                new Disc(6, 17, 19),

            };

            int tRelease = ReleaseAtToGetCapsule(discs);

            return tRelease.ToString();
        }

        public override string SecondStar()
        {
            var discs = new List<Disc>
            {
                new Disc(1, 11, 13),
                new Disc(2, 0, 5),
                new Disc(3, 11, 17),
                new Disc(4, 0, 3),
                new Disc(5, 2, 7),
                new Disc(6, 17, 19),
                new Disc(7, 0, 11)
            };

            int tRelease = ReleaseAtToGetCapsule(discs);

            return tRelease.ToString();
        }

        public class Disc
        {
            public int Offset { get; set; }
            public int Position { get; set; }
            public int Size { get; set; }

            public Disc(int offset, int position, int size)
            {
                Offset = offset;
                Position = position;
                Size = size;
            }

            public int PositionAtPassage(int t)
            {
                return (Position + t + Offset) % Size;
            }

            public override string ToString()
            {
                return $"Position {Position}({Size})";
            }
        }

        public static int ReleaseAtToGetCapsule(List<Disc> discs)
        {
            int t;
            for (t = 0; discs.Any(d => d.PositionAtPassage(t) != 0); t++) {}
            return t;
        }
    }
}