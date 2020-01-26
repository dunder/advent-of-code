using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 21: Springdroid Adventure ---
    public class Day21
    {
        public long ScriptWalk(string input)
        {
            var computer = IntCodeComputer.Load(input);
            var asciiProgram = new List<string>
            {
                // if A or B or C is a hole and D is not a hole
                "NOT A T\n",
                "NOT B J\n",
                "OR T J\n",
                "NOT C T\n",
                "OR T J\n",
                "NOT D T\n",
                "NOT T T\n",
                "AND T J\n",
                "WALK\n"
            };  

            computer.ExecuteAscii(asciiProgram);

            Console.Write(computer.OutputAscii);

            return computer.Output.Last();
        }

        public long ScriptRun(string input)
        {
            var computer = IntCodeComputer.Load(input);
            var asciiProgram = new List<string>
            {
                // if A or B or C is a hole and D is not a hole
                "NOT A T\n",
                "NOT B J\n",
                "OR T J\n",
                "NOT C T\n",
                "OR T J\n",
                "NOT D T\n",
                "NOT T T\n",
                "AND T J\n",
                // and H is not a hole (can not make second jump)
                "NOT H T\n",
                "NOT T T\n",
                "AND T J\n",
                // force jump if a is hole and d is not a hole
                "NOT A T\n",
                "AND D T\n",
                "OR T J\n",
                "RUN\n"
            };  

            computer.ExecuteAscii(asciiProgram);

            Console.Write(computer.OutputAscii);

            return computer.Output.Last();
        }


        public long FirstStar()
        {
            var input = ReadInput();
            return ScriptWalk(input);
        }

        public long SecondStar()
        {
            var input = ReadInput();
            return ScriptRun(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(19355862, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1140470745, SecondStar());
        }
    }
}
