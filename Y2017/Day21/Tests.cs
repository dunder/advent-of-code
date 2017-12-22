using System.Collections.Generic;
using Xunit;

namespace Y2017.Day21 {
    public class Tests {
        [Fact]
        public void Problem1_Example() {
            string[] input = {
                "../.# => ##./#../...",
                ".#./..#/### => #..#/..../..../#..#"
            };

            var onCountAfterExpansion = PixelArt.CountPixelsOnAfterExpansion(input, 2);

            Assert.Equal(12, onCountAfterExpansion);
        }

        [Fact]
        public void TestEquals() {

            var pixelSet = new PixelSet(".#./..#/###");
            var duplicate = new PixelSet(".#./..#/###");

            Assert.True(pixelSet.Equals(duplicate));
        }

        [Fact]
        public void Flip() {

            var pixelSet = new PixelSet(".#./..#/###");

            var flipped = pixelSet.Flip();

            var expected = new PixelSet(".#./#../###");

            Assert.Equal(expected, flipped);
        }

        [Fact]
        public void Rotate() {

            var pixelSet = new PixelSet(".#./..#/###");
            
            var rotated = pixelSet.Rotate();

            var expected = new PixelSet("#../#.#/##.");

            Assert.Equal(expected, rotated);
        }

        [Fact]
        public void ConstructorList() {
            
            var pixelSet1 = new PixelSet("##./#../...");
            var pixelSet2 = new PixelSet("##./#../...");
            var pixelSet3 = new PixelSet("##./#../...");
            var pixelSet4 = new PixelSet("##./#../...");

            var pixelSet = new PixelSet(new List<PixelSet> {
                pixelSet1,
                pixelSet2,
                pixelSet3,
                pixelSet4,
            });

            var expectedPixelSet = new PixelSet("##.##./#..#../....../##.##./#..#../......");

            Assert.Equal(expectedPixelSet, pixelSet);
        }

        [Fact]
        public void Expand() {

            var pixelSet = new PixelSet(".#./..#/###");

            string[] input = {
                "../.# => ##./#../...",
                ".#./..#/### => #..#/..../..../#..#",
            };
            var rules = PixelArt.ReadRules(input);
            var rotated = pixelSet.Expand(rules);

            var expected = new PixelSet("#..#/..../..../#..#");

            Assert.Equal(expected, rotated);

        }
    }
}
