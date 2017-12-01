using System;

namespace Utilities {
    public static class ArrayExtensions {
        public static  T GetWithWrappedIndex<T>(this T[] array, int index) {
            int wrappedIndex = index;
            if (wrappedIndex < 0) { 
                wrappedIndex = array.Length - Math.Abs(wrappedIndex);
            }
            return array[wrappedIndex > array.Length - 1 ? wrappedIndex % array.Length : wrappedIndex];
        }
    }
}
