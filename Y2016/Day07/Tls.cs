using System;
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
            var hypernetPattern = @"\[[a-z]+\]";
            var hypernetFree = string.Join(" ", Regex.Split(candidate, hypernetPattern));



            var abs = Regex.Matches(hypernetFree, @"([a-z])(?!\1)[a-z](?=\1)");
            if (abs.Count == 0) {
                return false;
            }

            var hypernetSequences = Regex.Matches(candidate, @"\[[a-z]+\]");
            var hypernetSet = new HashSet<string>();

            foreach (Match hypernetSequence in hypernetSequences) {
                var trimmed = hypernetSequence.Value.Trim('[', ']');
                var abas =  Regex.Matches(trimmed, @"([a-z])(?!\1)[a-z](?=\1)");
                foreach (Match abaMatch in abas) {
                    var aba = abaMatch.Value;
                    hypernetSet.Add(new string(new [] {aba[0], aba[1], aba[0]}));
                }
            }

            var invertedSet = new HashSet<string>();
            foreach (Match abaMatch in abs) {
                var aba = abaMatch.Value;
                var inverted = new string(new []{ aba[1], aba[0], aba[1]});
                invertedSet.Add(inverted);
            }

            return hypernetSet.Overlaps(invertedSet);
        }

        private static bool IsAbba(string candidate) {
            return Regex.IsMatch(candidate, @"([a-z])(?!\1)([a-z])(\2)(\1)");
        }
    }
}