using System.Collections.Generic;

namespace Utilities {
    public static class LinqExtensions {
        public static IEnumerable<T> Yield<T>(this T item) {
            yield return item;
        }
    }
}
