using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2016.Day20
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day20;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = "Not implemented";
            return result.ToString();
        }


        public class IpRange
        {
            public int From { get; set; }
            public int To { get; set; }

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

        public static List<IpRange> ParseBlacklist(string[] blacklist)
        {
            var blackListRanges = new List<IpRange>();
            foreach (var ipRange in blacklist)
            {
                var separated = ipRange.Split('-');
                blackListRanges.Add(new IpRange
                {
                    From = int.Parse(separated[0]),
                    To = int.Parse(separated[1])
                });
            }

            return blackListRanges;
        }

        public class Blacklist
        {
            public int MaxValue { get; }

            public Blacklist(int maxValue, List<IpRange> ipRangeBlacklist)
            {
                MaxValue = maxValue;

                ipRangeBlacklist.Sort();

                MergeOverlaps(ipRangeBlacklist);
            }

            private static void MergeOverlaps(List<IpRange> ipRangeBlacklist)
            {
                List<IpRange> mergedRanges = new List<IpRange>();
                IpRange currentMerged = ipRangeBlacklist.First();
                for (int i = 1; i < ipRangeBlacklist.Count; i++)
                {
                    IpRange current = ipRangeBlacklist[i];
                    if (currentMerged.Overlaps(current))
                    {
                        currentMerged = currentMerged.Merge(current);
                        current = currentMerged;
                    }
                    else
                    {
                        mergedRanges.Add(current);
                    }
                }
            }

            public int LowestValid()
            {
                return 0;
            }
        }
    }
}