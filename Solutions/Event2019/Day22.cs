using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 22: Slam Shuffle ---
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
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
            //Assert.Equal(-1, FirstStar()); // 87887 too high
            //Assert.Equal(-1, FirstStar()); // 99055 too high
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
    }
}
