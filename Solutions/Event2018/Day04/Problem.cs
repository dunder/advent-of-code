using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solutions.Event2018.Day04
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day04;

        public override string FirstStar()
        {
            var notes = ReadLineInput();
            var guards = ParseGuards(notes);
            var result = SleepyGuardStrategy1(guards);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var notes = ReadLineInput();
            var guards = ParseGuards(notes);
            var result = SleepyGuardStrategy2(guards);
            return result.ToString();
        }

        public static IDictionary<int, Guard> ParseGuards(IList<string> notes)
        {
            notes = notes
                .OrderBy(n => n.Substring(6, 2))
                .ThenBy(n => n.Substring(9, 2))
                .ThenBy(n => n.Substring(12, 2))
                .ThenBy(n => n.Substring(15, 2)).ToList();

            Regex minuteExpression = new Regex(@"\[.*:(\d{2})\]");
            Regex beginsShiftExpression = new Regex(@"Guard #(\d+) begins shift");
            Regex fallsAsleepExpression = new Regex(@"falls asleep");
            Regex wakesUpExpression = new Regex(@"wakes up");

            var guards = new Dictionary<int, Guard>();

            Guard currentGuard = null;
            int fellAsleepMinute = 0;

            foreach (var note in notes)
            {
                var timeMatch = minuteExpression.Match(note);
                var minute = int.Parse(timeMatch.Groups[1].Value);

                switch (note)
                {
                    case var begins when beginsShiftExpression.IsMatch(note):

                        var beginsMatch = beginsShiftExpression.Match(begins);
                        var id = int.Parse(beginsMatch.Groups[1].Value);

                        if (guards.ContainsKey(id))
                        {
                            currentGuard = guards[id];
                        }
                        else
                        {
                            currentGuard = new Guard
                            {
                                Id = id
                            };
                            guards.Add(id, currentGuard);
                        }
                        break;
                    case var _ when fallsAsleepExpression.IsMatch(note):
                        fellAsleepMinute = minute;
                        break;
                    case var _ when wakesUpExpression.IsMatch(note):
                        currentGuard?.AddSleepPeriod(fellAsleepMinute, minute);
                        break;
                    default:
                        continue;
                }
            }

            return guards;
        }

        public static int SleepyGuardStrategy1(IDictionary<int, Guard> guards)
        {
            var sleepyGuard = guards.OrderByDescending(g => g.Value.TotalSleep).First();
            return sleepyGuard.Value.Id * sleepyGuard.Value.SleepStatistics.mostSleepyMinute;
        }

        public static int SleepyGuardStrategy2(IDictionary<int, Guard> guards)
        {
            var sleepyGuard = guards.OrderByDescending(g => g.Value.SleepStatistics.frequency).First();
            return sleepyGuard.Value.Id * sleepyGuard.Value.SleepStatistics.mostSleepyMinute;
        }
    }

    public class Guard
    {
        public int Id { get; set; }
        public int[] Sleeping { get; set; } = new int[60];

        public void AddSleepPeriod(int from, int to)
        {
            for (int i = from; i < to; i++)
            {
                Sleeping[i]++;
                TotalSleep++;
            }
        }

        public int TotalSleep { get; set; }

        public (int mostSleepyMinute, int frequency) SleepStatistics
        {
            get
            {
                var max = 0;
                var maxIndex = 0;
                for (int i = 0; i < Sleeping.Length; i++)
                {
                    if (Sleeping[i] > max)
                    {
                        max = Sleeping[i];
                        maxIndex = i;
                    }
                }

                return (maxIndex, max);
            }
        }

        public override string ToString()
        {
            return $"#{Id} Total sleep: {TotalSleep} sleepy minute: {SleepStatistics.mostSleepyMinute} ({SleepStatistics.frequency})";
        }
    }
}