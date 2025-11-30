using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2022
{
    // --- Day 2: Rock Paper Scissors ---
    public class Day02
    {
        private readonly ITestOutputHelper output;

        public Day02(ITestOutputHelper output)
        {
            this.output = output;
        }

        private enum Shape { Rock, Paper, Scissors }
        private enum Outcome { Won, Lost, Tie }

        private int Score(Shape shape, Outcome outcome)
        {
            var outcomeScore = outcome switch
            {
                Outcome.Won => 6,
                Outcome.Tie => 3,
                Outcome.Lost => 0,
                _ => throw new ArgumentException($"Unknown outcome: {outcome}")
            };

            var shapeScore = shape switch
            {
                Shape.Rock => 1,
                Shape.Paper => 2,
                Shape.Scissors => 3,
                _ => throw new ArgumentException($"Unknown shape: {shape}")
            };

            return shapeScore + outcomeScore;
        }

        private int TotalScore(IList<string> input)
        {
            var totalScore = 0;

            foreach (var round in input)
            {
                var parts = round.Split(" ");
                var opponentsMove = parts[0];
                var opponent = opponentsMove switch
                {
                    "A" => Shape.Rock,
                    "B" => Shape.Paper,
                    "C" => Shape.Scissors,
                    _ => throw new ArgumentException($"Unknown shape: {opponentsMove}")
                };

                var yourMove = parts[1];
                var you = yourMove switch
                {
                    "X" => Shape.Rock,
                    "Y" => Shape.Paper,
                    "Z" => Shape.Scissors,
                    _ => throw new ArgumentException($"Unknown shape: {yourMove}")
                };

                var outcome = (opponent, you) switch
                {
                    _ when opponent == you => Outcome.Tie,
                    (Shape.Rock, Shape.Scissors) => Outcome.Lost,
                    (Shape.Rock, Shape.Paper) => Outcome.Won,
                    (Shape.Paper, Shape.Rock) => Outcome.Lost,
                    (Shape.Paper, Shape.Scissors) => Outcome.Won,
                    (Shape.Scissors, Shape.Rock) => Outcome.Won,
                    (Shape.Scissors, Shape.Paper) => Outcome.Lost,
                    _ => throw new InvalidOperationException($"Unexpected combination of shapes: {opponent}, {you}")
                };

                totalScore += Score(you, outcome);
            }

            return totalScore;
        }

        private int TotalScore2(IList<string> input)
        {
            var totalScore = 0;

            foreach (var round in input)
            {
                var parts = round.Split(" ");

                var opponentsMove = parts[0];
                var opponent = opponentsMove switch
                {
                    "A" => Shape.Rock,
                    "B" => Shape.Paper,
                    "C" => Shape.Scissors,
                    _ => throw new ArgumentException($"Unknown shape: {opponentsMove}")

                };

                var yourMove = parts[1];
                var expectedOutcome = yourMove switch
                {
                    "X" => Outcome.Lost,
                    "Y" => Outcome.Tie,
                    "Z" => Outcome.Won,
                    _ => throw new ArgumentException($"Unknown outcome: {yourMove}")
                };

                var you = (opponent, expectedOutcome) switch
                {
                    (_, Outcome.Tie) => opponent,
                    (Shape.Rock, Outcome.Won) => Shape.Paper,
                    (Shape.Rock, Outcome.Lost) => Shape.Scissors,
                    (Shape.Paper, Outcome.Won) => Shape.Scissors,
                    (Shape.Paper, Outcome.Lost) => Shape.Rock,
                    (Shape.Scissors, Outcome.Won) => Shape.Rock,
                    (Shape.Scissors, Outcome.Lost) => Shape.Paper,
                    _ => throw new InvalidOperationException($"Unexpected combination of shape and outcome: {opponent}, {expectedOutcome}")

                };

                totalScore += Score(you, expectedOutcome);
            }

            return totalScore;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return TotalScore(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return TotalScore2(input);
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarTest()
        {
            Assert.Equal(13924, FirstStar());
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarTest()
        {
            Assert.Equal(13448, SecondStar());
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "A Y",
                "B X",
                "C Z"
            };

            Assert.Equal(15, TotalScore(example));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "A Y",
                "B X",
                "C Z"
            };

            Assert.Equal(12, TotalScore2(example));
        }
    }
}
