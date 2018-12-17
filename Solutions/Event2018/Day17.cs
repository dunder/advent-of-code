using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using Solutions.Event2017.Day09;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day17 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day17;
        public string Name => "Reservoir Research";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = CountTiles(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        private int CountTiles(IList<string> input)
        {
            var clay = ParseClay(input);



            return 0;
        }

       
        public HashSet<Point> ParseClay(IList<string> input)
        {
            var clay = new HashSet<Point>();

            var horisontalExpression = new Regex(@"y=(\d+), x=(\d+)..(\d+)");
            var verticalExpression = new Regex(@"x=(\d+), y=(\d+)..(\d+)");
            foreach (var line in input)
            {
                if (horisontalExpression.IsMatch(line))
                {
                    var match = horisontalExpression.Match(line);
                    var y = int.Parse(match.Groups[1].Value);
                    var xFrom = int.Parse(match.Groups[2].Value);
                    var xTo = int.Parse(match.Groups[3].Value);

                    for (int x = xFrom; x <= xTo; x++)
                    {
                        clay.Add(new Point(x, y));
                    }
                }
                else
                {
                    var match = verticalExpression.Match(line);
                    var x = int.Parse(match.Groups[1].Value);
                    var yFrom = int.Parse(match.Groups[2].Value);
                    var yTo = int.Parse(match.Groups[3].Value);

                    for (int y = yFrom; y <= yTo; y++)
                    {
                        clay.Add(new Point(x, x));
                    }
                }
            }
            return clay;
        }

        [Fact]
        public void FirstStarExapmle()
        {
            var lines = new[]
            {
                "x=495, y=2..7",
                "y=7, x=495..501",
                "x=501, y=3..7",
                "x=498, y=2..4",
                "x=506, y=1..2",
                "x=498, y=10..13",
                "x=504, y=10..13",
                "y=13, x=498..504"
            };

            var result = CountTiles(lines);

            Assert.Equal(57, result);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual);
        }
    }
}