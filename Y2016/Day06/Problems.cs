using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day06 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day06\input.txt");
            var result = SignalDecoder.Decode(input, SignalDecoder.Frequency.Most);

            Assert.Equal("tzstqsua", result);
            _output.WriteLine($"Day 6 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day06\input.txt");
            var result = SignalDecoder.Decode(input, SignalDecoder.Frequency.Least);

            Assert.Equal("myregdnr", result);
            _output.WriteLine($"Day 6 problem 2: {result}");
        }

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
}
