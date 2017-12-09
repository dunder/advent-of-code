using System.Collections.Generic;

namespace Y2017.Day6 {
    public class DebuggerMemory {
        public static (int, int) CountRedistsToSame(int[] slots) {
            int count = 0;
            int firstCycleCount = 0;
            bool cycle = true;
            HashSet<string> targets = new HashSet<string>();
            targets.Add(string.Join(" ", slots));
            while (true) {

                int maxIndex = 0;
                int max = 0;
                for (int i = 0; i < slots.Length; i++) {
                    if (slots[i] > max) {
                        max = slots[i];
                        maxIndex = i;
                    }
                }
                int distribute = slots[maxIndex];
                slots[maxIndex] = 0;
                int index = maxIndex;
                while (true) {

                    if (++index == slots.Length) {
                        index = 0;
                    }

                    slots[index] += 1;
                    distribute--;
                    
                    if (distribute == 0) {
                        count++;

                        var slotCheck = string.Join(" ", slots);
                        if (!targets.Add(slotCheck)) {
                            if (cycle) {
                                firstCycleCount = count;
                                count = 0;
                                cycle = false;
                                targets.Clear();
                                targets.Add(slotCheck);
                            }
                            else {
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