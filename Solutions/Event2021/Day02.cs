using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2021
{
    // --- Day 2: Dive! ---
    public class Day02
    {
        private readonly ITestOutputHelper output;

        public Day02(ITestOutputHelper output)
        {
            this.output = output;
        }

        private int CalculatePosition(IList<string> input)
        {
            (int depth, int position) = (0, 0);

            foreach (var instruction in input)
            {
                var parts = instruction.Split(" ");
                var command = parts[0];
                var units = int.Parse(parts[1]);

                switch(command)
                {
                    case "forward":
                        position = position + units; 
                        break;
                    case "up":
                        depth = depth - units; 
                        break;
                    case "down":
                        depth = depth + units; 
                        break;
                    default:
                        throw new Exception($"Unknown command: {command}");

                }

            }
            return depth * position;
        }
        
        private int CalculatePositionWithAim(IList<string> input)
        {
            int depth = 0;
            int position = 0;
            int aim = 0;

            foreach (var instruction in input)
            {
                var parts = instruction.Split(" ");
                var command = parts[0];
                var units = int.Parse(parts[1]);

                switch(command)
                {
                    case "forward":
                        position = position + units;
                        depth = depth + aim * units;
                        break;
                    case "up":
                        aim = aim - units; 
                        break;
                    case "down":
                        aim = aim + units; 
                        break;
                    default:
                        throw new Exception($"Unknown command: {command}");

                }

            }
            return depth * position;
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return CalculatePosition(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return CalculatePositionWithAim(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(1840243, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1727785422, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "forward 5",
                "down 5",
                "forward 8",
                "up 3",
                "down 8",
                "forward 2"
            };

            Assert.Equal(150, CalculatePosition(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "forward 5",
                "down 5",
                "forward 8",
                "up 3",
                "down 8",
                "forward 2"
            };

            Assert.Equal(900, CalculatePositionWithAim(example));
        }
    }
}
