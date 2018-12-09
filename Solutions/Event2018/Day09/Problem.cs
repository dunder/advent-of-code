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
            var circle = new LinkedList<int>();
            circle.AddFirst(1);
            circle.AddFirst(0);

            var current = circle.Last;

            var scores = new Dictionary<int, long>();
            for (var marble = 2; marble <= lastMarblePoint; marble++)
            {
                var player = marble % players;
                if (player > players) player = 1;
                if (marble % 23 != 0)
                {
                    var next = current?.Next ?? circle.First;
                    current = circle.AddAfter(next, marble);
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        current = current.Previous ?? circle.Last;
                    }
                    var score = marble + current.Value;
                    var tmp = current.Next;
                    circle.Remove(current);
                    current = tmp;
                    
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
    }
}