using System.Linq;
using Xunit;

namespace Solutions.Event2016.Day06
{
    public class Tests
    {
        [Fact]
        public void FirstStarFrequentLetter()
        {
            const string input = "eedadn";

            var frequency =
                from indexed in input.Select((c, i) => new {Char = c, Index = i})
                group indexed by new
                {
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
        public void FirstStarExample()
        {
            string[] input =
            {
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

            var decoded = SignalDecoder.Decode(input, SignalDecoder.Frequency.Most);

            Assert.Equal("easter", decoded);
        }

        [Fact]
        public void SecondStarExample()
        {
            string[] input =
            {
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

            var decoded = SignalDecoder.Decode(input, SignalDecoder.Frequency.Least);

            Assert.Equal("advent", decoded);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("tzstqsua", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("myregdnr", actual);
        }
    }
}