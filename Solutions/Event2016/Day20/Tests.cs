using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Solutions.Event2016.Day20
{
    public class Tests
    {
        [Fact]
        public void BlacklistParser_CreatesIpRanges()
        {
            var blacklist = new[]
            {
                "5-8",
                "0-2",
                "4-7",
            };

            List<Problem.IpRange> blacklisted = Problem.ParseBlacklist(blacklist);


            Assert.Equal(blacklist.Length, blacklisted.Count);

            var sortedByFrom = blacklisted.OrderBy(b => b.From).ToList();

            Assert.Equal(0, sortedByFrom[0].From);
            Assert.Equal(2, sortedByFrom[0].To);
            Assert.Equal(4, sortedByFrom[1].From);
            Assert.Equal(7, sortedByFrom[1].To);
            Assert.Equal(5, sortedByFrom[2].From);
            Assert.Equal(8, sortedByFrom[2].To);
        }

        [Fact]
        public void Blacklist()
        {
            var ipRangeBlacklist = new List<Problem.IpRange>
            {
                new Problem.IpRange(),
            };

            Problem.Blacklist blacklist = new Problem.Blacklist(9, ipRangeBlacklist);

            Assert.Equal(3, blacklist.LowestValid());
        }

        [Theory]
        [InlineData(0, 4, 0, 4, true)]
        [InlineData(0, 4, 2, 5, true)]
        [InlineData(0, 4, 1, 2, true)]
        [InlineData(1, 2, 0, 4, true)]
        [InlineData(0, 4, 5, 7, false)]
        [InlineData(5, 7, 0, 4, false)]
        public void IpRangeOverlaps(int r1From, int r1To, int r2From, int r2To, bool expectedOverlap)
        {
            var r1 = new Problem.IpRange { From = r1From, To = r1To };
            var r2 = new Problem.IpRange { From = r2From, To = r2To };

            var isOverlap = r1.Overlaps(r2);

            Assert.Equal(expectedOverlap, isOverlap);
        }

        [Theory]
        [InlineData(0,4,0,4,0,4)]
        [InlineData(0,4,2,5,0,5)]
        [InlineData(0,4,1,2,0,4)]
        [InlineData(1,2,0,4,0,4)]
        public void IpRangeMerge(int r1From, int r1To, int r2From, int r2To, int expectedFrom, int expectedTo)
        {
            var r1 = new Problem.IpRange {From = r1From, To = r1To};
            var r2 = new Problem.IpRange {From = r2From, To = r2To};

            var merged = r1.Merge(r2);

            Assert.Equal(expectedFrom, merged.From);
            Assert.Equal(expectedTo, merged.To);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("17348574", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("104", actual);
        }
    }
}
