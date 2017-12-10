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

        public static T[] SubArrayWithWrap<T>(this T[] data, int index, int length) {
            if (index + length <= data.Length) {
                return data.SubArray(index, length);
            }
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, data.Length - index);
            Array.Copy(data, 0, result, data.Length - index, length - (data.Length - index));
            return result;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        
    }
}
