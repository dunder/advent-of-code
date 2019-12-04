using System;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;
using Xunit;


namespace Solutions.Event2019
{
    // --- Day 4: Secure Container ---
    public class Day04
    {
        private const string Input = "108457-562041";

        public static (int, int) ParseRange(string input)
        {
            var parts = input.Split('-').Select(int.Parse).ToList();
            return (parts[0], parts[1]);
        }

        public static int CountMatchingPasswords(int from, int to, Func<int, bool> ruleSet)
        {
            return Enumerable.Range(from, to - from + 1).Where(ruleSet).Count();
        }

        public static bool HasDouble(int password)
        {
            var sPassword = password.ToString();
            return Regex.IsMatch(sPassword, @"([0-9])\1");
        }

        public static bool HasDoubleNonGroup(int password)
        {
            return password.ToString()
                .Segment((current, previous, i) => !current.Equals(previous))
                .Any(g => g.Count() == 2);
        }

        public static bool IsIncreasing(int password)
        {
            return password.ToString().Segment((current, previous, i) => current < previous).Count() == 1;
        }

        public int FirstStar()
        {
            var (from, to) = ParseRange(Input);
            return CountMatchingPasswords(from, to, password => HasDouble(password) && IsIncreasing(password));
        }

        public int SecondStar()
        {
            var (from, to) = ParseRange(Input);

            return CountMatchingPasswords(from, to, password => HasDoubleNonGroup(password) && IsIncreasing(password));
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(2779, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1972, SecondStar());
        }

        [Theory]
        [InlineData(123444)]
        public void HasDoubleNonGroupNegativeTest(int password)
        {
            bool hasDoubleNonGroup = HasDoubleNonGroup(password);
            Assert.False(hasDoubleNonGroup);
        }

        [Theory]
        [InlineData(112233)]
        [InlineData(111122)]
        public void HasDoubleNonGroupPositiveTest(int password)
        {
            bool hasDoubleNonGroup = HasDoubleNonGroup(password);
            Assert.True(hasDoubleNonGroup);
        }

        [Theory]
        [InlineData(123456)]
        [InlineData(111122)]
        public void IsIncreasingPositiveTest(int password)
        {
            bool isIncreasing = IsIncreasing(password);
            Assert.True(isIncreasing);
        }        
        
        [Theory]
        [InlineData(121234)]
        [InlineData(345676)]
        public void IsIncreasingNegativeTest(int password)
        {
            bool isIncreasing = IsIncreasing(password);
            Assert.False(isIncreasing);
        }
    }
}
