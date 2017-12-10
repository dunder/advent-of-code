using System;
using System.Collections.Generic;
using System.Linq;
using Facet.Combinatorics;

namespace Y2017.Day04 {
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
                    count += 1; 
                }
            }
            return count;
        }
    }
}