using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;


namespace Solutions.Event2020
{
    // --- Day 23: Crab Cups ---

    public class Day23
    {
        private static readonly string Input = "586439172";

        private readonly ITestOutputHelper output;


        private class Cup
        {
            public Cup (int label)
            {
                Label = label;
            }

            public Cup Next { get; set; }

            public int Label { get; private set; }

            public override string ToString()
            {
                return $"Cup {Label}";
            }
        }

        public Day23(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<int> Parse(string input)
        {
            return input.Select(c => int.Parse(c.ToString())).ToList();
        }

        private List<int> Move(List<int> input, int times)
        {
            var current = 0;
            var state = input;
            var maxLabel = input.Max();

            foreach (var i in Enumerable.Range(1, times))
            {
                var currentLabel = state[current];
                var selected = TakeClockwise(state, current, 3);
                var circle = LeftAfterTake(state, current, 3);
                var destination = Destination(selected, currentLabel, maxLabel);
                state = Insert(circle, selected, destination);
                current = NextCurrent(state, currentLabel);
            }

            return state;
        }

        private List<int> LeftAfterTake(List<int> input, int current, int take)
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

        private List<int> TakeClockwise(List<int> input, int current, int take)
        {
            var output = new List<int>();

            for (int i = current + 1; i < current + take + 1; i++)
            {
                var iw = i > input.Count - 1 ? i % input.Count : i;

                output.Add(input[iw]);
            }

            return output;
        }

        private int Destination(List<int> selected, int current, int maxLabel)
        {
            int destination = current;

            do
            {
                destination -= 1;

                if (destination < 1)
                {
                    destination = maxLabel;
                }
            } while (selected.Any(i => i == destination));

            return destination;
        }

        private int NextCurrent(List<int> input, int currentLabel)
        {
            return (input.IndexOf(currentLabel) + 1) % input.Count;
        }

        private List<int> Insert(List<int> input, List<int> selected, int destination)
        {
            var i = input.IndexOf(destination) + 1 % input.Count;

            var firstPart = input.Take(i).ToList();

            var secondPart = input.Skip(i).Take(input.Count - i).ToList();

            return firstPart.Concat(selected).Concat(secondPart).ToList();
        }

        private string LabelsFrom(List<int> labels, int from)
        {
            var i = labels.IndexOf(from);

            var firstPart = labels.Skip(i + 1).Take(labels.Count - i - 1).ToList();
            var secondPart = labels.Take(i).ToList();

            var result = firstPart.Concat(secondPart).ToList();

            return string.Join("", result);
        }

        private string Play(string input, int times)
        {
            var labels = Parse(input);

            var byLabel = new Dictionary<int, Cup>();

            var cups = labels.Select(label => new Cup(label)).ToList();

            foreach (var cup in cups)
            {
                byLabel.Add(cup.Label, cup);
            }

            for (int i = 0; i < cups.Count; i++)
            {
                Cup cup = cups[i];
                Cup nextCup = i + 1 == cups.Count ? cups[0] : cups[i + 1];
                cup.Next = nextCup;
            }

            int maxLabel = labels.Max();

            var current = cups[0];

            foreach (var i in Enumerable.Range(1, times))
            {
                var selected = current.Next;

                // close circle
                current.Next = selected.Next.Next.Next;

                int destinationLabel = DestinationLabel(current, selected, maxLabel);
                Cup destinationCup = byLabel[destinationLabel];

                Cup afterDestinationCup = destinationCup.Next;

                // insert in circle after destination cup
                destinationCup.Next = selected;
                selected.Next.Next.Next = afterDestinationCup;

                current = current.Next;
            }

            current = byLabel[1].Next;

            var result = new StringBuilder();

            foreach (var i in Enumerable.Range(1, labels.Count - 1))
            {
                result.Append(current.Label.ToString());
                current = current.Next;
            }

            return result.ToString();
        }

        private long Play2(string input, int cupCount, int times)
        {
            var labels = Parse(input);

            int maxLabel = labels.Max();

            var byLabel = new Dictionary<int, Cup>();

            var cups = labels.Select(label => new Cup(label)).ToList();

            var extraCups = Enumerable.Range(maxLabel + 1, cupCount - maxLabel).Select(i => new Cup(i));

            cups = cups.Concat(extraCups).ToList();

            foreach (var cup in cups)
            {
                byLabel.Add(cup.Label, cup);
            }

            for (int i = 0; i < cups.Count; i++)
            {
                Cup cup = cups[i];
                Cup nextCup = i + 1 == cups.Count ? cups[0] : cups[i + 1];
                cup.Next = nextCup;
            }

            // new max label after adding an additional (large) set of cups
            maxLabel = cupCount;

            var current = cups[0];

            foreach (var i in Enumerable.Range(1, times))
            {
                var selected = current.Next;

                // close circle
                current.Next = selected.Next.Next.Next;

                int destinationLabel = DestinationLabel(current, selected, maxLabel);
                Cup destinationCup = byLabel[destinationLabel];

                Cup afterDestinationCup = destinationCup.Next;

                // insert in circle after destination cup
                destinationCup.Next = selected;
                selected.Next.Next.Next = afterDestinationCup;

                current = current.Next;
            }

            var nextAfter1 = byLabel[1].Next;
            var nextAfterThat = nextAfter1.Next;

            return (long)nextAfter1.Label * nextAfterThat.Label;
        }

        private int DestinationLabel(Cup current, Cup selected, int maxLabel)
        {
            int destinationLabel = current.Label;
            do
            {
                destinationLabel -= 1;

                if (destinationLabel < 1)
                {
                    destinationLabel = maxLabel;
                }

            } while (IsInNextThree(selected, destinationLabel));

            return destinationLabel;
        }

        private bool IsInNextThree(Cup from, int label)
        {
            return from.Label == label || from.Next.Label == label || from.Next.Next.Label == label;
        }

        public string FirstStar()
        {
            var result = Play(Input, 100);

            return result;
        }

        public long SecondStar()
        {
            var result = Play2(Input, 1_000_000, 10_000_000);
            return result;
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
        [InlineData("389125467", 10, "92658374")]
        [InlineData("389125467", 100, "67384529")]
        public void FirstStarExample(string input, int times, string expected)
        {
            var output = Play(input, times);

            Assert.Equal(expected, output);
        }

        [Fact]
        public void SecondStarExample()
        {
            var cupCount = 1_000_000;
            var times = cupCount * 10;
            var result = Play2("389125467", cupCount, times);

            Assert.Equal(149245887792, result);
        }
    }
}
