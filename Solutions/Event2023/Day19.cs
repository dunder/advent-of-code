using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 19: Aplenty ---
    public class Day19
    {
        private readonly ITestOutputHelper output;

        public Day19(ITestOutputHelper output)
        {
            this.output = output;
        }

        private (List<Workflow> Workflows, List<Dictionary<char, int>> Parts) Parse(IList<string> input)
        {
            List<Workflow> workflows = new();
            List<Dictionary<char,int>> machineParts = new();

            bool readingWorkflows = true;

            var ruleRegex = new Regex(@"^(.+)\{(.*)\}$");
            var partRegex = new Regex(@"x=(\d+),m=(\d+),a=(\d+),s=(\d+)");

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    readingWorkflows = false;
                    continue;
                }
                if (readingWorkflows)
                {
                    // px{a<2006:qkq,m>2090:A,rfg}

                    var match = ruleRegex.Match(line);
                    var name = match.Groups[1].Value;
                    var ruleDescriptions = match.Groups[2].Value.Split(",");

                    Rule ParseRule(string description)
                    {
                        if (description.Contains(":"))
                        {
                            var parts = description.Split(":");
                            var evaluation = parts[0];
                            var outcome = parts[1];
                            var evaluationParts = evaluation.Split('<', '>');
                            var category = evaluationParts[0];
                            var value = int.Parse(evaluationParts[1]);
                            var parameters =  new RuleParameters(category[0], value, outcome);

                            Operation operation = evaluation.Contains('<') ? Operation.LessThan : Operation.GreaterThan;

                            return new Rule(parameters, operation);
                        }
                        else
                        {
                            return new Rule(new RuleParameters(' ', 0, description), Operation.NoOp);
                        }
                    }

                    List<Rule> rules = ruleDescriptions.Select(ParseRule).ToList();

                    workflows.Add(new Workflow(name, rules));
                }
                else
                {
                    // {x=787,m=2655,a=1222,s=2876}
                    var match = partRegex.Match(line);

                    var part = new Dictionary<char, int>
                    {
                        { 'x', int.Parse(match.Groups[1].Value) },
                        { 'm', int.Parse(match.Groups[2].Value) },
                        { 'a', int.Parse(match.Groups[3].Value) },
                        { 's', int.Parse(match.Groups[4].Value) }
                    };

                    machineParts.Add(part);
                }
            }

            return (workflows, machineParts);
        }
        private enum Operation { LessThan, GreaterThan, NoOp }

        private record RuleParameters(char Category, int Value, string NextWorkflow)
        {
            public bool Terminates => NextWorkflow == "A" || NextWorkflow == "R";
        }

        private record Rule(RuleParameters Parameters, Operation Operation)
        {
            public string Execute(Dictionary<char, int> part)
            {
                switch (Operation)
                {
                    case Operation.GreaterThan:
                        return part[Parameters.Category] > Parameters.Value ? Parameters.NextWorkflow : "";
                    case Operation.LessThan:
                        return part[Parameters.Category] < Parameters.Value ? Parameters.NextWorkflow : "";
                    case Operation.NoOp:
                        return Parameters.NextWorkflow;
                    default:
                        throw new InvalidOperationException($"Unknown operation: {Operation}");
                }
            }
        }

        private record Workflow(string Name, List<Rule> Rules);

        private string ProcessWorkflow(Dictionary<string, Workflow> workflows, Workflow workflow, Dictionary<char, int> part)
        {
            foreach (var rule in workflow.Rules)
            {
                var result = rule.Execute(part);

                switch (result)
                {
                    case "A":
                    case "R":
                        return result;
                    case "":
                        continue;
                    default:
                        return ProcessWorkflow(workflows, workflows[result], part);
                }

            }
            throw new InvalidOperationException("Rule sequence did not return a valid result");
        }

        private int Run1(IList<string> input)
        {
            (List<Workflow> workflows, List<Dictionary<char, int>> parts) = Parse(input);

            Dictionary<string, Workflow> workflowLookup = workflows.ToDictionary(wf => wf.Name, wf => wf);

            List<Dictionary<char, int>> accepted = new();

            foreach (var part in parts)
            {
                var result = ProcessWorkflow(workflowLookup, workflowLookup["in"], part);
                if (result == "A")
                {
                    accepted.Add(part);
                }
            }

            return accepted.Select(part => part.Values.Sum()).Sum();
        }

        private record Range(int Start, int End)
        {
            public int Length => End - Start + 1;

            public (Range lower, Range upper) SplitGreaterThan(int value)
            {
                if (value < Start)
                {
                    return (new Range(0,0), this);
                }
                if (value < End)
                {
                    return (new Range(Start, value), new Range(value + 1, End));
                }
                return (this, new Range(0,0));
            }

            public (Range lower, Range upper) SplitLessThan(int value)
            {
                if (value < Start)
                {
                    return (new Range(0, 0), this);
                }
                if (value < End)
                {
                    return (new Range(Start, value - 1), new Range(value, End));
                }
                return (this, new Range(0, 0));
            }
        }

        private long Combinations(Dictionary<char, Range> xmas)
        {
            return xmas.Values.Aggregate(1L, (total, r) => r.Length * total);
        }

        private Dictionary<char, Range> Replace(Dictionary<char, Range> xmas, char category, Range range)
        {
            var copy = new Dictionary<char, Range>(xmas);
            copy[category] = range;
            return copy;
        }

        private long CountRuleCombinations(Dictionary<string, Workflow> workflows, Dictionary<char, Range> initialParts)
        {
            var workflowsToProcess = new Queue<(Dictionary<char, Range>, string)>();

            var accepted = new List<Dictionary<char, Range>>();

            workflowsToProcess.Enqueue((initialParts, "in"));

            while (workflowsToProcess.Count > 0)
            {
                (Dictionary<char, Range> xmas, string workflowName) = workflowsToProcess.Dequeue();

                Workflow workFlow = workflows[workflowName];
                 
                foreach (var rule in workFlow.Rules)
                {
                    int value = rule.Parameters.Value;
                    char category = rule.Parameters.Category;
                    string nextWorkflow = rule.Parameters.NextWorkflow;

                    if (rule.Operation == Operation.GreaterThan)
                    {
                        (Range lower, Range upper) split = xmas[category].SplitGreaterThan(value);

                        if (rule.Parameters.Terminates)
                        {
                            if (rule.Parameters.NextWorkflow == "A")
                            {
                                accepted.Add(Replace(xmas, category, split.upper));
                            }
                        }
                        else
                        {
                            workflowsToProcess.Enqueue((Replace(xmas, category, split.upper), nextWorkflow));
                        }

                        xmas = Replace(xmas, category, split.lower);
                    }
                    else if (rule.Operation == Operation.LessThan)
                    {
                        (Range lower, Range upper) split = xmas[category].SplitLessThan(value);

                        if (rule.Parameters.Terminates)
                        {
                            if (rule.Parameters.NextWorkflow == "A")
                            {
                                accepted.Add(Replace(xmas, category, split.lower));
                            }
                        }
                        else
                        {
                            workflowsToProcess.Enqueue((Replace(xmas, category, split.lower), nextWorkflow));
                        }

                        xmas = Replace(xmas, category, split.upper);
                    }
                    else
                    {
                        if (rule.Parameters.Terminates)
                        {
                            if (rule.Parameters.NextWorkflow == "A")
                            {
                                accepted.Add(xmas);
                            }
                        }
                        else
                        {
                            workflowsToProcess.Enqueue((xmas, nextWorkflow));
                        }
                    }
                }
            }

            return accepted.Select(Combinations).Sum();
        }

        private long Run2(IList<string> input)
        {
            (List<Workflow> workflows, List<Dictionary<char, int>> _) = Parse(input);

            Dictionary<string, Workflow> workflowLookup = workflows.ToDictionary(wf => wf.Name, wf => wf);

            var initialParts = new Dictionary<char, Range>
            {
                { 'x', new Range(1, 4000) },
                { 'm', new Range(1, 4000) },
                { 'a', new Range(1, 4000) },
                { 's', new Range(1, 4000) }
            };

            return CountRuleCombinations(workflowLookup, initialParts);
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Run1(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();
            return Run2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(397643, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(132392981697081, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "px{a<2006:qkq,m>2090:A,rfg}",
                "pv{a>1716:R,A}",
                "lnx{m>1548:A,A}",
                "rfg{s<537:gd,x>2440:R,A}",
                "qs{s>3448:A,lnx}",
                "qkq{x<1416:A,crn}",
                "crn{x>2662:A,R}",
                "in{s<1351:px,qqz}",
                "qqz{s>2770:qs,m<1801:hdj,R}",
                "gd{a>3333:R,R}",
                "hdj{m>838:A,pv}",
                "",
                "{x=787,m=2655,a=1222,s=2876}",
                "{x=1679,m=44,a=2067,s=496}",
                "{x=2036,m=264,a=79,s=2244}",
                "{x=2461,m=1339,a=466,s=291}",
                "{x=2127,m=1623,a=2188,s=1013}"
            };

            Assert.Equal(19114, Run1(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "px{a<2006:qkq,m>2090:A,rfg}",
                "pv{a>1716:R,A}",
                "lnx{m>1548:A,A}",
                "rfg{s<537:gd,x>2440:R,A}",
                "qs{s>3448:A,lnx}",
                "qkq{x<1416:A,crn}",
                "crn{x>2662:A,R}",
                "in{s<1351:px,qqz}",
                "qqz{s>2770:qs,m<1801:hdj,R}",
                "gd{a>3333:R,R}",
                "hdj{m>838:A,pv}",
                "",
                "{x=787,m=2655,a=1222,s=2876}",
                "{x=1679,m=44,a=2067,s=496}",
                "{x=2036,m=264,a=79,s=2244}",
                "{x=2461,m=1339,a=466,s=291}",
                "{x=2127,m=1623,a=2188,s=1013}"
            };

            Assert.Equal(167409079868000, Run2(example));
        }
    }
}
