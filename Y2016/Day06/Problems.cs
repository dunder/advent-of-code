using System.Collections.Generic;
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
            var result = SignalDecoder.Decode(input);

            Assert.Equal("tzstqsua", result);
            _output.WriteLine($"Day 6 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day06\input.txt");
            var result = SignalDecoder.Decode2(input);

            Assert.Equal("myregdnr", result);
            _output.WriteLine($"Day 6 problem 2: {result}");
        }

        public class SignalDecoder {
            public static string Decode(string[] input) {
                StringBuilder decoded = new StringBuilder();
                int columnWith = input.First().Length;
                for (int column = 0; column < columnWith; column++) {
                    char[] coded = input.Select(l => l[column]).ToArray();
                    var frequency =
                        from indexed in coded.Select((c, i) => new { Char = c, Index = i })
                        group indexed by new {
                            indexed.Char
                        }
                        into g
                        orderby g.Count() descending, g.First().Index
                        select g;
                    decoded.Append(frequency.First().Key.Char);
                }

                return decoded.ToString();
            }

            public static string Decode2(string[] input) {
                StringBuilder decoded = new StringBuilder();
                int columnWith = input.First().Length;
                for (int column = 0; column < columnWith; column++) {
                    char[] coded = input.Select(l => l[column]).ToArray();
                    var frequency =
                        from indexed in coded.Select((c, i) => new { Char = c, Index = i })
                        group indexed by new {
                            indexed.Char
                        }
                        into g
                        orderby g.Count() descending, g.First().Index
                        select g;
                    decoded.Append(frequency.Last().Key.Char);
                }

                return decoded.ToString();
            }
        }
    }
}
