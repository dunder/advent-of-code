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
        
        private long ShuffleIndex(long index, long cardsInDeck, IEnumerable<string> rules)
        {
            var dealPattern = new Regex(@"deal with increment (\d+)");
            var cutPattern = new Regex(@"cut (-?\d+)");
            var newIndex = index;
            foreach (var rule in rules.Reverse())
            {
                switch (rule)
                {
                    case var dealIncrement when dealPattern.IsMatch(rule):
                    {
                        var m = dealPattern.Match(dealIncrement);
                        var increment = int.Parse(m.Groups[1].Value);

                        newIndex = DealIndex(newIndex, cardsInDeck, increment);

                        break;
                    }
                    case var cutN when cutPattern.IsMatch(rule):
                    {
                        var m = cutPattern.Match(cutN);
                        var n = int.Parse(m.Groups[1].Value);

                        newIndex = CutIndex(newIndex, cardsInDeck, n);

                        break;
                    }

                    case var _ when rule.Equals("deal into new stack"):

                        newIndex = NewStackIndex(newIndex, cardsInDeck);
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown rule: {rule}");
                }
            }

            return newIndex;
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

        // calculate what index the card of this index had before dealing the new stack
        public static long NewStackIndex(long index, long cardsInDeck)
        {
            var newIndex = index - cardsInDeck + 1;
            if (newIndex < 0)
            {
                newIndex = 0 - newIndex;
            }
            return newIndex;
        }

        // calculate what index the card of this index had before this cut
        public static long CutIndex(long index, long cardsInDeck, int n)
        {
            // shift right with overflow
            if (n > 0)
            {
                return (index + n) % cardsInDeck;

            }

            return (index + cardsInDeck + n) % cardsInDeck;
        }

        // calculate what index the card of this index had before dealing with this increment
        public static long DealIndex(long index, long cardsInDeck, int increment)
        {
            var remainder = index % increment == 0 ? 0 : 1;

            var newIndex = ((increment - (index % increment)) % increment) * increment + index / increment + remainder;

            return newIndex % cardsInDeck;
        }

        private const long Cards = 119315717514047;
        private const long Shuffles = 101741582076661;

        private static long Pow(long value, long exponent, long modulus)
        {
            var bigValue = new BigInteger(value);
            var bigExponent = new BigInteger(exponent);
            var bigModulus = new BigInteger(modulus);
            return (long) BigInteger.ModPow(bigValue, bigExponent, bigModulus);
        }


        private static long ModInverse(long value, long modulus)
        {
            var bigValue = new BigInteger(value);
            var bigExponent = new BigInteger(modulus - 2);
            var bigModulus = new BigInteger(modulus);
            return (long) BigInteger.ModPow(bigValue, bigExponent, bigModulus);
        }


        public static long ModInverse2(long a, long m)
        {
            if (m == 1) return 0;
            long m0 = m;
            (long x, long y) = (1, 0);

            while (a > 1)
            {
                long q = a / m;
                (a, m) = (m, a % m);
                (x, y) = (y, x - q * y);
            }
            return x < 0 ? x + m0 : x;
        }

        BigInteger Mod(BigInteger a, BigInteger m) => ((a % m) + m) % m;
        BigInteger ModInv(BigInteger a, BigInteger m) => BigInteger.ModPow(a, m - 2, m);

        //private readonly Func<long, long, long, long, (long, long)> NewStackFast = (a, b, n, m) => ((a - b) % m, -b % m);
        //private readonly Func<long, long, long, long, (long, long)> CutFast = (a, b, n, m) => ((a + b * n) % m, b);
        //private readonly Func<long, long, long, long, (long, long)> DealWithIncrement = (a, b, n, m) => (a, (b * ModInverse(n, m)) % m);
        private readonly Func<long, long, long, long, (long, long)> NewStackFast = (a, b, n, m) => (Maths.Mod(a - b, m) % m, Maths.Mod(-b, m));
        private readonly Func<long, long, long, long, (long, long)> CutFast = (a, b, n, m) => (Maths.Mod(a + b * n, m), b);
        private readonly Func<long, long, long, long, (long, long)> DealWithIncrement = (a, b, n, m) => (a, Maths.Mod(b * ModInverse2(n, m), m));

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
        private List<(Func<long, long, long, long, (long, long)>, long)> ParseRules(IEnumerable<string> input)
        {
            var dealPattern = new Regex(@"deal with increment (\d+)");
            var cutPattern = new Regex(@"cut (-?\d+)");

            var rules = new List<(Func<long, long, long, long, (long, long)>, long)>();
            foreach (var rule in input)
            {
                switch (rule)
                {
                    case var dealIncrement when dealPattern.IsMatch(rule):
                    {
                        var m = dealPattern.Match(dealIncrement);
                        var increment = int.Parse(m.Groups[1].Value);

                        rules.Add((DealWithIncrement, increment));

                        break;
                    }
                    case var cutN when cutPattern.IsMatch(rule):
                    {
                        var m = cutPattern.Match(cutN);
                        var n = int.Parse(m.Groups[1].Value);

                        rules.Add((CutFast, n));

                        break;
                    }

                    case var _ when rule.Equals("deal into new stack"):

                        rules.Add((NewStackFast, 0));

                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown rule: {rule}");
                }
            }

            return rules;
        }

        private long ShuffleFastShort(IEnumerable<string> input, long cards, long shuffles, long index)
        {
            var (a, b) = Parse(input, cards, shuffles);

            return (long)Mod(ModInv(a, cards) * (index - b), cards);

        }
        private (long a, long b) ShuffleFast(List<(Func<long, long, long, long, (long, long)>, long)> rules,long cards)
        {
            var (a, b) = (0L,1L);
            foreach (var (rule, n)  in rules)
            {
                (a, b) = rule(a, b, n, cards);
            }
            return (a, b);
        }

        private long ShuffleManyFast(IEnumerable<string> input, long cards, long shuffles, int index)
        {
            var rules = ParseRules(input);
            var (offset, increment) = ShuffleFast(rules, cards);
            offset *= (1 - Pow(increment, Shuffles, Cards)) * ModInverse(1 - increment, Cards);
            increment = Pow(increment, shuffles, cards);
            return (offset + increment * index) % cards;
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
            return ShuffleFastShort(input, Cards, Shuffles, 2020);
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
            //Assert.Equal(-1, SecondStar()); //   63245867756113 not right
            //Assert.Equal(-1, SecondStar()); // 79149270479368 is too high (using Maths.Mod function)
            //Assert.Equal(-1, SecondStar()); // 83908278694325 is too high (m-2)
            //                                   58348342289943
            //Assert.Equal(-1, SecondStar()); // 4836123455273 is too low (n-2)
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

        [Theory]
        [InlineData(0, 9)]
        [InlineData(1, 8)]
        [InlineData(2, 7)]
        [InlineData(3, 6)]
        [InlineData(4, 5)]
        [InlineData(5, 4)]
        [InlineData(6, 3)]
        [InlineData(7, 2)]
        [InlineData(8, 1)]
        [InlineData(9, 0)]
        public void NewStackIndexTests(int index, int expectedFromIndex)
        {
            var actualNewIndex = NewStackIndex(index, 10);

            Assert.Equal(expectedFromIndex, actualNewIndex);
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(1, 4)]
        [InlineData(2, 5)]
        [InlineData(3, 6)]
        [InlineData(4, 7)]
        [InlineData(5, 8)]
        [InlineData(6, 9)]
        [InlineData(7, 0)]
        [InlineData(8, 1)]
        [InlineData(9, 2)]
        public void CutIndexTests(int index, int expectedFromIndex)
        {
            var actualNewIndex = CutIndex(index, 10, 3);

            Assert.Equal(expectedFromIndex, actualNewIndex);
        }

        [Theory]
        [InlineData(0, 6)]
        [InlineData(1, 7)]
        [InlineData(2, 8)]
        [InlineData(3, 9)]
        [InlineData(4, 0)]
        [InlineData(5, 1)]
        [InlineData(6, 2)]
        [InlineData(7, 3)]
        [InlineData(8, 4)]
        [InlineData(9, 5)]
        public void CutNegativeIndexTests(int index, int expectedFromIndex)
        {
            var actualNewIndex = CutIndex(index, 10, -4);

            Assert.Equal(expectedFromIndex, actualNewIndex);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 7)]
        [InlineData(2, 4)]
        [InlineData(3, 1)]
        [InlineData(4, 8)]
        [InlineData(5, 5)]
        [InlineData(6, 2)]
        [InlineData(7, 9)]
        [InlineData(8, 6)]
        [InlineData(9, 3)]
        public void DealIndexTests(int index, int expectedFromIndex)
        {
            var actualNewIndex = DealIndex(index, 10, 3);

            Assert.Equal(expectedFromIndex, actualNewIndex);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 3)]
        [InlineData(2, 6)]
        [InlineData(3, 9)]
        [InlineData(4, 2)]
        [InlineData(5, 5)]
        [InlineData(6, 8)]
        [InlineData(7, 1)]
        [InlineData(8, 4)]
        [InlineData(9, 7)]
        public void SecondStarExample1(int index, int expected)
        {
            var input = new[]
            {
                "deal with increment 7",
                "deal into new stack",
                "deal into new stack"
            };
            var fromIndex = ShuffleFastShort(input, 10, 1, index);
            Assert.Equal(expected, fromIndex);
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(1, 0)]
        [InlineData(2, 7)]
        [InlineData(3, 4)]
        [InlineData(4, 1)]
        [InlineData(5, 8)]
        [InlineData(6, 5)]
        [InlineData(7, 2)]
        [InlineData(8, 9)]
        [InlineData(9, 6)]
        public void SecondStarExample2(int index, int expected)
        {
            var input = new[]
            {
                "cut 6",
                "deal with increment 7",
                "deal into new stack"
            };
            var fromIndex = ShuffleFastShort(input, 10, 1, index);
            Assert.Equal(expected, fromIndex);
        }

        [Fact]

        public void SecondStarExample3Part()
        {
            var input = new[]
            {
                //"deal with increment 7",
                "deal with increment 9",
                // "cut -2"
            };
            var deck = Enumerable.Range(0, 10).ToList();
            var shuffledDeck = Shuffle(deck, input);
            var shuffledDeck2 = new List<long>();
            for (int i = 0; i < shuffledDeck.Count; i++)
            {
                var fromIndex = ShuffleIndex(i, 10, input);
                shuffledDeck2.Add(fromIndex);
            }
            Assert.Equal(shuffledDeck.Select(x => (long)x).ToList(), shuffledDeck2);
        }

        [Theory]
        [InlineData(0, 6)]
        [InlineData(1, 3)]
        [InlineData(2, 0)]
        [InlineData(3, 7)]
        [InlineData(4, 4)]
        [InlineData(5, 1)]
        [InlineData(6, 8)]
        [InlineData(7, 5)]
        [InlineData(8, 2)]
        [InlineData(9, 9)]
        public void SecondStarExample3(int index, int expected)
        {
            var input = new[]
            {
                "deal with increment 7",
                "deal with increment 9",
                "cut -2"
            };
            var fromIndex = ShuffleIndex(index, 10, input);
            Assert.Equal(expected, fromIndex);
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
