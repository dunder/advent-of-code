using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Y2017.Day13 {
    public class Firewall {
        public static int CountSeverity(string[] input) {
            Dictionary<int, int> layers = new Dictionary<int, int>();

            foreach (var line in input) {
                var depthAndRange = Regex.Split(line, @": ");
                var depth = int.Parse(depthAndRange[0]);
                var range = int.Parse(depthAndRange[1]);
                layers.Add(depth, range);
            }

            int maxDepth = layers.Keys.Max();
            int severity = 0;
            for (int t = 0; t <= maxDepth; t++) {
                if (layers.ContainsKey(t)) {
                    var scannerStepsToReturn = layers[t] * 2 - 2;
                    if (t % scannerStepsToReturn == 0) {
                        severity += t * layers[t];
                    }
                }
            }
            return severity;
        }

        public static int DelayToSafe(string[] input) {
            Dictionary<int, int> layers = new Dictionary<int, int>();

            foreach (var line in input) {
                var depthAndRange = Regex.Split(line, @": ");
                var depth = int.Parse(depthAndRange[0]);
                var range = int.Parse(depthAndRange[1]);
                layers.Add(depth, range * 2 - 2); // steps to return to starting position
            }

            var scannerMaxPosition = layers.Max(kvp => kvp.Key);

            int delay = 0;
            bool caught = true;
            while (caught) {
                caught = false;
                for (int t = 0; t < scannerMaxPosition + 1; t++) {
                    if (layers.ContainsKey(t)) {
                        if ((t + delay) % layers[t] == 0) {
                            caught = true;
                        }
                    }
                }
                if (caught) {
                    delay += 1;
                }
            }

            return delay;
        }
    }
}
