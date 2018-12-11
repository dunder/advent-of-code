using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day04
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day04;

        public string FirstStar()
        {
            var notes = ReadLineInput();
            var guards = ParseGuards(notes);
            var result = SleepyGuardStrategy1(guards);
            return result.ToString();
        }

        public string SecondStar()
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

        [Fact]
        public void FirstStarExample()
        {
            var notes = new List<string>
            {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up"
            };

            var guards = ParseGuards(notes);

            var guard = guards[99];

            Assert.Equal(30, guard.TotalSleep);

            Assert.Equal(240, SleepyGuardStrategy1(guards));
            Assert.Equal(4455, SleepyGuardStrategy2(guards));
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("67558", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("78990", actual);
        }

    }
}
