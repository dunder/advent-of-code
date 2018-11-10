using System;

namespace Solutions {
    class Program {
        static void Main(string[] args)
        {
            ProblemBase day = new Event2016.Day13.Problem();

            Console.WriteLine($"Running {day.Event} {day.Day}");

            var firstStar = day.FirstStar();
            var secondStar = day.SecondStar();

            var maze = Event2016.Day13.Problem.Print(10, 9, 6);
            foreach (var row in maze)
            {
                Console.WriteLine(row);
            }

            Console.WriteLine("First star:");
            Console.WriteLine($"{firstStar}");
            Console.WriteLine("Second star:");
            Console.WriteLine($"{secondStar}");
            Console.ReadKey();
        }
    }
}
