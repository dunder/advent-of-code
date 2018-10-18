using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solutions.Event2017.Day06
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day06;

        public override string FirstStar()
        {
            var input = ReadInput();
            int[] slots = Regex.Replace(input, @"\s+", " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            (int firstCycleCount, _) = DebuggerMemory.CountRedistsToSame(slots);

            return firstCycleCount.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            int[] slots = Regex.Replace(input, @"\s+", " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            (_, int count) = DebuggerMemory.CountRedistsToSame(slots);
            return count.ToString();
        }
    }

    public class DebuggerMemory
    {
        public static (int firstCycleCOunt, int count) CountRedistsToSame(int[] slots)
        {
            int count = 0;
            int firstCycleCount = 0;
            bool cycle = true;
            HashSet<string> targets = new HashSet<string>();
            targets.Add(string.Join(" ", slots));
            while (true)
            {
                int maxIndex = 0;
                int max = 0;
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i] > max)
                    {
                        max = slots[i];
                        maxIndex = i;
                    }
                }

                int distribute = slots[maxIndex];
                slots[maxIndex] = 0;
                int index = maxIndex;
                while (true)
                {
                    if (++index == slots.Length)
                    {
                        index = 0;
                    }

                    slots[index] += 1;
                    distribute--;

                    if (distribute == 0)
                    {
                        count++;

                        var slotCheck = string.Join(" ", slots);
                        if (!targets.Add(slotCheck))
                        {
                            if (cycle)
                            {
                                firstCycleCount = count;
                                count = 0;
                                cycle = false;
                                targets.Clear();
                                targets.Add(slotCheck);
                            }
                            else
                            {
                                return (firstCycleCount, count);
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}