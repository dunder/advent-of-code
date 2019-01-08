using System.Collections.Generic;
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
            var result = LengthOfPolymerAfterFullReduction(input);
            return result.ToString();
        }

        public string SecondStar()
        {
            var input = ReadInput();
            var result = LengthOfReducedPolymerAfterRemovalOfProblematicUnit(input);
            return result.ToString();
        }

        public static int LengthOfPolymerAfterFullReduction(string polymer)
        {
            var linkedPolymer = new LinkedList<char>(polymer);
            
            var left = linkedPolymer.First;
            var right = left.Next;

            while (right != null)
            {
                var leftUnit = left.Value;
                var rightUnit = right.Value;

                if (leftUnit != rightUnit && char.ToUpper(leftUnit) == char.ToUpper(rightUnit))
                {
                    var newLeft = left.Previous;
                    var newRight = right.Next;
                    
                    if (newLeft == null)
                    {
                        newLeft = newRight;
                        newRight = newLeft?.Next;
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

        public static int LengthOfReducedPolymerAfterRemovalOfProblematicUnit(string polymer)
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

                var reduced = LengthOfPolymerAfterFullReduction(polymerToReduce);

                if (reduced < min)
                {
                    min = reduced;
                }
            }

            return min;
        }

        [Fact]
        public void FirstStarExample()
        {
            var polymer = "dabAcCaCBAcCcaDA";
            var reducedLength = LengthOfPolymerAfterFullReduction(polymer);

            Assert.Equal(10, reducedLength);
        }
        
        [Fact]
        public void SecondStarExample()
        {
            var polymer = "dabAcCaCBAcCcaDA";
            var reduced = LengthOfReducedPolymerAfterRemovalOfProblematicUnit(polymer);

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
