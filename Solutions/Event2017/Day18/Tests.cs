using Xunit;

namespace Solutions.Event2017.Day18 {
    public class Tests {
        [Fact]
        public static void Problem1_Example() {

            string[] input = {
                "set a 1 ",
                "add a 2 ",
                "mul a a ",
                "mod a 5 ",
                "snd a   ",
                "set a 0 ",
                "rcv a   ",
                "jgz a -1",
                "set a 1 ",
                "jgz a -2"
            };

            var answer = DuetAssembly.RecoveredFrequency(input);

            Assert.Equal(4, answer);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("4601", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("6858", actual);
        }
    }
}
