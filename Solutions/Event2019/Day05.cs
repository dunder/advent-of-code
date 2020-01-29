using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 5: Sunny with a Chance of Asteroids ---
    public class Day05
    {
        public long FirstStar()
        {
            var input = ReadInput();
            var code = IntCodeComputer.Load(input, 1);
            code.Execute();
            return code.Output.Last();
        }

        public long SecondStar()
        {
            var input = ReadInput();
            var code = IntCodeComputer.Load(input, 5);
            code.Execute();
            return code.Output.Last();
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(7265618, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(7731427, SecondStar());
        }

        [Fact]
        public void ReadOpCodeTest()
        {
            int x = 12345;

            int op = x % 100;

            Assert.Equal(45, op);
        }

        [Fact]
        public void ReadFirstParameterTest()
        {
            int x = 12345;

            int p1 = (x / 100) % 10;

            Assert.Equal(3, p1);
        }
    }
}
