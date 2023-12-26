using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 18: Lavaduct Lagoon ---
    public class Day18
    {
        private readonly ITestOutputHelper output;

        public Day18(ITestOutputHelper output)
        {
            this.output = output;
        }

        private enum Direction { Up, Right, Down, Left }

        private record DigInstruction(Direction Direction, int Steps); 

        private List<DigInstruction> Parse(IList<string> input)
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

                return new DigInstruction(direction, steps);
            }).ToList();
        }

        private List<DigInstruction> Parse2(IList<string> input)
        {
            var colorRegex = new Regex(@"[0-9a-f]+");

            return input.Select(line =>
            {
                var parts = line.Split(" ");
                
                var color = colorRegex.Match(parts[2]).Value;

                int steps = Convert.ToInt32($"0x{color[0..5]}", 16);

                var direction = int.Parse(color[5].ToString()) switch
                {
                    3 => Direction.Up,
                    0 => Direction.Right,
                    1 => Direction.Down,
                    2 => Direction.Left,
                    _ => throw new InvalidOperationException($"Unkown direction : {color}")
                };

                return new DigInstruction(direction, steps);
            }).ToList();
        }

        private (int,int) Move((int, int) position, DigInstruction draw)
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

        private (int,int) Move2((int, int) position, DigInstruction draw)
        {
            (int x, int y) = position;

            switch(draw.Direction)
            {
                case Direction.Up:
                    return (x, y - draw.Steps);
                case Direction.Right:
                    return (x + draw.Steps, y);
                case Direction.Down:
                    return (x, y + draw.Steps);
                case Direction.Left:
                    return (x - draw.Steps, y);
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
                DigInstruction instruction = digPlan[i];

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

            return visited.Count + inside.Count;
        }

        private record Edge(int Left, int Right, int RowTop, int RowBottom, bool UTurn = false)
        {
            public int Height => Math.Abs(RowTop - RowBottom) + 1;
            public int Width => Math.Abs(Left - Right) + 1;

            public bool Overlaps(int yTop, int yBottom)
            {
                return yTop >= RowTop && yBottom <= RowBottom;
            }
        }

        private long DugOut2(List<DigInstruction> digPlan)
        {
            var position = (0, 0);

            List<Edge> edges = new();

            for (var i = 0; i < digPlan.Count; i++)
            {
                DigInstruction instruction = digPlan[i];

                (int x, int y) = position;

                bool IsUTurn()
                {
                    var previous = i == 0 ? digPlan[^1] : digPlan[i - 1];
                    var next = i == digPlan.Count - 1 ? digPlan[0] : digPlan[i + 1];

                    return previous.Direction != next.Direction;
                }

                switch (instruction.Direction)
                {
                    case Direction.Up:
                        edges.Add(new Edge(x, x, y - instruction.Steps + 1, y - 1));
                        break;
                    case Direction.Down:
                        edges.Add(new Edge(x, x, y + 1, y + instruction.Steps - 1));
                        break;
                    case Direction.Right:
                        edges.Add(new Edge(x, x + instruction.Steps, y, y, IsUTurn()));
                        break;
                    case Direction.Left:
                        edges.Add(new Edge(x - instruction.Steps, x, y, y, IsUTurn()));
                        break;
                }

                position = Move2(position, instruction);
            }

            var edgesOrderedByLeft = edges.OrderBy(edge => edge.Left).ToList();

            var horizontalLines = new HashSet<int>();

            foreach (var edge in edges)
            {
                horizontalLines.Add(edge.RowTop);
                horizontalLines.Add(edge.RowBottom);
            }

            List<int> horizontalLinesOrderedByY = horizontalLines.OrderBy(x => x).ToList();

            long insideCount = 0;

            var lefts = new HashSet<(int, int)>();
            var rights = new HashSet<(int, int)>();

            HashSet<(int, int)> inside = new HashSet<(int, int)>();

            for (int i = 1; i < horizontalLinesOrderedByY.Count; i++)
            {
                var yTop = horizontalLinesOrderedByY[i - 1];
                var yBottom = horizontalLinesOrderedByY[i] - 1;

                long height = Math.Abs(yBottom - yTop) + 1;

                if (height > 1)
                {
                    List<Edge> columnOrdered1 = edges
                        .Where(edge => edge.Overlaps(yTop, yTop))
                        .OrderBy(edge => edge.Left)
                        .ThenBy(edge => edge.Right)
                        .ToList();

                    insideCount += Space(yTop, yTop, columnOrdered1);

                    List<Edge> columnOrdered2 = edges
                        .Where(edge => edge.Overlaps(yTop + 1, yBottom))
                        .OrderBy(edge => edge.Left)
                        .ThenBy(edge => edge.Right)
                        .ToList();

                    insideCount += Space(yTop + 1, yBottom, columnOrdered2);
                }
                else
                {
                    List<Edge> columnOrdered = edges
                        .Where(edge => edge.Overlaps(yTop, yBottom))
                        .OrderBy(edge => edge.Left)
                        .ThenBy(edge => edge.Right)
                        .ToList();

                    insideCount += Space(yTop, yBottom, columnOrdered);
                }
            }

            long borderCount = digPlan.Select(d => d.Steps).Sum();

            return borderCount + insideCount;
        }

        private long Space(int yTop, int yBottom, List<Edge> columnOrdered)
        {
            long insideCount = 0;
            long height = Math.Abs(yBottom - yTop) + 1;

            for (int j = 1, leftCounter = 1; j < columnOrdered.Count; j++, leftCounter++)
            {
                var xFrom = columnOrdered[j - 1].Right;
                var xTo = columnOrdered[j].Left;

                long spaceWidth = Math.Abs(xTo - xFrom) - 1;

                if (columnOrdered[j - 1].UTurn)
                {
                    leftCounter++;
                }

                if (leftCounter % 2 != 0)
                {
                    insideCount += spaceWidth * height;
                }
            }

            return insideCount;
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

        public long FirstStar()
        {
            var input = ReadLineInput();


            return DugOut(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();
            List<DigInstruction> digPlan = Parse2(input);
            return DugOut2(digPlan);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(28911, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(77366737561114, SecondStar());
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
        public void FirstStarExample2()
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

            var digPlan = Parse(example);
            
            Assert.Equal(62, DugOut2(digPlan));
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

            var digPlan = Parse2(example);

            Assert.Equal(952408144115, DugOut2(digPlan));
        }
    }
}
