using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 12: JSAbacusFramework.io ---
    public class Day12
    {
        public static int SumNumbers(string input)
        {
            var expression = new Regex(@"-?\d+");
            return expression.Matches(input).Sum(m => int.Parse(m.Value));
        }
        public static int FirstStar()
        {
            var input = ReadInput();
            return SumNumbers(input);
        }

        public static int SecondStar()
        {
            var input = ReadInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(-1, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData(@"[1,2,3]", 6)]
        [InlineData(@"{""a"":2,""b"":4}", 6)]
        [InlineData(@"[[[3]]]", 3)]
        [InlineData(@"{""a"":{""b"":4},""c"":-1}", 3)]
        [InlineData(@"{""a"":[-1,1]}", 0)]
        [InlineData(@"[-1,{""a"":1}]", 0)]
        [InlineData(@"[]", 0)]
        [InlineData(@"{}", 0)]
        public void FirstStarExamples(string input, int expectedSum)
        {
            var sum = SumNumbers(input);
            Assert.Equal(expectedSum, sum);
        }

    }
}
