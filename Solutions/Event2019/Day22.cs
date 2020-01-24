using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Shared;
using Xunit;
using Xunit.Sdk;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 22: Slam Shuffle ---

    /// <summary>
    /// This blog post: https://codeforces.com/blog/entry/72527 gives the basics in
    /// modular arithmetic which is exactly what is needed to solve star 2 of this day.
    ///
    /// If that is not enough to get you on the right track to solve this problem there
    /// is a problem specific tutorial also: https://codeforces.com/blog/entry/72593 
    /// </summary>
    public class Day22
    {
        private List<int> Shuffle(List<int> deck, IEnumerable<string> rules)
        {
            var dealPattern = new Regex(@"deal with increment (\d+)");
            var cutPattern = new Regex(@"cut (-?\d+)");

            foreach (var rule in rules)
            {
                switch (rule)
                {
                    case var dealIncrement when dealPattern.IsMatch(rule):
                    {
                        var m = dealPattern.Match(dealIncrement);
                        var increment = int.Parse(m.Groups[1].Value);
                        deck = Deal(deck, increment);
                        break;
                    }
                    case var cutN when cutPattern.IsMatch(rule):
                    {
                        var m = cutPattern.Match(cutN);
                        var n = int.Parse(m.Groups[1].Value);
                        deck = Cut(deck, n);
                        break;
                    }

                    case var _ when rule.Equals("deal into new stack"):
                        deck = NewStack(deck);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown rule: {rule}");
                }
            }

            return deck; 
        }

        private List<int> NewStack(List<int> deck)
        {
            deck.Reverse();
            return deck;
        }

        private List<int> Cut(List<int> deck, int n)
        {
            if (n < 0)
            {
                var first = deck.Take(deck.Count + n);
                var second = deck.Skip(deck.Count + n);
                return second.Concat(first).ToList();
            }
            else
            {
                var first = deck.Take(n);
                var second = deck.Skip(n);
                return second.Concat(first).ToList();
            }
        }

        private List<int> Deal(List<int> deck, int increment)
        {
            var newDeck = new int[(deck.Count)];
            int p = 0;
            for (int i = 0; i < deck.Count; i++, p += increment)
            {
                newDeck[p % deck.Count] = deck[i];
            }

            return newDeck.ToList();
        }

        BigInteger Mod(BigInteger a, BigInteger m) => ((a % m) + m) % m;
        BigInteger ModInv(BigInteger a, BigInteger m) => BigInteger.ModPow(a, m - 2, m);

        private (BigInteger a, BigInteger b) Parse(IEnumerable<string> input, long cards, long shuffles)
        {
            var a = BigInteger.One;
            var b = BigInteger.Zero;

            var dealPattern = new Regex(@"deal with increment (\d+)");
            var cutPattern = new Regex(@"cut (-?\d+)");

            foreach (var rule in input)
            {
                switch (rule)
                {
                    case var dealIncrement when dealPattern.IsMatch(rule):
                    {
                        var match = dealPattern.Match(dealIncrement);
                        var increment = int.Parse(match.Groups[1].Value);

                        a *= increment;
                        b *= increment;

                        break;
                    }
                    case var cutN when cutPattern.IsMatch(rule):
                    {
                        var match = cutPattern.Match(cutN);
                        var n = int.Parse(match.Groups[1].Value);

                        b = cards + b - n;

                        break;
                    }

                    case var _ when rule.Equals("deal into new stack"):
                    {
                        a = -a;
                        b = cards - b - 1;

                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown rule: {rule}");
                }
            }

            var resA = BigInteger.ModPow(a, shuffles, cards);
            // resB = b * (1 + a + a^2 + ... a^n) = b * (a^n - 1) / (a - 1);
            var resB = b * (BigInteger.ModPow(a, shuffles, cards) - 1) * ModInv(a - 1, cards) % cards;

            return (resA, resB);
        }

        private long ShuffleFast(IEnumerable<string> input, long cards, long shuffles, long index)
        {
            var (a, b) = Parse(input, cards, shuffles);

            return (long)Mod(ModInv((long)a, cards) * (index - b), cards);

        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            var deck = Enumerable.Range(0, 10007).ToList();
            var shuffledDeck = Shuffle(deck, input);
            return shuffledDeck.IndexOf(2019);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();
            return ShuffleFast(input, 119315717514047, 101741582076661, 2020);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(6289, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(58348342289943, SecondStar());
        }

        [Fact]
        public void FirstStarExample1()
        {
            var input = new[]
            {
                "deal with increment 7",
                "deal into new stack",
                "deal into new stack"
            };
            var deck = Enumerable.Range(0, 10).ToList();
            var shuffledDeck = Shuffle(deck, input);
            Assert.Equal(new List<int> { 0,3,6,9,2,5,8,1,4,7 }, shuffledDeck);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var input = new[]
            {
                "cut 6",
                "deal with increment 7",
                "deal into new stack"
            };
            var deck = Enumerable.Range(0, 10).ToList();
            var shuffledDeck = Shuffle(deck, input);
            Assert.Equal(new List<int> { 3,0,7,4,1,8,5,2,9,6 }, shuffledDeck);
        }

        [Fact]
        public void FirstStarExample3()
        {
            var input = new[]
            {
                "deal with increment 7",
                "deal with increment 9",
                "cut -2"
            };
            var deck = Enumerable.Range(0, 10).ToList();
            var shuffledDeck = Shuffle(deck, input);
            Assert.Equal(new List<int> { 6,3,0,7,4,1,8,5,2,9 }, shuffledDeck);
        }

        [Fact]
        public void FirstStarExample4()
        {
            var input = new[]
            {
                "deal into new stack",
                "cut -2",
                "deal with increment 7",
                "cut 8",
                "cut -4",
                "deal with increment 7",
                "cut 3",
                "deal with increment 9",
                "deal with increment 3",
                "cut -1"
            };
            var deck = Enumerable.Range(0, 10).ToList();
            var shuffledDeck = Shuffle(deck, input);
            Assert.Equal(new List<int> { 9,2,5,8,1,4,7,0,3,6 }, shuffledDeck);
        }

        [Fact]
        public void ModFunctionCheckup()
        {
            // check if n mod m is n - [n/m] * m where [x] is the largest integer that does not exceed x
            var actual = -8 % 7;

            try
            {
                Assert.Equal(6, actual);

            }
            catch (EqualException)
            {
                // the % operator in .NET does not follow the mod function but the result can be calculated by
                // taking m + the result of n - [n/m] * m if n - [n/m] * m is negative
                var mod = actual + 7;
                Assert.Equal(6, mod);
            }

            // so for simplicity encapsulate it in its own shared method
            actual = Maths.Mod(-8, 7);
            Assert.Equal(6, actual);
        }

        [Fact]
        public void DeckCountPrimeTest()
        {
            long deckCount = 119315717514047;
            Assert.True(Prime.IsPrime(deckCount));
        }

        [Fact]
        public void ShuffleCountPrimeTest()
        {
            long shuffles = 101741582076661;
            Assert.True(Prime.IsPrime(shuffles));
        }
    }
}
