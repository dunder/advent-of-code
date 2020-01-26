using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 23: Category Six ---
    public class Day23
    {
        private long Run(string input)
        {
            var computers = new List<IntCodeComputer>();
            
            foreach (var i in Enumerable.Range(0,50))
            {
                var computer = IntCodeComputer.Load(input);
                computer.Input.Enqueue(i);
                computers.Add(computer);
            }

            while (true)
            {
                foreach (var computer in computers)
                {
                    if (!computer.Input.Any())
                    {
                        computer.Input.Enqueue(-1);
                    }
                    
                    computer.Execute();

                    while (computer.Output.Any())
                    {
                        int address = (int)computer.Output.Dequeue();
                        var x = computer.Output.Dequeue();
                        var y = computer.Output.Dequeue();

                        if (address == 255)
                        {
                            return y;
                        }

                        computers[address].Input.Enqueue(x);
                        computers[address].Input.Enqueue(y);
                    }
                }
            }
        }

        private long Run2(string input)
        {
            var computers = new List<IntCodeComputer>();
            
            foreach (var i in Enumerable.Range(0,50))
            {
                var computer = IntCodeComputer.Load(input);
                computer.Input.Enqueue(i);
                computers.Add(computer);
            }

            long? natX = null;
            long? natY = null;

            Stack<long> lastDeliveredY = new Stack<long>();

            while (true)
            {
                foreach (var computer in computers)
                {
                    if (!computer.Input.Any())
                    {
                        computer.Input.Enqueue(-1);
                    }
                    
                    computer.Execute();

                    while (computer.Output.Any())
                    {
                        int address = (int)computer.Output.Dequeue();
                        var x = computer.Output.Dequeue();
                        var y = computer.Output.Dequeue();

                        if (address == 255)
                        {
                            natX = x;
                            natY = y;
                            continue;
                        }

                        computers[address].Input.Enqueue(x);
                        computers[address].Input.Enqueue(y);
                    }
                }

                var idle = computers.All(c => !c.Input.Any() && !c.Output.Any());
                if (idle && natX.HasValue)
                {
                    computers[0].Input.Enqueue(natX.Value);
                    computers[0].Input.Enqueue(natY.Value);
                    if (lastDeliveredY.Any() && lastDeliveredY.Peek() == natY.Value)
                    {
                        return natY.Value;
                    }

                    lastDeliveredY.Push(natY.Value);
                }
            }
        }

        public long FirstStar()
        {
            var input = ReadInput();
            return Run(input);
        }

        public long SecondStar()
        {
            var input = ReadInput();
            return Run2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(23886, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(18333, SecondStar());
        }
    }
}
