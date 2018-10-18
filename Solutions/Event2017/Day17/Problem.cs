using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2017.Day17
{
    public class Problem : ProblemBase
    {
        private const int Input = 301;

        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day17;

        public override string FirstStar()
        {
            var result = CircularBuffer.ValueAfter(Input, 2017);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var result = CircularBuffer.ValueAfterZero(Input, 50_000_000);
            return result.ToString();
        }
    }

    public class CircularBuffer
    {
        internal static int ValueAfter(int stepLength, int insertions)
        {
            List<int> circularBuffer = new List<int> {0};

            int currentPosition = 0;

            for (int i = 1; i <= insertions; i++)
            {
                currentPosition += stepLength;
                currentPosition = currentPosition > circularBuffer.Count - 1
                    ? currentPosition % circularBuffer.Count
                    : currentPosition;
                currentPosition += 1;
                circularBuffer.Insert(currentPosition, i);
            }

            return circularBuffer.ElementAt(currentPosition + 1);
        }

        public static object ValueAfterZero(int stepLength, int insertions)
        {
            int currentPosition = 0;
            int lastInsertedAt0 = 0;
            int currentLength = 1;

            for (int i = 1; i <= insertions; i++)
            {
                currentPosition += stepLength;
                currentPosition = currentPosition > currentLength - 1
                    ? currentPosition % currentLength
                    : currentPosition;
                if (currentPosition == 0)
                {
                    lastInsertedAt0 = i;
                }

                currentPosition += 1;
                currentLength += 1;
            }

            return lastInsertedAt0;
        }
    }
}