using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;
using Shared.MapGeometry;
using static Solutions.InputReader;


namespace Solutions.Event2015
{
    public class Day03
    {
        public static Direction Parse(char c)
        {
            switch (c)
            {
                case '^':
                    return Direction.North;
                case '>':
                    return Direction.East;
                case 'v':
                    return Direction.South;
                case '<':
                    return Direction.West;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown instruction: {c}");
            }
        }

        public static IEnumerable<Direction> Parse(string input)
        {
            return input.Select(Parse);
        }

        public static IEnumerable<Point> Visited(Point startingPoint, IEnumerable<Direction> movements)
        {
            var visited = new List<Point> {startingPoint};

            return movements.Aggregate(visited, (v, direction) =>
            {
                visited.Add(v.Last().Move(direction));
                return visited;
            });
        }

        public static int CountDistinctVisited(string input)
        {
            var directions = Parse(input);

            var visited = Visited(new Point(0, 0), directions);

            return visited.Distinct().Count();
        }

        public static (IList<Direction> santa1, IList<Direction> santa2) Split(IList<Direction> directions)
        {
            var groupedDirections = directions
                .Select((d, i) => new {Index = i, Direction = d})
                .GroupBy(x => x.Index % 2 == 0)
                .ToDictionary(g => g.Key, g => g.Select(gi => gi.Direction).ToList());

            return (groupedDirections[true], groupedDirections[false]);
        }

        public static int CountVisitedOnceOrMoreTwoSanta(string input)
        {
            var directions = Parse(input).ToList();

            var (santa1Directions, santa2Directions) = Split(directions);

            var santa1Visited = Visited(new Point(0, 0), santa1Directions);
            var santa2Visited = Visited(new Point(0, 0), santa2Directions);

            return santa1Visited.Concat(santa2Visited).Distinct().Count();
        }

        public static int FirstStar()
        {
            var input = ReadInput();

            return CountDistinctVisited(input);
        }

        public static int SecondStar()
        {
            var input = ReadInput();

            return CountVisitedOnceOrMoreTwoSanta(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(2592, result);
        }
        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(2360, result);
        }

        [Theory]
        [InlineData('^', Direction.North)]
        [InlineData('>', Direction.East)]
        [InlineData('v', Direction.South)]
        [InlineData('<', Direction.West)]
        public void Parse_SingleDirectionChar_ReturnsExpectedDirection(char c, Direction expectedDirection)
        {
            Direction direction = Parse(c);

            Assert.Equal(expectedDirection, direction);
        }

        [Fact]
        public void Parse_ManyDirections_ReturnsOrderedCollection()
        {
            var input = "^>v<";

            var directions = Parse(input);

            Assert.Collection(directions, 
                c => Assert.Equal(Direction.North, c),
                c => Assert.Equal(Direction.East, c),
                c => Assert.Equal(Direction.South, c),
                c => Assert.Equal(Direction.West, c)
                );
        }

        [Fact]
        public void Visited_Example1()
        {
            var visited = Visited(new Point(0, 0), new[] {Direction.East}).ToList();

            var expected = new List<Point> {new Point(0, 0), new Point(1, 0)};

            Assert.Equal(expected, visited);
        }

        [Fact]
        public void Visited_Example2()
        {
            var visited = Visited(new Point(0, 0), new[] {Direction.North, Direction.East, Direction.South, Direction.West}).ToList();

            var expected = new List<Point> {new Point(0, 0), new Point(0, -1), new Point(1,-1), new Point(1,0), new Point(0,0)};

            Assert.Equal(expected, visited);
        }

        [Fact]
        public void Split_Example1()
        {
            var (santa1, santa2) = Split(new List<Direction> { Direction.East, Direction.North, Direction.South, Direction.West});

            Assert.Equal(new List<Direction> {Direction.East, Direction.South}, santa1);
            Assert.Equal(new List<Direction> {Direction.North, Direction.West}, santa2);
        }

        [Theory]
        [InlineData("^v", 2)]
        [InlineData("^>v<", 4)]
        [InlineData("^v^v^v^v^v", 2)]
        public void FirstStar_Examples(string input, int expectedVisited)
        {
            var visitedCount = CountDistinctVisited(input);

            Assert.Equal(expectedVisited, visitedCount);
        }

        [Theory]
        [InlineData("^v", 3)]
        [InlineData("^>v<", 3)]
        [InlineData("^v^v^v^v^v", 11)]
        public void SecondStar_Examples(string input, int expectedVisited)
        {
            var visitedCount = CountVisitedOnceOrMoreTwoSanta(input);

            Assert.Equal(expectedVisited, visitedCount);
        }
    }
}
