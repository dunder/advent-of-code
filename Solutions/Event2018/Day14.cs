using System;
using System.Linq;
using System.Reflection.Metadata;
using Shared.Extensions;
using Xunit;
using static Solutions.InputReader;

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
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }

        public static long Score(int input, int next)
        {
            var scoreBoard = new[] {3, 7};


            var elf1 = 0;
            var elf2 = 1;


            while (scoreBoard.Length < input + next)
            {
                var elf1Score = scoreBoard[elf1];
                var elf2Score = scoreBoard[elf2];

                var newScores = (elf1Score + elf2Score).ToString().Split().Select(int.Parse).ToArray();

                scoreBoard = scoreBoard.Concat(newScores).ToArray();

                elf1 = newScores.WrappedIndex(elf1Score + 1);
                elf2 = newScores.WrappedIndex(elf2Score + 1);
            }

            return scoreBoard.Skip(input).Take(next).Sum();
        }

        public static int[] GenerateNewScoreBoard(int[] initialScoreBoard, int iterations)
        {
            var elf1 = 0;
            var elf2 = 1;

            var scoreBoard = initialScoreBoard;

            for (int _ = 1; _ <= iterations; _++)
            {
                var elf1Score = scoreBoard[elf1];
                var elf2Score = scoreBoard[elf2];

                var newScores = (elf1Score + elf2Score).ToString().Split().Select(int.Parse).ToArray();

                scoreBoard = scoreBoard.Concat(newScores).ToArray();

                elf1 = newScores.WrappedIndex(elf1Score + 1);
                elf2 = newScores.WrappedIndex(elf2Score + 1);
            }

            return scoreBoard;
        }

        [Fact]
        public void FirstStartTest()
        {
            var result = Score(9, 10);

            Assert.Equal("5158916779", result.ToString());
            // 
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("", actual);
        }
    }
}