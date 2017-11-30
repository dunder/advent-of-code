using System;
using System.Linq;

namespace Y2016 {
    public class Triangle {
        public static int CountPossibleTriangles(string[] input) {
            int sum = 0;
            foreach (var suggestion in input) {
                int[] sides = suggestion.Trim().Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).OrderBy(x => x).ToArray();

                if (sides[0] + sides[1] > sides[2]) {
                    sum += 1;
                }
            }
            return sum;
        }

        public static int CountPossibleTrianglesVertically(string[] input) {
            int sum = 0;
            int skip = 0;
            for (int i = 0; i < input.Length / 3; i++) {
                var rowSet = input.Skip(skip).Take(3).SelectMany(x => x.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)).ToArray();
                for (int t = 0; t < 3; t++) {
                    var sides = new[] { rowSet[0+t], rowSet[3+t], rowSet[6+t] }.OrderBy(x => x).ToArray();
                    if (sides[0] + sides[1] > sides[2]) {
                        sum += 1;
                    }
                }
                skip += 3;
            }
            return sum;
        }
    }
}