using System.Collections.Generic;
using System.Text;

namespace Solutions.Event2018.Day05
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day05;

        public override string FirstStar()
        {
            var input = ReadInput();
            var result = ReduceAll(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = ReduceEnhanced(input);
            return result.ToString();
        }

        public static int ReduceAll(string polymer)
        {
            var closed = new HashSet<int>();
            var reduction = 0;
            for (int i1 = 0, i2 = 1; i2 < polymer.Length;)
            {
                char c1 = polymer[i1];
                char c2 = polymer[i2];

                if (c1 == c2)
                {
                    i1 = i2;
                    i2++;
                    continue;
                }

                if (char.ToUpper(c1) == char.ToUpper(c2))
                {
                    closed.Add(i1);
                    closed.Add(i2);
                    do
                    {
                        i1--;
                    } while (closed.Contains(i1));

                    if (i1 < 0)
                    {
                        i2++;
                        i1 = i2;
                        i2++;
                    }
                    else
                    {
                        i2++;
                    }
;
                    reduction += 2;
                    continue;
                }

                i1 = i2;
                i2++;
            }

            return polymer.Length - reduction;
        }

        public static int ReduceEnhanced(string polymer)
        {
            var all = new HashSet<char>(polymer);

            int min = int.MaxValue;
            foreach (var unit in all)
            {
                var polymerToReduce = polymer;
                char cLower = char.ToLower(unit);
                char cUpper = char.ToUpper(unit);
                polymerToReduce = polymerToReduce.Replace(cLower.ToString(), "");
                polymerToReduce = polymerToReduce.Replace(cUpper.ToString(), "");

                var reduced = ReduceAll(polymerToReduce);

                if (reduced < min)
                {
                    min = reduced;
                }
            }

            return min;
        }

        public static string ReactReduce(string polymer)
        {
            int i = 0;
            bool reduced = false;
            for (; i < polymer.Length - 1; i++)
            {
                var p1 = polymer[i];
                var p2 = polymer[i + 1];

                if (p1 == p2)
                {
                    continue;
                };

                if (char.ToLower(p1) == char.ToLower(p2))
                {
                    reduced = true;
                    break;
                }                
            }

            return reduced ? polymer.Remove(i, 2) : polymer;
        }
    }
}