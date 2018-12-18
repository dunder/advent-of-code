using System;
using Solutions.Event2018;

namespace Solutions {
    class Program {
        static void Main(string[] args)
        {

            var day15 = new Day18();

            day15.SecondStar();

            Console.ReadKey();
        }

        private static void Run2018Day10Example()
        {
            new Day10().FirstStarExample();
        }

        private static void Run2018Day10()
        {
            new Day10().SecondStar();
        }

        private static void PrintDay22MemorySetup()
        {
            Event2016.Day22.Problem.PrintInitialMemoryGrid();
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
