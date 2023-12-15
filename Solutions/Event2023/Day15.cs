using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 15: Lens Library ---
    public class Day15
    {
        private readonly ITestOutputHelper output;

        public Day15(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<string> Parse(string input)
        {
            return input.Split(",").ToList();
        }

        private static int CurrentValue(int current, char code)
        {
            int ascii = code;
            current = current + ascii;
            current = current * 17;
            current = current % 256;
            return current;
        }

        public static int Hash(string input)
        {
            return input.Aggregate(0, (current, c) => CurrentValue(current, c));
        }

        private int SumOfHashes(string input)
        {
            var values = Parse(input);
            return values.Select(Hash).Sum();
        }

        private record Lens(string Label, int FocalLength);

        private void RemoveLens(Dictionary<int, List<Lens>> Content, string label)
        {
            var box = Hash(label);

            if (Content.ContainsKey(box))
            {
                Content[box] = Content[box].Where(box => box.Label != label).ToList();
            }
        }

        private void AddOrRemoveLens(Dictionary<int, List<Lens>> Content, string label, int focusLength)
        {
            var box = Hash(label);

            if (Content.TryGetValue(box, out List<Lens> lenses))
            {
                var newLens = new Lens(label, focusLength);

                int index = lenses.FindIndex(lens => lens.Label == label);
                if (index >= 0)
                {
                    lenses[index] = newLens;
                }
                else
                {
                    lenses.Add(newLens);
                }
            }
            else
            {
                Content.Add(box, new List<Lens> { new Lens(label, focusLength) });
            }
        }

        private int SumOfFocusingPower(string input)
        {
            var values = Parse(input);

            var boxSetup = new Dictionary<int, List<Lens>>();

            foreach (var operation in values)
            {
                if (operation.EndsWith("-"))
                {
                    var label = operation[..^1];
                    RemoveLens(boxSetup, label);
                }
                else
                {
                    var parts = operation.Split('=');
                    var label = parts[0];
                    var focusLength = int.Parse(parts[1]);
                    AddOrRemoveLens(boxSetup, label, focusLength);
                }
            }

            int FocusingPower((int box, int slot, int focalLength) lens)
            {
                return (lens.box + 1) * lens.slot * lens.focalLength;
            }

            return boxSetup
                .Select(kvp => kvp.Value.Select((lens, slot) => (kvp.Key, slot + 1, lens.FocalLength)))
                .SelectMany(x => x)
                .Select(FocusingPower)
                .Sum();
        }

        public int FirstStar()
        {
            var input = ReadInput();
            return SumOfHashes(input);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            return SumOfFocusingPower(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(505427, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(243747, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = "HASH";

            Assert.Equal(52, Hash(example));

            var sequence = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";

            Assert.Equal(1320, SumOfHashes(sequence));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";

            Assert.Equal(145, SumOfFocusingPower(example));
        }
    }
}
