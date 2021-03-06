﻿using System;
using System.Collections.Generic;
using Solutions.Event2019;

namespace Solutions {
    class Program {
        static void Main(string[] args)
        {
            var day = new Day25();
            
            day.Interactive();

            Console.ReadKey();
        }


        private static void PrintProblemSolution(ProblemBase problem)
        {
            Console.WriteLine($"Running {problem.Event} {problem.Day}");

            var firstStar = problem.FirstStar();
            var secondStar = problem.SecondStar();

            Console.WriteLine("First star:");
            Console.WriteLine($"{firstStar}");
            Console.WriteLine("Second star:");
            Console.WriteLine($"{secondStar}");
            Console.ReadKey();
        }
    }
}
