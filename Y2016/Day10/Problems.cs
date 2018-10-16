using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            int result = Robots.FindBot(input, 17, 61);

            Assert.Equal(161, result);
            _output.WriteLine($"Day 10 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day10\input.txt");

            var result = Robots.CalculateOutputProduct(input);

            Assert.Equal(133163, result);
            _output.WriteLine($"Day 10 problem 2: {result}");
        }
    }

    public class Robots {
        public static int FindBot(string[] input, int lowTargetValue, int highTargetValue) {
           
            (Dictionary<int, Bot> bots, _) = RunInstructions(input);

            return bots.Values.Single(bot => bot.Low == lowTargetValue && bot.High == highTargetValue).Id;
        }

        public static int CalculateOutputProduct(string[] input)
        {
            var (_, outputs) = RunInstructions(input);

            return outputs.Values.Where(o => o.Id == 0 || o.Id == 1 || o.Id == 2).Aggregate(1, (x, robot) => x * robot.Value);
        }

        private static (Dictionary<int, Bot> bots, Dictionary<int, Output>) RunInstructions(string[] input) {
            Dictionary<int, Bot> bots = new Dictionary<int, Bot>();
            Dictionary<int, Output> outputs = new Dictionary<int, Output>();

            var inputRegex = new Regex(@"value (\d+) goes to bot (\d+)");
            var handoverRegex = new Regex(@"bot (\d+) gives low to (output|bot) (\d+) and high to (output|bot) (\d+)");

            foreach (var instruction in input.Where(line => handoverRegex.IsMatch(line))) {
                var instructionMatch = handoverRegex.Match(instruction);
                var givingBotId = int.Parse(instructionMatch.Groups[1].Value);
                var lowReceiverType = instructionMatch.Groups[2].Value;
                var lowReceiverId = int.Parse(instructionMatch.Groups[3].Value);
                var highReceiverType = instructionMatch.Groups[4].Value;
                var highReceiverId = int.Parse(instructionMatch.Groups[5].Value);

                var givingBot = GetBot(givingBotId, bots);

                switch (lowReceiverType) {
                    case var _ when lowReceiverType.Equals("bot"):
                        givingBot.LowReceiver = GetBot(lowReceiverId, bots);
                        break;
                    case var _ when lowReceiverType.Equals("output"):
                        givingBot.LowReceiver = GetOutput(lowReceiverId, outputs);
                        break;
                }

                switch (highReceiverType) {
                    case var _ when highReceiverType.Equals("bot"):
                        givingBot.HighReceiver = GetBot(highReceiverId, bots);
                        break;
                    case var _ when highReceiverType.Equals("output"):
                        givingBot.HighReceiver = GetOutput(highReceiverId, outputs);
                        break;
                }
            }

            foreach (var instruction in input.Where(line => inputRegex.IsMatch(line))) {
                var inputMatch = inputRegex.Match(instruction);
                var value = int.Parse(inputMatch.Groups[1].Value);
                var botId = int.Parse(inputMatch.Groups[2].Value);

                GetBot(botId, bots).Take(value);
            }

            return (bots, outputs);
        }

        private static Bot GetBot(int botId, Dictionary<int, Bot> bots) {
            if (!bots.ContainsKey(botId)) {
                bots.Add(botId, new Bot(botId));
            }
            return bots[botId];
        }

        private static Output GetOutput(int outputId, Dictionary<int, Output> outputs) {
            if (!outputs.ContainsKey(outputId)) {
                outputs.Add(outputId, new Output(outputId));
            }
            return outputs[outputId];
        }

        private interface IReceiver {
            int Id { get; }
            void Take(int value);
        }

        private class Output : IReceiver {
            public Output(int id) {
                Id = id;
            }

            public int Id { get; }
            public int Value { get; private set; }
            public void Take(int value) {
                Value = value;
            }

            public override string ToString() {
                return $"Output {Id} Value = {Value}";
            }
        }
        private class Bot : IReceiver {
            public Bot(int id) {
                Id = id;
            }

            public void Take(int value) {
                if (Low.HasValue && High.HasValue) {
                    throw new InvalidOperationException($"{this} is full");
                }
                if (!Low.HasValue) {
                    Low = value;
                } else if (value > Low) {
                    High = value;
                } else {
                    High = Low;
                    Low = value;
                }

                if (High.HasValue) {
                    if (LowReceiver == null || HighReceiver == null) {
                        throw new InvalidOperationException($"{this} is full but has not full receiver instructions");
                    }

                    LowReceiver.Take(Low.Value);
                    HighReceiver.Take(High.Value);
                }
            }

            public int Id { get; }
            public int? Low { get; private set; }
            public int? High { get; private set; }

            public IReceiver LowReceiver { get; set; }
            public IReceiver HighReceiver { get; set; }

            public override string ToString() {
                return $"Bot {Id} Low = {Low} High = {High} LowReceiver = {LowReceiver?.Id} HighReceiver = {HighReceiver?.Id}";
            }
        }

    }

}
