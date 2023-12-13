
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2023
{
    // --- Day 13: Point of Incidence ---
    public class Day13
    {
        private readonly ITestOutputHelper output;

        public Day13(ITestOutputHelper output)
        {
            this.output = output;
        }

        private List<List<string>> Parse(IList<string> input)
        {
            List<List<string>> maps = new();
            List<string> map = new();

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    maps.Add(map);
                    map = new();
                }
                else
                {
                    map.Add(line);
                }
            }

            maps.Add(map);

            return maps;
        }

        private int Calculate(IList<string> input)
        {
            List<List<string>> maps = Parse(input);

            int count = 0;
            int mapNumber = 0;

            foreach (List<string> map in maps)
            {
                count += FindMirror(map, mapNumber++, new());
            }

            return count;
        }

        private int PairwiseCompare(List<string> rows, int mapNumber, Dictionary<int, (int, bool)> skipReflections, bool isColumn)
        {
            if (isColumn)
            {
                rows = ColumnsToRows(rows);
            }

            for (int row = 1; row < rows.Count; row++)
            {
                var left = rows.Take(row).Reverse();
                var right = rows.Skip(row);

                if (left.Zip(right).All(x => x.First == x.Second))
                {
                    if (skipReflections.TryGetValue(mapNumber, out (int column, bool isRow) value))
                    {
                        bool skip = isColumn ? !value.isRow : value.isRow;
                        if (skip && value.column == row)
                        {
                            continue;
                        }
                    }
                    skipReflections[mapNumber] = (row, !isColumn);
                    return isColumn ? row : row * 100;
                }
            }

            return 0;
        }

        private int FindMirror(List<string> map, int mapNumber, Dictionary<int, (int, bool)> skipReflections)
        {
            int result = PairwiseCompare(map, mapNumber, skipReflections, isColumn: false);

            if (result > 0)
            {
                return result;
            }

            return PairwiseCompare(map, mapNumber, skipReflections, isColumn: true);
        }

        private int Calculate2(IList<string> input)
        {
            List<List<string>> maps = Parse(input);
            Dictionary<int, (int, bool)> smudgedReflections = new();

            int mapNumber = 0;

            foreach (List<string> map in maps)
            {
                FindMirror(map, mapNumber++, smudgedReflections);
            }

            mapNumber = 0;
            int count = 0;

            foreach (List<string> smudgedMap in maps)
            {
                var unsmudgedMaps = Unsmudged(smudgedMap);

                foreach (List<string> map in unsmudgedMaps)
                {
                    var result = FindMirror(map, mapNumber, smudgedReflections);

                    if (result > 0)
                    {
                        count += result;
                        break;
                    }
                }

                mapNumber++;
            }

            return count;
        }

        private List<List<string>> Unsmudged(List<string> map)
        {
            List<List<string>> maps = new();

            for (int row = 0; row < map.Count; row++)
            {
                for (int column = 0; column < map[0].Length; column++)
                {
                    maps.Add(Unsmudge(map, row, column));
                }
            }

            return maps;
        }

        private string Swap(string source, int column)
        {
            var newValue = source[column] == '#' ? '.' : '#';

            return source.Remove(column, 1).Insert(column, newValue.ToString());
        }

        private List<string> Unsmudge(List<string> map, int row, int column)
        {
            var unsmudgedMap = new List<string>(map);
            unsmudgedMap[row] = Swap(map[row], column);
            return unsmudgedMap;
        }

        private List<string> ColumnsToRows(List<string> map)
        {
            List<char[]> columns = new();

            for (int column = 0; column < map[0].Length; column++)
            {
                var c = new char[map.Count];

                for (int row = 0; row < map.Count; row++)
                {
                    c[row] = map[row][column];
                }

                columns.Add(c);
            }

            return columns.Select(column => new string(column)).ToList();
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            return Calculate(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return Calculate2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(33728, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(28235, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "#.##..##.",
                "..#.##.#.",
                "##......#",
                "##......#",
                "..#.##.#.",
                "..##..##.",
                "#.#.##.#.",
                "",
                "#...##..#",
                "#....#..#",
                "..##..###",
                "#####.##.",
                "#####.##.",
                "..##..###",
                "#....#..#",
            };

            Assert.Equal(405, Calculate(example));
        }
        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "#.##..##.",
                "..#.##.#.",
                "##......#",
                "##......#",
                "..#.##.#.",
                "..##..##.",
                "#.#.##.#.",
                "",
                "#...##..#",
                "#....#..#",
                "..##..###",
                "#####.##.",
                "#####.##.",
                "..##..###",
                "#....#..#",
            };

            Assert.Equal(400, Calculate2(example));
        }
    }
}
