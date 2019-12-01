using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Solutions.Event2015
{
    // --- Day 10: Elves Look, Elves Say ---
    public class Day10
    {
        private const string Input = "3113322113";

        public class NumberGroup
        {
            public int Value { get; }
            public int Count { get; }

            public NumberGroup(int value, int count)
            {
                Value = value;
                Count = count;
            }

            public string Expand()
            {
                return $"{Count}{Value}";
            }

            protected bool Equals(NumberGroup other)
            {
                return Value == other.Value && Count == other.Count;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((NumberGroup) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Value * 397) ^ Count;
                }
            }

            public override string ToString()
            {
                return $"{Value} is repeated {Count} times";
            }
        }

        public static string Expand(string input)
        {
            IEnumerable<NumberGroup> groups = EnumerateNumberGroups(input);
            return string.Join("", groups.Select(group => group.Expand()));
        }

        public static IEnumerable<NumberGroup> EnumerateNumberGroups(string input)
        {
            var previous = int.Parse(input.Substring(0, 1));
            var counter = 0;
            
            if (input.Length == 1)
            {
                yield return new NumberGroup(previous, 1);
            }

            for (int i = 1; i < input.Length; i++)
            {
                var current = int.Parse(input[i].ToString());
                counter++;
                
                if (current == previous)
                {
                    if (i == input.Length - 1)
                    {
                        counter++;
                        yield return new NumberGroup(current, counter);
                    }

                    previous = current;
                    continue;
                }

                yield return new NumberGroup(previous, counter);
                if (i == input.Length - 1)
                {
                    yield return new NumberGroup(current, 1);
                }
                counter = 0;
                previous = current;
            }
        }

        public static int RepeatedExpand(string input, int times)
        {
            
            foreach (var _ in Enumerable.Range(1, times))
            {
                input = Expand(input);
            }

            return input.Length;
        }

        public static int FirstStar()
        {
            return RepeatedExpand(Input, 40);
        }

        public static int SecondStar()
        {
            return RepeatedExpand(Input, 50);
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(329356, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(4666278, result);
        }

        [Fact]
        public void EnumerateNumberGroupTest()
        {
            var groups = EnumerateNumberGroups("111455661").ToList();

            Assert.Collection(groups,
                group => Assert.Equal(new NumberGroup(1,3), group),
                group => Assert.Equal(new NumberGroup(4,1), group),
                group => Assert.Equal(new NumberGroup(5, 2), group),
                group => Assert.Equal(new NumberGroup(6, 2), group),
                group => Assert.Equal(new NumberGroup(1, 1), group));
        }

        [Fact]
        public void EnumerateNumberGroup_EndsWithRepetition_Test()
        {
            var groups = EnumerateNumberGroups("1114556611").ToList();

            Assert.Collection(groups,
                group => Assert.Equal(new NumberGroup(1,3), group),
                group => Assert.Equal(new NumberGroup(4,1), group),
                group => Assert.Equal(new NumberGroup(5, 2), group),
                group => Assert.Equal(new NumberGroup(6, 2), group),
                group => Assert.Equal(new NumberGroup(1, 2), group));
        }

        [Theory]
        [InlineData("1", "11")]
        [InlineData("11", "21")]
        [InlineData("21", "1211")]
        [InlineData("1211", "111221")]
        [InlineData("111221", "312211")]
        public void ExpandTests(string input, string expected)
        {
            string expanded = Expand(input);

            Assert.Equal(expected, expanded);
        }
    }
}
