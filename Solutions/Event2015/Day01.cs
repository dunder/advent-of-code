using System.Linq;
using Xunit;

using static Solutions.InputReader;


namespace Solutions.Event2015
{
    public class Day01
    {
        public int FinalFloor(string input)
        {
            return input.Count(c => c == '(') - input.Count(c => c == ')');
        }

        public int ReachesBasement(string input)
        {
            int currentFloor = 0;

            for (int position = 0; position < input.Length; position++)
            {
                var c = input[position];
                if (c == '(')
                {
                    currentFloor++;
                }
                else
                {
                    currentFloor--;
                }

                if (currentFloor == -1)
                {
                    return position + 1;
                }

            }

            return -1;
        }

        public int FirstStar()
        {
            var input = ReadInput();
            return FinalFloor(input);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            return ReachesBasement(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(74, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1795, SecondStar());
        }

        [Theory]
        [InlineData("(())", 0)]
        [InlineData("()()", 0)]
        [InlineData("(((", 3)]
        [InlineData("(()(()(", 3)]
        [InlineData("))(((((", 3)]
        [InlineData("())", -1)]
        [InlineData("))(", -1)]
        [InlineData(")))", -3)]
        [InlineData(")())())", -3)]
        public void ResultingFloorTests(string input, int expectedFinalFloor)
        {
            var finalFloor = FinalFloor(input);

            Assert.Equal(expectedFinalFloor, finalFloor);
        }

        [Theory]
        [InlineData(")", 1)]
        [InlineData("()())", 5)]
        public void ReachesBasementTests(string input, int expectedPosition)
        {
            var position = ReachesBasement(input);

            Assert.Equal(expectedPosition, position);
        }
    }
}
