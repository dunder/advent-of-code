
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 1: Trebuchet?! ---
    public class Day01
    {
        private readonly ITestOutputHelper output;

        public Day01(ITestOutputHelper output)
        {
            this.output = output;
        }

        private int SumOfCalibrationValues(IList<string> input)
        {
            return input.Select(text =>
            {
                var x = text.First(char.IsDigit);
                var y = text.Last(char.IsDigit);

                return int.Parse(string.Join("", x, y));
            }
            ).Sum();
        }

        private static string ToDigit(string number) => number switch
        {
            "one" => "1",
            "two" => "2",
            "three" => "3",
            "four" => "4",
            "five" => "5",
            "six" => "6",
            "seven" => "7",
            "eight" => "8",
            "nine" =>  "9",
            _ => number,
        };

        private static List<string> numbers = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        private static bool TryParseDigit(string text, out string digit) {
            var found = numbers.FirstOrDefault(n => text.StartsWith(n));
            if (found != null)
            {
                digit = ToDigit(found);
                return true;
            }
            digit = default;
            return false;
        }

        private int SumOfCalibrationValuesWithLetters(IList<string> input)
        {
            return input.Select(text =>
            {
                string first = "";
                for (int i = 0; i < text.Length; i++)
                {
                    var c = text[i];
                    var rest = text.Substring(i);
                    if (char.IsDigit(c))
                    {
                        first = c.ToString();
                        break;
                    }
                    if (TryParseDigit(rest, out first))
                    {
                        break;
                    }
                }

                string last = "";
                for (int i = text.Length - 1; i >= 0; i--)
                {
                    var c = text[i];
                    var rest = text.Substring(i);
                    if (char.IsDigit(c))
                    {
                        last = c.ToString();
                        break;
                    }
                    if (TryParseDigit(rest, out last))
                    {
                        break;
                    }
                }

                return int.Parse(first + last);
            }
            ).Sum();
        }
        public int FirstStar()
        {
            var input = ReadLineInput();
            return SumOfCalibrationValues(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return SumOfCalibrationValuesWithLetters(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(55386, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(54824, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "1abc2",
                "pqr3stu8vwx",
                "a1b2c3d4e5f",
                "treb7uchet"
            };

            Assert.Equal(142, SumOfCalibrationValues(example));
        }

        [Fact]
        public void SecondStarExample()
        {
             var example = new List<string>
            {
                "two1nine",
                "eightwothree",
                "abcone2threexyz",
                "xtwone3four",
                "4nineeightseven2",
                "zoneight234",
                "7pqrstsixteen"
            };

            Assert.Equal(281, SumOfCalibrationValuesWithLetters(example));
        }
    }
}
