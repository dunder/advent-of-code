using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static string RemoveEnclosingRedObjects(string input)
        {
            var reduced = new StringBuilder(input);

            var openingBraces = new Stack<int>();

            for (int i = 0; i < reduced.Length; i++)
            {
                char c = reduced[i];

                if (c == '{')
                {
                    openingBraces.Push(i);
                }

                if (c == '}')
                {
                    int startIndex = openingBraces.Pop();
                    var currentObject = reduced.ToString().Substring(startIndex + 1, i - startIndex);
                    if (currentObject.Contains(@":""red"))
                    {
                        reduced = reduced.Remove(startIndex + 1, i - startIndex - 1);
                        i = 0;
                    }
                }
            }

            return reduced.ToString();
        }
        public static int FirstStar()
        {
            var input = ReadInput();
            return SumNumbers(input);
        }

        public static int SecondStar()
        {
            var input = ReadInput();
            return SumNumbers(RemoveEnclosingRedObjects(input));
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(156366, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();
            Assert.Equal(96852, result);
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

        [Theory]
        [InlineData(@"[1,2,3]", 6)]
        [InlineData(@"[1,{""c"":""red"",""b"":2},3]", 4)]
        [InlineData(@"{""d"":""red"",""e"":[1,2,3,4],""f"":5}", 0)]
        [InlineData(@"[1,""red"",5]", 6)]
        public void SecondStarExamples(string input, int expectedSum)
        {
            var sum = SumNumbers(RemoveEnclosingRedObjects(input));
            Assert.Equal(expectedSum, sum);
        }
    }
}
