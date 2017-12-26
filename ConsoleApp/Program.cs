using System;
using System.IO;
using Y2017.Day23;

namespace ConsoleApp {
    public class Program {
        static void Main(string[] args) {

            string[] input = File.ReadAllLines(@".\Day23\input.txt");

            var result = Coprocessor.Run2(input);

            Console.WriteLine($"Register h is {result}");
            Console.ReadKey();
        }
    }
}
