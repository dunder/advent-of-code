using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solutions.Event2016.Day6
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day6;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = SignalDecoder.Decode(input, SignalDecoder.Frequency.Most);
            return result;
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = SignalDecoder.Decode(input, SignalDecoder.Frequency.Least);
            return result;
        }
    }

    public class SignalDecoder {
        public enum Frequency {
            Most,
            Least
        }

        public static string Decode(IList<string> input, Frequency frequency) {
            StringBuilder decoded = new StringBuilder();
            int columnWith = input.First().Length;
            for (int column = 0; column < columnWith; column++) {
                char[] coded = input.Select(l => l[column]).ToArray();
                var frequencyChar = FindFrequent(coded, frequency);
                decoded.Append(frequencyChar);
            }

            return decoded.ToString();
        }

        private static char FindFrequent(char[] input, Frequency frequency) {
            var groupedByFrequency =
                from indexed in input.Select((c, i) => new { Char = c, Index = i })
                group indexed by new {
                    indexed.Char
                }
                into g
                orderby g.Count() descending, g.First().Index
                select g;
            return frequency == Frequency.Most ? groupedByFrequency.First().Key.Char : groupedByFrequency.Last().Key.Char;
        }
    }

}