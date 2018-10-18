using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Facet.Combinatorics;

namespace Solutions.Event2017.Day04
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day04;

        public override string FirstStar()
        {
            var input = ReadLineInput().Select(x => Regex.Replace(x, @"\s+", " ")).ToList();
            var result = PassPhrase.Count(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput().Select(x => Regex.Replace(x, @"\s+", " ")).ToList();
            var result = PassPhrase.CountAnagrams(input);
            return result.ToString();
        }
    }

    public class PassPhrase {
        public static int Count(IList<string> input) {
            var count = 0;
            foreach (var line in input) {
                var words = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var wordCount = words.Length;
                var distinctWords = new HashSet<string>(words);
                if (wordCount == distinctWords.Count) {
                    count += 1;
                }
            }
            return count;
        }

        public static int CountAnagrams(IList<string> input) {
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