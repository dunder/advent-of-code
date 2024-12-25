using Newtonsoft.Json.Linq;
using Solutions.Event2016.Day02;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 21: Phrase ---
    public class Day21
    {
        private readonly ITestOutputHelper output;

        public Day21(ITestOutputHelper output)
        {
            this.output = output;
        }

        static Day21()
        {
            foreach (var fromKey in KeyPositions(Keyboard.Numeric).Keys)
            {
                foreach (var toKey in KeyPositions(Keyboard.Numeric).Keys)
                {
                    if (fromKey == toKey)
                    {
                        numpadShortestPaths.Add((fromKey, toKey), ["A"]);
                    }
                    else
                    {
                        (Dictionary<Position, int> dist, Dictionary<Position, List<Position>> prev) = DjikstraShortestPath(fromKey, Keyboard.Numeric);

                        var shortestPaths = ConstructPaths(KeyPositions(Keyboard.Numeric)[fromKey], KeyPositions(Keyboard.Numeric)[toKey], dist, prev);
                        //List<string> shortestPaths = [];

                        numpadShortestPaths.Add((fromKey, toKey), shortestPaths);
                    }
                }
            }


            foreach (var fromKey in KeyPositions(Keyboard.Directional).Keys)
            {
                foreach (var toKey in KeyPositions(Keyboard.Directional).Keys)
                {
                    if (fromKey == toKey)
                    {
                        dirpadShortestPaths.Add((fromKey, toKey), ["A"]);
                    }
                    else
                    {
                        (Dictionary<Position, int> dist, Dictionary<Position, List<Position>> prev) = DjikstraShortestPath(fromKey, Keyboard.Directional);

                        var shortestPaths = ConstructPaths(KeyPositions(Keyboard.Directional)[fromKey], KeyPositions(Keyboard.Directional)[toKey], dist, prev);
                        //List<string> shortestPaths = [];

                        dirpadShortestPaths.Add((fromKey, toKey), shortestPaths);
                    }
                }
            }
        }

        // positive x goes left, positive y goes up
        private static readonly Dictionary<char, (int x, int y)> numpad = new()
            {
                { 'A', (0, 0) },
                { '0', (1, 0) },
                { '1', (2, 1) },
                { '2', (1, 1) },
                { '3', (0, 1) },
                { '4', (2, 2) },
                { '5', (1, 2) },
                { '6', (0, 2) },
                { '7', (2, 3) },
                { '8', (1, 3) },
                { '9', (0, 3) },
            };

        private static readonly HashSet<(int x, int y)> numpadKeySet = numpad.Values.ToHashSet();

        private static readonly Dictionary<char, (int x, int y)> dirpad = new()
            {
                { 'A', (0, 0) },
                { '^', (1, 0) },
                { '>', (0, -1) },
                { 'v', (1, -1) },
                { '<', (2, -1) },
            };

        private static readonly HashSet<(int x, int y)> dirpadKeySet = dirpad.Values.ToHashSet();

        private static HashSet<(int x, int y)> KeySet(Keyboard keyboard) => keyboard switch
        {
            Keyboard.Numeric => numpadKeySet,
            Keyboard.Directional => dirpadKeySet,
            _ => throw new ArgumentOutOfRangeException(nameof(keyboard)),
        };

        private static Dictionary<char, (int x, int y)> KeyPositions(Keyboard keyboard) => keyboard switch
        {
            Keyboard.Numeric => numpad,
            Keyboard.Directional => dirpad,
            _ => throw new ArgumentOutOfRangeException(nameof(keyboard)),
        };

        private record Keypad(Keyboard Keyboard)
        {
            public char CurrentPosition { get; set; }
        }

        private record Position(int x, int y, char dir);

        private static readonly Dictionary<(char, char), List<string>> numpadShortestPaths = new();
        private static readonly Dictionary<(char, char), List<string>> dirpadShortestPaths = new();

        private enum Keyboard
        {
            Numeric,
            Directional
        }

        public static int ManhattanDistance((int x, int y) point, (int x, int y) toPoint)
        {
            return Math.Abs(point.x - toPoint.x) + Math.Abs(point.y - toPoint.y);
        }

        private static int Complexity(string code, int shortestPath)
        {
            return int.Parse(code.TrimEnd('A')) * shortestPath;
        }

        private static string Punch(string key, int times)
        {
            return string.Join("", Enumerable.Repeat(key, Math.Abs(times)));
        }

        //private static string EnterCode(string inputKeyPresses, Dictionary<int, Keypad> keypads, int keypadIndex)
        //{
        //    if (!keypads.ContainsKey(keypadIndex))
        //    {
        //        return inputKeyPresses;
        //    }
        //    string outputKeyPresses = "";

        //    Keypad keypad = keypads[keypadIndex];

        //    foreach (var key in inputKeyPresses)
        //    {
        //        var keyFrom = keypad.CurrentPosition;
        //        (int x, int y) numFrom = keypad.Keys[keyFrom];

        //        keypad.CurrentPosition = key;

        //        (int x, int y) numTo = keypad.Keys[key];

        //        if (keypadIndex == 0)
        //        {
        //            char[] left = ['1', '4', '7'];
        //            char[] bottom = ['0', 'A'];

        //            if (left.Contains(keyFrom) && bottom.Contains(key))
        //            {
        //                outputKeyPresses += Punch(">", 1);
        //                numFrom = (numFrom.x - 1, numFrom.y);
        //            }
        //            else if (bottom.Contains(keyFrom) && left.Contains(key))
        //            {
        //                outputKeyPresses += Punch("^", 1);
        //                numFrom = (numFrom.x, numFrom.y + 1);
        //            }
        //        }
        //        else
        //        {
        //            char[] left = ['<'];
        //            char[] top = ['^', 'A'];

        //            if (left.Contains(keyFrom) && top.Contains(key))
        //            {
        //                outputKeyPresses += Punch(">", 1);
        //                numFrom = (numFrom.x - 1, numFrom.y);
        //            }
        //            else if (top.Contains(keyFrom) && left.Contains(key))
        //            {
        //                outputKeyPresses += Punch("v", 1);
        //                numFrom = (numFrom.x, numFrom.y - 1);
        //            }
        //        }

        //        var xdiff = (numTo.x - numFrom.x);
        //        var ydiff = (numTo.y - numFrom.y);

        //        if (xdiff > 0)
        //        {
        //            outputKeyPresses += Punch("<", xdiff);
        //        }
        //        else
        //        {
        //            outputKeyPresses += Punch(">", xdiff);
        //        }

        //        if (ydiff > 0)
        //        {
        //            outputKeyPresses += Punch("^", ydiff);
        //        }
        //        else
        //        {
        //            outputKeyPresses += Punch("v", ydiff);
        //        }

        //        outputKeyPresses += "A";
        //    }

        //    return EnterCode(outputKeyPresses, keypads, keypadIndex + 1);
        //}


        private static Position Move(int x, int y, int d) => d switch
        {
            0 => new Position(x, y + 1, '^'),
            1 => new Position(x - 1, y, '>'),
            2 => new Position(x, y - 1, 'v'),
            3 => new Position(x + 1, y, '<'),
            _ => throw new InvalidOperationException()
        };

        private static List<Position> Neighbors(Keyboard keyboard, Position position)
        {
            (int x, int y, _) = position;

            List<Position> neighbors = [Move(x, y, 0), Move(x, y, 1), Move(x, y, 2), Move(x, y, 3)];

            var valid = KeySet(keyboard);

            var ns = neighbors.Where(neighbor => valid.Contains((neighbor.x, neighbor.y))).ToList();

            return ns;
        }

        private static (Dictionary<Position, int> dist, Dictionary<Position, List<Position>> prev) DjikstraShortestPath(char fromKey, Keyboard keyboard)
        {
            var queue = new PriorityQueue<Position, int>();

            var startPosition = KeyPositions(keyboard)[fromKey];

            var startNode = new Position(startPosition.x, startPosition.y, 'x');

            queue.Enqueue(startNode, 0);

            var distances = new Dictionary<Position, int>
            {
                { startNode, 0 }
            };

            var prev = new Dictionary<Position, List<Position>>();

            int Distance(Position position)
            {
                if (distances.TryGetValue(position, out int distance))
                {
                    return distances[position];
                }
                else
                {
                    return int.MaxValue;
                }
            }

            // djikstra https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var neighbor in Neighbors(keyboard, current))
                {
                    (int x, int y, char dir) = neighbor;

                    var alt = Distance(current) + 1;

                    if (alt == Distance(neighbor))
                    {
                        prev[neighbor].Add(current);
                    }

                    if (alt < Distance(neighbor))
                    {
                        prev[neighbor] = [current];
                        distances[neighbor] = alt;
                        queue.Enqueue(neighbor, alt);
                    }
                }
            }

            return (distances, prev);
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private static List<string> ConstructPaths((int x, int y) start, (int x, int y) end, Dictionary<Position, int> distances, Dictionary<Position, List<Position>> prev)
        {
            var min = distances.Where(d => d.Key.x == end.x && d.Key.y == end.y).Select(d => d.Value).Min();
            var best = distances.Where(d => d.Key.x == end.x && d.Key.y == end.y && d.Value == min).Select(d => d.Key).ToList();

            void FindKeyStrokes(List<string> paths, string path, Position currentNode)
            {
                if ((currentNode.x, currentNode.y) == start)
                {
                    paths.Add(Reverse(path) + "A");
                    return;
                }

                foreach (var parent in prev[currentNode])
                {
                    FindKeyStrokes(paths, path + currentNode.dir, parent);
                }
            }

            List<string> keypaths = new();

            foreach (var b in best)
            {
                FindKeyStrokes(keypaths, "", b);
            }

            return keypaths;
        }

        private static Dictionary<(char, char), List<string>> ShortestPaths(Keyboard keyboard)
        {
            return keyboard == Keyboard.Numeric ? numpadShortestPaths : dirpadShortestPaths;
        }

        private static void CombineCodes(string code, List<List<string>> parts, List<string> codes)
        {
            if (parts.Count == 0)
            {
                codes.Add(code);
                return;
            }

            foreach (string alternative in parts.First())
            {
                CombineCodes(code + alternative, parts.Skip(1).ToList(), codes);
            }
        }

        private static void EnterCode(string code, string path, List<List<string>> paths, Dictionary<int, Keypad> keypads, int keypadIndex)
        {
            Keypad keypad = keypads[keypadIndex];
            Keyboard keyboard = keypadIndex == 0 ? Keyboard.Numeric : Keyboard.Directional;

            List<List<string>> codes = new ();

            foreach (var toKey in code)
            {
                char fromKey = keypad.CurrentPosition;
                var shortestPaths = ShortestPaths(keyboard)[(fromKey, toKey)];
                keypad.CurrentPosition = toKey;

                codes.Add(shortestPaths);
            }

            List<string> combinations = new();

            CombineCodes("", codes, combinations);

            if (keypadIndex == keypads.Keys.Max())
            {
                paths.Add(combinations);
            }
            else
            {
                foreach (string c in combinations)
                {
                    EnterCode(c, "", paths, keypads, keypadIndex + 1);
                }
            }
        }

        private int Problem1(IList<string> input)
        {
            var totalComplexity = 0;

            Dictionary<int, Keypad> keypads = new()
                {
                    { 0, new Keypad(Keyboard.Numeric) { CurrentPosition = 'A' } },
                    { 1, new Keypad(Keyboard.Directional) { CurrentPosition = 'A' } },
                    { 2, new Keypad(Keyboard.Directional) { CurrentPosition = 'A' } },
                };

            foreach (var code in input)
            {
                List<List<string>> paths = new();
                EnterCode(code, "", paths, keypads, 0);

                int min = paths.SelectMany(p => p).Select(s => s.Length).Min();

                totalComplexity += Complexity(code, min);
            }

            return totalComplexity;
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

            Assert.Equal(-1, Problem1(input));
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
            Assert.Equal(-1, Problem1(exampleInput));
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

        [Fact]
        [Trait("Event", "2024")]
        public void CombineCodesTest()
        {
            List<string> codes = [];

            CombineCodes("", [["A", "B"], ["C"], ["D", "E"]], codes);

            Assert.Equal(4, codes.Count);
            Assert.Equal("ACD", codes[0]);
            Assert.Equal("ACE", codes[1]);
            Assert.Equal("BCD", codes[2]);
            Assert.Equal("BCE", codes[3]);
        }
    }
}
