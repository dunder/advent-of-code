using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 19: Monster Messages ---

    public class Day19
    {
        private readonly ITestOutputHelper output;

        public Day19(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static List<List<string>> SplitIn2(string message)
        {
            var result = new List<List<string>>();

            for (int i = 1; i < message.Length; i++)
            {
                var seq1 = message.Substring(0, i);
                var seq2 = message.Substring(i, message.Length - i);

                result.Add(new List<string> { seq1, seq2 });
            }

            return result;
        }

        private static List<List<string>> SplitIn3(string message)
        {
            var result = new List<List<string>>();

            for (int i = 1; i < message.Length - 1; i++)
            {
                for (int j = i+1; j < message.Length; j++)
                {
                    var seq1 = message.Substring(0, i);
                    var seq2 = message.Substring(i, j - i);
                    var seq3 = message.Substring(j, message.Length - j);

                    result.Add(new List<string> { seq1, seq2, seq3 });
                }
            }

            return result;
        }

        private abstract class Rule
        {
            public Rule(string id)
            {
                Id = id;
            }

            public IDictionary<string, bool> RuleCache = new Dictionary<string, bool>();

            public string Id { get; private set; }

            public abstract bool IsValid(string message, IDictionary<string, Rule> rules);
        }

        private class MatchRule : Rule
        {
            public static Regex MatchExpression = new Regex(@"(\d+): ""(a|b)""");

            public static bool MatchesRuleDescription(string description)
            {
                return MatchExpression.IsMatch(description);
            }

            public static MatchRule Parse(string line)
            {
                var m = MatchExpression.Match(line);

                var id = m.Groups[1].Value;
                var match = m.Groups[2].Value;

                return new MatchRule(id)
                {
                    Match = match
                };
            }

            public MatchRule(string id) : base(id) { }

            public string Match { get; private set; }

            public override bool IsValid(string message, IDictionary<string, Rule> rules)
            {
                return message == Match;
            }

            public override string ToString()
            {
                return $@"""{Match}""";
            }
        }

        private class SequenceRule : Rule
        {
            public SequenceRule(string id) : base(id) {}

            public static bool MatchesRuleDescription(string description)
            {
                return description.Contains(":") && !description.Contains("|") && !description.Contains(@"""");
            }

            public static SequenceRule Parse(string line)
            {
                var idIdx = line.IndexOf(":");
                var id = line.Substring(0, idIdx);

                var ruleIds = line.Substring(idIdx + 1).Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                return new SequenceRule(id)
                {
                    RulesSequence = ruleIds
                };
            }

            public List<string> RulesSequence { get; private set; }

            public override bool IsValid(string message, IDictionary<string, Rule> rules)
            {
                if (RuleCache.ContainsKey(message))
                {
                    return RuleCache[message];
                }

                if (RulesSequence.Count == 1)
                {
                    var result = rules[RulesSequence[0]].IsValid(message, rules);
                    RuleCache.Add(message, result);
                    return result;
                }

                if (RulesSequence.Count == 2)
                {
                    var pairs = SplitIn2(message);

                    var result = pairs.Any(pair => rules[RulesSequence[0]].IsValid(pair[0], rules) && rules[RulesSequence[1]].IsValid(pair[1], rules));

                    RuleCache.Add(message, result);

                    return result;
                }

                if (RulesSequence.Count == 3)
                {
                    var triplets = SplitIn3(message);
                    var result = triplets.Any(triple => 
                        rules[RulesSequence[0]].IsValid(triple[0], rules) && 
                        rules[RulesSequence[1]].IsValid(triple[1], rules) && 
                        rules[RulesSequence[2]].IsValid(triple[2], rules));

                    RuleCache.Add(message, result);

                    return result;
                }

                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Join(" ", RulesSequence);
            }
        }

        private class OrRule : Rule
        {
            public OrRule(string id) : base(id) { }

            public static bool MatchesRuleDescription(string description)
            {
                return description.Contains(":") && description.Contains("|");
            }

            public SequenceRule Left { get; set; }
            public SequenceRule Right { get; set; }

            public static OrRule Parse(string line)
            {
                var idIdx = line.IndexOf(":");
                var id = line.Substring(0, idIdx);

                var operands = line.Substring(idIdx + 1).Split("|", StringSplitOptions.RemoveEmptyEntries).ToList();

                var left = operands[0];
                var right = operands[1];

                return new OrRule(id)
                {
                    Left = SequenceRule.Parse($"{id}: {left}"),
                    Right = SequenceRule.Parse($"{id}: {right}")
                };
            }

            public override bool IsValid(string message, IDictionary<string, Rule> rules)
            {
                if (RuleCache.ContainsKey(message))
                {
                    return RuleCache[message];
                }

                var result = Left.IsValid(message, rules) || Right.IsValid(message, rules);
                
                RuleCache.Add(message, result);
                return result;
            }

            public override string ToString()
            {
                return $"{Left} | {Right}";
            }
        }

        private static (IDictionary<string, Rule>, List<string>) Parse(List<string> lines)
        {
            var rules = new Dictionary<string, Rule>();
            var messages = new List<string>();

            foreach (var line in lines)
            {
                if (OrRule.MatchesRuleDescription(line))
                {
                    var rule = OrRule.Parse(line);
                    rules.Add(rule.Id, rule);
                }
                else if (MatchRule.MatchesRuleDescription(line))
                {
                    MatchRule matchRule = MatchRule.Parse(line);
                    rules.Add(matchRule.Id, matchRule);
                }
                else if (SequenceRule.MatchesRuleDescription(line))
                {
                    SequenceRule sequenceRule = SequenceRule.Parse(line);
                    rules.Add(sequenceRule.Id, sequenceRule);
                }
                else if (!string.IsNullOrEmpty(line))
                {
                    messages.Add(line);
                }
            }
            return (rules, messages);
        }

        private static int MatchesRule0(IDictionary<string, Rule> rules, List<string> messages)
        {
            return messages.Count(message => rules["0"].IsValid(message, rules));
        }

        private static int MatchesRule0Cyclic(List<string> lines)
        {
            var (newRules, messages) = Parse(lines);

            newRules["8"] = OrRule.Parse("8: 42 | 42 8");
            newRules["11"] = OrRule.Parse("11: 42 31 | 42 11 31");

            var result = messages.Count(message => newRules["0"].IsValid(message, newRules));
            return result;
        }

        public long FirstStar()
        {
            var input = ReadLineInput().ToList();

            var (rules, messages) = Parse(input);

            return MatchesRule0(rules, messages);
        }

        public long SecondStar()
        {
            var input = ReadLineInput().ToList();

            return MatchesRule0Cyclic(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(122, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(287, SecondStar());
        }

        [Theory]
        [InlineData("aab", true)]
        [InlineData("aba", true)]
        [InlineData("bab", false)]
        [InlineData("baa", false)]
        public void FirstStarExample(string message, bool expectMatch)
        {
            var input = new List<string> {
                @"0: 1 2",
                @"1: ""a""",
                @"2: 1 3 | 3 1",
                @"3: ""b"""
            };

            var (rules, _) = Parse(input);

            var count = MatchesRule0(rules, new List<string> { message });
            var expected = expectMatch ? 1 : 0;

            Assert.Equal(expected, count);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = new List<string> {
                @"0: 4 1 5",
                @"1: 2 3 | 3 2",
                @"2: 4 4 | 5 5",
                @"3: 4 5 | 5 4",
                @"4: ""a""",
                @"5: ""b""",
                @"",
                @"ababbb",
                @"bababa",
                @"abbbab",
                @"aaabbb",
                @"aaaabbb",
            };

            var (rules, messages) = Parse(input);

            var count = MatchesRule0(rules, messages);

            Assert.Equal(2, count);
        }

        [Fact]
        public void SecondStarExample()
        {
            var inputTest = new List<string> {
                @"42: 9 14 | 10 1",
                @"9: 14 27 | 1 26",
                @"10: 23 14 | 28 1",
                @"1: ""a""",
                @"11: 42 31",
                @"5: 1 14 | 15 1",
                @"19: 14 1 | 14 14",
                @"12: 24 14 | 19 1",
                @"16: 15 1 | 14 14",
                @"31: 14 17 | 1 13",
                @"6: 14 14 | 1 14",
                @"2: 1 24 | 14 4",
                @"0: 8 11",
                @"13: 14 3 | 1 12",
                @"15: 1 | 14",
                @"17: 14 2 | 1 7",
                @"23: 25 1 | 22 14",
                @"28: 16 1",
                @"4: 1 1",
                @"20: 14 14 | 1 15",
                @"3: 5 14 | 16 1",
                @"27: 1 6 | 14 18",
                @"14: ""b""",
                @"21: 14 1 | 1 14",
                @"25: 1 1 | 1 14",
                @"22: 14 14",
                @"8: 42",
                @"26: 14 22 | 1 20",
                @"18: 15 15",
                @"7: 14 5 | 1 21",
                @"24: 14 1",
                "",
                "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa",
                "bbabbbbaabaabba",
                "babbbbaabbbbbabbbbbbaabaaabaaa",
                "aaabbbbbbaaaabaababaabababbabaaabbababababaaa",
                "bbbbbbbaaaabbbbaaabbabaaa",
                "bbbababbbbaaaaaaaabbababaaababaabab",
                "ababaaaaaabaaab",
                "ababaaaaabbbaba",
                "baabbaaaabbaaaababbaababb",
                "abbbbabbbbaaaababbbbbbaaaababb",
                "aaaaabbaabaaaaababaa",
                "aaaabbaaaabbaaa",
                "aaaabbaabbaaaaaaabbbabbbaaabbaabaaa",
                "babaaabbbaaabaababbaabababaaab",
                "aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba"
            };

            var count = MatchesRule0Cyclic(inputTest);

            Assert.Equal(12, count);
        }

        [Fact]
        public void SplitIn2Test()
        {
            var result = SplitIn2("abcde");

            Assert.Contains(result, pair => pair[0] == "a" && pair[1] == "bcde");
            Assert.Contains(result, pair => pair[0] == "ab" && pair[1] == "cde");
            Assert.Contains(result, pair => pair[0] == "abc" && pair[1] == "de");
            Assert.Contains(result, pair => pair[0] == "abcd" && pair[1] == "e");

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void SplitIn3Test()
        {
            var result = SplitIn3("abcde");

            Assert.Contains(result, triplet => triplet[0] == "a" && triplet[1] == "b" && triplet[2] == "cde");
            Assert.Contains(result, triplet => triplet[0] == "a" && triplet[1] == "bc" && triplet[2] == "de");
            Assert.Contains(result, triplet => triplet[0] == "a" && triplet[1] == "bcd" && triplet[2] == "e");
            Assert.Contains(result, triplet => triplet[0] == "ab" && triplet[1] == "c" && triplet[2] == "de");
            Assert.Contains(result, triplet => triplet[0] == "ab" && triplet[1] == "cd" && triplet[2] == "e");
            Assert.Contains(result, triplet => triplet[0] == "abc" && triplet[1] == "d" && triplet[2] == "e");

            Assert.Equal(6, result.Count);
        }
    }
}
