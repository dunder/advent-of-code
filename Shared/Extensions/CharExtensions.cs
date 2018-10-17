namespace Shared.Extensions {
    public static class CharExtensions {
        /// <summary>
        /// Right shifts letters <paramref name="shift"/> steps and wraps around  
        /// </summary>
        /// <param name="c">The char to shift</param>
        /// <param name="shift">The number of steps to shift</param>
        /// <returns>The character <paramref name="shift"/> steps to the right of <paramref name="c"/> with wrap around</returns>
        public static char Shift(this char c, int shift) {
            int weightedIndex = shift % 26;
            int shifted = c + weightedIndex > 'z' ? 'a' + weightedIndex - ('z' - c) - 1 : c + weightedIndex;
            return (char)shifted;
        }
    }
}
