using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 4: Scratchcards ---
    public class Day04
    {
        private readonly ITestOutputHelper output;

        public Day04(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record ScratchCard(int id, HashSet<int> Winning, HashSet<int> Yours)
        {
            public int Winners => Yours.Count(Winning.Contains);

            public double Score
            {
                get
                {
                    if (Winners == 0) { return 0; };

                    return Enumerable.Range(0, Winners - 1).Aggregate(1, (total, n) => total* 2);
                }
            }
        }

        private List<ScratchCard> Parse(IList<string> input)
        {
            return input.Select(line =>
            {
                int id = int.Parse(line.Substring(5,3).Trim());

                var numberSplit = 
                    line.Substring(line.IndexOf(":") + 1)
                    .Split(" | ")
                    .Select(part => part.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                    .Select(numbers => numbers.Select(int.Parse).ToHashSet())
                    .ToArray();

                return new ScratchCard(id, numberSplit[0], numberSplit[1]);
            }).ToList();
        }

        private double Points(IList<string> input)
        {
            return Parse(input)
                .Select(card => card.Score)
                .Sum();
        }

        private int TotalScratchCards(IList<string> input)
        {
            var cards = Parse(input).ToArray();
            var cardCount = cards.ToDictionary(card => card.id, card => 0);

            foreach (var card in cards.Reverse())
            {
                if (card.id - 1 < cards.Length && card.Winners > 0)
                {
                    var stop = Math.Min(cards.Length, card.id + card.Winners);

                    var wonCards = cards[card.id..stop];

                    var total = wonCards.Length + wonCards.Select(card => cardCount[card.id]).Sum();

                    cardCount[card.id] = total;
                }
            }

            return cards.Length + cardCount.Values.Sum();
        }
        public double FirstStar()
        {
            var input = ReadLineInput();
            
            return Points(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return TotalScratchCards(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(23235, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(5920640, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "Card   1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
                "Card   2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
                "Card   3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
                "Card   4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
                "Card   5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
                "Card   6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
            };
            Assert.Equal(13, Points(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "Card   1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
                "Card   2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
                "Card   3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
                "Card   4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
                "Card   5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
                "Card   6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
            };
            Assert.Equal(30, TotalScratchCards(example));
        }
    }
}
