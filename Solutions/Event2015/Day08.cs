using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 8: Matchsticks ---
    public class Day08
    {
        public static string Escape(string input)
        {
            input = input.Substring(1, input.Length - 2);


            input = Regex.Replace(input, @"\\x[0-9a-f][0-9a-f]", "-");
            input = input.Replace(@"\""", @"""");
            input = input.Replace(@"\\", @"\");
            return input;
        }

        public static int Run(IEnumerable<string> input)
        {
            int charactersOfCode = 0;
            int charactersInMemory = 0;

            foreach (var line in input)
            {
                charactersOfCode += line.Length;

                var escaped = Escape(line);
                charactersInMemory += escaped.Length;
            }

            return charactersOfCode - charactersInMemory;
        }

        public static string Encode(string input)
        {
            input = input.Replace(@"\", @"\\");
            input = input.Replace(@"""", @"\""");

            return $"\"{input}\"";
        }

        public static int Run2(IEnumerable<string> input)
        {
            int charactersOfCode = 0;
            int charactersEncoded = 0;

            foreach (var line in input)
            {
                charactersOfCode += line.Length;

                var encoded = Encode(line);
                charactersEncoded += encoded.Length;
            }

            return charactersEncoded - charactersOfCode;
        }

        public static int FirstStar()
        {
            var input = ReadLineInput();

            return Run(input);
        }

        public static int SecondStar()
        {
            var input = ReadLineInput();

            return Run2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(1350, result);
        }

        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(2085, result);
        }

        [Theory]
        [InlineData("\"\"", "")]
        [InlineData("\"abc\"", "abc")]
        [InlineData("\"aaa\\\"aaa\"", "aaa\"aaa")]
        [InlineData("\"\\x27\"", "-")]
        public void FirstStarExamples(string input, string expectedString)
        {
            var escaped = Escape(input);

            Assert.Equal(expectedString, escaped);
        }

        [Fact]
        public void FirstStar_ExampleRun()
        {
            var input = new[]
            {
                "\"\"",
                "\"abc\"",
                "\"aaa\\\"aaa\"",
                "\"\\x27\""
            };

            var result = Run(input);

            Assert.Equal(12, result);
        }

        [Fact]
        public void SecondStar_ExampleRun()
        {
            var input = new[]
            {
                "\"\"",
                "\"abc\"",
                "\"aaa\\\"aaa\"",
                "\"\\x27\""
            };

            var result = Run2(input);

            Assert.Equal(19, result);
        }
    }
}
