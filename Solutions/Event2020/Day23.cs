using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;


namespace Solutions.Event2020
{
    // --- Day 23: Crab Cups ---

    public class Day23
    {
        private static readonly string Input = "586439172";

        private readonly ITestOutputHelper output;


        public Day23(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<int> Parse(string input)
        {
            return input.Select(c => int.Parse(c.ToString())).ToList();
        }

        private List<int> MoveInt(List<int> input, int times)
        {
            var current = 0;
            var state = input;
            var maxLabel = input.Max();

            foreach (var i in Enumerable.Range(1, times))
            {
                var currentLabel = state[current];
                var selected = TakeClockwiseInt(state, current, 3);
                var circle = LeftAfterTakeInt(state, current, 3);
                var destination = DestinationInt(selected, currentLabel, maxLabel);
                state = InsertInt(circle, selected, destination);
                current = NextCurrentInt(state, currentLabel);
            }

            return state;
        }

        private List<int> LeftAfterTakeInt(List<int> input, int current, int take)
        {
            if (take < 1)
            {
                return input;
            }

            var startAt = current + take + 1 < input.Count ? 0 : current + take + 1 % input.Count;
            var skipFrom = current + 1 % input.Count;
            var skipTo = skipFrom + take - 1 % input.Count;

            var output = new List<int>();

            var i = startAt;

            while (output.Count < input.Count - take)
            {
                var iw = i % input.Count;

                if (iw < skipFrom || iw > skipTo)
                {
                    output.Add(input[iw]);
                }
                i++;
            }

            return output;
        }

        private List<int> TakeClockwiseInt(List<int> input, int current, int take)
        {
            var output = new List<int>();

            for (int i = current + 1; i < current + take + 1; i++)
            {
                var iw = i > input.Count - 1 ? i % input.Count : i;

                output.Add(input[iw]);
            }

            return output;
        }

        private int DestinationInt(List<int> selected, int current, int maxLabel)
        {
            var destination = current - 1;
            if (destination < 1)
            {
                destination = maxLabel;
            }

            while (selected.Any(i => i == destination))
            {
                destination -= 1;

                if (destination < 1)
                {
                    destination = maxLabel;
                }
            }

            return destination;
        }

        private int NextCurrentInt(List<int> input, int currentLabel)
        {
            return (input.IndexOf(currentLabel) + 1) % input.Count;
        }

        private List<int> InsertInt(List<int> input, List<int> selected, int destination)
        {
            var i = input.IndexOf(destination) + 1 % input.Count;

            var firstPart = input.Take(i).ToList();

            var secondPart = input.Skip(i).Take(input.Count - i).ToList();

            return firstPart.Concat(selected).Concat(secondPart).ToList();
        }

        private string LabelsFromInt(List<int> labels, int from)
        {
            var i = labels.IndexOf(from);

            var firstPart = labels.Skip(i + 1).Take(labels.Count - i - 1).ToList();
            var secondPart = labels.Take(i).ToList();

            var result = firstPart.Concat(secondPart).ToList();

            return string.Join("", result);
        }

        private long MultiplyNextToCupsFrom(List<int> input, int from)
        {
            var i = input.IndexOf(from);

            var nextTwo = input.Skip(i + 1).Take(2).ToList();

            return nextTwo[0]*nextTwo[1];
        }

        public string FirstStar()
        {
            var input = Input.Select(c => int.Parse(c.ToString())).ToList();
            var output = MoveInt(input, 100);
            var result = LabelsFromInt(output, 1);

            return result;
        }

        public int SecondStar()
        {
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal("28946753", FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();
            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData("583741926", "92658374")]
        [InlineData("926583741", "92658374")]
        [InlineData("192658374", "92658374")]
        public void FirstStarLabelsFromTest(string input, string expected)
        {
            var result = LabelsFromInt(Parse(input), 1);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void FirstStarMoveTest()
        { 
            var input = "389125467";

            var output = MoveInt(Parse(input), 10);

            Assert.Equal("583741926", string.Join("", output));
        }

        [Theory]
        [InlineData(0, "891")]
        [InlineData(6, "673")]
        [InlineData(7, "738")]
        [InlineData(8, "389")]
        public void FirstStarTakeClockwiseTest(int current, string expected)
        { 
            var input = "389125467";

            var output = TakeClockwiseInt(Parse(input), current, 3);

            Assert.Equal(expected, string.Join("", output));
        }

        [Theory]
        [InlineData(0, "325467")]
        [InlineData(6, "891254")]
        [InlineData(7, "912546")]
        [InlineData(8, "125467")]
        public void FirstStarLeftAfterTakeTest(int current, string expected)
        { 
            var input = "389125467";

            var output = LeftAfterTakeInt(Parse(input), current, 3);

            Assert.Equal(expected, string.Join("", output));
        }

        [Theory]
        [InlineData(3, "891", 2)]
        [InlineData(2, "891", 7)]
        [InlineData(5, "467", 3)]
        [InlineData(8, "913", 7)]
        [InlineData(4, "679", 3)]
        [InlineData(1, "367", 9)]
        [InlineData(9, "367", 8)]
        [InlineData(2, "583", 1)]
        [InlineData(6, "741", 5)]
        [InlineData(5, "741", 3)]
        public void FirstStarDestinationTest(int current, string selected, int expected)
        { 
            var destination = DestinationInt(Parse(selected), current, 9);

            Assert.Equal(expected, destination);
        }

        [Theory]
        [InlineData("325467", 2, "891", "328915467")]
        [InlineData("325467", 7, "891", "325467891")]
        [InlineData("325891", 3, "467", "346725891")]
        [InlineData("725846", 7, "913", "791325846")]
        [InlineData("325841", 3, "679", "367925841")]
        [InlineData("925841", 9, "367", "936725841")]
        [InlineData("258419", 8, "367", "258367419")]
        [InlineData("674192", 1, "583", "674158392")]
        [InlineData("583926", 5, "741", "574183926")]
        [InlineData("583926", 3, "741", "583741926")]

        public void FirstStarInsertTest(string circle, int destination, string taken, string expected)
        { 
            var output = InsertInt(Parse(circle), Parse(taken), destination);

            Assert.Equal(expected, string.Join("", output));
        }

        [Theory]
        [InlineData("389125467", 3, 1)]
        [InlineData("389125467", 6, 8)]
        [InlineData("389125467", 7, 0)]

        public void FirstStarNextCurrentTest(string input, char currentLabel, int expected)
        { 
            var i = NextCurrentInt(Parse(input), currentLabel);

            Assert.Equal(expected, i);
        }

        [Theory]
        [InlineData("389125467", 10, "92658374")]
        [InlineData("389125467", 100, "67384529")]
        public void FirstStarExample(string input, int times, string expected)
        {
            var inp = input.Select(c => int.Parse(c.ToString())).ToList();
            var output = MoveInt(inp, times);
            var result = string.Join("", LabelsFromInt(output, 1));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SecondStarExample()
        {
            var range = 100;
            var times = range*10;
            var input = Parse("389125467");
            var max = input.Max();
            var tail = Enumerable.Range(max + 1, range - max);
            var all = input.Concat(tail).ToList();
            var output = MoveInt(all, times);
            var result = MultiplyNextToCupsFrom(output, 1);

            Assert.Equal(149245887792, result);
        }
    }
}
