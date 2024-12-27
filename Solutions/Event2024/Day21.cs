
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 21: Keypad Conundrum ---
    public class Day21
    {
        private readonly ITestOutputHelper output;

        public Day21(ITestOutputHelper output)
        {
            this.output = output;
        }

        static Day21()
        {
            foreach (var fromKey in numpadKeys)
            {
                foreach (var toKey in numpadKeys)
                {
                    numpadShortestPaths.Add((fromKey, toKey), CalculateShortestPath(fromKey, toKey, Keyboard.Numeric));
                }
            }

            foreach (var fromKey in dirpadKeys)
            {
                foreach (var toKey in dirpadKeys)
                {
                    dirpadShortestPaths.Add((fromKey, toKey), CalculateShortestPath(fromKey, toKey, Keyboard.Directional));
                }
            }
        }

        private static readonly Dictionary<char, (int x, int y)> numpad = new()
        {
            { '7', (0, 0) },
            { '8', (1, 0) },
            { '9', (2, 0) },
            { '4', (0, 1) },
            { '5', (1, 1) },
            { '6', (2, 1) },
            { '1', (0, 2) },
            { '2', (1, 2) },
            { '3', (2, 2) },
            { '0', (1, 3) },
            { 'A', (2, 3) },
        };

        private static readonly HashSet<char> numpadKeys = numpad.Keys.ToHashSet();
        private static readonly HashSet<(int x, int y)> numpadKeyPositions = numpad.Values.ToHashSet();
        private static readonly Dictionary<(char, char), string> numpadShortestPaths = new();

        private static readonly Dictionary<char, (int x, int y)> dirpad = new()
        {
            { '^', (1, 0) },
            { 'A', (2, 0) },
            { '<', (0, 1) },
            { 'v', (1, 1) },
            { '>', (2, 1) },
        };

        private static readonly HashSet<char> dirpadKeys = dirpad.Keys.ToHashSet();
        private static readonly HashSet<(int x, int y)> dirpadKeyPositions = dirpad.Values.ToHashSet();
        private static readonly Dictionary<(char, char), string> dirpadShortestPaths = new();

        private static HashSet<char> KeySet(Keyboard keyboard) => keyboard switch
        {
            Keyboard.Numeric => numpadKeys,
            Keyboard.Directional => dirpadKeys,
            _ => throw new ArgumentOutOfRangeException(nameof(keyboard)),
        };

        private static HashSet<(int x, int y)> KeyPositions(Keyboard keyboard) => keyboard switch
        {
            Keyboard.Numeric => numpadKeyPositions,
            Keyboard.Directional => dirpadKeyPositions,
            _ => throw new ArgumentOutOfRangeException(nameof(keyboard)),
        };

        private static (int x, int y) KeyPosition(char key, Keyboard keyboard) => keyboard switch
        {
            Keyboard.Numeric => numpad[key],
            Keyboard.Directional => dirpad[key],
            _ => throw new ArgumentOutOfRangeException(nameof(keyboard)),
        };

        private record Keypad(Keyboard Keyboard, char CurrentPosition);

        private record Position(int x, int y, char dir);


        public enum Keyboard
        {
            Numeric,
            Directional
        }

        private static string Press(string key, int times)
        {
            return string.Join("", Enumerable.Repeat(key, Math.Abs(times)));
        }

        private static string CalculateShortestPath(char fromKey, char toKey, Keyboard keyboard)
        {
            (int x, int y) start = KeyPosition(fromKey, keyboard);
            (int x, int y) end = KeyPosition(toKey, keyboard);

            string pressSequence = "";

            while (start != end)
            {
                if (end.x < start.x)
                {
                    if (KeyPositions(keyboard).Contains((end.x, start.y)))
                    {
                        pressSequence += "<";
                        start.x -= 1;
                    }
                    else
                    {
                        pressSequence += Press("^", (start.y - end.y));
                        start.y = end.y;
                    }
                }
                else if (end.y < start.y)
                {
                    pressSequence += "^";
                    start.y -= 1;
                }
                else if (end.y > start.y)
                {
                    if (KeyPositions(keyboard).Contains((start.x, end.y)))
                    {
                        pressSequence += "v";
                        start.y += 1;
                    }
                    else
                    {
                        pressSequence += Press(">", (end.x - start.x));
                        start.x = end.x;
                    }
                }
                else if (end.x > start.x)
                {
                    pressSequence += ">";
                    start.x += 1;
                }
            }

            return pressSequence + "A";
        }

        private static string ShortestPath(char fromKey, char toKey, Keyboard keyboard) => keyboard switch
        {
            Keyboard.Numeric => numpadShortestPaths[(fromKey, toKey)],
            Keyboard.Directional => dirpadShortestPaths[(fromKey, toKey)],
            _ => throw new ArgumentOutOfRangeException(nameof(keyboard)),
        };

        private static int Complexity(string code, int shortestPath)
        {
            return int.Parse(code.TrimEnd('A')) * shortestPath;
        }

        private static List<string> Sequences(string sequence, Keyboard keyboard)
        {
            List<string> keys = [];

            char fromKey = 'A';

            foreach (var toKey in sequence)
            {
                keys.Add(CalculateShortestPath(fromKey, toKey, keyboard));
                fromKey = toKey;
            }

            return keys;
        }

        private int Problem1(IList<string> input)
        {
            List<Keyboard> keyboards = [Keyboard.Numeric, Keyboard.Directional, Keyboard.Directional];

            var codes = input.ToList();

            var sequences = input.Select(code => string.Join("", Sequences(code, Keyboard.Numeric))).ToList();

            sequences = sequences.Select(code => string.Join("", Sequences(code, Keyboard.Directional))).ToList();
            sequences = sequences.Select(code => string.Join("", Sequences(code, Keyboard.Directional))).ToList();

            return codes.Select((code, i) => Complexity(code, sequences[i].Length)).Sum();
        }

        private static int Problem2(IList<string> input)
        {
            return 0;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(179444, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "029A",
                "980A",
                "179A",
                "456A",
                "379A",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(126384, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(-1, Problem2(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void ComplexityTest()
        {
            Assert.Equal(68 * 29, Complexity("029A", "<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A".Length));
        }

        [Theory]
        [InlineData('A', '9', Keyboard.Numeric, "^^^A")]
        [InlineData('A', '1', Keyboard.Numeric, "^<<A")]
        [InlineData('A', '4', Keyboard.Numeric, "^^<<A")]
        [InlineData('A', '7', Keyboard.Numeric, "^^^<<A")]
        [InlineData('7', 'A', Keyboard.Numeric, ">>vvvA")]
        [InlineData('7', '0', Keyboard.Numeric, ">vvvA")]
        [InlineData('1', '0', Keyboard.Numeric, ">vA")]
        [InlineData('A', '<', Keyboard.Directional, "^<<A")]
        [InlineData('A', '>', Keyboard.Directional, "vA")]
        [InlineData('^', '<', Keyboard.Directional, "^<A")]
        [InlineData('<', 'A', Keyboard.Directional, "^>>A")]
        [InlineData('A', 'A', Keyboard.Directional, "A")]
        [InlineData('>', '^', Keyboard.Directional, "<^A")]
        [Trait("Event", "2024")]
        public void ShortestPathTest(char fromKey, char toKey, Keyboard keyboard, string expected)
        {
            var path = CalculateShortestPath(fromKey, toKey, keyboard);

            Assert.Equal(expected, path);
        }
    }
}
