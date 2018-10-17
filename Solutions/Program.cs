using System;
using System.Collections;
using Solutions.Event2017.Day1;

namespace Solutions {
    class Program {
        static void Main(string[] args)
        {
            ProblemBase day = new Problem();

            Console.WriteLine($"Running {day.Event} {day.Day}");

            var firstStar = day.FirstStar();
            var secondStar = day.SecondStar();

            Console.WriteLine($"First star 1: '{firstStar}', Second star: '{secondStar}'");
            Console.ReadKey();
        }
    }

    public enum Event
    {
        Event2016,
        Event2017
    }

    public enum Day
    {
        Day1,
        Day2,
        Day3,
        Day4,
        Day5,
        Day6,
        Day7,
        Day8,
        Day9,
        Day11,
        Day12,
        Day13,
        Day14,
        Day15,
        Day16,
        Day17,
        Day18,
        Day19,
        Day20,
        Day21,
        Day22,
        Day23,
        Day24,
        Day25
    }
}
