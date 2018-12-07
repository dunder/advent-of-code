using System.Collections.Generic;
using Xunit;

namespace Solutions.Event2016.Day23
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            var instructions = new List<string>
            {
                "cpy 2 a",
                "tgl a",
                "tgl a",
                "tgl a",
                "cpy 1 a",
                "dec a",
                "dec a"
            };

            Dictionary<char, int> registers = new Dictionary<char, int>
            {
                {'a', 0 },
                {'b', 0 },
                {'c', 0 },
                {'d', 0 },
            };

            var a = Problem.RegisterAfterInstructions(instructions, registers, 'a');

            Assert.Equal(3, a);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("11640", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("479008200", actual);
        }
    }
}
