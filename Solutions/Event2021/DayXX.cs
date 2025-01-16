using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2021
{
    // --- Day X: Phrase ---
    public class DayXX
    {
        private readonly ITestOutputHelper output;

        public DayXX(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static int Problem1(IList<string> input)
        {
            return 0;
        }

        private static int Problem2(IList<string> input)
        {
            return 0;
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2021")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem2(exampleInput));
        }
    }
}
