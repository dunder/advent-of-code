using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // --- Day 8: Space Image Format ---
    public class Day08
    {
        private readonly ITestOutputHelper output;
        
        public Day08(ITestOutputHelper output)
        {
            this.output = output;
        }

        private const int Width = 25;
        private const int Height = 6;

        public enum Color { Black = 0, White, Transparent }

        public static int CountZeroes(string input)
        {
            var fewestZeroes = input.Batch(Width * Height).OrderBy(layer => layer.Count(c => c == '0')).First().ToList();
            return fewestZeroes.Count(c => c == '1') * fewestZeroes.Count(c => c == '2');
        }

        public List<List<Color>> ReadLayers(string input, int width, int height)
        {
            var layers = input.Batch(width * height)
                .Select(layer => layer.Select(x => (Color)int.Parse(x.ToString())).ToList())
                .ToList();

            return layers;
        }

        public Color[,] VisiblePixels(List<List<Color>> layers, int width, int height)
        {
            var display = new Color[width,height];
            var layerCount = layers.Count;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = 0;
                    for (int layer = 0; layer < layerCount; layer++)
                    {
                        pixel = layers[layer][x + y * width];
                        if (pixel != Color.Transparent)
                        {
                            break;
                        }
                    }
                    display[x, y] = pixel;
                }
            }

            return display;
        }

        private void Print(Color[,] visiblePixels)
        {
            int width = visiblePixels.GetLength(0);
            int height = visiblePixels.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                var line = new StringBuilder();

                for (int x = 0; x < width; x++)
                {
                    line.Append(visiblePixels[x,y] == Color.White ? '0' : ' ');
                }

                output.WriteLine(line.ToString());
            }
        }

        public int FirstStar()  
        {
            var input = ReadInput();
            return CountZeroes(input);
        }

        public int SecondStar()
        {
            var input = ReadInput();
            int width = 25;
            int height = 6;
            Print(VisiblePixels(ReadLayers(input, width, height), 25, 6));
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(1485, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            SecondStar(); // RLAKF
        }

        [Fact]
        public void SecondStarExample()
        {
            string input = "0222112222120000";
            int width = 2;
            int height = 2;
            Print(VisiblePixels(ReadLayers(input, width, height), width, height));
        }
    }
}
