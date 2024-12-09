using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2024
{
    // --- Day 9: Phrase ---
    public class Day09
    {
        private readonly ITestOutputHelper output;

        public Day09(ITestOutputHelper output)
        {
            this.output = output;
        }

        public record Fragment(int Id);

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarTest()
        {
            var input = ReadInput();

            long Execute()
            {
                var memory = new List<Fragment>();

                for (int i = 0; i < input.Length; i++)
                {

                    if (i % 2 == 0)
                    {
                        var n = int.Parse(input[i].ToString());
                        var id = i / 2;

                        foreach (var _ in Enumerable.Range(1, n))
                        {
                            memory.Add(new Fragment(id));
                        }

                    }
                    else
                    {
                        var n = int.Parse(input[i].ToString());

                        foreach (var _ in Enumerable.Range(1, n))
                        {
                            memory.Add(new Fragment(-1));
                        }

                    }

                }

                int diskPosition = 0;

                int FindFree(int position)
                {
                    return memory.IndexOf(new Fragment(-1));
                }

                for (int i = memory.Count - 1; i >= 0; i--)
                {
                    if (memory[i].Id == -1)
                    {
                        continue;
                    }

                    diskPosition = FindFree(diskPosition);

                    if (diskPosition > i || diskPosition == -1)
                    {
                        break;
                    }

                    memory[diskPosition] = memory[i];
                    memory[i] = new Fragment(-1);

                    diskPosition++;
                }

                long total = 0;

                for (int i = 0; i < memory.Count; i++)
                {
                    if (memory[i].Id != -1)
                    {
                        total += memory[i].Id * i;
                    }
                }

                return total;
            }

            Assert.Equal(-1, Execute());
        }

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarTest()
        {
            var input = ReadInput();

            long Execute()
            {
                var memory = new List<Segment>();
                int memoryLocation = 0;

                Dictionary<int, Segment> fileIndex = new();

                for (int i = 0; i < input.Length; i++)
                {
                    var n = int.Parse(input[i].ToString());

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

                int currentFileIndex = fileIndex.Keys.Max();

                while (currentFileIndex >= 0)
                {
                    var sourceSegment = fileIndex[currentFileIndex];

                    Segment targetSegment = memory.FirstOrDefault(segment => segment.Available >= sourceSegment.Size && segment.MemoryLocation < sourceSegment.MemoryLocation);

                    if (targetSegment != null)
                    {
                        targetSegment.TryAdd(sourceSegment.Files.Single());
                        sourceSegment.Available = sourceSegment.Size;
                        sourceSegment.Files.Clear();
                    }

                    currentFileIndex--;
                }

                return memory.Where(segment => segment.Files.Any()).Select(segment => segment.Checksum).Sum();
            }

            Assert.Equal(-1, Execute()); // 5428154845694 too low
        }

        private string exampleText = "2333133121414131402";

        [Fact]
        [Trait("Event", "2024")]
        public void FirstStarExample()
        {
            var input = exampleText;

            int Execute()
            {
                var memory = new List<Fragment>();


                for (int i = 0; i < input.Length; i++)
                {

                    if (i % 2 == 0)
                    {
                        var n = int.Parse(input[i].ToString());
                        var id = i / 2;

                        foreach (var _ in Enumerable.Range(1, n))
                        {
                            memory.Add(new Fragment(id));
                        }

                    }
                    else
                    {
                        var n = int.Parse(input[i].ToString());

                        foreach (var _ in Enumerable.Range(1, n))
                        {
                            memory.Add(new Fragment(-1));
                        }

                    }

                }

                int diskPosition = 0;

                int FindFree(int position)
                {
                    return memory.IndexOf(new Fragment(-1));
                }                

                for (int i = memory.Count - 1; i >= 0; i--)
                {
                    if (memory[i].Id == -1)
                    {
                        continue;
                    }

                    diskPosition = FindFree(diskPosition);

                    if (diskPosition > i || diskPosition == -1)
                    {
                        break;
                    }

                    memory[diskPosition] = memory[i];
                    memory[i] = new Fragment(-1);

                    diskPosition++;
                }

                var total = 0;

                for (int i = 0; i < memory.Count; i++)
                {
                    if (memory[i].Id != -1)
                    {
                        total += memory[i].Id * i;
                    }
                }

                return total;
            }


            Assert.Equal(-1, Execute());
        }

        private record File(int Id, int Size)
        {
            public override string ToString()
            {
                return $"[{Id}: {Size}]";
            }
        }

        private record Segment()
        {
            public int MemoryLocation { get; set; }
            public int Size { get; set; }
            public int Available { get; set; }
            public List<File> Files { get; set; } = [];

            public bool TryAdd(File file) 
            {
                if (file.Size > Available)
                {
                    return false;
                }

                Files.Add(file);

                Available -= file.Size;

                return true;
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

        [Fact]
        [Trait("Event", "2024")]
        public void SecondStarExample()
        {
            var input = exampleText;

            long Execute()
            {
                var memory = new List<Segment>();
                int memoryLocation = 0;

                Dictionary<int, Segment> fileIndex = new();

                for (int i = 0; i < input.Length; i++)
                {
                    var n = int.Parse(input[i].ToString());

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

                int currentFileIndex = fileIndex.Keys.Max();

                while (currentFileIndex >= 0)
                {
                    var sourceSegment = fileIndex[currentFileIndex];

                    Segment targetSegment = memory.FirstOrDefault(segment => segment.Available >= sourceSegment.Size && segment.MemoryLocation < sourceSegment.MemoryLocation);

                    if (targetSegment != null)
                    {
                        targetSegment.TryAdd(sourceSegment.Files.Single());
                        sourceSegment.Available = sourceSegment.Size;
                        sourceSegment.Files.Clear();
                    }

                    currentFileIndex--;
                }
                
                return memory.Where(segment => segment.Files.Any()).Select(segment => segment.Checksum).Sum();
            }

            Assert.Equal(-1, Execute());
        }
    }
}
