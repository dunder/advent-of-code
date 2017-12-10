using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day06 {
    public class Tests {

        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1_Example1() {
            string[] input = new[] {
                "eedadn",
                "drvtee",
                "eandsr",
                "raavrd",
                "atevrs",
                "tsrnev",
                "sdttsa",
                "rasrtv",
                "nssdts",
                "ntnada",
                "svetve",
                "tesnvt",
                "vntsnd",
                "vrdear",
                "dvrsen",
                "enarar"
            };

            var decoded = SignalDecoder.Decode(input);

            Assert.Equal("easter", decoded);
        }
    }

    public class SignalDecoder {
        public static IEnumerable<char> Decode(string[] input) {
            string decoded = "";

            foreach (var scrambledWord in input) {
                var charCount = new Dictionary<char, int>();
                foreach (var c in scrambledWord) {
                    if (!charCount.ContainsKey(c)) {
                        charCount.Add(c, 1);
                    }
                    charCount[c]++;
                }
                //charCount.
            }
            return decoded;
        }
    }
}
