using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 16: Flawed Frequency Transmission ---
    public class Day16
    {
        private static List<int> pattern = new List<int> {0,1,0,-1};

        private static List<int> Parse(string input)
        {
            return input.Select(x => int.Parse(x.ToString())).ToList();
        }

        private static string Run(List<int> input, int phases)
        {
            for (int phase = 1; phase <= phases; phase++)
            {
                var output = new List<int>();
                for (int outputPosition = 1; outputPosition <= input.Count; outputPosition++)
                {
                    var positionPattern = pattern
                        .SelectMany(d => Enumerable.Repeat(d, outputPosition))
                        .ToList();
                    var first = positionPattern[0];
                    positionPattern = positionPattern.Skip(1).ToList();
                    positionPattern.Add(first);

                    var positionOutput = new List<int>();
                    for (int digit = 0; digit < input.Count; digit++)
                    {
                        var patternIndex = digit > positionPattern.Count - 1 ? digit % positionPattern.Count : digit;
                        var patternValue = positionPattern[patternIndex];
                        var outputValue = input[digit];
                        var positionValue = outputValue * patternValue;
                        if (positionValue > 9)
                        {
                            positionValue = positionValue % 10;
                        }
                        positionOutput.Add(positionValue);
                    }

                    var outputForPosition = Math.Abs(positionOutput.Sum());
                    output.Add(outputForPosition > 9 ? outputForPosition % 10 : outputForPosition);
                }

                input = output;
            }

            var s = new StringBuilder();
            foreach (var x in input.Take(8))
            {
                s.Append(x);
            }
            return s.ToString();
        }

        private static string Run2(List<int> input, int phases)
        {
            var offset = int.Parse(string.Join("", input.Take(7).Select(c => int.Parse(c.ToString()))));
            
            for (int phase = 1; phase <= phases; phase++)
            {
                var output = new List<int>();
                for (int outputPosition = 1; outputPosition <= input.Count; outputPosition++)
                {
                    // mönstret måste upprepa sig med multipel mellan längd för input och output position * 4
                    var positionOutput = new List<int>();
                    for (int digit = 0; digit < input.Count; digit++)
                    {
                        var patternIndex = ((digit + 1) / outputPosition) % 4;
                        var patternValue = pattern[patternIndex];
                        var outputValue = input[digit];
                        var positionValue = outputValue * patternValue;
                        if (positionValue > 9)
                        {
                            positionValue = positionValue % 10;
                        }
                        positionOutput.Add(positionValue);
                    }

                    var outputForPosition = Math.Abs(positionOutput.Sum());
                    output.Add(outputForPosition > 9 ? outputForPosition % 10 : outputForPosition);
                }

                input = output;
            }

            var s = new StringBuilder();
            foreach (var x in input.Skip(offset).Take(8))
            {
                s.Append(x);
            }
            return s.ToString();
        }

        public string FirstStar()
        {
            var input = ReadInput();
            var digits = Parse(input);
            return Run(digits, 100);
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var multipliedInput = string.Join("", Enumerable.Repeat(input, 10_000));
            var digits = Parse(multipliedInput);
            return Run2(digits, 100);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal("", FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal("", SecondStar());
        }

        [Fact]
        public void FirstStarExample1()
        {
            var input = "12345678";
            var digits = Parse(input);
            var first8 = Run(digits, 1);

            Assert.Equal("48226158", first8);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = "12345678";
            var digits = Parse(input);
            var first8 = Run(digits, 2);

            Assert.Equal("34040438", first8);
        }

        private static int GetPatternValue(int outputPosition, int inputPosition)
        {
            
            return 0;
        }

        

        [Fact]
        public void SecondStarExample1()
        {
            var input = "03036732577212944063491565474664";
            var multipliedInput = string.Join("", Enumerable.Repeat(input, 10_000));
            var digits = Parse(multipliedInput);
            var first8 = Run2(digits, 100);

            Assert.Equal("34040438", first8);
        }
    }
}
