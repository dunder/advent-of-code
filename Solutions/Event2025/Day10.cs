using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 10: Phrase ---
    public class Day10
    {
        private readonly ITestOutputHelper output;

        public Day10(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Machine(List<bool> Lights, List<List<int>> Buttons, List<int> Joltage);


        private static int MinMachine(Machine machine)
        {
            var lights = Enumerable.Repeat(false, machine.Lights.Count).ToList();

            int min = int.MaxValue;

            Queue<(int button, List<bool> state, int depth)> queue = [];

            for (int button = 0; button < machine.Buttons.Count; button++)
            {
                queue.Enqueue((button, Enumerable.Repeat(false, machine.Lights.Count).ToList(), 1));
            }

            while (queue.Count > 0)
            {
                (int button, List<bool> state, int depth) next = queue.Dequeue();

                var nextState = Press(next.state, machine.Buttons[next.button]);

                if (Match(machine.Lights, nextState))
                {
                    min = Math.Min(next.depth, min);
                    break;
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

        private static List<int> JoltPress(List<int> jolts, List<int> button)
        {
            var after = new List<int>(jolts);

            foreach (int jolt in button)
            {
                after[jolt] = jolts[jolt] + 1;
            }

            return after;
        }

        private static bool JoltMatch(List<int> machine, List<int> current)
        {
            return machine.Zip(current).All(pair => pair.First == pair.Second);
        }

        private static bool JoltTryPress(List<int> jolts, List<int> button, List<int> machine)
        {
            var result = JoltPress(jolts, button);

            return result.Zip(machine).All(pair => pair.First <= pair.Second);
        }

        private static int MinJoltMachine(Machine machine)
        {
            var joltages = Enumerable.Repeat(false, machine.Joltage.Count).ToList();

            int min = int.MaxValue;

            Queue<(int button, List<int> state, int depth)> queue = [];

            for (int button = 0; button < machine.Buttons.Count; button++)
            {
                queue.Enqueue((button, Enumerable.Repeat(0, machine.Joltage.Count).ToList(), 1));
            }

            while (queue.Count > 0)
            {
                (int button, List<int> state, int depth) next = queue.Dequeue();

                List<int> nextState = JoltPress(next.state, machine.Buttons[next.button]);

                if (JoltMatch(machine.Joltage, nextState))
                {
                    min = Math.Min(next.depth, min);
                    break;
                }

                List<(int button, List<int> state, int depth)> candidates = [];

                for (int i = 0; i < machine.Buttons.Count; i++)
                {
                    if (JoltTryPress(nextState, machine.Buttons[i], machine.Joltage))
                    {
                        candidates.Add((i, nextState, next.depth + 1));
                    }
                }

                foreach (var left in candidates)
                {
                    queue.Enqueue(left);
                }
            }

            return min;
        }

        private static int Problem1(IList<string> input)
        {
            var machines = ParseMachines(input);

            var minMachines = machines.Select(MinMachine).Sum();

            return minMachines;
        }

        private static int Problem2(IList<string> input)
        {
            var machines = ParseMachines(input);

            var minMachines = machines.Select(MinJoltMachine).Sum();

            return minMachines;
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
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input));
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