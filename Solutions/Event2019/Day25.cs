using System;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Combinatorics;
using Shared.Extensions;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 25: Cryostasis ---
    public class Day25
    {
        internal void Interactive()
        {
            var programInput = ReadInput();
            var computer = IntCodeComputer.Load(programInput);
            computer.Execute();

            while (true)
            {
                Console.Write(computer.OutputAscii);
                var command = Console.ReadLine();
                if (command == "quit")
                {
                    return;
                }
                computer.ExecuteAscii($"{command}\n");
            }
        }

        public long Solver(Action<string> onOutput = null)
        {
            if (onOutput == null)
            {
                onOutput = s => { };
            }

            var programInput = ReadInput();
            var computer = IntCodeComputer.Load(programInput);
            computer.Execute();

            onOutput(computer.OutputAscii);

            var collectCommands = new[]
            {
                "south\n",
                "south\n",
                "south\n",
                "take fixed point\n",
                "south\n",
                "take festive hat\n",
                "west\n",
                "west\n",
                "take jam\n",
                "south\n",
                "take easter egg\n",
                "north\n",
                "east\n",
                "east\n",
                "north\n",
                "west\n",
                "take asterisk\n",
                "east\n",
                "north\n",
                "west\n",
                "north\n",
                "north\n",
                "take tambourine\n",
                "south\n",
                "south\n",
                "east\n",
                "north\n",
                "west\n",
                "south\n",
                "take antenna\n",
                "north\n",
                "west\n",
                "west\n",
                "take space heater\n",
                "west\n",
                "drop fixed point\n",
                "drop festive hat\n",
                "drop jam\n",
                "drop easter egg\n",
                "drop asterisk\n",
                "drop tambourine\n",
                "drop antenna\n",
                "drop space heater\n"
            };
            foreach (var command in collectCommands)
            {
                computer.ExecuteAscii(command.Yield().ToList());

                onOutput(computer.OutputAscii);
            }

            var allItems = new[] { "fixed point", "jam", "easter egg", "asterisk", "tambourine", "antenna", "festive hat", "space heater" };

            var passwordExpression = new Regex(@"(\d+)");

            for (int itemsToCarry = 2; itemsToCarry < allItems.Length; itemsToCarry++)
            {
                var combinationsOfX = allItems.Combinations(itemsToCarry).ToList();
                foreach (var items in combinationsOfX)
                {
                    var listOfItems = items.ToList();
                    computer.ExecuteAscii(listOfItems.Select(item => $"take {item}\n").ToList());
                    computer.ExecuteAscii("west\n".Yield().ToList());
                    var outputText = computer.OutputAscii;
                    if (passwordExpression.IsMatch(outputText))
                    {
                        var password = long.Parse(passwordExpression.Match(outputText).Groups[1].Value);
                        onOutput(outputText);
                        return password;
                    }
                    computer.ExecuteAscii(listOfItems.Select(item => $"drop {item}\n").ToList());
                }
            }

            throw new InvalidOperationException("Could not find any combination of items to access the password");
        }

        public long FirstStar()
        {
            return Solver();
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(2147485856, FirstStar());
        }
    }
}
