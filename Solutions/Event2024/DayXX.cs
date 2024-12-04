using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day X: Phrase ---
    public class DayXX
    {
        private readonly ITestOutputHelper output;

        public DayXX(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "",
                ""
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }
    }
}
