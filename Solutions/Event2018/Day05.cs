﻿using System.Collections.Generic;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day05 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day05;
        public string Name => "Alchemical Reduction";

        public string FirstStar()
        {
            var input = ReadInput();
            var result = ReduceAll(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = ReduceEnhanced(input);
            return result.ToString();
        }

        public static int LinkedReduce(string polymer)
        {
            var linkedPolymer = new LinkedList<char>(polymer);
            
            var left = linkedPolymer.First;
            var right = left.Next;

            while (right.Next != null)
            {
                var leftUnit = left.Value;
                var rightUnit = right.Value;

                if (leftUnit != rightUnit && char.ToUpper(leftUnit) == char.ToUpper(rightUnit))
                {
                    var newLeft = left.Previous;
                    var newRight = right.Next;

                    if (newLeft == null)
                    {
                        newLeft = right;
                        newRight = right.Next;
                    }

                    linkedPolymer.Remove(left);
                    linkedPolymer.Remove(right);

                    left = newLeft;
                    right = newRight;
                }
                else
                {
                    left = right;
                    right = left.Next;
                }
            }

            return linkedPolymer.Count;
        }

        public static int ReduceAll(string polymer)
        {
            var closed = new HashSet<int>();
            var reduction = 0;
            for (int i1 = 0, i2 = 1; i2 < polymer.Length;)
            {
                char c1 = polymer[i1];
                char c2 = polymer[i2];

                if (c1 == c2)
                {
                    i1 = i2;
                    i2++;
                    continue;
                }

                if (char.ToUpper(c1) == char.ToUpper(c2))
                {
                    closed.Add(i1);
                    closed.Add(i2);
                    do
                    {
                        i1--;
                    } while (closed.Contains(i1));

                    if (i1 < 0)
                    {
                        i2++;
                        i1 = i2;
                        i2++;
                    }
                    else
                    {
                        i2++;
                    }
;
                    reduction += 2;
                    continue;
                }

                i1 = i2;
                i2++;
            }

            return polymer.Length - reduction;
        }

        public static int ReduceEnhanced(string polymer)
        {
            var all = new HashSet<char>(polymer);

            int min = int.MaxValue;
            foreach (var unit in all)
            {
                var polymerToReduce = polymer;
                char cLower = char.ToLower(unit);
                char cUpper = char.ToUpper(unit);
                polymerToReduce = polymerToReduce.Replace(cLower.ToString(), "");
                polymerToReduce = polymerToReduce.Replace(cUpper.ToString(), "");

                var reduced = ReduceAll(polymerToReduce);

                if (reduced < min)
                {
                    min = reduced;
                }
            }

            return min;
        }

        public static string ReactReduce(string polymer)
        {
            int i = 0;
            bool reduced = false;
            for (; i < polymer.Length - 1; i++)
            {
                var p1 = polymer[i];
                var p2 = polymer[i + 1];

                if (p1 == p2)
                {
                    continue;
                };

                if (char.ToLower(p1) == char.ToLower(p2))
                {
                    reduced = true;
                    break;
                }
            }

            return reduced ? polymer.Remove(i, 2) : polymer;
        }

        [Theory]
        [InlineData("dabAcCaCBAcCcaDA", "dabAaCBAcCcaDA")]
        [InlineData("dabAaCBAcCcaDA", "dabCBAcCcaDA")]
        public void ReactReduceTest(string polymer, string expected)
        {
            var reduced = ReactReduce(polymer);

            Assert.Equal(expected, reduced);
        }

        [Fact]
        public void FirstStarExample()
        {
            var polymer = "dabAcCaCBAcCcaDA";
            var reduced = ReduceAll(polymer);

            Assert.Equal(10, reduced);
        }

        [Fact]
        public void LinkedReducedExample()
        {
            var polymer = "dabAcCaCBAcCcaDA";
            var reduced = LinkedReduce(polymer);

            Assert.Equal(10, reduced);
        }

        [Fact]
        public void SecondStarExample()
        {
            var polymer = "dabAcCaCBAcCcaDA";
            var reduced = ReduceEnhanced(polymer);

            Assert.Equal(4, reduced);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("10132", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("4572", actual);
        }
    }
}