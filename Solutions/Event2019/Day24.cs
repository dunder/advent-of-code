using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Solutions.InputReader;



namespace Solutions.Event2019
{
    // --- Day 24: Planet of Discord ---
    public class Day24
    {
        private const int Size = 5;

        public int Parse(List<string> input)
        {
            var binaryParts = input.Select(x => string.Join("", x.Select(c => c == '#' ? "1" : "0")));
            var binaryString = string.Join("", binaryParts);

            return Convert.ToInt32(binaryString, 2);
        }

        private int Generate(int current)
        {
            var newGeneration = 0;
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    var bugsAround = BugsAround(current, x, y).Sum();

                    var isBug = IsBug(current, x, y);
                    var bugLives = isBug && bugsAround == 1;
                    var infected = !isBug && bugsAround > 0 && bugsAround < 3;
                    var nextGenerationIsBug = bugLives || infected;

                    if (nextGenerationIsBug)
                    {
                        newGeneration |= PositionToInt(x, y);
                    }
                }
            }

            return newGeneration;
        }

        private TileLevels GenerateRecursive(TileLevels levels)
        {
            var level = 0;

            int newGeneration = GenerateLevel(level, levels);
            var newTileLevels = new TileLevels(newGeneration);
            int max = levels.Levels.Keys.Max();
            int min = levels.Levels.Keys.Min();

            level = 1;
            do
            {
                newGeneration = GenerateLevel(level, levels);
                newTileLevels.AddTile(level, newGeneration);
                level++;
            } while (newGeneration != 0 || level < max);

            level = -1;
            do
            {
                newGeneration = GenerateLevel(level, levels);
                newTileLevels.AddTile(level, newGeneration);
                level--;
            } while (newGeneration != 0 || level > min);

            return newTileLevels;
        }

        private int GenerateLevel(int level, TileLevels levels)
        {
            var current = levels.TileAt(level);
            int newGeneration = 0;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x == 2 && y == 2)
                    {
                        continue;
                    }

                    var bugsAround = BugsAroundRecursive(level, x, y, levels).Sum();

                    var isBug = IsBug(current, x, y);
                    var bugLives = isBug && bugsAround == 1;
                    var infected = !isBug && bugsAround > 0 && bugsAround < 3;
                    var nextGenerationIsBug = bugLives || infected;

                    if (nextGenerationIsBug)
                    {
                        newGeneration |= PositionToInt(x, y);
                    }
                }
            }

            return newGeneration;
        }

        private string Decode(int encodedTiles, char bug, char space)
        {
            var decoded = new StringBuilder(Size*Size);

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    decoded.Append(IsBug(encodedTiles, x, y) ? bug : space);
                }
            }

            return decoded.ToString();
        }

        private bool Inside(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }

        private int PositionToInt(int x, int y)
        {
            int shift = Size * (Size - 1 - y) + (Size - 1 - x);
            int bit = 1 << shift;
            return bit;
        }

        private bool IsBug(int tiles, int x, int y)
        {
            return Inside(x,y) && (PositionToInt(x,y) & tiles) > 0;
        }

        private int[] BugsAround(int tiles, int x, int y)
        {
            var neighbors = new int[4];

            neighbors[0] = IsBug(tiles, x, y-1) ? 1 : 0;
            neighbors[1] = IsBug(tiles, x+1, y) ? 1 : 0;
            neighbors[2] = IsBug(tiles, x, y+1) ? 1 : 0;
            neighbors[3] = IsBug(tiles, x-1, y) ? 1 : 0;

            return neighbors;
        }

        private List<int> BugsAroundRecursive(int level, int x, int y, TileLevels tiles)
        {
            var encodedTilesAtLevel = tiles.TileAt(level);
            var encodedTilesAtNextLevel = tiles.TileAt(level + 1);
            var encodedTilesAtPreviousLevel = tiles.TileAt(level - 1);

            var neighbors = new List<int>();
            var topAtLevel = IsBug(encodedTilesAtLevel, x, y - 1) ? 1 : 0; 
            var rightAtLevel = IsBug(encodedTilesAtLevel, x + 1, y) ? 1 : 0;  
            var belowAtLevel = IsBug(encodedTilesAtLevel, x, y + 1) ? 1 : 0;
            var leftAtLevel = IsBug(encodedTilesAtLevel, x - 1, y) ? 1 : 0;
            if (x == 2 & y == 1)
            {
                neighbors.AddRange(new[] { topAtLevel, rightAtLevel, leftAtLevel });

                // next level top side
                for (int xi = 0; xi < Size; xi++)
                {
                    neighbors.Add(IsBug(encodedTilesAtNextLevel, xi, 0) ? 1 : 0);
                }
            }
            else if (x == 3 && y == 2)
            {
                neighbors.AddRange(new[] { topAtLevel, rightAtLevel, belowAtLevel });

                // next level right side
                for (int yi = 0; yi < Size; yi++)
                {
                    neighbors.Add(IsBug(encodedTilesAtNextLevel, Size - 1, yi) ? 1 : 0);
                }
            }
            else if (x == 2 && y == 3)
            {
                neighbors.AddRange(new[] { rightAtLevel, belowAtLevel, leftAtLevel });

                // next level bottom side
                for (int xi = 0; xi < Size; xi++)
                {
                    neighbors.Add(IsBug(encodedTilesAtNextLevel, xi, Size - 1) ? 1 : 0);
                }
            }
            else if (x == 1 && y == 2)
            {
                neighbors.AddRange(new[] { topAtLevel, belowAtLevel, leftAtLevel });
                // next level left side
                for (int yi = 0; yi < Size; yi++)
                {
                    neighbors.Add(IsBug(encodedTilesAtNextLevel, 0, yi) ? 1 : 0);
                }
            }
            else if (x == 0 || x == Size - 1 || y == 0 || y == Size - 1)
            {
                if (x == 0 && y == 0)
                {
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 2, 1) ? 1 : 0);
                    neighbors.Add(rightAtLevel);
                    neighbors.Add(belowAtLevel);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 1, 2) ? 1 : 0);
                } 
                else if (x > 0 && x < Size - 1 && y == 0)
                {
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 2, 1) ? 1 : 0);
                    neighbors.Add(rightAtLevel);
                    neighbors.Add(belowAtLevel);
                    neighbors.Add(leftAtLevel);
                }
                else if (x == Size - 1 && y == 0)
                {

                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 2, 1) ? 1 : 0);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 3, 2) ? 1 : 0);
                    neighbors.Add(belowAtLevel);
                    neighbors.Add(leftAtLevel);
                }
                else if (x == Size - 1 && y > 0 && y < Size - 1)
                {
                    neighbors.Add(topAtLevel);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 3, 2) ? 1 : 0);
                    neighbors.Add(belowAtLevel);
                    neighbors.Add(leftAtLevel);

                }
                else if (x == Size - 1 && y == Size - 1)
                {
                    neighbors.Add(topAtLevel);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 3, 2) ? 1 : 0);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 2, 3) ? 1 : 0);
                    neighbors.Add(leftAtLevel);
                }
                else if (x > 0 && x < Size - 1 && y == Size - 1)
                {
                    neighbors.Add(topAtLevel);
                    neighbors.Add(rightAtLevel);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 2,3) ? 1 : 0);
                    neighbors.Add(leftAtLevel);
                }
                else if (x == 0 && y == Size - 1)
                {
                    neighbors.Add(topAtLevel);
                    neighbors.Add(rightAtLevel);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 2, 3) ? 1 : 0);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 1, 2) ? 1 : 0);
                }
                else if (x == 0 && y > 0 && y < Size - 1)
                {
                    neighbors.Add(topAtLevel);
                    neighbors.Add(rightAtLevel);
                    neighbors.Add(belowAtLevel);
                    neighbors.Add(IsBug(encodedTilesAtPreviousLevel, 1, 2) ? 1 : 0);
                }
            }
            else
            {
                neighbors.AddRange(new [] {topAtLevel, rightAtLevel, belowAtLevel, leftAtLevel});
            }

            return neighbors;
        }

        private int UntilTwice(int initialEncodedTiles)
        {
            var generations = new HashSet<int> {initialEncodedTiles};

            var generation = Generate(initialEncodedTiles);
            while (!generations.Contains(generation))
            {
                generations.Add(generation);
                generation = Generate(generation);
            }

            return generation;
        }

        private int BiodiversityRating(int encodedTiles)
        {
            var decoded = Decode(encodedTiles, '1', '0');
            return Convert.ToInt32(new string(decoded.Reverse().ToArray()), 2);
        }

        private class TileLevels
        {
            public Dictionary<int, int> Levels { get; } = new Dictionary<int, int>();

            public TileLevels(int level0)
            {
                Levels.Add(0, level0);
            }

            public int TileAt(int level)
            {
                if (!Levels.ContainsKey(level))
                {
                    Levels.Add(level, 0);
                }

                return Levels[level];
            }

            public void AddTile(int level, int encodedTiles)
            {
                if (!Levels.ContainsKey(level))
                {
                    Levels.Add(level, encodedTiles);

                }
                else
                {
                    Levels[level] = encodedTiles;
                }
            }
        }

        public int BugsAfterMinutes(int encodedTilesAtBaseLevel, int minutes)
        {
            var tileLevels = new TileLevels(encodedTilesAtBaseLevel);
            foreach (var _ in Enumerable.Range(1, minutes))
            {
                tileLevels = GenerateRecursive(tileLevels);
            }

            return tileLevels.Levels.Values.Sum(tile => Decode(tile, '1', '0').Count(c => c == '1'));
        }

        public int FirstStar()
        {
            var input = ReadLineInput();
            var encodedStartLayout = Parse(input.ToList());
            var firstTwice = UntilTwice(encodedStartLayout);
            return BiodiversityRating(firstTwice);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            var encodedStartLayout = Parse(input.ToList());
            return BugsAfterMinutes(encodedStartLayout, 200);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(27562081, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(1893, SecondStar());
        }

        [Fact]
        public void FirstStarExample1()
        {
            var startState = new List<string>
            {
                "....#",
                "#..#.",
                "#..##",
                "..#..",
                "#...."
            };

            var startGeneration = Parse(startState);

            var nextState = new List<string>
            {
                "#..#.",
                "####.",
                "###.#",
                "##.##",
                ".##.."
            };

            var expected = Parse(nextState);

            var nextGeneration = Generate(startGeneration);

            Assert.Equal(expected, nextGeneration);
        }

        [Fact]
        public void FirstStarExample2()
        {
            var startState = new List<string>
            {
                "#..#.",
                "####.",
                "###.#",
                "##.##",
                ".##.." 
            };

            var startGeneration = Parse(startState);

            var nextState = new List<string>
            {
                "#####",
                "....#",
                "....#",
                "...#.",
                "#.###"
            };

            var expected = Parse(nextState);    

            var nextGeneration = Generate(startGeneration);

            Assert.Equal(expected, nextGeneration);
        }

        [Fact]
        public void FirstStarExample3()
        {
            var startState = new List<string>
            {
                "....#",
                "#..#.",
                "#..##",
                "..#..",
                "#...."
            };

            var encodedStartTiles = Parse(startState);

            var expectedFirstTwiceState = new List<string>
            {
                ".....",
                ".....",
                ".....",
                "#....",
                ".#..."
            };

            var encodedExpected = Parse(expectedFirstTwiceState);

            var firstTwice = UntilTwice(encodedStartTiles);

            Assert.Equal(encodedExpected, firstTwice);
        }

        [Fact]
        public void FirstStarBiodiversityRatingExample()
        {
            var tiles = new List<string>
            {
                ".....",
                ".....",
                ".....",
                "#....",
                ".#..."
            };

            var encodedTiles = Parse(tiles);

            var biodiversityPoints = BiodiversityRating(encodedTiles);

            Assert.Equal(2129920, biodiversityPoints);
        }

        [Theory]
        [InlineData(4, 4, 1)]
        [InlineData(3, 4, 2)]
        [InlineData(2, 4, 4)]
        [InlineData(1, 4, 8)]
        [InlineData(0, 4, 16)]
        [InlineData(4, 3, 32)]
        [InlineData(3, 3, 64)]
        [InlineData(2, 3, 128)]
        [InlineData(1, 3, 256)]
        [InlineData(0, 3, 512)]
        [InlineData(4, 2, 1024)]
        [InlineData(3, 2, 2048)]
        [InlineData(2, 2, 4096)]
        public void ToPositionTests(int x, int y, int expected)
        {
            var actual = PositionToInt(x, y);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0,0, false)]
        [InlineData(1,0, true)]
        [InlineData(2,0, true)]
        [InlineData(3,0, true)]
        [InlineData(4,0, false)]
        [InlineData(0,1, false)]
        [InlineData(1,1, false)]
        [InlineData(2,1, true)]
        [InlineData(3,1, false)]
        [InlineData(4,1, true)]
        [InlineData(0,2, false)]
        [InlineData(1,2, false)]
        [InlineData(2,2, false)]
        [InlineData(3,2, true)]
        [InlineData(4,2, true)]
        [InlineData(0,3, true)]
        [InlineData(1,3, false)]
        [InlineData(2,3, true)]
        [InlineData(3,3, true)]
        [InlineData(4,3, true)]
        [InlineData(0,4, false)]
        [InlineData(1,4, false)]
        [InlineData(2,4, true)]
        [InlineData(3,4, false)]
        [InlineData(4,4, false)]
        [InlineData(-1,-1, false)]
        [InlineData(5,5, false)]
        public void IsBugTests(int x, int y, bool expected)
        {
            var input = new List<string>
            {
                ".###.",
                "..#.#",
                "...##",
                "#.###",
                "..#..",
            };

            var tiles = Parse(input);
            var actual = IsBug(tiles, x, y);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SecondStarExample1()
        {
            var startState = new List<string>
            {
                "....#",
                "#..#.",
                "#..##",
                "..#..",
                "#...."
            };

            var encodedStartTiles = Parse(startState);

            var bugs = BugsAfterMinutes(encodedStartTiles, 10);

            Assert.Equal(99, bugs);
        }
    }
}
