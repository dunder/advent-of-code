using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day X: Phrase ---
    public class Day07
    {
        private readonly ITestOutputHelper output;

        public Day07(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Card(char Value, bool jokers = false) : IComparable<Card>
        {
            public int Rank
            {
                get
                { 
                    switch (Value)
                    {
                        case 'A': return 14;
                        case 'K': return 13;
                        case 'Q': return 12;
                        case 'J': return jokers ? 1 : 11;
                        case 'T': return 10;
                        default:
                            {
                                int cardValue = Value - '0';
                                if (char.IsDigit(Value) && cardValue > 1)
                                {
                                    return Value - '0';
                                }
                                else
                                {
                                    throw new ArgumentException($"Invalid card: {Value}");
                                }
                            }
                    }
                }
            }

            public int CompareTo(Card other)
            {
                return Rank-other.Rank;
            }
        }
        private enum HandType { FiveOfAKind, FourOfAKind, FullHouse, ThreeOfAKind, TwoPair, OnePair, HighCard }

        private record Hand(List<Card> Cards, bool jokers = false) : IComparable<Hand>
        {
            public static Hand From(string input)
            {
                return new Hand(input.Select(c => new Card(c)).ToList());
            }

            public HandType Type
            {
                get
                {
                    var groupedCards = Cards.GroupBy(c => c);

                    var test = groupedCards.Select(g => g.Count()).ToList();

                    var jokerGroup = jokers ? groupedCards.Where(g => g.Key.Value == 'J') : Enumerable.Empty<IGrouping<Card, Card>>();
                    var jokerCount = jokerGroup.Sum(g => g.Count());


                    if (groupedCards.Count() == 1)
                    {
                        return HandType.FiveOfAKind;
                    }

                    // 41 32
                    if (groupedCards.Count() == 2)
                    {
                        if (groupedCards.Any(group => group.Count() == 4))
                        {
                            switch(jokerCount)
                            {
                                case 4: return HandType.FiveOfAKind;
                                case 1: return HandType.FiveOfAKind;
                                case 0: return HandType.FourOfAKind;
                                default: throw new Exception($"Unexpected joker count: {jokerCount}");
                            }
                        }
                        else
                        {
                            switch (jokerCount)
                            {
                                case 3: return HandType.FiveOfAKind;
                                case 2: return HandType.FiveOfAKind;
                                case 0: return HandType.FullHouse;
                                default: throw new Exception($"Unexpected joker count: {jokerCount}");
                            }
                        }
                    }
                    // 221 311
                    if (groupedCards.Count() == 3)
                    {
                        if (groupedCards.Any(group => group.Count() == 3))
                        {

                            switch (jokerCount)
                            {
                                case 3: return HandType.FourOfAKind;
                                case 1: return HandType.FourOfAKind;
                                case 0: return HandType.ThreeOfAKind;
                                default: throw new Exception($"Unexpected joker count: {jokerCount}");
                            }
                        }
                        else
                        {
                            switch (jokerCount)
                            {
                                case 2: return HandType.FourOfAKind;
                                case 1: return HandType.FullHouse;
                                case 0: return HandType.TwoPair;
                                default: throw new Exception($"Unexpected joker count: {jokerCount}");
                            }
                        }
                    }

                    // 2111
                    if (groupedCards.Count() == 4)
                    {
                        switch (jokerCount)
                        {
                            case 2: return HandType.ThreeOfAKind;
                            case 1: return HandType.ThreeOfAKind;
                            case 0: return HandType.OnePair;
                            default: throw new Exception($"Unexpected joker count: {jokerCount}");
                        }
                    }

                    switch (jokerCount)
                    {
                        case 1: return HandType.OnePair;
                        case 0: return HandType.HighCard;
                        default: throw new Exception($"Unexpected joker count: {jokerCount}");
                    }
                }
            }

            public int Compare(Hand hand1, Hand hand2)
            {
                int rank1 = (int)hand1.Type;
                int rank2 = (int)hand2.Type;

                if (rank1 != rank2)
                {
                    return rank1 - rank2;
                }

                for (int i = 0; i < 5; i++)
                {
                    var card1 = hand1.Cards[i].Rank;
                    var card2 = hand2.Cards[i].Rank;
                    if (card1 != card2)
                    {
                        return card2 - card1;
                    }
                }

                return 0;
            }

            public int CompareTo(Hand other)
            {
                int rank1 = (int)Type;
                int rank2 = (int)other.Type;

                if (rank1 != rank2)
                {
                    return rank1 - rank2;
                }

                for (int i = 0; i < 5; i++)
                {
                    var card1 = Cards[i].Rank;
                    var card2 = other.Cards[i].Rank;
                    if (card1 != card2)
                    {
                        return card2 - card1;
                    }
                }

                return 0;
            }
        }

        private record HandWithBid(Hand hand, int bid);

        private int TotalWinnings(IList<string> input, bool jokers = false)
        {
            return input
                .Select(line => {
                    var parts = line.Split(" ");
                    var cards = parts[0].Select(c => new Card(c, jokers)).ToList();
                    var bid = int.Parse(parts[1]);
                    return new HandWithBid(new Hand(cards, jokers), bid);
                })
                .OrderByDescending(x => x.hand)
                .Select((card, i) => (i+1) * card.bid)
                .Sum();
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return TotalWinnings(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return TotalWinnings(input, true);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(253933213, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(253473930, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "32T3K 765",
                "T55J5 684",
                "KK677 28",
                "KTJJT 220",
                "QQQJA 483",
            };

            Assert.Equal(6440, TotalWinnings(example));
        }

        [Fact]
        public void SortTests()
        {
            var example = new List<string> { "AAAAA", "KKKKK" };

            Assert.True(Hand.From("AAAAA").CompareTo(Hand.From("KKKKK")) < 0);
            Assert.True(Hand.From("AAAAA").CompareTo(Hand.From("QQQQQ")) < 0);
            Assert.True(Hand.From("AAAAA").CompareTo(Hand.From("TTTTT")) < 0);
            Assert.True(Hand.From("AAAAA").CompareTo(Hand.From("99999")) < 0);
            Assert.True(Hand.From("AAAAA").CompareTo(Hand.From("22222")) < 0);

            Assert.True(Hand.From("AAAAA").CompareTo(Hand.From("KKKKK")) < 0);
            
            Assert.True(Hand.From("KKKKK").CompareTo(Hand.From("AAAAA")) > 0);
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "32T3K 765",
                "T55J5 684",
                "KK677 28",
                "KTJJT 220",
                "QQQJA 483",
            };

            Assert.Equal(5905, TotalWinnings(example, true));
        }
    }
}
