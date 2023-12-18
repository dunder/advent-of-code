using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.Event2018.Day24;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day X: Phrase ---
    public class Day18
    {
        private readonly ITestOutputHelper output;

        public Day18(ITestOutputHelper output)
        {
            this.output = output;
        }

        private enum Direction { Up, Right, Down, Left }

        private record Draw(Direction Direction, int Steps, string Color); 

        private List<Draw> Parse(IList<string> input)
        {
            return input.Select(line =>
            {
                var parts = line.Split(" ");
                var directionPart = parts[0];
                var steps = int.Parse(parts[1]);
                var color = parts[2];

                var direction = directionPart switch
                {
                    "U" => Direction.Up,
                    "R" => Direction.Right,
                    "D" => Direction.Down,
                    "L" => Direction.Left,
                    _ => throw new InvalidOperationException($"Unkown direction : {directionPart}")
                };

                return new Draw(direction, steps, color);
            }).ToList();
        }

        private (int,int) Move((int, int) position, Draw draw)
        {
            (int x, int y) = position;

            switch(draw.Direction)
            {
                case Direction.Up:
                    return (x, y - 1);
                case Direction.Right:
                    return (x + 1, y);
                case Direction.Down:
                    return (x, y + 1);
                case Direction.Left:
                    return (x - 1, y);
                default:
                    throw new InvalidOperationException($"Unknown direction: {draw.Direction}");
            }
        }

        private int DugOut(IList<string> input)
        {
            var digPlan = Parse(input);


            var position = (0, 0);

            HashSet<(int, int)> visited = new()
            {
                position
            };

            Dictionary<int, HashSet<int>> blocks = new();

            void AddBlock(int x, int y)
            {
                if (blocks.ContainsKey(y))
                {
                    blocks[y].Add(x);
                }
                else
                {
                    blocks.Add(y, new HashSet<int> { x });
                }
            }

            bool IsUTurn(int i)
            {
                var previous = i == 0 ? digPlan[^1] : digPlan[i - 1];
                var next = i == digPlan.Count-1 ? digPlan[0] : digPlan[i + 1];
                
                return previous.Direction != next.Direction;
            }


            for (var i = 0; i < digPlan.Count; i++)
            {
                Draw instruction = digPlan[i];

                for (var s = 1; s <= instruction.Steps; s++)
                {
                    if (s > 1 && (instruction.Direction == Direction.Up || instruction.Direction == Direction.Down))
                    {
                        AddBlock(position.Item1, position.Item2);
                    }

                    position = Move(position, instruction);

                    visited.Add(position);
                }

                if (instruction.Direction == Direction.Right || instruction.Direction == Direction.Left)
                {
                    if (!IsUTurn(i))
                    {
                        AddBlock(position.Item1, position.Item2);
                    }
                }
            }

            var border = visited.Count;

            var top = visited.Select(x => x.Item2).Min();
            var bottom = visited.Select(x => x.Item2).Max();
            var right = visited.Select(x => x.Item1).Max();
            var left = visited.Select(x => x.Item2).Min();

            var inside = new HashSet<(int, int)>();

            for (var y = top+1; y < bottom; y++)
            {
                for (var x = left; x <= right; x++)
                {
                    if (visited.Contains((x,y)))
                    {
                        continue;
                    }

                    var leftOf = 0;

                    if (blocks.ContainsKey(y))
                    {
                        leftOf += blocks[y].Where(xb => xb < x).Count();
                    }

                    if (leftOf % 2 == 1)
                    {
                        inside.Add((x, y));
                    }
                }
            }

            DrawMap(visited, inside);


            return visited.Count + inside.Count;
        }

        private void DrawMap(HashSet<(int, int)> visited, HashSet<(int, int)> inside)
        {
            var top = visited.Select(x => x.Item2).Min();
            var bottom = visited.Select(x => x.Item2).Max();
            var right = visited.Select(x => x.Item1).Max();
            var left = visited.Select(x => x.Item1).Min();

            for (var y = top; y < bottom + 1; y++)
            {
                var line = new StringBuilder();
                for (var x = left; x < right + 1; x++)
                {
                    if (visited.Contains((x,y)) && inside.Contains((x,y)))
                    {
                        line.Append('X');
                    }
                    else if (visited.Contains((x,y)))
                    {
                        line.Append('#');
                    }
                    else if (inside.Contains((x,y)))
                    {
                        line.Append('O');
                    }
                    else
                    {
                        line.Append('.');
                    }
                }
                output.WriteLine(line.ToString());
            }
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return DugOut(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            // 22488 your answer is too low
            // 22585 your answer is too low
            // 28168 That's not the right answer.
            Assert.Equal(28911, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "R 6 (#70c710)",
                "D 5 (#0dc571)",
                "L 2 (#5713f0)",
                "D 2 (#d2c081)",
                "R 2 (#59c680)",
                "D 2 (#411b91)",
                "L 5 (#8ceee2)",
                "U 2 (#caa173)",
                "L 1 (#1b58a2)",
                "U 2 (#caa171)",
                "R 2 (#7807d2)",
                "U 3 (#a77fa3)",
                "L 2 (#015232)",
                "U 2 (#7a21e3)"
            };

            Assert.Equal(62, DugOut(example));
        }
        

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "R 6 (#70c710)",
                "D 5 (#0dc571)",
                "L 2 (#5713f0)",
                "D 2 (#d2c081)",
                "R 2 (#59c680)",
                "D 2 (#411b91)",
                "L 5 (#8ceee2)",
                "U 2 (#caa173)",
                "L 1 (#1b58a2)",
                "U 2 (#caa171)",
                "R 2 (#7807d2)",
                "U 3 (#a77fa3)",
                "L 2 (#015232)",
                "U 2 (#7a21e3)"
            };

            Assert.Equal(952408144115, DugOut(example));

        }
    }
}
