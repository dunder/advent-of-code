using System;
using System.Collections.Generic;
using System.Linq;
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

        public int FirstStar()
        {
            var input = ReadLineInput();
            var deck = Enumerable.Range(0, 10007).ToList();
            var shuffledDeck = Shuffle(deck, input);
            return shuffledDeck.IndexOf(2019);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            var deck = Enumerable.Range(0, 10007).ToList();
            var shuffledDeck = Shuffle(deck, input);
            return shuffledDeck.IndexOf(2019);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(6289, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
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
            var fromIndex = ShuffleIndex(index, 10, input);
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
            var fromIndex = ShuffleIndex(index, 10, input);
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
    }
}
