using System.Linq;
using System.Text;
using Xunit;

namespace Solutions.Event2015
{
    // --- Day 11: Corporate Policy ---
    public class Day11
    {
        private const string Input = "vzbxkghb";

        public static bool HasThreeStraight(string input)
        {
            for (int i = 0; i < input.Length - 2; i++)
            {
                char first = input[i];
                char second = input[i+1];
                char third = input[i+2];

                if (third - second == 1 && second - first == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasForbiddenLetters(string input)
        {
            return input.Any(c => c == 'i' || c == 'o' || c == 'l');
        }

        public static bool HasTwoPairs(string input)
        {
            char? firstFound = null;

            for (int i = 0; i < input.Length - 1; i++)
            {
                char first = input[i];
                char second = input[i + 1];

                if (first == second)
                {
                    if (firstFound == null)
                    {
                        firstFound = first;
                    }
                    else
                    {
                        if (first != firstFound)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static string Increment(string input)
        {
            var result = new StringBuilder(input);
            for (int i = result.Length - 1; i >= 0; i--)
            {
                bool wrap = false;

                char c = result[i];

                char cPlus = (char)(c + 1);

                if (cPlus > 'z')
                {
                    cPlus = 'a';
                    wrap = true;
                }

                result[i] = cPlus;

                if (!wrap)
                {
                    break;
                }
            }

            return result.ToString();
        }

        public static bool IsValidPassword(string attemptedPassword)
        {
            return HasThreeStraight(attemptedPassword) && !HasForbiddenLetters(attemptedPassword) && HasTwoPairs(attemptedPassword);
        }

        public static string NextPassword(string input)
        {
            var next = input;
            do
            {
                next = Increment(next);
            } while (!IsValidPassword(next));

            return next;
        }

        public static string FirstStar()
        {
            return NextPassword(Input);
        }

        public static string SecondStar()
        {
            return NextPassword(NextPassword(Input));
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal("vzbxxyzz", result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal("", result);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("def")]
        [InlineData("kjyklm")]
        public void HasThreeStraightTest(string input)
        {
            var threeStraight = HasThreeStraight(input);

            Assert.True(threeStraight);
        } 

        [Theory]
        [InlineData("abd")]
        [InlineData("matias")]
        [InlineData("boeno")]
        public void HasNotThreeStraightTest(string input)
        {
            var threeStraight = HasThreeStraight(input);

            Assert.False(threeStraight);
        }       
        
        [Theory]
        [InlineData("abcdefghi")]
        [InlineData("labcdefgh")]
        [InlineData("abcdefghijklmno")]
        [InlineData("abcdefghjkmno")]
        public void HasForbiddenLettersTest(string input)
        {
            var forbiddenLetters = HasForbiddenLetters(input);

            Assert.True(forbiddenLetters);
        }    
        
        [Theory]
        [InlineData("aabb")]
        [InlineData("bbcc")]
        [InlineData("abbcdeff")]
        public void HasTwoPairsTest(string input)
        {
            var twoPairs = HasTwoPairs(input);

            Assert.True(twoPairs);
        }
        
        [Theory]
        [InlineData("abab")]
        [InlineData("bbcde")]
        [InlineData("aabdeaa")]
        public void HasNotTwoPairsTest(string input)
        {
            var twoPairs = HasTwoPairs(input);

            Assert.False(twoPairs);
        }

        [Theory]
        [InlineData("abc", "abd")]
        [InlineData("abz", "aca")]
        [InlineData("xz", "ya")]
        public void IncrementTest(string input, string expected)
        {
            var incremented = Increment(input);

            Assert.Equal(expected, incremented);
        }

        [Theory]
        [InlineData("abcdefgh", "abcdffaa")]
        [InlineData("ghijklmn", "ghjaabcc")]
        public void NextPasswordTests(string input, string expected)
        {
            var nextPassword = NextPassword(input);

            Assert.Equal(expected, nextPassword);
        }
    }
}
