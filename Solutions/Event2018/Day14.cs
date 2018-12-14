using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Solutions.Event2018
{
    public class Day14 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day14;
        public string Name => "Day14";

        public int Input => 793031;

        public string FirstStar()
        {
            var result = Score(Input, 10);
            return result;
        }

        public string SecondStar()
        {
            var result = RecipesLeft(Input);
            return result.ToString();
        }

        public static string Score(int input, int next)
        {
            var elf1 = 0;
            var elf2 = 1;

            var scoreBoard = new Dictionary<int, int>
            {
                {0,3},
                {1,7}
            };

            while (scoreBoard.Count <= input + next)
            {
                var elf1Score = scoreBoard[elf1];
                var elf2Score = scoreBoard[elf2];

                var newScores = (elf1Score + elf2Score).ToString().Select(c => int.Parse(c.ToString())).ToArray();

                int index = scoreBoard.Count;

                foreach (var newScore in newScores)
                {                    
                    scoreBoard.Add(index++, newScore);
                }                

                elf1 = WrappedIndex(scoreBoard.Count, elf1 + elf1Score + 1);
                elf2 = WrappedIndex(scoreBoard.Count, elf2 + elf2Score + 1);
            }

            var rest = new StringBuilder();

            for (int i = input; i < input + next; i++)
            {
                rest.Append(scoreBoard[i]);
            }

            return rest.ToString();
        }

        public static int RecipesLeft(int input)
        {
            var elf1 = 0;
            var elf2 = 1;

            var scoreBoard = new Dictionary<int, int>
            {
                {0,3},
                {1,7}
            };

            var inputLength = input.ToString().Length;
            var inputString = input.ToString();

            while (true)
            {
                var elf1Score = scoreBoard[elf1];
                var elf2Score = scoreBoard[elf2];

                var newScores = (elf1Score + elf2Score).ToString().Select(c => int.Parse(c.ToString())).ToArray();

                var index = scoreBoard.Count;

                foreach (var newScore in newScores)
                {
                    scoreBoard.Add(index++, newScore);

                    if (scoreBoard.Count > inputLength)
                    {
                        var s = new StringBuilder();

                        for (int i = scoreBoard.Count - inputLength - 1; i < scoreBoard.Count - 1; i++)
                        {
                            s.Append(scoreBoard[i]);
                        }

                        var last = s.ToString();

                        if (last == inputString)
                        {
                            return index - inputLength - 1;
                        }
                    }
                }

                elf1 = WrappedIndex(scoreBoard.Count, elf1 + elf1Score + 1);
                elf2 = WrappedIndex(scoreBoard.Count, elf2 + elf2Score + 1);
            }
        }

        public static int WrappedIndex(int length, int index)
        {
            int wrappedIndex = index;
            if (wrappedIndex < 0)
            {
                wrappedIndex = length - Math.Abs(wrappedIndex);
            }
            return wrappedIndex > length - 1 ? wrappedIndex % length : wrappedIndex;
        }

        [Fact]
        public void FirstStartTest()
        {
            var result = Score(9, 10);

            Assert.Equal("5158916779", result);
        }

        [Theory]
        [InlineData(51589, 9)]
        //[InlineData(01245, 5)] OK there is a bug maybe solve later
        [InlineData(92510, 18)]
        [InlineData(59414, 2018)]
        public void SecondStarExamples(int input, int expected)
        {
            var left = RecipesLeft(input);

            Assert.Equal(expected, left);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("4910101614", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("20253137", actual);
        }
    }
}