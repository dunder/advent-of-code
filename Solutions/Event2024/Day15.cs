using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.Event2018.Day23;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 15: Phrase ---
    public class Day15
    {
        private readonly ITestOutputHelper output;

        public Day15(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static (char[,], (int x, int y)) ParseMap(IList<string> input)
        {
            var mapLines = input.TakeUntil(string.IsNullOrWhiteSpace).ToList();

            int maxx = mapLines.First().Length;
            int maxy = mapLines.Count - 1;

            char[,] map = new char[maxx, maxy];
            (int x, int y) robot = (0, 0);

            for (var y = 0; y < maxy; y++)
            {
                var line = mapLines[y];

                for (var x = 0; x < maxx; x++)
                {
                    var c = line[x];

                    if (c == '@')
                    {
                        robot = (x, y);
                        map[x, y] = '.';
                    }
                    else
                    {
                        map[x, y] = c;
                    }
                }
            }

            return (map, robot);
        }

        private static List<char> ParseInstructions(IList<string> input)
        {
            var result = new List<char>();

            return input.SkipUntil(string.IsNullOrWhiteSpace).SelectMany(line => line).ToList();
        }

        private static (char[,], (int x, int y), List<char>) Parse(IList<string> input)
        {
            var (map, robot) = ParseMap(input);
            var instructions = ParseInstructions(input);

            return (map, robot, instructions);
        }

        private static (int x, int y) Move((int x, int y) from, char direction) => direction switch
        {
            '^' => (from.x, from.y - 1),
            '>' => (from.x + 1, from.y),
            'v' => (from.x, from.y + 1),
            '<' => (from.x - 1, from.y),
            _ => throw new InvalidOperationException($"Cannot move '{direction}' from {from}")
        };


        private static ((int x, int y), char) Next(char[,] map, (int x, int y) from, char direction)
        {
            (int x, int y) = Move(from, direction);

            return ((x, y), map[x, y]);
        }

        private static (int x, int y) TryMove(char[,] map, (int x, int y) from, char direction)
        {
            (var pos, char c) = Next(map, from, direction);

            if (c == '.')
            {
                return pos;
            }

            if (c == '#')
            {
                return from;
            }

            (int x, int y) move = pos;

            while (c == 'O')
            {
                (move, c) = Next(map, move, direction);
            };

            if (c == '#')
            {
                return from;
            }

            (var last, c) = Next(map, move, direction);

            map[move.x, move.y] = 'O';
            map[pos.x, pos.y] = '.';

            return pos;
        }

        private string Print(char[,] map, (int x, int y) robot)
        {
            int maxy = map.GetLength(1);
            int maxx = map.GetLength(0);

            StringBuilder s = new StringBuilder();

            for (var y = 0; y < maxy; y++)
            {
                for (var x = 0; x < maxx; x++)
                {
                    if ((x, y) == (robot.x, robot.y))
                    {
                        s.Append('@');
                    }else
                    {
                        s.Append(map[x, y]);
                    }
                }
                s.AppendLine();
            }

            output.WriteLine(s.ToString());
            output.WriteLine("");

            return s.ToString();
        }

        private long Problem1(IList<string> input)
        {
            (char[,] map, (int x, int y) robot, List<char> instructions) = Parse(input);

            foreach (var instruction in instructions)
            {
                robot = TryMove(map, robot, instruction);
                //var s = Print(map, robot);
            }

            int maxy = map.GetLength(1);
            int maxx = map.GetLength(0);

            long sum = 0;

            for (var y = 0; y < maxy; y++)
            {
                for (var x = 0; x < maxx; x++)
                {
                    if (map[x,y] == 'O')
                    {
                        sum += x + 100 * y;
                    }
                }
            }

            return sum;
        }

        private char[,] SecondWarehouse(char[,] warehouse)
        {
            var width = 2 * warehouse.GetLength(0);
            var height = warehouse.GetLength(1);
            
            char[,] second = new char[width, height];

            for (var y = 0; y < warehouse.GetLength(1); y++)
            {
                for (var x = 0; x < warehouse.GetLength(0); x++)
                {
                    switch(warehouse[x, y])
                    {
                        case '#':
                            second[x * 2, y] = '#';
                            second[x * 2 + 1, y] = '#';
                            break;
                        case 'O':
                            second[x * 2, y] = '[';
                            second[x * 2 + 1, y] = ']';
                            break;
                        case '.':
                            second[x * 2, y] = '.';
                            second[x * 2 + 1, y] = '.';
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"Unknown map characer at {x}, {y}: {warehouse[x,y]}");
                    }
                }
            }

            return second;
        }

        private static (int x, int y) TryMove2(char[,] map, (int x, int y) from, char direction)
        {
            switch (direction)
            {
                case '^':
                    return TryMoveUp(map, from, direction);
                case '>':
                    return TryMoveHorizontal(map, from, direction);
                case 'v':
                    return TryMoveDown(map, from, direction);
                case '<':
                    return TryMoveHorizontal(map, from, direction);
                default:
                    throw new ArgumentOutOfRangeException($"Invalid move: {direction}");
            }
        }

        private static (bool, List<(int x, int y)>) MarkUp(char[,] map, (int x, int y) from, char direction, List<(int x, int y)> move)
        {
            char fromc = map[from.x, from.y];
            (var pos, char c) = Next(map, from, direction);

            switch (c)
            {
                case '#':
                    return (false, move);
                case '[':
                    {
                        if (fromc == c)
                        {
                            move.Add(pos);
                            return MarkUp(map, pos, direction, move);

                        }
                        else
                        {
                            move.Add(pos);
                            move.Add((pos.x + 1, pos.y));

                            (bool lmove, List<(int x, int y)> lmoves) = MarkUp(map, pos, direction, move);
                            (bool rmove, List<(int x, int y)> rmoves) = MarkUp(map, (pos.x + 1, pos.y), direction, move);

                            return (lmove && rmove, lmoves.Concat(rmoves).ToList());
                        }
                    }
                case ']':
                    {
                        if (fromc == c)
                        {
                            move.Add(pos);
                            return MarkUp(map, pos, direction, move);
                        }
                        else
                        {
                            move.Add(pos);
                            move.Add((pos.x - 1, pos.y));

                            (bool rmove, List<(int x, int y)> rmoves) = MarkUp(map, pos, direction, move);
                            (bool lmove, List<(int x, int y)> lmoves) = MarkUp(map, (pos.x - 1, pos.y), direction, move);

                            return (lmove && rmove, lmoves.Concat(rmoves).ToList());
                        }
                    }
                case '.':
                    return (true, move);
            }

            throw new InvalidOperationException("Undetermined move");
        }

        private static (int x, int y) TryMoveUp(char[,] map, (int x, int y) from, char direction)
        {
            (var moveTo, char _) = Next(map, from, direction);

            (bool move, List<(int x, int y)> boxes) = MarkUp(map, from, direction, new List<(int x, int y)>());

            if (move)
            {
                boxes = boxes.Distinct().OrderBy(b => b.y).ToList();

                foreach ((int x, int y) part in boxes)
                {
                    char p = map[part.x, part.y];

                    (var pos, char c) = Next(map, part, direction);

                    map[part.x, part.y] = '.';
                    map[pos.x, pos.y] = p;
                }

                return moveTo;
            }

            return from;
        }

        private static (int x, int y) TryMoveDown(char[,] map, (int x, int y) from, char direction)
        {
            (var moveTo, char _) = Next(map, from, direction);

            (bool move, List<(int x, int y)> boxes) = MarkUp(map, from, direction, new List<(int x, int y)>());

            if (move)
            {
                boxes = boxes.Distinct().OrderByDescending(b => b.y).ToList();

                foreach ((int x, int y) part in boxes)
                {
                    char p = map[part.x, part.y];

                    (var pos, char c) = Next(map, part, direction);

                    map[part.x, part.y] = '.';
                    map[pos.x, pos.y] = p;
                }

                return moveTo;
            }

            return from;
        }

        private static (int x, int y) TryMoveHorizontal(char[,] map, (int x, int y) from, char direction)
        {

            (var pos, char c) = Next(map, from, direction);

            if (c == '.')
            {
                return pos;
            }

            if (c == '#')
            {
                return from;
            }

            (int x, int y) move = pos;

            while (c == '[' || c == ']')
            {
                (move, c) = Next(map, move, direction);
            };

            if (c == '#')
            {
                return from;
            }

            var xstart = direction == '<' ? move.x : pos.x + 1;
            var xstop = direction == '<' ? pos.x : move.x + 1;
            var y = from.y;

            for (int x = xstart; x < xstop; x = x + 2)
            {
                map[x, y] = '[';
                map[x + 1, y] = ']';
            }

            map[pos.x, pos.y] = '.';

            return pos;
        }

        private long Problem2(IList<string> input)
        {
            (char[,] map, (int x, int y) robot, List<char> instructions) = Parse(input);

            map = SecondWarehouse(map);

            robot = (robot.x * 2, robot.y);
            //Print(map, robot);

            foreach (var instruction in instructions)
            {
                robot = TryMove2(map, robot, instruction);
                //var s = Print(map, robot);
            }

            int maxy = map.GetLength(1);
            int maxx = map.GetLength(0);

            long sum = 0;

            for (var y = 0; y < maxy; y++)
            {
                for (var x = 0; x < maxx; x++)
                {
                    if (map[x, y] == '[')
                    {
                        sum += x + 100 * y;
                    }
                }
            }

            return sum;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1463512, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1486520, Problem2(input));
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "########",
                "#..O.O.#",
                "##@.O..#",
                "#...O..#",
                "#.#.O..#",
                "#...O..#",
                "#......#",
                "########",
                "",
                "<^^>>>vv<v>>v<<",
            ];

        private List<string> exampleInputLarge =
            [
                "##########",
                "#..O..O.O#",
                "#......O.#",
                "#.OO..O.O#",
                "#..O@..O.#",
                "#O#..O...#",
                "#O..O..O.#",
                "#.OO.O.OO#",
                "#....O...#",
                "##########",
                "",
                "<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^",
                "vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v",
                "><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<",
                "<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^",
                "^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><",
                "^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^",
                ">^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^",
                "<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>",
                "^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>",
                "v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(2028, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarLargeExample()
        {
            Assert.Equal(10092, Problem1(exampleInputLarge));
        }

        private List<string> example2Input1 =
        [
            "#######",
            "#...#.#",
            "#.....#",
            "#..OO@#",
            "#..O..#",
            "#.....#",
            "#######",
            "",
            "<vv<<^^<<^^",
        ];

        private List<string> example2Input2 =
        [
            "#######",
            "#...#.#",
            "#.....#",
            "#.@OO.#",
            "#..O..#",
            "#.....#",
            "#######",
            "",
            ">>>>>>>",
        ];

        private List<string> example2Input3 =
        [
            "#######",
            "#...#.#",
            "#.....#",
            "#..OO@#",
            "#..O..#",
            "#.....#",
            "#######",
            "",
            "<<<<<<<<",
        ];

        private List<string> example2Input4 =
        [
            "#######",
            "#...#.#",
            "#.....#",
            "#..OO.#",
            "#...O.#",
            "#...@.#",
            "#######",
            "",
            "^^^^^^^^^"
        ];

        private List<string> example2Input5 =
        [
            "#######",
            "#...#.#",
            "#...@.#",
            "#..OO.#",
            "#...O.#",
            "#.....#",
            "#OOOOO#",
            "#.....#",
            "#######",
            "",
            "vvvvv"
        ];

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample1()
        {
            Assert.Equal(-1, Problem2(example2Input1));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample2()
        {
            Assert.Equal(-1, Problem2(example2Input2));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample3()
        {
            Assert.Equal(-1, Problem2(example2Input3));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample4()
        {
            Assert.Equal(-1, Problem2(example2Input4));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample5()
        {
            Assert.Equal(-1, Problem2(example2Input5));
        }
    }
}
