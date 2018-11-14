using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Solutions.Event2016.Day08
{
    public class Tests
    {
        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new[]
            {
                "rect 3x2",
                "rotate column x=1 by 1",
                "rotate row y=0 by 4"
            };
            var display = new Display(3, 7);
            var count = display.CountPixelsLit(input);

            Assert.Equal(6, count);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("123", actual);
        }

        [Fact]
        public void SecondStar()
        {
            string[] input = File.ReadAllLines(@".\Event2016\Day08\input.txt");
            var display = new Display(6, 50);
            display.SendInstructions(input);
            var result = display.Print();

            // needs manual verification

            _output.WriteLine($"Day 8 problem 2: {Environment.NewLine}{result}"); // AFBUPZBJPS
        }
    }
}