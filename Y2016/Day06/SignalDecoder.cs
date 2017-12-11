using System.Linq;
using System.Text;

namespace Y2016.Day06 {
    public class SignalDecoder {
        public enum Frequency {
            Most,
            Least
        }

        public static string Decode(string[] input, Frequency frequency) {
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