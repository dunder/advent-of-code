using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;


namespace Solutions.Event2020
{
    // 
    public class Day15
    {
        private readonly ITestOutputHelper output;

        private static readonly List<int> Numbers = new List<int> { 16, 11, 15, 0, 1, 7 };
        public Day15(ITestOutputHelper output)
        {
            this.output = output;
        }

        private int GetPreviouslySpoken(List<int> spoken, int lastSpoken)
        {
            for (int i = spoken.Count - 2; i >= 0; i--)
            {
                if (spoken[i] == lastSpoken)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetPreviouslySpoken2(Dictionary<int, int> spokenBefore, int lastSpoken)
        {
            if (spokenBefore.ContainsKey(lastSpoken))
            {
                return spokenBefore[lastSpoken];
            }
            return -1;
        }

        private int Run(List<int> numbers, int to)
        {
            var spoken = new List<int>();
            var spokenBeforeRounds = new Dictionary<int, List<int>>();


            foreach (var turn in Enumerable.Range(0, to))
            {
                if (turn < numbers.Count)
                {
                    var spokenNumber = numbers[turn];
                    spoken.Add(spokenNumber);
                    spokenBeforeRounds.Add(spokenNumber, new List<int> { turn });
                }
                else
                {
                    var lastSpoken = spoken[spoken.Count - 1];
                    var previouslySpoken = -1;
                    if (spokenBeforeRounds.ContainsKey(lastSpoken))
                    {
                        var spokenBefore = spokenBeforeRounds[lastSpoken];
                        if (spokenBefore.Count == 1)
                        {
                            previouslySpoken = spokenBefore[0];
                        }
                        else
                        {
                            previouslySpoken = spokenBefore[spokenBefore.Count - 2];
                        }
                    }

                    if (previouslySpoken == -1)
                    {
                        spoken.Add(0);
                        spokenBeforeRounds.Add(0, new List<int> { turn });
                    }
                    else
                    {
                        var nextSpoken = turn - 1 - previouslySpoken;
                        spoken.Add(nextSpoken);

                        if (spokenBeforeRounds.ContainsKey(nextSpoken))
                        {
                            var spokenBefore = spokenBeforeRounds[nextSpoken];
                            spokenBefore.Add(turn);
                        }
                        else
                        {
                            spokenBeforeRounds.Add(nextSpoken, new List<int> { turn });
                        }
                    }
                }
            }

            return spoken[spoken.Count - 1];
        }

        public int FirstStar()
        {
            return Run(Numbers, 2020);
        }

        public int SecondStar()
        {
            return Run(Numbers, 30000000);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(662, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(37312, SecondStar());
        }

        [Fact]
        public void Example1()
        {
            var lastSpoken = Run(new List<int> { 0, 3, 6 }, 2020);

            Assert.Equal(436, lastSpoken);
        }

        [Fact]
        public void Example2()
        {
            var lastSpoken = Run(new List<int> { 1, 3, 2 }, 2020);

            Assert.Equal(1, lastSpoken);
        }

        [Fact]
        public void Example3()
        {
            var lastSpoken = Run(new List<int> { 1, 2, 3 }, 2020);

            Assert.Equal(27, lastSpoken);
        }

        [Fact]
        public void Example4()
        {
            var lastSpoken = Run(new List<int> { 2, 3, 1 }, 2020);

            Assert.Equal(78, lastSpoken);
        }
    }
}
