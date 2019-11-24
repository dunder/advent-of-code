using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 5: Doesn't He Have Intern-Elves For This? ---
    public class Day05
    {
        public static bool HasThreeVowels(string input)
        {
            const string vowels = "aeiou";
            return input.Count(c => vowels.Contains(c)) >= 3;
        }

        public static bool HasOneLetterTwiceInARow(string input)
        {
            var pattern = @"([a-z])\1";
            return Regex.IsMatch(input, pattern);
        }

        public static bool HasAnyForbidden(string input)
        {
            var forbidden = new[] {"ab", "cd", "pq", "xy"};

            return forbidden.Any(input.Contains);
        }

        public static bool HasPairOfTwoTwice(string input)
        {
            for (int i = 0; i < input.Length - 2; i++)
            {
                var pair = new string(new[] {input[i], input[i + 1]});
                if (input.Substring(i + 2).Contains(pair))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasLetterRepeatedWithOneBetween(string input)
        {
            var pattern = @"([a-z])[a-z]\1";
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsNice(string input)
        {
            return HasThreeVowels(input) && HasOneLetterTwiceInARow(input) && !HasAnyForbidden(input);
        }

        public static bool IsNice2(string input)
        {
            return HasPairOfTwoTwice(input) && HasLetterRepeatedWithOneBetween(input);
        }

        public static int FirstStar()
        {
            var input = ReadLineInput();

            return input.Count(IsNice);
        }

        public static int SecondStar()
        {
            var input = ReadLineInput();

            return input.Count(IsNice2);
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(238, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(69, result);
        }

        [Theory]
        [InlineData("aei")]
        [InlineData("xazegov")]
        [InlineData("aeiouaeiouaeiou")]
        public void HasThreeVowels_NiceExamples(string input)
        {
            var hasThreeVowels = HasThreeVowels(input);
            Assert.True(hasThreeVowels);
        }

        [Theory]
        [InlineData("klmn")]
        [InlineData("abcde")]
        [InlineData("mate")]
        public void HasThreeVowels_NaughtyExamples(string input)
        {
            var hasThreeVowels = HasThreeVowels(input);
            Assert.False(hasThreeVowels);
        }        
        
        [Theory]
        [InlineData("xx")]
        [InlineData("abcdde")]
        [InlineData("aabbccdd")]
        public void HasOneLetterTwiceInARow_NiceExamples(string input)
        {
            var hasThreeVowels = HasOneLetterTwiceInARow(input);
            Assert.True(hasThreeVowels);
        }

        [Theory]
        [InlineData("ugknbfddgicrmopn", true)]
        [InlineData("aaa", true)]
        [InlineData("jchzalrnumimnmhp", false)]
        [InlineData("haegwjzuvuyypxyu", false)]
        [InlineData("dvszwmarrgswjxmb", false)]
        public void FirstStar_Examples(string input, bool expectedNice)
        {
            var isNice = IsNice(input);

            Assert.Equal(expectedNice, isNice);
        }

        [Theory]
        [InlineData("xyxy", true)]
        [InlineData("aabcdefgaa", true)]
        [InlineData("aaa", false)]
        public void TwoLettersTwice_Examples(string input, bool expected)
        {
            var actual = HasPairOfTwoTwice(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("xyx")]
        [InlineData("abcdefeghi")]
        [InlineData("aaa")]
        public void HasLetterRepeatedWithOneBetween_Examples(string input)
        {
            var found = HasLetterRepeatedWithOneBetween(input);

            Assert.True(found);
        }

        [Theory]
        [InlineData("qjhvhtzxzqqjkmpb", true)]
        [InlineData("xxyxx", true)]
        [InlineData("uurcxstgmygtbstg", false)]
        [InlineData("ieodomkazucvgmuy", false)]
        public void SecondStar_Examples(string input, bool expectedNice)
        {
            var isNice = IsNice2(input);

            Assert.Equal(expectedNice, isNice);
        }
    }
}
