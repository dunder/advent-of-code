using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facet.Combinatorics;

namespace Solutions.Event2018.Day02
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day02;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = Checksum(input);
            return result.ToString();
        }

        public override string SecondStar()
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


            for (int i = 0; i < first.Length;i++)
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
    }
}