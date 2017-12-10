using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Y2016.Day06 {
    public class Tests {

        [Fact]
        public void Problem1_TestFrequentLetter() {
            const string input = "eedadn";

            var frequency =
                from indexed in input.Select((c, i) => new {Char = c, Index = i})
                group indexed by new {
                    indexed.Char
                }
                into g
                orderby g.Count() descending 
                orderby g.First().Index
                select g;

            var mostFrequent = frequency.First().Key.Char;
            Assert.Equal('e', mostFrequent);
        }

        [Fact]
        public void Problem1_Example1() {
            string[] input = {
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

            var decoded = Problems.SignalDecoder.Decode(input);

            Assert.Equal("easter", decoded);
        }

        [Fact]
        public void Problem2_Example1() {
            string[] input = {
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

            var decoded = Problems.SignalDecoder.Decode2(input);

            Assert.Equal("advent", decoded);
        }
    }

    
}
