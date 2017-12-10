using Xunit;

namespace Y2017.Day08 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {
            string[] input = {
               "b inc 5 if a > 1",
               "a inc 1 if b < 5",
               "c dec -10 if a >= 1",
               "c inc -20 if c == 10"
            };

            (var highestFinal, var highestStored) = Interpreter.LargestRegisterCount(input);

            Assert.Equal(1, highestFinal);
            Assert.Equal(10, highestStored);
        }
    }
}
