using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;


namespace Solutions.Event2019
{
    // 
    public class Day04
    {
        private const string Input = "108457-562041";

        public static (int, int) ParseRange(string input)
        {
            var parts = input.Split('-').Select(int.Parse).ToList();
            return (parts[0], parts[1]);
        }

        public static bool RuleSet1(int password)
        {
            return HasDouble(password) && Increasing(password);
        }

        public static bool RuleSet2(int password)
        {
            return HasDoubleNonGroup(password) && Increasing(password);
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
            var sPassword = password.ToString();
            char? cPrev = sPassword[0];

            var counts = new HashSet<int>();
            int counter = 1;
            for (int i = 1; i < sPassword.Length; i++)
            {
                char c = sPassword[i];
                if (c != cPrev)
                {
                    counts.Add(counter);
                    counter = 0;
                }

                if (i == sPassword.Length - 1)
                {
                    counts.Add(counter + 1);
                }

                counter++;
                cPrev = c;
            }

            return counts.Contains(2);
        }

        public static bool Increasing(int password)
        {
            var sPassword = password.ToString();
            var cPrev = '0';
            foreach (var c in sPassword)
            {
                if (c < cPrev)
                {
                    return false;
                }

                cPrev = c;
            }

            return true;
        }

        public int FirstStar()
        {
            var (from, to) = ParseRange(Input);
            return CountMatchingPasswords(from, to, RuleSet1);
        }

        public int SecondStar()
        {
            var (from, to) = ParseRange(Input);

            return CountMatchingPasswords(from, to, RuleSet2);
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
        public void SecondStarExample1(int password)
        {
            bool isDouble = HasDoubleNonGroup(password);
            Assert.False(isDouble);
        }

        [Theory]
        [InlineData(112233)]
        [InlineData(111122)]
        public void SecondStarExample2(int password)
        {
            bool isDouble = HasDoubleNonGroup(password);
            Assert.True(isDouble);
        }
    }
}
