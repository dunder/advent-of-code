using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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

        private class Directory
        {
            public Directory (string name, Directory parent)
            {
                Name = name;
                Parent = parent;
            }
            public Directory Parent { get; private set; }
            public string Name { get; }
            public List<Directory> Directories { get; } = [];
            public List<File> Files { get; } = [];
        }

        private record File(string Name, int Size);

        private static int Problem1(IList<string> input)
        {
            Dictionary<string, Directory> disk = [];

            Directory current = null;

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];

                if (line.StartsWith("$ cd"))
                {
                    string name = line.Substring("$ cd ".Length);

                    Directory directory = new Directory(name, current);

                }
                else if (line.StartsWith("$ ls"))
                {

                    for (i = i + 1; i < input.Count; i++)
                    {
                        string content = input[i];

                        if (content.StartsWith("dir "))
                        {

                        }
                        else
                        {

                        }

                        if (i + 1 >= input.Count || input[i + 1].StartsWith("$"))
                        {
                            break;
                        }
                    }
                }
            }

            return 0;
        }

        private static int Problem2(IList<string> input)
        {
            return 0;
        }

        [Fact]
        [Trait("Event", "2022")]
        public void FirstStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2022")]
        public void SecondStarTest()
        {
            var input = ReadLineInput();

            Assert.Equal(-1, Problem2(input));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void FirstStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem1(exampleInput));
        }

        [Fact]
        [Trait("Example", "2022")]
        public void SecondStarExample()
        {
            var exampleInput = ReadExampleLineInput("Example");

            Assert.Equal(-1, Problem2(exampleInput));
        }
    }
}