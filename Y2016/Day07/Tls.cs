using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Y2016.Day07 {
    public class Tls {
        public static int CountAbba(string[] input) {
            return input.Count(IsAutonomousBridgeBypassingAnnotation);
        }

        public static int CountAba(string[] input) {
            return input.Count(IsAreaBroadcastAccessor);
        }

        public static bool IsAutonomousBridgeBypassingAnnotation(string candidate) {
            var hypernetPattern = @"\[[a-z]+\]";
            var hypernetSequences = Regex.Matches(candidate, hypernetPattern);
            foreach (Match hypernetSequence in hypernetSequences) {
                if (IsAbba(hypernetSequence.Value)) {
                    return false;
                }
            }
            return Regex.Split(candidate, hypernetPattern).Any(IsAbba);
        }

        public static bool IsAreaBroadcastAccessor(string candidate) {
            var hypernetRegex = new Regex(@"\[[a-z]+\]");
            var hypernetFree = string.Join(" ", hypernetRegex.Split(candidate));

            var abas = ReadAbas(hypernetFree);

            if (abas.Count == 0) {
                return false;
            }

            var hypernetSequences = hypernetRegex.Matches(candidate);
            var hypernetSet = new HashSet<string>();

            foreach (Match hypernetSequence in hypernetSequences) {
                var trimmed = hypernetSequence.Value.Trim('[', ']');
                var babs = ReadAbas(trimmed);
                foreach (var bab in babs) {
                    hypernetSet.Add(new string(new [] {bab[0], bab[1], bab[0]}));
                }
            }

            var invertedSet = new HashSet<string>(abas.Select(aba => new string(new[] { aba[1], aba[0], aba[1] })));
            
            return hypernetSet.Overlaps(invertedSet);
        }

        private static List<string> ReadAbas(string input) {
            List<string> abs = new List<string>();
            for (int i = 0; i < input.Length - 2; i++) {
                var part = input.Substring(i, 3);
                if (part.All(char.IsLetter) && part[0] != part[1] && part[0] == part[2]) {
                    abs.Add(part);
                }
            }
            return abs;
        }

        private static bool IsAbba(string candidate) {
            return Regex.IsMatch(candidate, @"([a-z])(?!\1)([a-z])(\2)(\1)");
        }
    }
}