using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 9: Sensor Boost ---
    public class Day09
    {
        public long FirstStar()
        {
            var input = ReadInput();
            var computer = IntCodeComputer.Load(input);
            computer.Input.Enqueue(1);
            computer.Execute();
            return computer.Output.Last();
        }

        public long SecondStar()
        {
            var input = ReadInput();
            var computer = IntCodeComputer.Load(input);
            computer.Input.Enqueue(2);
            computer.Execute();
            return computer.Output.Last();
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(3280416268, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(80210, SecondStar());
        }

        [Fact]
        public void FirstStarExample1()
        {
            var code = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";
            var computer = IntCodeComputer.Load(code);
            computer.Execute();
            Assert.Equal(code, string.Join(",", computer.Output));
        }

        [Fact]
        public void FirstStarExample2()
        {
            var code = "1102,34915192,34915192,7,4,7,99,0";
            var computer = IntCodeComputer.Load(code);
            computer.Execute();
            Assert.Equal(1219070632396864, computer.Output.Last());
        }

        [Fact]
        public void FirstStarExample3()
        {
            var code = "104,1125899906842624,99";
            var computer = IntCodeComputer.Load(code);
            computer.Execute();
            Assert.Equal(1125899906842624, computer.Output.Last());
        }
    }
}
