using System;

namespace Y2016 {
    public static class StringExtensions {
        public static string[] SplitOnCommaSpaceSeparated(this string input) {
            return input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
