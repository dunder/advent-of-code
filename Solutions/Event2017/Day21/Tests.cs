using System.Collections.Generic;
using Shared.Grid;
using Xunit;

namespace Solutions.Event2017.Day21 {
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

            var pixelSet = new Grid(".#./..#/###");
            var duplicate = new Grid(".#./..#/###");

            Assert.True(pixelSet.Equals(duplicate));
        }

        [Fact]
        public void Flip() {

            var pixelSet = new Grid(".#./..#/###");

            var flipped = pixelSet.Flip();

            var expected = new Grid(".#./#../###");

            Assert.Equal(expected, flipped);
        }

        [Fact]
        public void Rotate() {

            var pixelSet = new Grid(".#./..#/###");
            
            var rotated = pixelSet.Rotate();

            var expected = new Grid("#../#.#/##.");

            Assert.Equal(expected, rotated);
        }

        [Fact]
        public void ConstructorList() {
            
            var pixelSet1 = new PixelSet(new Grid("##./#../..."));
            var pixelSet2 = new PixelSet(new Grid("##./#../..."));
            var pixelSet3 = new PixelSet(new Grid("##./#../..."));
            var pixelSet4 = new PixelSet(new Grid("##./#../..."));

            var pixelSet = new PixelSet(new List<PixelSet> {
                pixelSet1,
                pixelSet2,
                pixelSet3,
                pixelSet4,
            });

            var expectedPixelSet = new PixelSet(new Grid("##.##./#..#../....../##.##./#..#../......"));

            Assert.Equal(expectedPixelSet, pixelSet);
        }

        [Fact]
        public void Expand() {

            var pixelSet = new PixelSet(new Grid(".#./..#/###"));

            string[] input = {
                "../.# => ##./#../...",
                ".#./..#/### => #..#/..../..../#..#",
            };
            var rules = PixelArt.ReadRules(input);
            var rotated = pixelSet.Expand(rules);

            var expected = new PixelSet(new Grid("#..#/..../..../#..#"));

            Assert.Equal(expected, rotated);

        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("152", actual);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("1956174", actual);
        }
    }
}
