using System.Collections.Generic;
using Xunit;

namespace Solutions.Event2018.Day07
{
    public class Tests
    {
        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "Step C must be finished before step A can begin.",
                "Step C must be finished before step F can begin.",
                "Step A must be finished before step B can begin.",
                "Step A must be finished before step D can begin.",
                "Step B must be finished before step E can begin.",
                "Step D must be finished before step E can begin.",
                "Step F must be finished before step E can begin."
            };

            var order = Problem.Order(input);

            Assert.Equal("CABDFE", order);
        }
        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("", actual);
        }
    }
}
