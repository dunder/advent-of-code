using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day10 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            string[] input = File.ReadAllLines(@".\Day10\input.txt");
            int result = Robots.BotComparing(input);

            Assert.Equal(161, result);
            _output.WriteLine($"Day 10 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day10\input.txt");

            var result = 0;

            Assert.Equal(133163, result);
            _output.WriteLine($"Day 10 problem 2: {result}");
        }
    }

    public class Robots {
        public static int BotComparing(string[] input, int value1, int value2) {
            var numberExpression = new Regex(@"\d+");
            var handoverExpressionLow = new Regex(@"low to (output|bot) (\d+)");
            var handoverExpressionHigh = new Regex(@"high to (output|bot) (\d+)");

            Dictionary<int, Bot> bots = new Dictionary<int, Bot>();

            foreach (var instruction in input) {
                if (instruction.StartsWith("value")) {
                    var data = numberExpression.Matches(instruction);
                    var value = int.Parse(data[0].Value);
                    var bot = int.Parse(data[1].Value);
                    if (bots.ContainsKey(bot)) {
                        bots[bot].Take(value);
                    } else {
                        bots.Add(bot, new Bot(value));
                    }
                }
            }

            foreach (var instruction in input) {
                
                if(instruction.StartsWith("bot")) {
                    var giver = int.Parse(numberExpression.Match(instruction).Value);

                    var lowData = handoverExpressionLow.Match(instruction);
                    var lowDataReceiver = lowData.Groups[1].Value;
                    var lowDataValue = int.Parse(lowData.Groups[2].Value);
                    if (lowDataReceiver.Equals("bot")) {
                        bots[lowDataValue].Take(lowDataValue);
                    }

                    var hightData = handoverExpressionHigh.Match(instruction);
                    var hightDataReceiver = hightData.Groups[1].Value;
                    var hightDataValue = int.Parse(hightData.Groups[2].Value);
                    if (hightDataReceiver.Equals("bot")) {
                        bots[hightDataValue].Take(hightDataValue);
                    }
                }
            }
        }

        private class Bot {
            public Bot(int value) {
                Value = value;
            }

            public void Take(int value) {
                if (value > Value) {
                    Low = Value;
                    Value = value;
                } else {
                    Low = value;
                }
            }

            public int Low { get; private set; }
            public int Value { get; private set; }
        }

    }

}
