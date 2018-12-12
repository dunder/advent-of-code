using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facet.Combinatorics;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day02 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day02;
        public string Name => "Inventory Management System";

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = Checksum(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = FindSimilar(input);
            return result;
        }

        public static string FindSimilar(IList<string> input)
        {
            var combinations = new Combinations<string>(input, 2);

            var combinationsOrderedBySimilarity = combinations.OrderByDescending(c => LetterSimilarity(c.First(), c.Last()));

            var sameLetters = new StringBuilder();

            var mostSimilarCombination = combinationsOrderedBySimilarity.First();
            var first = mostSimilarCombination.First();
            var second = mostSimilarCombination.Last();


            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] == second[i])
                {
                    sameLetters.Append(first[i]);
                }
            }

            return sameLetters.ToString();
        }

        public static int Checksum(IList<string> input)
        {
            int twoCounter = 0;
            int threeCounter = 0;

            foreach (var value in input)
            {
                if (HasLetterExactlyTimes(value, 2))
                {
                    twoCounter++;
                }

                if (HasLetterExactlyTimes(value, 3))
                {
                    threeCounter++;
                }
            }

            return twoCounter * threeCounter;
        }

        public static bool HasLetterExactlyTimes(string input, int count)
        {
            var letters = new Dictionary<char, int>();

            foreach (var letter in input)
            {
                if (letters.ContainsKey(letter))
                {
                    letters[letter] += 1;
                }
                else
                {
                    letters.Add(letter, 1);
                }
            }
            return letters.Values.Any(i => i == count);
        }


        public static int LetterSimilarity(string input1, string input2)
        {
            int count = 0;
            for (int i = 0; i < input1.Length; i++)
            {
                if (input1[i] == input2[i])
                {
                    count++;
                }
            }

            return count;
        }

        [Theory]
        [InlineData("abcdef", 2, false)]
        [InlineData("abcdef", 3, false)]
        [InlineData("bababc", 2, true)]
        [InlineData("bababc", 3, true)]
        [InlineData("abbcde", 2, true)]
        [InlineData("abbcde", 3, false)]
        [InlineData("abcccd", 2, false)]
        [InlineData("abcccd", 3, true)]
        [InlineData("aabcdd", 2, true)]
        [InlineData("aabcdd", 3, false)]
        [InlineData("abcdee", 2, true)]
        [InlineData("abcdee", 3, false)]
        [InlineData("ababab", 2, false)]
        [InlineData("ababab", 3, true)]
        public void HasLetterExactly(string input, int count, bool expected)
        {
            var actual = HasLetterExactlyTimes(input, count);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string> { "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" };

            var checksum = Checksum(input);

            Assert.Equal(4 * 3, checksum);
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string> { "abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz" };

            var similar = FindSimilar(input);

            Assert.Equal("fgij", similar);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("7470", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("kqzxdenujwcstybmgvyiofrrd", actual);
        }


    }
}
