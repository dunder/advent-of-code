using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;
namespace Solutions.Event2018
{
    public class Day01 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day01;
        public string Name => "Chronal Calibration";

        public string FirstStar()
        {
            var frequencyChanges = ReadLineInput();
            var adjustedFrequency = ApplyFrequencyChanges(0, frequencyChanges);
            return adjustedFrequency.ToString();
        }

        public string SecondStar()
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
            var foundFrequencies = new HashSet<int> { currentFrequency };
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

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("513", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("287", actual);
        }
    }
}
