using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 3: Phrase ---
    public class Day03
    {
        private readonly ITestOutputHelper output;

        public Day03(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static int Problem1(string input)
        {
            var r = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
            var x = r.Match(input);

            var result = 0;

            while (x.Success)
            {
                result += int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value);
                x = x.NextMatch();
            }

            return result;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadInput();

            Assert.Equal(184122457, Problem1(input));
        }

        private static int Problem2(string input)
        {
            var r = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)");
            var x = r.Match(input);

            var result = 0;

            bool apply = true;

            while (x.Success)
            {
                if (x.Value == "do()")
                {
                    apply = true;
                }
                else if (x.Value == "don't()")
                {
                    apply = false;
                }
                else
                {
                    if (apply)
                    {
                        result += int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value);
                    }
                }

                x = x.NextMatch();
            }

            return result;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadInput();

            Assert.Equal(107862689, Problem2(input));
        }


        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            string input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
            
            Assert.Equal(161, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            string input = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";           

            Assert.Equal(48, Problem2(input));
        }
    }
}
