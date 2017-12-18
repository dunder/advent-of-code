using Xunit;

namespace Y2017.Day18 {
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
    }
}
