using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // 

    public class Day19
    {
        private readonly ITestOutputHelper output;

        public Day19(ITestOutputHelper output)
        {
            this.output = output;
        }

        private abstract class Rule
        {
            public Rule(string id)
            {
                Id = id;
            }

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
                        return true;
                    }
                }

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
                for (int i = 1; i < message.Length; i++)
                {
                    var seq1 = message.Substring(0, i);
                    var seq2 = message.Substring(i, message.Length - i);

                    var matchLeft = rules[OptionLeft1].IsValid(seq1, rules) && rules[OptionLeft2].IsValid(seq2, rules);
                    var matchRight = rules[OptionRight1].IsValid(seq1, rules) && rules[OptionRight2].IsValid(seq2, rules);

                    if (matchLeft || matchRight)
                    {
                        return true;
                    }
                }

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
               
                    var matchLeft = rules[OptionLeft].IsValid(message, rules);
                    var matchRight = rules[OptionRight].IsValid(message, rules);

                    return matchLeft || matchRight;
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

        public long FirstStar()
        {
            var input = ReadLineInput().ToList();

            var (rules, messages) = Parse(input);

            return MatchesRule0(rules, messages);
        }

        public long SecondStar()
        {
            var input = ReadLineInput().ToList();

            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
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
    }
}
