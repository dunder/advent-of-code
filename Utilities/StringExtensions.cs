using System;

namespace Utilities {
    public static class StringExtensions {
        public static string[] SplitOnCommaSpaceSeparated(this string input) {
            return input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
