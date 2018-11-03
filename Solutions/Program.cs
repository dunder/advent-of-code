using System;

namespace Solutions {
    class Program {
        static void Main(string[] args)
        {
            ProblemBase day = new Event2016.Day11.Problem();

            Console.WriteLine($"Running {day.Event} {day.Day}");

            var firstStar = day.FirstStar();
            var secondStar = ""; //day.SecondStar();

            Console.WriteLine("First star:");
            Console.WriteLine($"{firstStar}");
            Console.WriteLine("Second star:");
            Console.WriteLine($"{secondStar}");
            Console.ReadKey();
        }
    }
}
