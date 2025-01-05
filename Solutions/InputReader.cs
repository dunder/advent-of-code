using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Solutions
{
    public static class InputReader
    {
        public static IList<string> ReadLineInput([CallerFilePath] string callerPath = "")
        {
            var fileName = Path.GetFileNameWithoutExtension(callerPath);
            var inputPath = Path.Combine(Path.GetDirectoryName(callerPath), "Input", $"{fileName}.txt");
            return File.ReadAllLines(inputPath);
        }

        public static IList<string> ReadExampleLineInput(string exampleName, [CallerFilePath]string callerPath = "")
        {
            var fileName = Path.GetFileNameWithoutExtension(callerPath);
            var inputPath = Path.Combine(Path.GetDirectoryName(callerPath), "Input", $"{fileName}{exampleName}.txt");
            return File.ReadAllLines(inputPath);
        }

        public static string ReadInput([CallerFilePath]string callerPath = "")
        {
            var fileName = Path.GetFileNameWithoutExtension(callerPath);
            var inputPath = Path.Combine(Path.GetDirectoryName(callerPath), "Input", $"{fileName}.txt");
            return File.ReadAllText(inputPath);
        }

        public static string ReadExampleInput(string exampleName, [CallerFilePath]string callerPath = "")
        {
            var fileName = Path.GetFileNameWithoutExtension(callerPath);
            var inputPath = Path.Combine(Path.GetDirectoryName(callerPath), "Input", $"{fileName}{exampleName}.txt");
            return File.ReadAllText(inputPath);
        }
    }
}
