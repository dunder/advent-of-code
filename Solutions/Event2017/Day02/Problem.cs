using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Facet.Combinatorics;

namespace Solutions.Event2017.Day02
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day02;

        public override string FirstStar()
        {
            var input = RemoveWhitespace(ReadLineInput());
            var result = new SpreadSheet(input).Checksum;
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = RemoveWhitespace(ReadLineInput());
            var result = new SpreadSheet(input).SumEvenDivisable;
            return result.ToString();
        }

        private IList<string> RemoveWhitespace(IList<string> input)
        {
            return input.Select(x => Regex.Replace(x, @"\s+", " ")).ToList();
        }
    }

    public class SpreadSheet
    {
        private IList<string> Rows { get; }

        public SpreadSheet(IList<string> rows)
        {
            Rows = rows;
        }

        public long Checksum
        {
            get
            {
                long checksum = 0;

                foreach (var row in Rows)
                {
                    var iRow = row.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(long.Parse)
                        .ToList();
                    checksum += iRow.Max() - iRow.Min();
                }

                return checksum;
            }
        }

        public long SumEvenDivisable
        {
            get
            {
                long sum = 0;
                foreach (var row in Rows)
                {
                    List<long> iRow = row.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)
                        .ToList();
                    var combinations = new Combinations<long>(iRow, 2);
                    foreach (var combination in combinations)
                    {
                        var first = combination[0];
                        var second = combination[1];

                        if (first > second && first % second == 0)
                        {
                            sum += first / second;
                        }
                        else if (second % first == 0)
                        {
                            sum += second / first;
                        }
                    }
                }

                return sum;
            }
        }
    }
}