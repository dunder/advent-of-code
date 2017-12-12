using System;
using System.Drawing;
using Xunit;

namespace Y2017.Day12 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {
            var input = new[] {
                "0 <-> 2      ",
                "1 <-> 1      ",
                "2 <-> 0, 3, 4",
                "3 <-> 2, 4   ",
                "4 <-> 2, 3, 6",
                "5 <-> 6      ",
                "6 <-> 4, 5   "
            };

            var connectionsToZero = MemoryBank.CountConnectedTo0(input);

            Assert.Equal(6, connectionsToZero);
        }

        [Fact]
        public static void Problem2_Example() {
            var input = new[] {
                "0 <-> 2      ",
                "1 <-> 1      ",
                "2 <-> 0, 3, 4",
                "3 <-> 2, 4   ",
                "4 <-> 2, 3, 6",
                "5 <-> 6      ",
                "6 <-> 4, 5   "
            };

            var connectionsToZero = MemoryBank.CountGroups(input);

            Assert.Equal(2, connectionsToZero);
        }
    }
}
