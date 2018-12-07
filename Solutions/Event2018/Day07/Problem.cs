using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Tree;

namespace Solutions.Event2018.Day07
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day07;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var order = Order(input);
            return order;
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public static Dictionary<string, Step> Parse(IList<string> input)
        {
            var expression = new Regex(@"Step (\w) must be finished before step (\w) can begin.");
            var steps = new Dictionary<string, Step>();
            var allPredecessors = new HashSet<string>();
            foreach (var line in input)
            {
                var match = expression.Match(line);
                var predecessorId = match.Groups[1].Value;
                var stepId = match.Groups[2].Value;

                allPredecessors.Add(predecessorId);

                if (!steps.ContainsKey(stepId))
                {
                    steps.Add(stepId, new Step(stepId));
                }

                if (!steps.ContainsKey(predecessorId))
                {
                    steps.Add(predecessorId, new Step(predecessorId));
                }

                steps[stepId].Predecessors.Add(predecessorId);
            }

            foreach (var step in steps)
            {
                foreach (var predecessor in step.Value.Predecessors)
                {
                    steps[predecessor].Successors.Add(step.Key);
                }
            }

            return steps;
        }

        public static string Order(IList<string> input)
        {
            var steps = Parse(input);

            var startStep = steps.Values.Single(s => !s.Predecessors.Any());

            var path = startStep.Id;

            // 1: keep list of current

            // 2: take first (with remove) from current and add this to path

            // 3: add successors of current to list of current and sort

            // 4: if there are no successors then we are done

            // 5: goto 2
                 

            return string.Join("", path);
        }

        public static Step FindStartNode(Dictionary<string,Step> steps, HashSet<string> allPredecessors)
        {
            return steps.Values.Single(s => !s.Predecessors.Any());
        }

    }
    public class Step
    {
        public Step(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public List<string> Predecessors { get; } = new List<string>();
        public List<string> Successors { get; } = new List<string>();

        public override string ToString()
        {
            return $"{Id} P:[{string.Join(",", Predecessors)}] S:[{string.Join(",", Successors)}]";
        }
    }

}