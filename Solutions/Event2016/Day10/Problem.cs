using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solutions.Event2016.Day10
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day10;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = Robots.FindBot(input, 17, 61);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = Robots.CalculateOutputProduct(input);
            return result.ToString();
        }
    }

    public class Robots {
        public static int FindBot(IList<string> input, int lowTargetValue, int highTargetValue) {

            (Dictionary<int, Bot> bots, _) = RunInstructions(input);

            return bots.Values.Single(bot => bot.Low == lowTargetValue && bot.High == highTargetValue).Id;
        }

        public static int CalculateOutputProduct(IList<string> input) {
            var (_, outputs) = RunInstructions(input);

            return outputs.Values.Where(o => o.Id == 0 || o.Id == 1 || o.Id == 2).Aggregate(1, (x, robot) => x * robot.Value);
        }

        private static (Dictionary<int, Bot> bots, Dictionary<int, Output>) RunInstructions(IList<string> input) {
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