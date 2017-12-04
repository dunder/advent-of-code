using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using Facet.Combinatorics;
using Xunit;

namespace Y2017.Day2 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {
            var input = new[] {
                "5 1 9 5",
                "7 5 3",
                "2 4 6 8"
            };

            var checksum = new SpreadSheet(input).Checksum;

            Assert.Equal(8+4+6, checksum);
        }

        [Fact]
        public void Problem2_Example1() {
            
            var input = new[] {
                "5 9 2 8",
                "9 4 7 3",
                "3 8 6 5"
            };

            var sumOfEvenDivisable = new SpreadSheet(input).SumEvenDivisable;

            Assert.Equal(8 / 2 + 9 / 3 + 6 / 3, sumOfEvenDivisable);
        }
        
    }

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
