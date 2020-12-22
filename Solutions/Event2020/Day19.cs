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

        private static List<List<string>> Split(string message, int times)
        {
            var splits = new List<List<string>>();

            return splits;
        }

        private static List<string> Split(string message, List<List<string>> result, List<string> current, int times)
        {   
            // idé 1: fullt rekursiv
            // idé 2: specialbehandla endast fallen 1, 2 och 3 där 3 blir en dubbel for-loop variant av 2
            
            // ababab -> a b abab, a ba bab, a bab ab, a baba b
            //           ab abab
            //           aba bab

            //if (!current.Any())
            //{
            //    current = new List<string>();
            //}

            //if (current.Count == times)
            //{
            //    return current;
            //}

            //for (int i = 1; i < message.Length; i++)
            //{
            //    var part1 = message.Substring(0, i);
            //    current.Add(part1);
            //    var part2 = message.Substring(i, message.Length - i);
            //    current = current.Concat(Split(part2, ))
            //}

            //return current;
            return null;
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

        private class SequenceRule : Rule
        {
            public static bool MatchesRuleDescription(string description)
            {
                return description.Contains(":") && !description.Contains("|") && !description.Contains(@"""");
            }

            public static SequenceRule Parse(string line)
            {
                var parts = line.Split(":");
                var id = parts[0];

                var rulesSequence = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                if (rulesSequence.Count > 2)
                {
                    throw new NotImplementedException("No support for more than two sequential rules!");
                }

                return new SequenceRule(id)
                {
                    RulesSequence = rulesSequence
                };
            }

            public SequenceRule(string id) : base(id) {}

            public List<string> RulesSequence { get; private set; }


            public override bool IsValid(string message, IDictionary<string, Rule> rules)
            {
                if (RuleCache.ContainsKey(message))
                {
                    return RuleCache[message];
                }

                if (RulesSequence.Count == 1)
                {
                    return rules[RulesSequence[0]].IsValid(message, rules);
                }

                for (int i = 1; i < message.Length; i++)
                {
                    var seq1 = message.Substring(0, i);
                    var seq2 = message.Substring(i, message.Length - i);

                    if (rules[RulesSequence[0]].IsValid(seq1, rules) && rules[RulesSequence[1]].IsValid(seq2, rules))
                    {
                        RuleCache.Add(message, true);
                        return true;
                    }
                }

                RuleCache.Add(message, false);

                return false;
            }

        }

        private class SubRule : Rule
        {
            public static Regex MatchExpression = new Regex(@"(\d+): (\d+) (\d+) \| (\d+) (\d+)");

            public static bool MatchesRuleDescription(string description)
            {
                return MatchExpression.IsMatch(description);
            }

            public static SubRule Parse(string line)
            {
                var m = MatchExpression.Match(line);

                var id = m.Groups[1].Value;
                var optionLeft1 = m.Groups[2].Value;
                var optionLeft2 = m.Groups[3].Value;
                var optionRight1 = m.Groups[4].Value;
                var optionRight2 = m.Groups[5].Value;

                return new SubRule(id)
                {
                    OptionLeft1 = optionLeft1,
                    OptionLeft2 = optionLeft2,
                    OptionRight1 = optionRight1,
                    OptionRight2 = optionRight2
                };
            }

            public SubRule(string id) : base(id) { }

            public string OptionLeft1 { get; set; }
            public string OptionLeft2 { get; set; }
            public string OptionRight1 { get; set; }
            public string OptionRight2 { get; set; }

            public override bool IsValid(string message, IDictionary<string, Rule> rules)
            {
                if (RuleCache.ContainsKey(message))
                {
                    return RuleCache[message];
                }

                for (int i = 1; i < message.Length; i++)
                {
                    var seq1 = message.Substring(0, i);
                    var seq2 = message.Substring(i, message.Length - i);

                    var matchLeft = rules[OptionLeft1].IsValid(seq1, rules) && rules[OptionLeft2].IsValid(seq2, rules);
                    var matchRight = rules[OptionRight1].IsValid(seq1, rules) && rules[OptionRight2].IsValid(seq2, rules);

                    if (matchLeft || matchRight)
                    {
                        RuleCache.Add(message, true);
                        return true;
                    }
                }

                RuleCache.Add(message, false);
                return false;
            }
        }

        private class SubRule2 : Rule
        {
            public static Regex MatchExpression = new Regex(@"(\d+): (\d+) \| (\d+)");

            public static bool MatchesRuleDescription(string description)
            {
                return MatchExpression.IsMatch(description);
            }

            public static SubRule2 Parse(string line)
            {
                var m = MatchExpression.Match(line);

                var id = m.Groups[1].Value;
                var optionLeft = m.Groups[2].Value;
                var optionRight = m.Groups[3].Value;

                return new SubRule2(id)
                {
                    OptionLeft = optionLeft,
                    OptionRight = optionRight,
                };
            }

            public SubRule2(string id) : base(id) { }

            public string OptionLeft { get; set; }
            public string OptionRight { get; set; }

            public override bool IsValid(string message, IDictionary<string, Rule> rules)
            {
                if (RuleCache.ContainsKey(message))
                {
                    return RuleCache[message];
                }

                var matchLeft = rules[OptionLeft].IsValid(message, rules);
                var matchRight = rules[OptionRight].IsValid(message, rules);

                var result = matchLeft || matchRight;

                RuleCache.Add(message, result);

                return result;
            }
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
        }

        private class SequenceRuleRecursive : Rule
        {
            public SequenceRuleRecursive(string id) : base(id)
            {

            }

            public static bool MatchesRuleDescription(string description)
            {
                return description.Contains(":") && !description.Contains("|") && !description.Contains(@"""");
            }

            public static SequenceRuleRecursive Parse(string line)
            {
                var idIdx = line.IndexOf(":");
                var id = line.Substring(0, idIdx);

                var ruleIds = line.Substring(idIdx).Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                return new SequenceRuleRecursive(id)
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
                    return rules[RulesSequence[0]].IsValid(message, rules);
                }

                if (RulesSequence.Count == 2)
                {
                    foreach (var pair in SplitIn2(message))
                    {
                        return rules[RulesSequence[0]].IsValid(pair[0], rules) && rules[RulesSequence[1]].IsValid(pair[1], rules);
                    }
                }

                if (RulesSequence.Count == 3)
                {
                    foreach (var triplet in SplitIn3(message))
                    {
                        return rules[RulesSequence[0]].IsValid(triplet[0], rules) && rules[RulesSequence[1]].IsValid(triplet[1], rules) && rules[RulesSequence[1]].IsValid(triplet[2], rules);
                    }
                }

                throw new NotImplementedException();
            }
        }

        private class OrRuleRecursive : Rule
        {
            public OrRuleRecursive(string id) : base(id)
            {

            }

            public static bool MatchesRuleDescription(string description)
            {
                return description.Contains(":") && description.Contains("|");
            }

            public SequenceRuleRecursive Left { get; set; }
            public SequenceRuleRecursive Right { get; set; }

            public static OrRuleRecursive Parse(string line)
            {
                var idIdx = line.IndexOf(":");
                var id = line.Substring(0, idIdx);

                var operands = line.Substring(idIdx).Split("|", StringSplitOptions.RemoveEmptyEntries).ToList();

                var left = operands[0];
                var right = operands[1];

                return new OrRuleRecursive(id)
                {
                    //new SLeft = equenceRuleRecursive(id) { RulesSequence = new List<string> { } },
                    //Right = new SequenceRuleRecursive(id)
                };
            }

            public override bool IsValid(string message, IDictionary<string, Rule> rules)
            {
                if (RuleCache.ContainsKey(message))
                {
                    return RuleCache[message];
                }

                throw new NotImplementedException();
            }
        }

        private static (IDictionary<string, Rule>, List<string>) Parse(List<string> lines)
        {

            var rules = new Dictionary<string, Rule>();
            var messages = new List<string>();


            foreach (var line in lines)
            {
                if (SubRule.MatchesRuleDescription(line))
                {
                    var rule = SubRule.Parse(line);
                    rules.Add(rule.Id, rule);
                }
                else if (SubRule2.MatchesRuleDescription(line))
                {
                    var rule = SubRule2.Parse(line);
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
                else
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

        private static int EvaluateRules(IDictionary<string, Rule> rules, List<string> messages)
        {
            var result = messages.Count(message => rules["0"].IsValid(message, rules));

            var rule31 = rules["31"].RuleCache.Where(kvp => kvp.Value);
            var rule42 = rules["42"].RuleCache.Where(kvp => kvp.Value);

            var rule31Lengths = rule31.Select(r => r.Key.Length).Distinct();
            var rule42Lengths = rule42.Select(r => r.Key.Length).Distinct();

            return result;
        }

        private static int MatchesRule0Cyclic(List<string> lines)
        {
            var (rules, messages) = Parse(lines);

            messages.ForEach(message => rules["0"].IsValid(message, rules));

            var maxMessageLength = messages.Max(message => message.Length);
            var rule31 = rules["31"].RuleCache.Where(kvp => kvp.Value).Select(r => r.Key).ToList();
            var rule42 = rules["42"].RuleCache.Where(kvp => kvp.Value).Select(r => r.Key).ToList();

            var (newRules, _) = Parse(lines);

            return messages.Count(message => rules["0"].IsValid(message, newRules));
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
            Assert.Equal(-1, SecondStar());  // 122 is of course not correct
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

            var input = ReadLineInput().ToList();

            var count = MatchesRule0Cyclic(input);

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
