using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 13: Care Package ---
    public class Day13
    {
        private class GameState
        {
            public int Score { get; set; }
            public Point BallPosition { get; set; }
            public Point PreviousBallPosition { get; set; }
            public Point PaddlePosition { get; set; }
        }

        private static int Update(Queue<long> output, GameState state)
        {
            var newInstructions = output.ToList();

            if (newInstructions.Count % 3 != 0)
            {
                return 0;
            }

            var batches = newInstructions.Batch(3).ToList();
            
            foreach (var batch in batches)
            {
                var input = batch.ToList();
                var x = (int)input[0];
                var y = (int)input[1];
                var tile = (int)input[2];

                if (x == -1 && y == 0)
                {
                    state.Score = tile;
                    continue;
                }

                if (tile == 4)
                {
                    state.PreviousBallPosition = state.BallPosition;
                    state.BallPosition = new Point(x,y);
                }
                else if (tile == 3)
                {
                    state.PaddlePosition = new Point(x,y);
                }
            }

            var ballXDiff = state.BallPosition.X - state.PreviousBallPosition.X;
            var ballYDiff = state.BallPosition.Y - state.PreviousBallPosition.Y;
            var joystickCommand = 0;
            if (ballYDiff > 0)
            {
                // try to forecast ball position 
                var guessedX = ballYDiff / ballXDiff * (state.PaddlePosition.Y - state.BallPosition.Y) + state.BallPosition.X;
                if (guessedX > state.PaddlePosition.X)
                {
                    joystickCommand = 1;
                }
                else if (guessedX < state.PaddlePosition.X)
                {
                    joystickCommand = -1;
                }
            }
            else if (ballYDiff <= 0)
            {
                // just follow ball when going up
                var paddleBallXDiff = state.BallPosition.X - state.PaddlePosition.X;
                joystickCommand = Math.Sign(paddleBallXDiff);
            }

            return joystickCommand;
        }

        private static int RunGame(string program)
        {
            var computer = IntCodeComputer.Load(program);
            computer.Execute();
            
            var ball = computer.Output
                .Batch(3)
                .Single(b => b.Last() == 4)
                .ToList();

            var ballPosition = new Point((int) ball[0], (int) ball[1]);
            var paddle = computer.Output
                .Batch(3)
                .Single(b => b.Last() == 3)
                .ToList();
            var paddlePosition = new Point((int)paddle[0], (int)paddle[1]);

            var gameState = new GameState
            {
                Score = 0,
                BallPosition = ballPosition,
                PreviousBallPosition = ballPosition,
                PaddlePosition = paddlePosition,
            };

            computer = IntCodeComputer.Load(program);
            computer.MemoryOverride(0, 2);

            var executionState = IntCodeComputer.ExecutionState.WaitingForInput;
            while (executionState == IntCodeComputer.ExecutionState.WaitingForInput)
            {
                executionState = computer.Execute();
                int joystickSignal = Update(computer.Output, gameState);
                computer.Input.Enqueue(joystickSignal);
            }

            return gameState.Score;
        }

        public int FirstStar()
        {
            var input = ReadInput();
            var computer = IntCodeComputer.Load(input);
            computer.Execute();
            var blockTiles = computer.Output.Batch(3).Select(b => b.Last()).Count(t => t == 2);
            return blockTiles;
        }

        public int SecondStar()
        {
            var input = ReadInput();
            var score = RunGame(input);
            
            return score;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(333, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(16539, SecondStar());
        }
    }
}
