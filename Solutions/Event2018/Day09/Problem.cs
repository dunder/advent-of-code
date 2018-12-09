using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2018.Day09
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day09;

        public override string FirstStar()
        {
            var score = WinnerScore(473, 70904);
            return score.ToString();
        }

        public override string SecondStar()
        {
            var score = WinnerScore(473, 70904*100);
            return score.ToString();
        }

        public static long WinnerScore(int players, int lastMarblePoint)
        {
            var board = new List<int> {0, 1};
            var circle = new LinkedList<int>();

            circle.AddFirst(1);
            circle.AddFirst(0);
            var current = circle.Last;

            var currentMarbleIndex = 1;
            var scores = new Dictionary<int, long>();
            var player = 1;
            for (int marble = 2; marble <= lastMarblePoint; marble++)
            {
                player++;
                if (player > players) player = 1;
                if (marble % 23 != 0)
                {
                    var next = current.Next ?? circle.First;
                    current = circle.AddAfter(next, marble);

                    //currentMarbleIndex = ClockwiseIndex(board, currentMarbleIndex, 2);
                    //board.Insert(currentMarbleIndex, marble);
                }
                else
                {
                    //var indexOfMarble7CounterClockwise = ClockwiseIndex(board, currentMarbleIndex, -7);

                    for (int i = 0; i < 7; i++)
                    {
                        current = current.Previous;
                        if (current == null)
                        {
                            current = circle.Last;
                        }
                    }

                    //var score = marble + board[indexOfMarble7CounterClockwise];
                    var score = marble + current.Value;
                    var tmp = current.Next;
                    circle.Remove(current);
                    current = tmp;
                    

                    //board.RemoveAt(indexOfMarble7CounterClockwise);
                    //currentMarbleIndex = indexOfMarble7CounterClockwise;
                    //if (currentMarbleIndex >= board.Count)
                    //{
                    //    currentMarbleIndex = 0;
                    //}

                    if (!scores.ContainsKey(player))
                    {
                        scores.Add(player, score);
                    }
                    else
                    {
                        scores[player] += score;
                    }
                }
            }

            return scores.Values.Max();
        }

        public static int ClockwiseIndex<T>(List<T> list, int index, int stepsRight)
        {
            int wrappedIndex = index + stepsRight;
            int count = list.Count;
            if (wrappedIndex < 0)
            {
                wrappedIndex = count - Math.Abs(wrappedIndex);
            }
            return wrappedIndex > count ? wrappedIndex % count : wrappedIndex;
        }
    }
}