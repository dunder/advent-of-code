using Facet.Combinatorics;
using Solutions.Event2016.Day11.EnumFlagsSolution;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 7: Phrase ---
    public class Day07
    {
        private readonly ITestOutputHelper output;

        public Day07(ITestOutputHelper output)
        {
            this.output = output;
        }

        List<(long result, List<int> numbers)> Parse(IList<string> lines)
        {
            List<(long result, List<int> numbers)> input = new();

            foreach (var line in lines)
            {
                var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                var result = long.Parse(parts[0]);
                var numbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                input.Add((result, numbers));
            }

            return input;
        }

        public enum Operator
        {
            Addition,
            Multiplication
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            var calibrations = Parse(input);

            long Execute()
            {
                var valid = new List<long>();

                foreach (var calibration in calibrations)
                {
                    var binaries = new List<string>();
                    int length = calibration.numbers.Count-1;
                    int numbers = 1 << length; // equivalent of 2^n

                    for (int i = 0; i < numbers; i++)
                    {
                        string binary = Convert.ToString(i, 2);
                        string leading_zeroes = "0000000000".Substring(0, length - binary.Length);
                        binaries.Add(leading_zeroes + binary);
                    }

                    var totals = new List<long>();

                    foreach (var binary in binaries)
                    {
                        long first = calibration.numbers.First();

                        var (total, _) = calibration.numbers.Skip(1).Aggregate((first, binary),
                            (state, number) =>
                            {
                                var (total, operators) = state;

                                var op = operators.First();

                                if (op == '1')
                                {
                                    total += number;
                                }
                                else
                                {
                                    total *= number;
                                }


                                return (total, operators.Substring(1));
                            });

                        totals.Add(total);
                    }

                    if (totals.Any(x => x == calibration.result))
                    {
                        valid.Add(calibration.result);
                    }
                }
                return valid.Sum();
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            var calibrations = Parse(input);


            long Execute()
            {
                var valid = new List<long>();

                foreach (var calibration in calibrations)
                {
                    var combinations = new Variations<int>(new List<int> { 1, 2, 3 }, calibration.numbers.Count, GenerateOption.WithRepetition);

                    var totals = new List<long>();

                    foreach (var combination in combinations)
                    {
                        long first = calibration.numbers.First();

                        var (total, _) = calibration.numbers.Skip(1).Aggregate((first, combination),
                            (state, number) =>
                            {
                                var (total, operators) = state;

                                var op = operators.First();

                                if (op == 1)
                                {
                                    total += number;
                                }
                                else if (op == 2)
                                {
                                    var merge = total.ToString() + number;
                                    total = long.Parse(merge);
                                }
                                else
                                {
                                    total *= number;
                                }

                                return (total, operators.Skip(1).ToList());
                            });

                        totals.Add(total);
                    }

                    if (totals.Any(x => x == calibration.result))
                    {
                        valid.Add(calibration.result);
                    }
                }
                return valid.Sum();
            }

            Assert.Equal(-1, Execute());
        }

        private string exampleText = "";
        private List<string> exampleInput =
            [
                "190: 10 19",
                "3267: 81 40 27",
                "83: 17 5",
                "156: 15 6",
                "7290: 6 8 6 15",
                "161011: 16 10 13",
                "192: 17 8 14",
                "21037: 9 7 18 13",
                "292: 11 6 16 20",
            ];

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {

            var calibrations = Parse(exampleInput);

            long Execute()
            {
                var valid = new List<long>();

                foreach (var calibration in calibrations)
                {
                    var binaries = new List<string>();
                    int length = 3;
                    int numbers = 1 << length; // equivalent of 2^n

                    for (int i = 0; i < numbers; i++)
                    {
                        string binary = Convert.ToString(i, 2);
                        string leading_zeroes = "00000000".Substring(0, length - binary.Length);
                        binaries.Add(leading_zeroes + binary);
                    }

                    var totals = new List<long>();

                    foreach (var binary in binaries)
                    {
                        long first = calibration.numbers.First();

                        var (total, _) = calibration.numbers.Skip(1).Aggregate((first, binary),
                            (state, number) =>
                            {
                                var (total, operators) = state;

                                var op = operators.First();

                                if (op == '1')
                                {
                                    total += number;
                                }
                                else
                                {
                                    total *= number;
                                }


                                return (total, operators.Substring(1));
                            });

                        totals.Add(total);
                    }

                    if (totals.Any(x => x == calibration.result))
                    {
                        valid.Add(calibration.result);
                    }
                }
                return valid.Sum();
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {

            var calibrations = Parse(exampleInput);


            long Execute()
            {
                var valid = new List<long>();

                foreach (var calibration in calibrations)
                {
                    var combinations = new Variations<int>(new List<int> { 1, 2, 3 }, 3, GenerateOption.WithRepetition);

                    var totals = new List<long>();

                    foreach (var combination in combinations)
                    {
                        long first = calibration.numbers.First();

                        var (total, _) = calibration.numbers.Skip(1).Aggregate((first, combination),
                            (state, number) =>
                            {
                                var (total, operators) = state;

                                var op = operators.First();

                                if (op == 1)
                                {
                                    total += number;
                                }
                                else if (op == 2)
                                {
                                    var merge = total.ToString() + number;
                                    total = long.Parse(merge);
                                }
                                else
                                {
                                    total *= number;
                                }

                                return (total, operators.Skip(1).ToList());
                            });

                        totals.Add(total);
                    }

                    if (totals.Any(x => x == calibration.result))
                    {
                        valid.Add(calibration.result);
                    }
                }
                return valid.Sum();
            }

            Assert.Equal(-1, Execute());
        }
    }
}
