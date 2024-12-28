using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 22: Monkey Market ---
    public class Day22
    {
        private readonly ITestOutputHelper output;

        public Day22(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static long Mix(long a, long b)
        {
            return a ^ b;
        }

        private static long Prune(long a)
        {
            return a % 16777216;
        }

        private static int OnesValue(long a)
        {
            return (int)(a % 10);
        }

        private static long NextSecret(long secret)
        {
            var part1 = Prune(Mix(secret * 64, secret));
            var part2 = Prune(Mix(part1 / 32, part1));

            return Prune(Mix(part2 * 2048 , part2));
        }

        private static long NextSecret(long secret, int generations)
        {
            do
            {
                secret = NextSecret(secret);
            }
            while (--generations > 0);

            return secret;
        }

        private static List<long> NextSecrets(long secret, int generations)
        {
            var secrets = new List<long>();

            do
            {
                secret = NextSecret(secret);
                secrets.Add(secret);
            }
            while (--generations > 0);

            return secrets;
        }

        private static long Problem1(IList<string> input)
        {
            return input.Select(long.Parse).Select(secret => NextSecret(secret, 2000)).Sum();
        }

        private static IEnumerable<((int, int, int, int), int)> GenerateSequence(long secret, int generations)
        {
            Queue<(int, int)> queue = new Queue<(int, int)>();

            int previousPrice = OnesValue(secret);

            do
            {
                secret = NextSecret(secret);

                var price = OnesValue(secret);

                var diff = price - previousPrice;

                previousPrice = price;

                queue.Enqueue((price, diff));

                if (queue.Count == 4)
                {
                    var items = new List<(int price, int diff)>(queue);

                    yield return ((items[0].diff, items[1].diff, items[2].diff, items[3].diff), items[3].price);

                    queue.Dequeue();
                }
            } while (--generations > 0);
        }

        private static Dictionary<(int, int, int, int), int> CollectSequences(long secret, int generations)
        {
            Dictionary<(int, int, int, int), int> sequences = new ();

            foreach (((int, int, int, int) sequence, int price) in GenerateSequence(secret, generations))
            {
                if (!sequences.ContainsKey(sequence))
                {
                    sequences.Add(sequence, price);
                }
            }

            return sequences;
        }

        private static int CrossCheckBananaCount(List<Dictionary<(int, int, int, int), int>> secretsLookup, (int, int, int, int) sequence)
        {
            int bananas = 0;

            for (int i = 0; i < secretsLookup.Count; i++)
            {
                if (secretsLookup[i].TryGetValue(sequence, out int otherPrice))
                {
                    bananas += otherPrice;
                }
            }

            return bananas;
        }

        private static int Problem2(IList<string> input)
        {
            List<Dictionary<(int, int, int, int), int>> secretsLookup = new();

            HashSet<(int, int, int, int)> allSequences = new();

            foreach (var secret in input.Select(long.Parse))
            {
                var sequences = CollectSequences(secret, 2000);

                allSequences.UnionWith(sequences.Keys);

                secretsLookup.Add(sequences);
            }

            int max = 0;

            foreach (var sequence in allSequences)
            {
                int crossCheck = CrossCheckBananaCount(secretsLookup, sequence);

                max = Math.Max(max, crossCheck);
            }

            return max;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(17163502021, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1938, Problem2(input));
        }

        private List<string> exampleInput =
            [
                "1",
                "10",
                "100",
                "2024",
            ];

        private List<string> exampleInput2 =
            [
                "1",
                "2",
                "3",
                "2024",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(37327623, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void MixTest()
        {
            Assert.Equal(37, Mix(42, 15));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void PruneTest()
        {
            Assert.Equal(16113920, Prune(100000000));
        }

        [Theory]
        [InlineData(123, 10, 5908254)]
        [InlineData(1, 2000, 8685429)]
        [InlineData(10, 2000, 4700978)]
        [InlineData(100, 2000, 15273692)]
        [InlineData(2024, 2000, 8667524)]
        [Trait("Event", "2024")]
        public void NextSecretTimesTest(long secret, int generations, long expected)
        {
            Assert.Equal(expected, NextSecret(secret, generations));
        }

        [Theory]
        [InlineData(123, 3)]
        [InlineData(15887950, 0)]
        [InlineData(16495136, 6)]
        [Trait("Event", "2024")]
        public void OnesTest(long secret, long expected)
        {
            Assert.Equal(expected, OnesValue(secret));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void NextSecretsTest()
        {
            NextSecrets(123, 10).Should().Equal(
                15887950,
                16495136,
                527345,
                704524,
                1553684,
                12683156,
                11100544,
                12249484,
                7753432,
                5908254);
        }

        [Fact]
        [Trait("Event", "2024")]
        public void GenerateSequenceTest()
        {
            GenerateSequence(123, 9).Should().Equal(
                ((-3, 6, -1, -1), 4),
                ((6, -1, -1, 0), 4),
                ((-1, -1, 0, 2), 6),
                ((-1, 0, 2, -2), 4),
                ((0, 2, -2, 0), 4),
                ((2, -2, 0, -2), 2)
                );
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {

            Assert.Equal(23, Problem2(exampleInput2));
        }
    }
}
