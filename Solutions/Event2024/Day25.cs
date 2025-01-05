using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 25: Code Chronicle ---
    public class Day25
    {
        private readonly ITestOutputHelper output;

        private const int PINS = 5;
        private const int PIN_HEIGHT = 5;

        private static (List<List<int>> locks, List<List<int>> keys) Parse(IList<string> input)
        {
            List<List<int>> locks = new();
            List<List<int>> keys = new();

            var batches = input.Split(string.IsNullOrWhiteSpace);
            
            foreach (List<string> batch in batches)
            {
                if (batch.First() == "#####")
                {
                    List<int> @lock = [0, 0, 0, 0, 0];

                    for (int x = 0; x < PINS; x++)
                    {
                        int height = 0;
                        do
                        {
                            @lock[x] = height;
                        }
                        while (batch[++height][x] == '#');
                    }

                    locks.Add(@lock);
                }
                else
                {
                    List<int> key = [0, 0, 0, 0, 0];

                    for (int x = 0; x < PINS; x++)
                    {
                        int height = 0;
                        do
                        {
                            key[x] = PIN_HEIGHT - height;
                        }
                        while (batch[++height][x] == '.');
                    }

                    keys.Add(key);
                }
            }

            return (locks, keys);
        }

        private static bool Fits(List<int> @lock, List<int> key)
        {
            return @lock.Zip(key, (l, k) => l + k <= PIN_HEIGHT).All(x => x);
        }

        public Day25(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static int Problem1(IList<string> input)
        {
            (List<List<int>> locks, List<List<int>> keys) = Parse(input);

            return locks.Select(@lock => keys.Count(key => Fits(@lock, key))).Sum();
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(3269, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(3, Problem1(exampleInput));
        }
    }
}
