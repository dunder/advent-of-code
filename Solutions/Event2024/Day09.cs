using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 9: Disk Fragmenter ---
    public class Day09
    {
        private readonly ITestOutputHelper output;

        public Day09(ITestOutputHelper output)
        {
            this.output = output;
        }

        public record File(int Id, int Size)
        {
            public override string ToString()
            {
                return $"[{Id}: {Size}]";
            }
        }

        public record Segment()
        {
            public int MemoryLocation { get; set; }
            public int Size { get; set; }
            public int Available { get; set; }
            public List<File> Files { get; set; } = [];

            public void Add(File file)
            {
                if (file.Size > Available)
                {
                    throw new InvalidOperationException("Out of memory");
                }

                Files.Add(file);

                Available -= file.Size;
            }

            public void Remove(int fragment)
            {
                Available = Available + fragment;
                var currentFile = Files.Single();
                Files.Clear();
                Files.Add(currentFile with { Size = currentFile.Size - fragment});
            }

            public void Clear()
            {
                Available = Size;
                Files = [];
            }

            public long Checksum
            {
                get
                {
                    long checksum = 0;
                    var m = MemoryLocation;

                    foreach (var file in Files)
                    {
                        foreach (var f in Enumerable.Range(1, file.Size))
                        {
                            checksum += m++ * file.Id;
                        }
                    }
                    return checksum;
                }
            }

            public override string ToString()
            {
                return $"{MemoryLocation} ({Available}): " + string.Join(",", Files);
            }
        }

        public (List<Segment> memory, Dictionary<int, Segment> fileIndex) MapMemory(string diskMap)
        {
            var memory = new List<Segment>();
            int memoryLocation = 0;

            Dictionary<int, Segment> fileIndex = new();

            for (int i = 0; i < diskMap.Length; i++)
            {
                var n = int.Parse(diskMap[i].ToString());

                if (i % 2 == 0)
                {
                    var id = i / 2;

                    File file = new File(id, n);
                    Segment segment = new Segment
                    {
                        MemoryLocation = memoryLocation,
                        Size = n,
                        Available = 0,
                        Files = [file]
                    };

                    fileIndex.Add(id, segment);

                    memory.Add(segment);
                }
                else
                {
                    Segment segment = new Segment
                    {
                        MemoryLocation = memoryLocation,
                        Size = n,
                        Available = n,
                    };

                    memory.Add(segment);
                }

                memoryLocation += n;
            }

            return (memory, fileIndex);
        }

        public long Problem1(string input)
        {
            (List<Segment> memory, Dictionary<int, Segment> fileIndex)  = MapMemory(input);

            int currentFileIndex = fileIndex.Keys.Max();

            while (currentFileIndex >= 0)
            {
                Segment sourceSegment = fileIndex[currentFileIndex];
                Segment targetSegment = memory.FirstOrDefault(segment => segment.Available > 0 && segment.MemoryLocation < sourceSegment.MemoryLocation);

                if (targetSegment == null)
                {
                    break;
                }

                File sourceFile = sourceSegment.Files.Single();

                if (sourceFile.Size <= targetSegment.Available)
                {
                    targetSegment.Add(sourceFile);
                    sourceSegment.Clear();
                    currentFileIndex--;
                }
                else
                {
                    var available = targetSegment.Available;
                    targetSegment.Add(sourceFile with { Id = sourceFile.Id, Size = available });
                    sourceSegment.Remove(available);
                }
            }

            return memory.Where(segment => segment.Files.Any()).Select(segment => segment.Checksum).Sum();
        }

        public long Problem2(string input)
        {
            (List<Segment> memory, Dictionary<int, Segment> fileIndex) = MapMemory(input);

            int currentFileIndex = fileIndex.Keys.Max();

            while (currentFileIndex >= 0)
            {
                var sourceSegment = fileIndex[currentFileIndex];

                Segment targetSegment = memory.FirstOrDefault(segment => segment.Available >= sourceSegment.Size && segment.MemoryLocation < sourceSegment.MemoryLocation);

                if (targetSegment != null)
                {
                    targetSegment.Add(sourceSegment.Files.Single());
                    sourceSegment.Available = sourceSegment.Size;
                    sourceSegment.Files.Clear();
                }

                currentFileIndex--;
            }

            return memory.Where(segment => segment.Files.Any()).Select(segment => segment.Checksum).Sum();
        }

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadInput();

            Assert.Equal(6200294120911, Problem1(input));
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadInput();

            Assert.Equal(6227018762750, Problem2(input));
        }

        private string exampleText = "2333133121414131402";

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            var input = exampleText;

            Assert.Equal(1928, Problem1(input));
        }


        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            var input = exampleText;

            Assert.Equal(2858, Problem2(input));
        }
    }
}
