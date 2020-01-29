using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 7: Amplification Circuit ---
    public class Day07
    {
        public int SignalToThrusters(string code, IList<int> phaseSetting)
        {
            var computer1 = IntCodeComputer.Load(code, phaseSetting[0], 0);
            var computer2 = computer1.CreateSerial(phaseSetting[1]);
            var computer3 = computer2.CreateSerial(phaseSetting[2]);
            var computer4 = computer3.CreateSerial(phaseSetting[3]);
            var computer5 = computer4.CreateSerial(phaseSetting[4]);

            computer1.Execute(false);
            computer2.Execute(false);
            computer3.Execute(false);
            computer4.Execute(false);
            computer5.Execute(false);

            return (int)computer5.Output.Last();
        }

        public int MaxSignalToThrusters(string code, List<int> phases, Func<string, IList<int>, int> amplification)
        {
            var allPossiblePhaseSettings = phases.Permutations();
            return allPossiblePhaseSettings.Select(phaseSetting => amplification(code, phaseSetting)).Max();
        }

        public int SignalToThrustersWithFeedbackLoop(string code, IList<int> phaseSetting)
        {
            var computer1 = IntCodeComputer.Load(code, phaseSetting[0], 0);
            var computer2 = computer1.CreateSerial(phaseSetting[1]);
            var computer3 = computer2.CreateSerial(phaseSetting[2]);
            var computer4 = computer3.CreateSerial(phaseSetting[3]);
            var computer5 = computer4.CreateSerial(phaseSetting[4]);
            computer5.ConnectOutputTo(computer1);

            IntCodeComputer.ExecutionState computer5State;
            do
            {
                computer1.Execute(false);
                computer2.Execute(false);
                computer3.Execute(false);
                computer4.Execute(false);
                computer5State = computer5.Execute(false);
            } while (computer5State != IntCodeComputer.ExecutionState.Ready);

            return (int)computer5.Output.Last();
        }

        public int FirstStar()
        {
            var input = ReadInput();
            return MaxSignalToThrusters(input, new List<int> { 0, 1, 2, 3, 4 }, SignalToThrusters);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            return MaxSignalToThrusters(input, new List<int> { 5, 6, 7, 8, 9 }, SignalToThrustersWithFeedbackLoop); 
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(929800, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(15432220, SecondStar());
        }

        [Theory]
        [InlineData("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", "4,3,2,1,0", 43210)]
        [InlineData("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", "0,1,2,3,4", 54321)]
        [InlineData("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", "1,0,4,3,2", 65210)]
        public void FirstStarExamples(string input, string phaseSetting, int expectedSignal)
        {
            var signal = SignalToThrusters(input, phaseSetting.Split(',').Select(int.Parse).ToList());
            Assert.Equal(expectedSignal, signal);
        }        
        
        [Theory]
        [InlineData("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5", "9,8,7,6,5", 139629729)]
        [InlineData("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10", "9,7,8,5,6", 18216)]
        public void SecondStarExamples(string input, string phaseSetting, int expectedMax)
        {
            var signal = SignalToThrustersWithFeedbackLoop(input, phaseSetting.Split(',').Select(int.Parse).ToList());
            Assert.Equal(expectedMax, signal);
        }
    }
}
