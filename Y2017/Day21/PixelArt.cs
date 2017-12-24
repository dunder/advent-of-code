using System.Collections.Generic;
using System.Text.RegularExpressions;
using Utilities.Grid;

namespace Y2017.Day21 {
    public class PixelArt {
        public static int CountPixelsOnAfterExpansion(string[] input, int iterations) {

            var initialPixelSet = new PixelSet(new Grid(".#./..#/###"));

            var rules = ReadRules(input);

            var expanded = initialPixelSet;
            for (int i = 0; i < iterations; i++) {
                expanded = expanded.Expand(rules);
            }

            return expanded.OnCount;
        }

        public static Dictionary<PixelSet, PixelSet> ReadRules(string[] input) {

            var rules = new Dictionary<PixelSet, PixelSet>();

            foreach (var rule in input) {
                var ruleSplit = Regex.Split(rule, " => ");
                var pixelInputDescription = ruleSplit[0];
                var pixelOutputDescription = ruleSplit[1];
                var pixelInput = new PixelSet(new Grid(pixelInputDescription));
                var pixelOutput = new PixelSet(new Grid(pixelOutputDescription));

                rules.Add(pixelInput, pixelOutput);
            }
            return rules;
        }
    }
}