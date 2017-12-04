using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Facet.Combinatorics;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day4 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day4\input.txt").Select(x => Regex.Replace(x, @"\s+", " ")).ToArray();

            var result = PassPhrase.Count(input);

            _output.WriteLine($"Day 4 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day4\input.txt").Select(x => Regex.Replace(x, @"\s+", " ")).ToArray();

            var result = PassPhrase.CountAnagrams(input);
            _output.WriteLine($"Day 4 problem 2: {result}");
        }
    }

    public class PassPhrase {
        public static int Count(string[] input) {
            var count = 0;
            foreach (var line in input) {
                var words = line.Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries);
                var wordCount = words.Length;
                var distinctWords = new HashSet<string>(words);
                if (wordCount == distinctWords.Count) {
                    count += 1;
                }
            }
            return count;
        }

        public static int CountAnagrams(string[] input) {
            var count = 0;
            foreach (var line in input) {
                var words = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var wordSet = new HashSet<string>(words);
                var wordCount = words.Length;
                if (wordCount != wordSet.Count) {
                    continue;
                }
                var letterizedWords = words.Select(word => new List<char>(word)).ToArray();
                bool valid = true;
                foreach (var word in letterizedWords) {
                    var otherWords = new HashSet<string>(wordSet);
                    otherWords.ExceptWith(new[] { new string(word.ToArray()) });
                    var combinations = new Permutations<char>(word);

                    if (combinations.Any(c => otherWords.Contains(new string(c.ToArray())))) {
                        valid = false;
                        break;
                    }
                }
                if (valid) {
                    count += 1; // 202 är för högt
                    continue;                    
                }
            }
            return count;
        }
    }
}
