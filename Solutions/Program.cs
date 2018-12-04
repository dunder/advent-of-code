using System;

namespace Solutions {
    class Program {
        static void Main(string[] args)
        {
            Console.ReadKey();
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
