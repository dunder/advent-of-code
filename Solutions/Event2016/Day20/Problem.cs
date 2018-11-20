using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2016.Day20
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day20;

        private const long MaxIp = 4_294_967_295;

        public override string FirstStar()
        {
            var blackListedIps = ReadLineInput();
            var blackList = new Blacklist(MaxIp, ParseBlacklist(blackListedIps));
            return blackList.LowestValid().ToString();
        }

        public override string SecondStar()
        {
            var blackListedIps = ReadLineInput();
            var blackList = new Blacklist(MaxIp, ParseBlacklist(blackListedIps));
            return blackList.ValidIps().ToString();
        }


        public class IpRange
        {
            public long From { get; set; }
            public long To { get; set; }

            // this:  |----|          |----|   |----|    |--|
            // other:     |----|   |----|       |--|    |----|
            public bool Overlaps(IpRange other)
            {
                // this:    |----|  
                // other: |----|     
                var otherToWithin = other.To >= From && other.To <= To;

                // this:  |----|    
                // other:   |----|  
                var otherFromWithin = other.From >= From && other.From <= To;

                // this:  |----|
                // other:  |--| 
                var otherCovered = other.From >= From && other.To <= To;
                
                // this:   |--|
                // other: |----|
                var thisCovered = From >= other.From && To <= other.To;

                return otherToWithin | otherFromWithin | otherCovered | thisCovered;
            }

            public IpRange Merge(IpRange other)
            {
                if (!Overlaps(other))
                {
                    throw new ArgumentOutOfRangeException(nameof(other), $"Cannot merge non overlapping ranges: {this} | {other}");
                }

                return new IpRange
                {
                    From = other.From < From ? other.From : From,
                    To = other.To > To ? other.To : To
                };
            }

            public override string ToString()
            {
                return $"{From}-{To}";
            }
        }

        public static List<IpRange> ParseBlacklist(IList<string> blacklist)
        {
            var blackListRanges = new List<IpRange>();
            foreach (var ipRange in blacklist)
            {
                var separated = ipRange.Split('-');
                blackListRanges.Add(new IpRange
                {
                    From = long.Parse(separated[0]),
                    To = long.Parse(separated[1])
                });
            }

            return blackListRanges;
        }

        public class Blacklist
        {
            public long MaxValue { get; }
            private readonly List<IpRange> _mergedOrderedIpRanges;

            public Blacklist(long maxValue, List<IpRange> ipRangeBlacklist)
            {
                MaxValue = maxValue;

                ipRangeBlacklist = ipRangeBlacklist.OrderBy(r => r.From).ToList();

                _mergedOrderedIpRanges = MergeOverlaps(ipRangeBlacklist);
            }

            private static List<IpRange> MergeOverlaps(List<IpRange> ipRangeBlacklist)
            {
                List<IpRange> mergedRanges = new List<IpRange>();
                IpRange currentMerged = ipRangeBlacklist.First();
                for (int i = 1; i < ipRangeBlacklist.Count; i++)
                {
                    IpRange current = ipRangeBlacklist[i];
                    if (current.Overlaps(currentMerged))
                    {
                        currentMerged = currentMerged.Merge(current);
                    }
                    else
                    {
                        mergedRanges.Add(currentMerged);
                        currentMerged = current;
                    }
                }

                if (currentMerged.To != mergedRanges.Last().To)
                {
                    mergedRanges.Add(currentMerged);
                }

                return mergedRanges;
            }

            public long LowestValid()
            {
                var firstRange = _mergedOrderedIpRanges.First();
                if (firstRange.From > 0)
                {
                    return 0;
                }
                return _mergedOrderedIpRanges.First().To + 1;
            }

            public long ValidIps()
            {
                long count = 0;

                var firstRange = _mergedOrderedIpRanges.First();

                if (firstRange.From > 0)
                {
                    count += firstRange.From;
                }

                IpRange previous = firstRange;
                for (int i = 1; i < _mergedOrderedIpRanges.Count; i++)
                {
                    var current = _mergedOrderedIpRanges[i];

                    count += current.From - previous.To - 1;

                    previous = current;
                }

                var lastRange = _mergedOrderedIpRanges.Last();
                if (lastRange.To < MaxValue)
                {
                    count += MaxValue - lastRange.To;
                }


                return count;
            }
        }
    }
}