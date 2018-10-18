using System.Linq;

namespace Solutions.Event2017.Day05
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day05;

        public override string FirstStar()
        {
            var input = ReadLineInput().Select(int.Parse).ToArray();
            var result = JumpInterrupting.StepsToExit(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput().Select(int.Parse).ToArray();
            var result = JumpInterrupting.StepsToExitNew(input);
            return result.ToString();
        }
    }

    public class JumpInterrupting
    {
        public static int StepsToExit(int[] input)
        {
            return StepsToExit(input, 0);
        }

        internal static int StepsToExit(int[] input, int startPosition)
        {
            int steps = 0;
            int currentIndex = startPosition;
            while (currentIndex < input.Length && currentIndex >= 0)
            {
                var instruction = input[currentIndex];
                input[currentIndex] += 1;
                currentIndex += instruction;
                steps++;
            }

            return steps;
        }

        public static int StepsToExitNew(int[] input)
        {
            int steps = 0;
            int currentIndex = 0;
            while (currentIndex < input.Length && currentIndex >= 0)
            {
                var instruction = input[currentIndex];
                if (instruction >= 3)
                {
                    input[currentIndex] -= 1;
                }
                else
                {
                    input[currentIndex] += 1;
                }

                currentIndex += instruction;
                steps++;
            }

            return steps;
        }
    }
}