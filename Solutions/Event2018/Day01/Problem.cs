using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2018.Day01
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day01;

        public override string FirstStar()
        {
            var frequencyChanges = ReadLineInput();
            var adjustedFrequency = ApplyFrequencyChanges(0, frequencyChanges);
            return adjustedFrequency.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = ApplyFrequencyChangesUntilFoundTwice(0, input);
            return result.ToString();
        }

        public static int ApplyFrequencyChanges(int initialFrequency, IList<string> changes)
        {
            return changes.Aggregate(initialFrequency, (current, input) => current + int.Parse(input));
        }

        public static int ApplyFrequencyChangesUntilFoundTwice(int initialFrequency, IList<string> input)
        {
            int currentFrequency = initialFrequency;
            var foundFrequencies = new HashSet<int> {currentFrequency};
            bool foundTwice = false;
            while (!foundTwice)
            {
                foreach (string line in input)
                {
                    var frequencyAdjustment = int.Parse(line);
                    currentFrequency += frequencyAdjustment;
                    if (foundFrequencies.Contains(currentFrequency))
                    {
                        foundTwice = true;
                        break;
                    }

                    foundFrequencies.Add(currentFrequency);
                }
            }

            return currentFrequency;
        }
    }
}