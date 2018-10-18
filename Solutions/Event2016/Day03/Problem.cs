using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2016.Day03
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day03;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = Triangle.CountPossibleTriangles(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = Triangle.CountPossibleTrianglesVertically(input);
            return result.ToString();
        }
    }

    public class Triangle {
        public static int CountPossibleTriangles(IList<string> input) {
            int sum = 0;
            foreach (var suggestion in input) {
                int[] sides = suggestion.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).OrderBy(x => x).ToArray();

                if (sides[0] + sides[1] > sides[2]) {
                    sum += 1;
                }
            }
            return sum;
        }

        public static int CountPossibleTrianglesVertically(IList<string> input) {
            int sum = 0;
            int skip = 0;
            for (int i = 0; i < input.Count / 3; i++) {
                var rowSet = input.Skip(skip).Take(3).SelectMany(x => x.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)).ToArray();
                for (int t = 0; t < 3; t++) {
                    var sides = new[] { rowSet[0 + t], rowSet[3 + t], rowSet[6 + t] }.OrderBy(x => x).ToArray();
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