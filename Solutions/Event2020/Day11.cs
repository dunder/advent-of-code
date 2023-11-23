using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 11: Seating System ---
    public class Day11
    {
        private readonly ITestOutputHelper output;

        public Day11(ITestOutputHelper output)
        {
            this.output = output;
        }

        public enum SurroundingStrategy { Adjacent, Visible }

        private delegate IEnumerable<(int, int)> Surrounding((int, int) position);

        private record SeatingPlan(int width, int height, HashSet<(int, int)> floor, HashSet<(int, int)> seats, HashSet<(int, int)> occupied)
        {
            public IEnumerable<(int, int)> Adjacent((int, int) position)
            {
                (int x, int y) = position;

                var xmin = Math.Max(0, x - 1);
                var xmax = Math.Min(width - 1, x + 1);

                var ymin = Math.Max(0, y - 1);
                var ymax = Math.Min(height, y + 1);

                for (int ya = ymin; ya <= ymax; ya++)
                {
                    for (int xa = xmin; xa <= xmax; xa++)
                    {
                        var adjacentPosition = (xa, ya);

                        if (position == adjacentPosition) { continue; }
                        if (floor.Contains(adjacentPosition)) { continue; }

                        yield return adjacentPosition;
                    }
                }
            }

            public IEnumerable<(int, int)> Visible((int, int) position)
            {
                (int x, int y) = position;

                // up
                for (int y1 = y - 1; y1 >= 0; y1--)
                {
                    if (seats.Contains((x, y1)))
                    {
                        yield return (x, y1);
                        break;
                    }
                }

                // up right
                for (int x1 = x + 1, y1 = y - 1; x1 < width && y1 >= 0; x1++, y1--)
                {
                    if (seats.Contains((x1, y1)))
                    {
                        yield return (x1, y1);
                        break;
                    }
                }

                // right
                for (int x1 = x + 1; x1 < width; x1++)
                {
                    if (seats.Contains((x1, y)))
                    {
                        yield return (x1, y);
                        break;
                    }
                }

                // down right
                for (int x1 = x + 1, y1 = y + 1; x1 < width && y1 < height; x1++, y1++)
                {
                    if (seats.Contains((x1, y1)))
                    {
                        yield return (x1, y1);
                        break;
                    }
                }

                // down
                for (int y1 = y + 1; y1 < height; y1++)
                {
                    if (seats.Contains((x, y1)))
                    {
                        yield return (x, y1);
                        break;
                    }
                }

                // down left
                for (int x1 = x - 1, y1 = y + 1; x1 >= 0 && y1 < height; x1--, y1++)
                {
                    if (seats.Contains((x1, y1)))
                    {
                        yield return (x1, y1);
                        break;
                    }
                }

                // left
                for (int x1 = x - 1; x1 >= 0; x1--)
                {
                    if (seats.Contains((x1, y)))
                    {
                        yield return (x1, y);
                        break;
                    }
                }

                // up left
                for (int x1 = x - 1, y1 = y - 1; x1 >= 0 && y1 >= 0; x1--, y1--)
                {
                    if (seats.Contains((x1, y1)))
                    {
                        yield return (x1, y1);
                        break;
                    }
                }
            }

            public SeatingPlan Next(SurroundingStrategy surroundingStrategy, int occupationLimit)
            {
                Surrounding surrounding = surroundingStrategy == SurroundingStrategy.Adjacent ? Adjacent : Visible;

                var occupiedSeats = seats.Where(occupied.Contains);
                var emptySeats = seats.Where(seat => !occupied.Contains(seat));

                var toBeOccupied = emptySeats
                    .Where(seat => 
                        surrounding(seat)
                            .All(seat => !occupied.Contains(seat)));

                var toBeEmpty = occupiedSeats
                    .Where(seat => 
                        surrounding(seat)
                        .Count(seat => occupied.Contains(seat)) >= occupationLimit);

                var newOccupied = new HashSet<(int, int)>(occupied);
                newOccupied.UnionWith(toBeOccupied);
                newOccupied.ExceptWith(toBeEmpty);

                return this with { occupied = newOccupied };
            }
        }

        private SeatingPlan ParseSeatingPlan(IList<string> input)
        {
            int width = input.First().Length;
            int height = input.Count;

            var floor = new HashSet<(int, int)>();
            var seats = new HashSet<(int, int)>();

            for (int y = 0; y < height; y++)
            {
                var row = input[y];

                for (int x = 0; x < width; x++)
                {
                    var column = row[x];
                    var position = (x, y);

                    if ('.'.Equals(column))
                    {
                        floor.Add(position);
                    }
                    else
                    {
                        seats.Add(position);
                    }
                }
            }

            return new SeatingPlan(width, height, floor, seats, new HashSet<(int, int)>());
        }

        private int RoundsToStable(IList<string> input, SurroundingStrategy surroundingStrategy, int occupationLimit)
        {
            var seatingPlan = ParseSeatingPlan(input);

            SeatingPlan next = seatingPlan;

            do
            {
                seatingPlan = next;
                next = seatingPlan.Next(surroundingStrategy, occupationLimit);
            } while (!seatingPlan.occupied.SetEquals(next.occupied));

            return seatingPlan.occupied.Count;
        }

        private void Print(SeatingPlan seatingPlan)
        {
            for (int y = 0; y < seatingPlan.height; y++)
            {
                var line = new StringBuilder();

                for (int x = 0; x < seatingPlan.height; x++)
                {
                    if (seatingPlan.floor.Contains((x, y)))
                    {
                        line.Append('.');
                    }
                    else
                    {
                        line.Append(seatingPlan.occupied.Contains((x, y)) ? '#' : 'L');
                    }
                }
                output.WriteLine(line.ToString());
            }
            output.WriteLine("");
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return RoundsToStable(input, SurroundingStrategy.Adjacent, 4);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return RoundsToStable(input, SurroundingStrategy.Visible, 5);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(2283, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(2054, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            };

            Assert.Equal(37, RoundsToStable(example, SurroundingStrategy.Adjacent, 4));
        }

        [Fact]
        public void SeatingPlanAdjacentCornerTest()
        {
            var example = new List<string>
            {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            };

            var seatingPlan = ParseSeatingPlan(example);

            var adjacent = seatingPlan.Adjacent((0, 0)).ToList();

            Assert.Collection(adjacent,
                a => Assert.Equal((0,1), a),
                a => Assert.Equal((1,1), a));
        }

        [Fact]
        public void SeatingPlanAdjacentAllAvailableTest()
        {
            var example = new List<string>
            {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            };

            var seatingPlan = ParseSeatingPlan(example);

            var adjacent = seatingPlan.Adjacent((5, 8)).ToList();

            Assert.Collection(adjacent,
                a => Assert.Equal((4,7), a),
                a => Assert.Equal((5,7), a),
                a => Assert.Equal((6,7), a),
                a => Assert.Equal((4,8), a),
                a => Assert.Equal((6,8), a),
                a => Assert.Equal((4,9), a),
                a => Assert.Equal((5,9), a),
                a => Assert.Equal((6,9), a));
        }

        [Fact]
        public void SeatingPlanVisibleTest()
        {
            var example = new List<string>
            {
                ".......#.",
                "...#.....",
                ".#.......",
                ".........",
                "..#L....#",
                "....#....",
                ".........",
                "#........",
                "...#....."
            };

            var seatingPlan = ParseSeatingPlan(example);

            var adjacent = seatingPlan.Visible((3, 4)).ToList();

            Assert.Collection(adjacent,
                v => Assert.Equal((3,1), v),
                v => Assert.Equal((7,0), v),
                v => Assert.Equal((8,4), v),
                v => Assert.Equal((4,5), v),
                v => Assert.Equal((3,8), v),
                v => Assert.Equal((0,7), v),
                v => Assert.Equal((2,4), v),
                v => Assert.Equal((1,2), v));
        }

        [Fact]
        public void SecondStarExample()
        {
            
        }
    }
}
