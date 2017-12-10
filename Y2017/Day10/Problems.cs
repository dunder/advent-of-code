﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Y2017.Day10 {
    public class Problems {

        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {
            const string input = "225,171,131,2,35,5,0,13,1,246,54,97,255,98,254,110";

            var result = StringHash.Hash(256, input);

            _output.WriteLine($"Day 10 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            const string input = "225,171,131,2,35,5,0,13,1,246,54,97,255,98,254,110";

            var result = StringHash.HashAscii(256, input);

            _output.WriteLine($"Day 10 problem 2: {result}");
        }
    }

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
                for (int i = 0; i < lenghts.Length; i++) {
                    var length = lenghts[i];
                    list = ArrayReverseReplace(list, listIndex, lenghts[i]);
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

            var hex1 = condencedHash[0].ToString("x2");
            var hex2 = condencedHash[1].ToString("x2");
            var hex3 = condencedHash[2].ToString("x2");
            string hash = string.Join("", condencedHash.Select(i => i.ToString("x2")));

            return hash;
        }
    }
}
