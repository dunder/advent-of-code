using System.Collections.Generic;
using Xunit;

namespace Solutions.Event2016.Day12
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            string[] input = {
                "cpy 41 a",
                "inc a",
                "inc a",
                "dec a",
                "jnz a 2",
                "dec a"
            };

            var registers = new Dictionary<char, int>
            {
                {'a', 0}
            };

            var result = Problem.RegisterAfterInstructions(input, registers, 'a');

            Assert.Equal(42, result);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("318009", actual);
        }

        [Trait("Category", "LongRunning")]  // 1 m 7 s
        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("9227663", actual);
        }
    }
}