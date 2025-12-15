using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 10: Factory ---
    public class Day10
    {
        private readonly ITestOutputHelper output;

        public Day10(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Machine(List<bool> Lights, List<List<int>> Buttons, List<int> Joltage)
        {
            public Machine LightsFromJoltageParity => this with
            {
                Lights = Joltage.Select(i => i % 2 != 0).ToList()
            };

            public Machine Press(List<int> button)
            {
                var after = new List<int>(Joltage);

                foreach (int jolt in button)
                {
                    after[jolt] = Joltage[jolt] - 1;
                }

                return this with { Joltage = after };
            }

            public Machine ReduceJoltageByHalf => this with { Joltage = Joltage.Select(j => j / 2).ToList() };

            public Machine JoltageReduceFromLights => this with
            {
                Joltage = Joltage.Zip(Lights).Select(p => p.Second ? p.First - 1 : p.First).ToList()
            };

            public Machine VectorizeButtons => this with
            {
                Buttons = Buttons.Select(button => Joltage.Select((_, i) => button.Contains(i) ? 1 : 0).ToList()).ToList()
            };
        }


        private static int MinMachine(Machine machine)
        {
            var lights = Enumerable.Repeat(false, machine.Lights.Count).ToList();

            int min = int.MaxValue;

            Queue<(int button, List<bool> state, int depth)> queue = [];

            for (int button = 0; button < machine.Buttons.Count; button++)
            {
                queue.Enqueue((button, Enumerable.Repeat(false, machine.Lights.Count).ToList(), 1));
            }

            HashSet<string> visited = [];

            while (queue.Count > 0)
            {
                (int button, List<bool> state, int depth) next = queue.Dequeue();

                List<bool> nextState = Press(next.state, machine.Buttons[next.button]);

                if (Match(machine.Lights, nextState))
                {
                    min = Math.Min(next.depth, min);
                    break;
                }

                if (!visited.Add(new string(nextState.Select(i => i ? '#' : '.').ToArray())))
                {
                    continue;
                }

                List<(int button, List<bool> state, int depth)> candidates = [];

                for (int i = 0; i < machine.Buttons.Count; i++)
                {
                    candidates.Add((i, nextState, next.depth + 1));
                }

                foreach (var left in candidates)
                {
                    queue.Enqueue(left);
                }
            }

            return min;
        }

        private static List<bool> Press(List<bool> lights, List<int> button)
        {
            var after = new List<bool>(lights);

            foreach (int light in button)
            {
                after[light] = !lights[light];
            }

            return after;
        }

        private static bool Match(List<bool> machine, List<bool> current)
        {
            return machine.Zip(current).All(pair => pair.First == pair.Second);
        }

        private static List<Machine> ParseMachines(IList<string> input)
        {
            return input.Select(line =>
            {
                var parts = line.Split(" ");

                var lightPart = parts[0][1..^1];
                var buttonParts = parts[1..^1];
                var joltagePart = parts[parts.Length - 1];

                var lights = lightPart.Select(c => c == '#').ToList();

                List<List<int>> buttons = buttonParts.Select(part => part[1..^1].Split(",").Select(int.Parse).ToList()).ToList();

                List<int> joltage = joltagePart[1..^1].Split(",").Select(int.Parse).ToList();

                return new Machine(lights, buttons, joltage);

            }).ToList();
        }

        public sealed class IntListComparer : IEqualityComparer<List<int>>
        {
            public static readonly IntListComparer Instance = new();

            public bool Equals(List<int> x, List<int> y)
            {
                if (ReferenceEquals(x, y)) { return true; }
                if (x is null || y is null) { return false; }
                if (x.Count != y.Count) { return false; }
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i] != y[i]) { return false; }
                }
                return true;
            }

            public int GetHashCode(List<int> obj)
            {
                var hc = new HashCode();
                hc.Add(obj.Count);
                foreach (var v in obj)
                {
                    hc.Add(v);
                }
                return hc.ToHashCode();
            }
        }

        private static IEnumerable<List<int>> BinaryCombinations(int n)
        {
            var seed = new List<List<int>> { new List<int>() };

            for (int i = 0; i < n; i++)
            {
                seed = [.. seed.SelectMany(prefix => new[] { 0, 1 }.Select(bit => prefix.Concat([bit]).ToList()))];
            }

            return seed;
        }

        private static int Problem1(IList<string> input)
        {
            var machines = ParseMachines(input);

            var minMachines = machines.Select(MinMachine).Sum();

            return minMachines;
        }

        private static int Presses(List<int> target, Dictionary<List<int>, List<int>> joltStates, Dictionary<List<int>, List<List<int>>> patterns, Dictionary<List<int>, int> cache)
        {
            if (cache.ContainsKey(target))
            {
                return cache[target];
            }

            if (target.All(j => j == 0))
            {
                return 0;
            }

            if (target.Any(j => j < 0))
            {
                return int.MaxValue;
            }

            List<int> lights = target.Select(j => j % 2).ToList();

            int total = int.MaxValue;

            List<List<int>> pressedForLights = patterns.ContainsKey(lights) ? patterns[lights] : [];

            foreach (var pressed in pressedForLights)
            {
                List<int> joltState = joltStates[pressed];

                List<int> newTarget = joltState.Zip(target).Select(p => (p.Second - p.First) / 2).ToList();

                int presses = Presses(newTarget, joltStates, patterns, cache);

                if (presses != int.MaxValue)
                {
                    cache.TryAdd(newTarget, presses);
                    total = Math.Min(total, pressed.Sum() + 2 * presses);
                }
            }

            return total;
        }

        // followed solutions by tenthmascot and olamberti
        // https://old.reddit.com/r/adventofcode/comments/1pity70/2025_day_10_solutions/
        private static long Problem2(IList<string> input)
        {
            var machines = ParseMachines(input);

            int totalPressCount = 0;

            foreach (var machine in machines)
            {
                // this first part is basically pre calculating all combinations of pressing one button (or
                // not pressing it) as needed for solving part 1 (where I used another solution method)

                var joltStates = new Dictionary<List<int>, List<int>>(IntListComparer.Instance);
                var patterns = new Dictionary<List<int>, List<List<int>>>(IntListComparer.Instance);

                foreach (List<int> pressed in BinaryCombinations(machine.Buttons.Count))
                {
                    List<int> jolts = Enumerable.Repeat(0, machine.Joltage.Count).ToList();

                    for (int i = 0; i < pressed.Count; i++)
                    {
                        int p = pressed[i];

                        foreach (int j in machine.Buttons[i])
                        {
                            jolts[j] += p;
                        }
                    }

                    List<int> lights = jolts.Select(j => j % 2).ToList();

                    joltStates[pressed] = jolts;

                    if (!patterns.ContainsKey(lights))
                    {
                        patterns.Add(lights, new List<List<int>>());
                    }

                    patterns[lights].Add(pressed);
                }

                // with the precalculated values we now recursively solve the problem of lettting the jolt
                // levels be our target but se the target as just reducing the levels to an even number
                // which is basically the same as turning on and off lights, ie consider the even/odd jolt
                // numbers be our target light. When a target state is reached the numbers can be divided in
                // half (reduce the problem) and count the number of presses as the number of buttons pressed
                // + the 2 * the number of presses to solve the rest

                Dictionary<List<int>, int> cache = new Dictionary<List<int>, int>(IntListComparer.Instance);

                totalPressCount += Presses(machine.Joltage, joltStates, patterns, cache);
            }

            return totalPressCount;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(375, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            // 7983 (too low)
            var input = ReadLineInput();

            Assert.Equal(15377, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(7, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void MatchTest()
        {
            Assert.True(Match([true, true, true, true], [true, true, true, true]));
            Assert.False(Match([true, true, true, true], [false, true, true, true]));
            Assert.False(Match([true, true, true, true], [true, false, false, true]));
            Assert.False(Match([true, true, true, true], [true, true, true, false]));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(33, Problem2(exampleInput));
        }
    }
}