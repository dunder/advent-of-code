using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 2: Password Philosophy ---
    public class Day02
    {
        private readonly ITestOutputHelper output;

        public Day02(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record PasswordPolicy(int Min, int Max, char Character, string Password)
        {
            public bool IsValid
            {
                get
                {
                    var count = Password.Count(c => c == Character);

                    return count >= Min && count <= Max;
                }
            }


            public bool IsValid2
            {
                get
                {
                    return Password[Min-1] == Character ^ Password[Max-1] == Character;
                }
            }
        }

        private static IList<PasswordPolicy> Parse(IList<string> input)
        {
            var regex = new Regex(@"(\d+)-(\d+) (\w): (.*)");
            return input.Select(line =>
            {
                List<Group> groups = regex.Match(line).Groups.Values.ToList();

                var min = int.Parse(groups[1].Value);
                var max = int.Parse(groups[2].Value);
                char character = groups[3].Value.ToCharArray()[0];
                string password = groups[4].Value;
                
                return new PasswordPolicy(min, max, character, password);
            }).ToList();
        }

        private static int CountValid(IList<string> input)
        {
            return Parse(input).Count(policy => policy.IsValid);
        }

        private static int CountValid2(IList<string> input)
        {
            return Parse(input).Count(policy => policy.IsValid2);
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return CountValid(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return CountValid2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            var policies = Parse(ReadLineInput());

            Assert.Equal(414, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(413, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc"
            };

            Assert.Equal(2, CountValid(input));
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string>
            {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc"
            };

            Assert.Equal(1, CountValid2(input));

        }
    }
}
