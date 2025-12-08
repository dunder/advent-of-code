using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2022
{
    // --- Day 7: No Space Left On Device ---
    public class Day07
    {
        private readonly ITestOutputHelper output;

        public Day07(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record File(string Name, int Size);

        private class FileSystem
        {
            public Dictionary<string, List<File>> Files {  get; set; }
            public Dictionary<string, List<string>> Directories {  get; set; }
        }

        private static FileSystem Parse(IList<string> input)
        {
            Dictionary<string, List<File>> files = [];
            Dictionary<string, List<string>> directories = [];

            Stack<string> path = [];

            string CurrentDirectory()
            {
                return "/" + string.Join("/", path.Reverse().Skip(1));
            }

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];

                if (line.StartsWith("$ cd"))
                {
                    string name = line.Substring("$ cd ".Length);

                    if ("..".Equals(name))
                    {
                        path.Pop();
                    }
                    else
                    {
                        path.Push(name);
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                    string directoryName = CurrentDirectory();

                    directories.Add(directoryName, []);
                    files.Add(directoryName, []);

                    for (i = i + 1; i < input.Count; i++)
                    {
                        string content = input[i];

                        if (content.StartsWith("dir "))
                        {
                            directories[directoryName].Add(content.Substring("dir ".Length));
                        }
                        else
                        {
                            var parts = content.Split(" ");
                            var size = int.Parse(parts[0]);
                            var name = parts[1];
                            files[directoryName].Add(new File(name, size));
                        }

                        if (i + 1 >= input.Count || input[i + 1].StartsWith("$"))
                        {
                            break;
                        }
                    }
                }
            }

            return new FileSystem
            {
                Files = files,
                Directories = directories,
            };
        }

        private static long Problem1(IList<string> input)
        {
            FileSystem fs = Parse(input);

            Dictionary<string, long> fileSizes = [];

            foreach (var directory in fs.Directories.Keys)
            {
                fileSizes[directory] = fs.Files[directory].Select(f => f.Size).Sum();
            }

            Dictionary<string, long> directorySizes = [];

            foreach (var directory in fs.Directories.Keys)
            {
                long subTotal = fileSizes.Where(f => f.Key.StartsWith(directory)).Select(kvp => kvp.Value).Sum();

                directorySizes[directory] = subTotal;
            }

            return directorySizes.Values.Where(size => size <= 100000).Sum();
        }

        private static long Problem2(IList<string> input)
        {
            FileSystem fs = Parse(input);

            Dictionary<string, long> fileSizes = [];

            foreach (var directory in fs.Directories.Keys)
            {
                fileSizes[directory] = fs.Files[directory].Select(f => f.Size).Sum();
            }

            Dictionary<string, long> directorySizes = [];

            foreach (var directory in fs.Directories.Keys)
            {
                long subTotal = fileSizes.Where(f => f.Key.StartsWith(directory)).Select(kvp => kvp.Value).Sum();

                directorySizes[directory] = subTotal;
            }

            int capacity = 70000000;
            int required = 30000000;
            var free = capacity - directorySizes["/"];
            var missing = required - free;

            return directorySizes.Values.OrderBy(size => size).First(size => size >= missing);
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(1513699, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(7991939, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(95437, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(24933642, Problem2(exampleInput));
        }
    }
}