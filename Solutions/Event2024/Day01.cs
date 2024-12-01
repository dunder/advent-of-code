using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day X: Phrase ---
    public class Day01
    {
        private readonly ITestOutputHelper output;

        public Day01(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();


            int Execute()
            {
                var y = input.Select(x => 
                {
                    var z = x.Split(' ');

                    return z;
                }
                );

                var left = new List<int>();
                var right = new List<int>();

                foreach (var value in y)
                {
                    left.Add(int.Parse(value[0]));
                    right.Add(int.Parse(value[3]));
                }

                left.Sort();
                right.Sort();

                return left.Zip(right).Select((f, s) => Math.Abs(f.First - f.Second)).Sum();

                
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            int Execute()
            {
                var y = input.Select(x =>
                {
                    var z = x.Split(' ');

                    return z;
                }
                );

                var left = new List<int>();
                var right = new Dictionary<int, int>();

                foreach (var value in y)
                {
                    left.Add(int.Parse(value[0]));
                    var r = int.Parse(value[3]);


                    if (right.TryGetValue(r, out var s))
                    {
                        right[r] = right[r] + 1;
                    }
                    else
                    {
                        right.Add(r, 1);
                    }
                }

                int total = 0;

                foreach (var value in left)
                {
                    if (right.TryGetValue((int)value, out var found))
                    {
                        total += found * value;
                    }
                }

                return total;


            }

            Assert.Equal(-1, Execute());
        }


        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            string inputText = "";
            List<string> inputLines = 
                [
                    "",
                    ""
                ];

            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            string inputText = "";
            List<string> inputLines = 
                [
                    "", 
                    ""
                ];

            int Execute()
            {
                return 0;
            }

            Assert.Equal(-1, Execute());
        }
    }
}
