using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 11: Phrase ---
    public class Day11
    {
        private readonly ITestOutputHelper output;

        public Day11(ITestOutputHelper output)
        {
            this.output = output;
        }



        private static int Problem1(string input)
        {
            var stones = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

            var line = new LinkedList<long>(stones);


            void Blink()
            {

                LinkedListNode<long> current = line.First;

                while (current != null)
                {

                    if (current.Value == 0)
                    {
                        current.Value = 1;
                    }
                    else if (current.Value.ToString().Length % 2 == 0)
                    {
                        var value = current.Value.ToString();
                        var left = value.Substring(0, value.Length / 2);
                        var right = value.Substring(left.Length);
                        current.Value = long.Parse(right);
                        line.AddBefore(current, long.Parse(left));
                    }
                    else
                    {
                        current.Value = current.Value * 2024;
                    }

                    current = current.Next;
                }

            }

            foreach (var _ in Enumerable.Range(1, 25))
            {
                Blink();
            }

            return line.Count;
        }

        private static long Problem2(string input)
        {
            var stones = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

            var line = new LinkedList<long>(stones);
            //var states = new List<List<long>>();
            //var primes = new List<(int i, int count, bool prime, int div)>();

            long Blink2(Dictionary<(long n, long t), long> precomputed, long number, int times)
            {
                if (times == 0)
                {
                    return 1;
                }

                if (precomputed.ContainsKey((number, times)))
                {
                    return precomputed[(number, times)];
                }

                if (number == 0)
                {
                    var result = Blink2(precomputed, 1, times - 1);

                    precomputed.Add((number, times), result);

                    return result;
                }
                else if (number.ToString().Length % 2 == 0)
                {
                    var value = number.ToString();
                    var left = value.Substring(0, value.Length / 2);
                    var right = value.Substring(left.Length);

                    var result = Blink2(precomputed, long.Parse(left), times - 1) + Blink2(precomputed, long.Parse(right), times - 1);

                    precomputed.Add((number, times), result);

                    return result;
                }
                else
                {
                    var result = Blink2(precomputed, number * 2024, times - 1);

                    precomputed.Add((number, times), result);

                    return result;
                }
            }

            void Blink()
            {

                LinkedListNode<long> current = line.First;

                while (current != null)
                {

                    if (current.Value == 0)
                    {
                        current.Value = 1;
                    }
                    else if (current.Value.ToString().Length % 2 == 0)
                    {
                        var value = current.Value.ToString();
                        var left = value.Substring(0, value.Length / 2);
                        var right = value.Substring(left.Length);
                        current.Value = long.Parse(right);
                        line.AddBefore(current, long.Parse(left));
                    }
                    else
                    {
                        current.Value = current.Value * 2024;
                    }

                    current = current.Next;
                }

            }

            long total = 0;

            Dictionary<(long n, long t), long> precomputed = new();

            foreach (var stone in line)
            {

                total += Blink2(precomputed, stone, 75);
            }

            //int c = line.Count;
            //primes.Add((0, c, false, c));
            //foreach (var round in Enumerable.Range(1, 25))
            //{
            //    Blink();

            //    var state = line.ToList();
            //    var indeces = state.Select((x, i) => (line.First.Value == x, i, x)).Where(x => x.Item1).ToList();

            //    var isPrime = Prime.IsPrime(line.Count);


            //    primes.Add((round, line.Count, isPrime, line.Count - c));
            //    states.Add(state);
            //    c = line.Count;
            //}

            return total;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadInput();

            Assert.Equal(-1, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadInput();

            Assert.Equal(-1, Problem2(input));
        }

        private string exampleText = "0 1 10 99 999";
        private string exampleText2 = "125 17";

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            Assert.Equal(55312, Problem1(exampleText));
        }
        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample2()
        {
            Assert.Equal(55312, Problem1(exampleText2));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            Assert.Equal(-1, Problem2(exampleText));
        }
    }
}
