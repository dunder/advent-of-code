using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 16: Ticket Translation ---

    public class Day16
    {
        private readonly ITestOutputHelper output;

        private class Range
        {
            public int Start { get; set; }
            public int End { get; set; }

            public bool IsWithin(int value)
            {
                return value >= Start && value <= End;
            }

            public override string ToString()
            {
                return $"{Start}-{End}";
            }
        }

        private class Rule
        {
            public Rule(string name, int start1, int end1, int start2, int end2)
            {
                Name = name;
                Ranges = new List<Range> { new Range { Start = start1, End = end1 }, new Range { Start = start2, End = end2 } };
            }
            public string Name { get; set; }
            public List<Range> Ranges { get; set; }

            public bool IsWithin(int value)
            {
                return Ranges[0].IsWithin(value) || Ranges[1].IsWithin(value);
            }

            public override string ToString()
            {
                return $"{Name}: {Ranges[0]} or {Ranges[1]}";
            }
        }

        private class RuleSet
        {
            public List<Rule> Rules { get; set; }

            public List<int> InvalidValues(Ticket ticket)
            {
                return ticket.Values.Where(value => !Rules.Any(rule => rule.IsWithin(value))).ToList();
            }

            public Dictionary<string, int> FindFields(List<Ticket> tickets)
            {
                var fieldMap = new Dictionary<string, int>();
                var fieldsTaken = new HashSet<string>();
                var values = tickets.First().Values.Count;

                var indeces = Enumerable.Range(0, values).OrderBy(i => {
                    var ticketValues = tickets.Select(ticket => ticket.Values[i]).ToList();
                    return Rules.Count(rule => ticketValues.All(value => rule.IsWithin(value)));
                });

                foreach (var i in indeces)
                {
                    var ticketValues = tickets.Select(ticket => ticket.Values[i]).ToList();
                    var rule = Rules.First(rule => ticketValues.All(value => rule.IsWithin(value) && !fieldsTaken.Contains(rule.Name)));
                    fieldMap.Add(rule.Name, i);
                    fieldsTaken.Add(rule.Name);
                }

                return fieldMap;
            }
        }

        private class Ticket
        {
            public List<int> Values { get; set; }
        }

        private List<Rule> ParseRules(List<string> lines)
        {
            var regex = new Regex(@"(([a-z]|\s)+): (\d+)-(\d+) or (\d+)-(\d+)");

            var ruleLines = lines.Where(line => regex.IsMatch(line));

            var rules = new List<Rule>();

            foreach (var rule in ruleLines)
            {
                var m = regex.Match(rule);
                var name = m.Groups[1].Value;
                var start1 = int.Parse(m.Groups[3].Value);
                var end1 = int.Parse(m.Groups[4].Value);
                var start2 = int.Parse(m.Groups[5].Value);
                var end2 = int.Parse(m.Groups[6].Value);

                rules.Add(new Rule(name, start1, end1, start2, end2));
            }

            return rules;
        }

        private Ticket ParseMyTicket(List<string> lines)
        {
            var myTicketLine = lines.First(lines => lines.Contains(","));

            var values = myTicketLine.Split(",").Select(int.Parse).ToList();

            return new Ticket { Values = values };
        }

        private List<Ticket> ParseNearbyTickets(List<string> lines)
        {
            var nearbyTicketLines = lines.Where(lines => lines.Contains(",")).Skip(1);

            var values = nearbyTicketLines.Select(line => new Ticket { Values = line.Split(",").Select(int.Parse).ToList()});

            return values.ToList();
        }

        public Day16(ITestOutputHelper output)
        {
            this.output = output;
        }

        public int FirstStar()
        {
            var input = ReadLineInput().ToList();
            var rules = ParseRules(input);
            var myTicket = ParseMyTicket(input);
            var nearbyTickets = ParseNearbyTickets(input);

            var ruleSet = new RuleSet { Rules = rules };

            return nearbyTickets.SelectMany(ticket => ruleSet.InvalidValues(ticket)).Sum();
        }

        public long SecondStar()
        {
            var input = ReadLineInput().ToList();
            var rules = ParseRules(input);
            var myTicket = ParseMyTicket(input);
            var nearbyTickets = ParseNearbyTickets(input);

            var ruleSet = new RuleSet { Rules = rules };
            var validTickets = nearbyTickets.Where(ticket => !ruleSet.InvalidValues(ticket).Any()).ToList();

            var fieldSet = ruleSet.FindFields(validTickets.Append(myTicket).ToList());

            return rules
                .Where(rule => rule.Name.StartsWith("departure"))
                .Select(rule => myTicket.Values[fieldSet[rule.Name]])
                .Select(x => Convert.ToInt64(x))
                .Aggregate(1L, (x, y) => x * y);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(26053, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1515506256421, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "class: 1-3 or 5-7",
                "row: 6-11 or 33-44",
                "seat: 13-40 or 45-50",
                "",
                "your ticket:",
                "7,1,14",
                "",
                "nearby tickets:",
                "7,3,47",
                "40,4,50",
                "55,2,20",
                "38,6,12"
            };
            var rules = ParseRules(input);
            var myTicket = ParseMyTicket(input);
            var nearbyTickets = ParseNearbyTickets(input);
            var ruleSet = new RuleSet { Rules = rules };

            var x = nearbyTickets.SelectMany(ticket => ruleSet.InvalidValues(ticket)).Sum();

            Assert.Equal(71, x);
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string>
            {
                "class: 0-1 or 4-19",
                "row: 0-5 or 8-19",
                "seat: 0-13 or 16-19",
                "",
                "your ticket:",
                "11,12,13",
                "",
                "nearby tickets:",
                "3,9,18",
                "15,1,5",
                "5,14,9"
            };
            var rules = ParseRules(input);
            var myTicket = ParseMyTicket(input);
            var nearbyTickets = ParseNearbyTickets(input);
            var ruleSet = new RuleSet { Rules = rules };

            var validTickets = nearbyTickets.Where(ticket => !ruleSet.InvalidValues(ticket).Any()).ToList();

            var fieldSet = ruleSet.FindFields(validTickets.Append(myTicket).ToList());

            var x = rules.Select(rule => myTicket.Values[fieldSet[rule.Name]]).Aggregate(1, (x, y) => x * y);

            Assert.Equal(12*11*13, x);
        }
    }
}
