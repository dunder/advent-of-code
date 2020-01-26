using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 19: Tractor Beam ---
    public class Day19
    {
        private readonly ITestOutputHelper output;

        public Day19(ITestOutputHelper output)
        {
            this.output = output;
        }

        private Dictionary<Point, int> Scan(string program, Point p, int xMax, int yMax)
        {
            var map = new Dictionary<Point, int>();
            for (int y = p.Y; y < p.Y + yMax; y++)
            {
                for (int x = p.X; x < p.X + xMax; x++)
                {
                    var computer = IntCodeComputer.Load(program);
                    computer.Execute(x, y);
                    map.Add(new Point(x,y), (int) computer.Output.Last());
                }
            }
            return map;
        }

        private bool ExecuteOne(string program, int x, int y)
        {
            var computer = IntCodeComputer.Load(program);
            computer.Execute(x, y);
            return computer.Output.Last() == 1;
        }

        private int FindNew(string program, int width, int height, Point start)
        {
            var xStep = start.X;
            var yStep = start.Y;
            var x = xStep;
            var y = yStep;
            var found = false;
            while (!found)
            {
                y++;
                while (!ExecuteOne(program, x, y))
                {
                    x++;
                }

                int x2 = x;
                while (ExecuteOne(program, x2, y))
                {
                    x2++;
                    if (CoversSimple(program, x2, y, width, height))
                    {
                        x = x2;
                        found = true;
                    }
                }
            }

            var beam = new Point(0, 0);
            var closest = Enumerable.Range(x, width).Select(xx => new Point(xx, y)).OrderBy(p => p.ManhattanDistance(beam)).First();

            return closest.X * 10000 + closest.Y;
        }
        
        private bool CoversSimple(string program, int x, int y, int width, int height)
        {
            var computer = IntCodeComputer.Load(program);
            computer.Execute(x + width - 1, y);
            
            if (computer.Output.Last() == 0)
            {
                return false;
            }

            computer = IntCodeComputer.Load(program);
            computer.Execute(x, y + height - 1);

            return computer.Output.Last() == 1;
        }

        private void PrintMap(Dictionary<Point, int> map, Point p, int xMax, int yMax)
        {
            for (int y = p.Y; y < p.Y + yMax; y++)
            {
                var s = new StringBuilder();
                for (int x = p.X; x < p.X + xMax; x++)
                {
                    s.Append(map[new Point(x, y)]);
                }
                output.WriteLine(s.ToString());

            }
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var map = Scan(input, new Point(0,0), 50, 50);
            return map.Values.Count(v => v == 1);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var value = FindNew(input, 100, 100, new Point(6, 5));
            return value;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(118, FirstStar());
        }

        [Trait("Category", "LongRunning")] // 1 m 37 s optimization potential
        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(18651593, SecondStar());
        }
    }
}
