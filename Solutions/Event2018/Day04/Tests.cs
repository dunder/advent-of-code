using System.Collections.Generic;
using Xunit;

namespace Solutions.Event2018.Day04
{
    public class Tests
    {
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

            var guards = Problem.ParseGuards(notes);

            var guard = guards[99];

            Assert.Equal(30, guard.TotalSleep);

            Assert.Equal(240, Problem.SleepyGuardStrategy1(guards));
            Assert.Equal(4455, Problem.SleepyGuardStrategy2(guards));
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("67558", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("78990", actual); 
        }
    }
}
