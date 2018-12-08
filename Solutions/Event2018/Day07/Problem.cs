using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            foreach (var line in input)
            {
                var match = expression.Match(line);
                var predecessorId = match.Groups[1].Value;
                var stepId = match.Groups[2].Value;


                if (!steps.ContainsKey(stepId))
                {
                    steps.Add(stepId, new Step(stepId));
                }

                if (!steps.ContainsKey(predecessorId))
                {
                    steps.Add(predecessorId, new Step(predecessorId));
                }

                steps[stepId].Predecessors.Add(steps[predecessorId]);
            }

            foreach (var step in steps)
            {
                foreach (var predecessor in step.Value.Predecessors)
                {
                    steps[predecessor.Id].Successors.Add(step.Value);
                }
            }

            return steps;
        }

        public static string Order(IList<string> input)
        {
            var steps = Parse(input);

            var startSteps = steps.Values.Where(s => s.IsStartStep).OrderBy(s => s.Id).ToList();

            var startStep = startSteps.First();
            startSteps.RemoveAt(0);

            var path = startStep.Id;

            // 1: keep list of current

            var successors = startSteps.Concat(startStep.Successors.ToList()).OrderBy(s => s).ToList();

            while (true)
            {
                // 2: take first (with remove) from current and add this to path

                var indexOfFirstCompleted = 0;

                for (; indexOfFirstCompleted < successors.Count; indexOfFirstCompleted++)
                {
                    var successor = successors[indexOfFirstCompleted];
                    if(successor.Predecessors.All(p => path.Contains(p.Id)))
                    {
                        break;
                    }
                }
                var currentStep = successors[indexOfFirstCompleted];
                successors.RemoveAt(indexOfFirstCompleted);
                path += currentStep.Id;

                var currentSuccessors = new HashSet<Step>(successors);

                // 3: add successors of current to list of current and sort
                successors = successors.Concat(currentStep.Successors.Where(s => !currentSuccessors.Contains(s))).OrderBy(s => s).ToList();

                // 4: if there are no successors then we are done
                if (!currentStep.Successors.Any())
                {
                    break;
                }
                // 5: goto 2

            }

            return path;
        }

        public static int TotalTime(IList<string> input, int workers, int timeOffset)
        {
            var steps = Parse(input);

            var startSteps = steps.Values.Where(s => s.IsStartStep).OrderBy(s => s.Id).ToList();

            var startStep = startSteps.First();
            startSteps.RemoveAt(0);

            var path = startStep.Id;
            var time = startStep.WorkerTime + timeOffset;

            // 1: keep list of current

            var successors = startSteps.Concat(startStep.Successors.ToList()).OrderBy(s => s).ToList();

            while (true)
            {
                // 2: take first (with remove) from current and add this to path

                var indexOfFirstCompleted = 0;

                for (; indexOfFirstCompleted < successors.Count; indexOfFirstCompleted++)
                {
                    var successor = successors[indexOfFirstCompleted];
                    if (successor.Predecessors.All(p => path.Contains(p.Id)))
                    {
                        break;
                    }
                }
                var currentStep = successors[indexOfFirstCompleted];
                successors.RemoveAt(indexOfFirstCompleted);
                path += currentStep.Id;

                var currentSuccessors = new HashSet<Step>(successors);

                // 3: add successors of current to list of current and sort
                successors = successors.Concat(currentStep.Successors.Where(s => !currentSuccessors.Contains(s))).OrderBy(s => s).ToList();

                // 4: if there are no successors then we are done
                if (!currentStep.Successors.Any())
                {
                    break;
                }
                // 5: goto 2

            }

            return 0;

        }
    }



    public class Step : IComparable<Step>
    {
        public Step(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public List<Step> Predecessors { get; } = new List<Step>();
        public List<Step> Successors { get; } = new List<Step>();
        public bool IsStartStep => !Predecessors.Any();
        public int WorkerTime => (char.Parse(Id) - 'A') + 1/*+ 60*/;

        protected bool Equals(Step other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Step) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"{Id} P:[{string.Join(",", Predecessors.Select(p => p.Id))}] S:[{string.Join(",", Successors.Select(s => s.Id))}]";
        }

        public int CompareTo(Step other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Id, other.Id, StringComparison.Ordinal);
        }
    }

}