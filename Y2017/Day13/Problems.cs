using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day13 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day13\input.txt");

            var result = Firewall.CountSeverity(input);

            Assert.Equal(2160, result);
            _output.WriteLine($"Day 13 problem 1: {result}");
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day13\input.txt");

            var result = Firewall.DelayToSafe(input);

            Assert.Equal(3907470, result);
            _output.WriteLine($"Day 13 problem 2: {result}");  // 303966 och 303870 är för lågt
        }

        public class Firewall {
            public static int CountSeverity(string[] input) {
                Dictionary<int, int> layers = new Dictionary<int, int>();
                

                foreach (var line in input) {
                    var depthAndRange = Regex.Split(line, @": ");
                    var depth = int.Parse(depthAndRange[0]);
                    var range = int.Parse(depthAndRange[1]);
                    layers.Add(depth, range - 1);
                }

                Dictionary<int, int> scanners = layers.ToDictionary(l => l.Key, l => 0);
                Dictionary<int, bool> scannerDirections = layers.ToDictionary(l => l.Key, l => true);
                int maxDepth = layers.Keys.Max();
                int severity = 0;
                for (int t = 0; t <= maxDepth; t++) {
                    if (scanners.ContainsKey(t)) {
                        var caught = scanners[t] == 0 ? 1 : 0;
                        var depth = t;
                        var range = layers[t] + 1;
                        severity += caught * depth * range;
                    }

                    foreach (var scanner in layers.Keys) {
                        var depth = layers[scanner];
                        
                       // move
                        if (scannerDirections[scanner]) { 
                            scanners[scanner] += 1;
                        }
                        else {
                            scanners[scanner] -= 1;
                        }

                        // change directions at ends
                        if (scanners[scanner] == depth) {
                            scannerDirections[scanner] = false;
                        }
                        if (scanners[scanner] == 0) {
                            scannerDirections[scanner] = true;
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
}
