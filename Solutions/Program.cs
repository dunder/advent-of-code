using System;
using System.Collections.Generic;
using Solutions.Event2019;

namespace Solutions {
    class Program {
        static void Main(string[] args)
        {
            // RunDay25Event2019();
            //RunDay20Event2023();
            RunDay23Event2023();

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }

        private static void RunDay23Event2023()
        {
            var day = new Event2023.Day23(null);

            var example = new List<string>
            {
                "#.#####################",
                "#.......#########...###",
                "#######.#########.#.###",
                "###.....#.>.>.###.#.###",
                "###v#####.#v#.###.#.###",
                "###.>...#.#.#.....#...#",
                "###v###.#.#.#########.#",
                "###...#.#.#.......#...#",
                "#####.#.#.#######.#.###",
                "#.....#.#.#.......#...#",
                "#.#####.#.#.#########v#",
                "#.#...#...#...###...>.#",
                "#.#.#v#######v###.###v#",
                "#...#.>.#...>.>.#.###.#",
                "#####v#.#.###v#.#.###.#",
                "#.....#...#...#.#.#...#",
                "#.#########.###.#.#.###",
                "#...###...#...#...#.###",
                "###.###.#.###v#####v###",
                "#...#...#.#.>.>.#.>.###",
                "#.###.###.#.###.#.#v###",
                "#.....###...###...#...#",
                "#####################.#"
            };

            day.Run1(example);
        }

        private static void RunDay20Event2023()
        {
            var day = new Event2023.Day20(null);
            
            day.FirstStar();
        }

        private static void RunDay25Event2019()
        {
            var day = new Day25();
            day.Interactive();
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
