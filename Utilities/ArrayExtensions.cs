using System;

namespace Utilities {
    public static class ArrayExtensions {
        public static  T GetWithWrappedIndex<T>(this T[] array, int index) {
            return array[array.WrappedIndex(index)];
        }

        public static int WrappedIndex<T>(this T[] array, int index) {
            int wrappedIndex = index;
            if (wrappedIndex < 0) {
                wrappedIndex = array.Length - Math.Abs(wrappedIndex);
            }
            return wrappedIndex > array.Length - 1 ? wrappedIndex % array.Length : wrappedIndex;
        }
    }
}
