using System;
using System.Linq;
using Utilities;

namespace Y2017.Day10 {
    public class StringHash {
        public static int Hash(int inputLength, string inputLenghts) {
            int[] list = Enumerable.Range(0, inputLength).ToArray();
            int[] lenghts = inputLenghts.SplitOnCommaSpaceSeparated().Select(int.Parse).ToArray();

            list = HashedList(lenghts, list, 1);

            return list[0]*list[1];
        }

        private static int[] HashedList(int[] lenghts, int[] list, int times) {
            int listIndex = 0;
            int skip = 0;
            for (int t = 0; t < times; t++) {
                foreach (int length in lenghts) {
                    list = ArrayReverseReplace(list, listIndex, length);
                    listIndex = listIndex + length + skip;
                    if (listIndex >= list.Length) {
                        listIndex = listIndex % list.Length;
                    }
                    skip++;
                }
            }
            
            return list;
        }

        public static T[] ArrayReverseReplace<T>(T[] array, int index, int length) {
            
            if (index + length <= array.Length) {
                T[] subArray = array.SubArray(index, length).Reverse().ToArray();
                Array.Copy(subArray, 0, array, index, length);
            }
            else {
                T[] wrappedArray = new T[length];
                Array.Copy(array, index, wrappedArray, 0, array.Length - index);
                Array.Copy(array, 0, wrappedArray, array.Length - index, length - (array.Length - index));
                wrappedArray = wrappedArray.Reverse().ToArray();
                Array.Copy(wrappedArray, 0, array, index, array.Length - index);
                Array.Copy(wrappedArray, array.Length - index, array, 0, length - (array.Length - index));
            }

            return array;
        }

        public static string HashAscii(int inputLength, string input) {
            int[] list = Enumerable.Range(0, inputLength).ToArray();
            int[] bytes = input.Select(c => (int)c).ToArray();
            int[] standarLengthSuffix = {17, 31, 73, 47, 23};
            int[] lengths = bytes.Concat(standarLengthSuffix).ToArray();

            int[] sparseHash = HashedList(lengths, list, 64);

            byte[] condencedHash = new byte[16];
            for (int skip = 0; skip < 16; skip ++) {
                condencedHash[skip] = (byte) sparseHash.Skip(skip*16).Take(16).Aggregate((a, b) => a ^ b);
            }

            return string.Join("", condencedHash.Select(i => i.ToString("x2")));
        }
    }
}