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

        private static List<int> Repeat(List<int> input, int times)
        {
            var output = input;
            foreach (var _ in Enumerable.Range(1,times))
            {
                output.Concat(input);
            }

            return output;
        }

        private static string Run(List<int> input, int phases)
        {
            for (int phase = 1; phase <= phases; phase++)
            {
                var output = new List<int>();
                for (int outputPosition = 1; outputPosition <= input.Count; outputPosition++)
                {
                    var positionOutput = new List<int>();
                    for (int digit = 0; digit < input.Count; digit++)
                    {
                        var patternIndex = ((digit + 1) / (outputPosition)) % 4;
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
            foreach (var x in input.Take(8))
            {
                s.Append(x);
            }
            return s.ToString();
        }

        private static string Run2(string inputText, int phases, int repeat)
        {
            //var multipliedInput = string.Join("", Enumerable.Repeat(inputText, 10_000));
            var input = Parse(inputText);

            //var offset = int.Parse(string.Join("", input.Take(7).Select(c => int.Parse(c.ToString()))));
            //input = input.Skip(offset).Take(8).ToList();

            for (int phase = 1; phase <= phases; phase++)
            {
                var output = new List<int>();
                for (int outputPosition = 1; outputPosition <= 1; outputPosition++)
                {
                    var positionOutput = new List<int>();
                    for (int digit = 0; digit < input.Count; digit++)
                    {
                        var patternIndex = ((digit + 1) / (outputPosition)) % 4;
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
            foreach (var x in input/*.Skip(offset).Take(8)*/)
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
            return Run2(input, 100, 10_000);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal("78009100", FirstStar());
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

        [Fact]
        public void OffsetTest()
        {
            var input = "98765432109876543210";
            var offset = 7;
            var result = string.Join("", input.Skip(offset).Take(8).ToList());
            Assert.Equal("21098765", result);
        }

        [Fact]
        public void SecondStarExample1()
        {
            var input = "03036732577212944063491565474664";
            var first8 = Run2(input, 100, 10_000);

            Assert.Equal("84462026", first8);
        }
        
        [Fact]
        public void SecondStarExample2()
        {
            var input = "03036732577212944063491565474664";
            var first8 = Run2(input, 100, 10_000);

            Assert.Equal("78725270", first8);
        }
        
        [Fact]
        public void SecondStarExample3()
        {
            var input = "03036732577212944063491565474664";
            var first8 = Run2(input, 100, 10_000);

            Assert.Equal("53553731", first8);
        }
    }
}
