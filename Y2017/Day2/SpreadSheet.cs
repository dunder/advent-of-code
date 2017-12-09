using System;
using System.Collections.Generic;
using System.Linq;
using Facet.Combinatorics;

namespace Y2017.Day2 {
    public class SpreadSheet {
        private string[] Rows { get; }

        public SpreadSheet(string[] rows) {
            Rows = rows;
        }

        public long Checksum  {
            get {
                long checksum = 0;

                foreach (var row in Rows) {
                    var iRow = row.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                    try {
                        checksum += iRow.Max() - iRow.Min();
                    }
                    catch (Exception e) {
                        throw e;
                    }

                }
                return checksum;
            }
        }

        public long SumEvenDivisable {
            get {
                long sum = 0;
                foreach (var row in Rows) {
                    List<long> iRow = row.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                    var combinations = new Combinations<long>(iRow, 2);
                    foreach (var combination in combinations) {
                        var first = combination[0];
                        var second = combination[1];

                        if (first > second && first % second == 0) {
                            sum += first / second;
                        }
                        else if (second % first == 0) {
                            sum += second / first;
                        }
                    }
                }
                return sum;
            }
        }
    }
}