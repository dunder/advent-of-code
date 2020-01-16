using System;
using System.Collections.Generic;
using Solutions.Event2019;

namespace Solutions {
    class Program {
        static void Main(string[] args)
        {
            var day = new Day24();
            
            var startState = new List<string>
            {
                ".###.",
                "..#.#",
                "...##",
                "#.###",
                "..#.."
            };



            var encodedStartTiles = day.Parse(startState);
            day.BugsAfterMinutes(encodedStartTiles, 200);
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
