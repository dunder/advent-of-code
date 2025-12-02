using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2025
{
    // --- Day 2: Gift Shop ---
    public class Day02
    {
        private readonly ITestOutputHelper output;

        public Day02(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static long Problem1(IList<string> input)
        {
            var text = input[0];

            var ranges = text.Split(",");

            long sum = 0;

            bool IsInvalid(long id)
            {
                var s = id.ToString();

                // if length is odd we cannot split in half
                if (s.Length % 2 != 0)
                {
                    return false;
                }

                var first = s.Substring(0, s.Length / 2);
                var second = s.Substring(s.Length / 2);
                if (first == second)
                {
                    return true;
                }

                return false;
            }

            foreach (var range in ranges)
            {
                var parts = range.Split("-");
                var from = long.Parse(parts[0]);
                var to = long.Parse(parts[1]);

                for (long id = from;  id <= to; id++)
                {
                    if (IsInvalid(id))
                    {
                        sum += id;
                    }
                }
            }

            return sum;
        }

        private static long Problem2(IList<string> input)
        {
            var text = input[0];

            var ranges = text.Split(",");

            long sum = 0;

            bool IsInvalid(long id)
            {
                var s = id.ToString();

                for (int chunkSize = s.Length / 2; chunkSize > 0; chunkSize--)
                {
                    if (s.Length % chunkSize == 0)
                    {
                        var chunks = s.Chunk(chunkSize).Select(chunk => new string(chunk)).ToList();
                        if (chunks.All(part => part == chunks[0]))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            foreach (var range in ranges)
            {
                var parts = range.Split("-");
                var from = long.Parse(parts[0]);
                var to = long.Parse(parts[1]);

                for (long id = from; id <= to; id++)
                {
                    if (IsInvalid(id))
                    {
                        sum += id;
                    }
                }
            }

            return sum;
        }

        [Fact]
        [Trait("Event", "2025")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(64215794229, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2025")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(85513235135, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(1227775554, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2025")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(4174379265, Problem2(exampleInput));
        }
    }
}
