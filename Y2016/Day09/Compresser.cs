using System.Text;
using System.Text.RegularExpressions;

namespace Y2016.Day09 {
    public class Compresser {
        public static string Decompress(string input) {

            StringBuilder compressed = new StringBuilder();
            
            var instructionExpression = new Regex(@"\((\d+)x(\d+)\)");
            while (instructionExpression.IsMatch(input)) {
                var match = instructionExpression.Match(input);
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);

                var skip = input.Substring(0, match.Index);
                input = input.Substring(skip.Length);
                compressed.Append(skip);

                input = input.Substring(match.Length);
                var toBeCompressed = input.Substring(0, x);
                input = input.Substring(x);
                for (int i = 0; i < y; i++) {
                    compressed.Append(toBeCompressed);
                }
            }
            compressed.Append(input);

            return compressed.ToString();
        }

        public static long DecompressV2(string input) {

            long compressedLength = 0;

            var instructionExpression = new Regex(@"\((\d+)x(\d+)\)");

            while (instructionExpression.IsMatch(input)) {
                var match = instructionExpression.Match(input);
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);

                var skip = input.Substring(0, match.Index);
                input = input.Substring(skip.Length);
                compressedLength += skip.Length;

                input = input.Substring(match.Length);
                var decompress = input.Substring(0, x);
                if (decompress.Contains("(")) {
                    compressedLength += y * DecompressV2(decompress);
                }
                else {
                    compressedLength += y * decompress.Length;
                }
                input = input.Substring(x);
            }

            compressedLength += input.Length;

            return compressedLength;
        }
    }
}