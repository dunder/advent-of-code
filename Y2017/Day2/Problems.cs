using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Facet.Combinatorics;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day2 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            string[] input = File.ReadAllLines(@".\Day2\input.txt").Select(x => Regex.Replace(x, @"\s+", " ")).ToArray();

            var result = new SpreadSheet(input).Checksum;

            _output.WriteLine($"Day 2 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day2\input.txt").Select(x => Regex.Replace(x, @"\s+", " ")).ToArray();


            var result = new SpreadSheet(input).SumEvenDivisable;

            _output.WriteLine($"Day 2 problem 2: {result}");
        }
    }

 
}
