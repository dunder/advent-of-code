using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 5: If You Give A Seed A Fertilizer ---
    public class Day05
    {
        private readonly ITestOutputHelper output;

        public Day05(ITestOutputHelper output)
        {
            this.output = output;
        }

        private record Map(string Destination, string Source, List<RangeMapping> Ranges);

        private record Almanac(List<long> Seeds, Dictionary<string, Map> MapsBySource, Dictionary<string, Map> MapsByDestination);

        private Almanac Parse(IList<string> input)
        {
            string seedsLine = input.First()["seeds: ".Length..];

            var seeds = seedsLine.Split(" ").Select(long.Parse).ToList();

            List<Map> maps = new List<Map>();

            foreach (var line in input.Skip(1).Where(line => !string.IsNullOrEmpty(line)))
            {
                if (char.IsLetter(line[0]))
                {
                    var sourceDestination = line.Split(" ")[0].Split("-");
                    var destination = sourceDestination[2];
                    var source = sourceDestination[0];
                    maps.Add(new Map(destination, source, new List<RangeMapping>()));
                }
                else
                {
                    var rangeParts = line.Split(" ").Select(long.Parse).ToList();
                    maps.Last().Ranges.Add(new RangeMapping(rangeParts[0], rangeParts[1], rangeParts[2]));

                }
            }

            var mapLookupBySource = maps.ToDictionary(map => map.Source, map => map);
            var mapLookupByDestination = maps.ToDictionary(map => map.Destination, map => map);

            return new Almanac(seeds, mapLookupBySource, mapLookupByDestination);
        }

        private record Range(long Start, long End);

        private record RangeMapping(long Destination, long Source, long Length)
        {
            public Range DestinationRange => new Range(Destination, Destination + Length - 1);
         
            public long SourceTo => Source + Length - 1;

            public bool SourceWithin(long source)
            {
                return source >= Source && source <= SourceTo;
            }

            public long DestinationFor(long source)
            {
                return Destination + source - Source;
            }

            public Range DestinationFor(Range range)
            {
                return new Range(DestinationFor(range.Start), DestinationFor(range.End));
            }
        }

        private record RangeMappingState(List<Range> Input, List<Range> Output)
        {
            public RangeMappingState Next(RangeMapping rangeMapping)
            {
                var nextState = new RangeMappingState(new List<Range>(), new List<Range>(Output));

                foreach (Range range in Input)
                {
                    // range covers the range map's source range
                    if (range.Start <= rangeMapping.Source && range.End >= rangeMapping.SourceTo)
                    {
                        if (range.Start < rangeMapping.Source)
                        {
                            nextState.Input.Add(new Range(range.Start, rangeMapping.Source - 1));
                        }

                        nextState.Output.Add(rangeMapping.DestinationRange);

                        if (range.End > rangeMapping.SourceTo)
                        {
                            nextState.Input.Add(new Range(rangeMapping.SourceTo + 1, range.End));
                        }
                    }
                    else
                    // range outside of range map's source range
                    if (range.End < rangeMapping.Source || range.Start > rangeMapping.SourceTo)
                    {
                        nextState.Input.Add(range);
                    }
                    else
                    // range within range map's source range
                    if (range.Start >= rangeMapping.Source && range.End <= rangeMapping.SourceTo)
                    {
                        nextState.Output.Add(rangeMapping.DestinationFor(range));
                    }
                    else
                    // range start within range map's source range
                    if (range.Start >= rangeMapping.Source && range.Start <= rangeMapping.SourceTo)
                    {
                        nextState.Output.Add(rangeMapping.DestinationFor(new Range(range.Start, rangeMapping.SourceTo)));
                        nextState.Input.Add(new Range(rangeMapping.SourceTo + 1, range.End));
                    }
                    else
                    // range end within range map's source range
                    if (range.End >= rangeMapping.Source && range.End <= rangeMapping.SourceTo)
                    {
                        nextState.Input.Add(new Range(range.Start, rangeMapping.Source - 1));
                        nextState.Output.Add(rangeMapping.DestinationFor(new Range(rangeMapping.Source, range.End)));

                    }
                }
                return nextState;
            }
        }

        private long Location(long seed, Almanac almanac)
        {
            string source = "seed";
            long output = seed;
            string destination;

            do
            {
                var map = almanac.MapsBySource[source];
                destination = map.Destination;
                source = destination;
                var range = map.Ranges.FirstOrDefault(range => range.SourceWithin(output));
                output = range != null ? range.DestinationFor(output) : output;
                
            } while (destination != "location");

            return output;
        }

        private List<Range> LocationRange(Almanac almanac)
        {
            string source = "seed";

            List<Range> seedRanges = new List<Range>();

            for (int i = 0; i < almanac.Seeds.Count - 1; i = i + 2)
            {
                var seedRangeStart = almanac.Seeds[i];
                var seedRangeLength = almanac.Seeds[i + 1];

                seedRanges.Add(new Range(seedRangeStart, seedRangeStart + seedRangeLength - 1));
            }

            List<Range> output = seedRanges;

            string destination;

            var state = new RangeMappingState(new List<Range>(), output);

            do
            {
                state = new RangeMappingState(state.Output.Concat(state.Input).ToList(), new List<Range>());

                Map map = almanac.MapsBySource[source];
                destination = map.Destination;
                source = destination;

                state = map.Ranges.Aggregate(state, (state, range) => state.Next(range));
                
            } while (destination != "location");

            return state.Output.Concat(state.Input).ToList();
        }

        private long LowestLocation(IList<string> input)
        {
            var almanac = Parse(input);

            var locations = almanac.Seeds.Select(seed => Location(seed, almanac));

            return locations.Min();
        }

        private long LowestRangeLocation(IList<string> input)
        {
            var almanac = Parse(input);

            var locations = LocationRange(almanac);

            return locations.Select(range => range.Start).Min();
        }

        public long FirstStar()
        {
            var input = ReadLineInput();
            return LowestLocation(input);
        }

        public long SecondStar()
        {
            var input = ReadLineInput();
            return LowestRangeLocation(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(31599214, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(20358599, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "seeds: 79 14 55 13",
                "",
                "seed-to-soil map:",
                "50 98 2",
                "52 50 48",
                "",
                "soil-to-fertilizer map:",
                "0 15 37",
                "37 52 2",
                "39 0 15",
                "",
                "fertilizer-to-water map:",
                "49 53 8",
                "0 11 42",
                "42 0 7",
                "57 7 4",
                "",
                "water-to-light map:",
                "88 18 7",
                "18 25 70",
                "",
                "light-to-temperature map:",
                "45 77 23",
                "81 45 19",
                "68 64 13",
                "",
                "temperature-to-humidity map:",
                "0 69 1",
                "1 0 69",
                "",
                "humidity-to-location map:",
                "60 56 37",
                "56 93 4",
            };

            Assert.Equal(35, LowestLocation(example));
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "seeds: 79 14 55 13",
                "",
                "seed-to-soil map:",
                "50 98 2",
                "52 50 48",
                "",
                "soil-to-fertilizer map:",
                "0 15 37",
                "37 52 2",
                "39 0 15",
                "",
                "fertilizer-to-water map:",
                "49 53 8",
                "0 11 42",
                "42 0 7",
                "57 7 4",
                "",
                "water-to-light map:",
                "88 18 7",
                "18 25 70",
                "",
                "light-to-temperature map:",
                "45 77 23",
                "81 45 19",
                "68 64 13",
                "",
                "temperature-to-humidity map:",
                "0 69 1",
                "1 0 69",
                "",
                "humidity-to-location map:",
                "60 56 37",
                "56 93 4",
            };
            
            Assert.Equal(46, LowestRangeLocation(example));
        }
    }
}
