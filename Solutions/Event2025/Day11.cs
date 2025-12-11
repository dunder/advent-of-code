using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 11: Reactor ---
    public class Day11
    {
        private readonly ITestOutputHelper output;

        public Day11(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static Dictionary<string, List<string>> Parse(IList<string> input)
        {
            Dictionary<string, List<string>> devices = [];

            foreach (var line in input)
            {
                var parts = line.Split(":");
                var device = parts[0];
                var outputs = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                devices.Add(device, outputs);
            }

            return devices;
        }

        private static int Problem1(IList<string> input)
        {
            var devices = Parse(input);

            var you = devices["you"];

            var s = new Stack<string>();
            s.Push("you");

            var d = new HashSet<string>();

            int counter = 0;

            while (s.Any())
            {
                var v = s.Pop();

                if (d.Contains(v)) { continue; }

                if (devices[v].Any(output => output == "out"))
                {
                    counter++;
                    continue;
                }

                foreach (var p in devices[v])
                {
                    s.Push(p);
                }
            }

            return counter;
        }

        private static int Count(Dictionary<string, List<string>> devices, string from, string to, Dictionary<(string, string), int> cache)
        {
            if (cache.ContainsKey((from, to)))
            {
                return cache[(from, to)];
            }

            int counter = 0;

            foreach (var output in devices[from])
            {
                if (output == to)
                {
                    counter += 1;
                }
                else if (output == "out")
                {
                    continue;
                }
                else
                {
                    int count = Count(devices, output, to, cache);
                    cache[(output, to)] = count;
                    counter += count;
                }
            }

            return counter;
        }

        private static long Problem2(IList<string> input)
        {
            Dictionary<string, List<string>> devices = Parse(input);

            int toFft = Count(devices, "svr", "fft", []);
            int toDac = Count(devices, "svr", "dac", []);
            int fftToDac = Count(devices, "fft", "dac", []);
            int dacToFft = Count(devices, "dac", "fft", []);
            int fftToOut = Count(devices, "fft", "out", []);
            int dacToOut = Count(devices, "dac", "out", []);

            return (long)toFft * fftToDac * dacToOut + (long)toDac * dacToFft * fftToOut;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(590, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(319473830844560, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(5, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example2");

            Assert.Equal(2, Problem2(exampleInput));
        }
    }
}