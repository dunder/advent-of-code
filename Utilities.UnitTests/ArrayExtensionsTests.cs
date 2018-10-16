using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Xunit;

namespace Utilities.UnitTests
{
    public class ArrayExtensionsTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 1)]
        [InlineData(-1, 3)]
        public void Test(int index, int expectedValue) {
            int[] array = {1, 2, 3};

            int value = array.GetWithWrappedIndex(index);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData(new[] {1, 2, 3, 4}, 2, 2, new[] {3,4})]
        [InlineData(new[] {1, 2, 3, 4}, 3, 2, new[] {4,1})]
        public void SubArrayWithWrap(int[] data, int index, int length, int[] expected) {

            var result = data.SubArrayWithWrap(index, length);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Kvälskluring_ForLoopsOnly() {
            var input = new[] { 2, 3, 5, 10 };

            var forwardFactors = new List<int>();
            var backwardFactors = new List<int>();

            int forwardProduct = 1;
            int backwardProduct = 1;
            for (int i = 0; i < input.Length; i++) {
                forwardProduct *= input[i];
                backwardProduct *= input[input.Length - i - 1];
                forwardFactors.Add(forwardProduct);
                backwardFactors.Add(backwardProduct);
            }

            var output = new List<int>();
            for (int i = 0; i < input.Length; i++) {
                var f = i - 1 >= 0 ? forwardFactors[i - 1] : 1;
                var b = input.Length - i - 1 >= 0 ? backwardFactors[input.Length - i - 1] : 1;
                output.Add(f * b);
            }

            Assert.Equal(new[] { 150, 100, 60, 30 }, output.ToArray());
        }

        [Fact]
        public void Kvälskluring_MixLinqAndForLoop() {
            var input = new[] { 2, 3, 5, 10 }; 


            var forwardFactors = input.Skip(1).Aggregate(new List<int> { input.First() }, (list, x) => { list.Add(x * list.Last()); return list; });
            var backwardFactors = input.Reverse().Skip(1).Aggregate(new List<int> { input.Last() }, (list, x) => { list.Add(x * list.Last()); return list; }).ToArray().Reverse().ToArray();


            var output = new List<int>();
            for (int i = 0; i < input.Length; i++) {
                var f = i - 1 >= 0 ? forwardFactors[i - 1] : 1;
                var b = i + 1 < input.Length ? backwardFactors[i + 1] : 1;
                output.Add(f * b);
            }

            Assert.Equal(new [] {150,100,60,30}, output.ToArray());
        }


        [Fact]
        public void Kvällskluring_PureLinq() {
            var input = new[] { 2, 3, 5, 10 };
            var forwardFactors2 = input.Skip(1).Take(input.Length - 2).Aggregate(new List<int> { 1, input.First() }, (list, x) => { list.Add(x * list.Last()); return list; });
            var backwardFactors2 = input.Reverse().Skip(1).Take(input.Length - 2).Aggregate(new List<int> { 1, input.Last() }, (list, x) => { list.Add(x * list.Last()); return list; }).ToArray().Reverse().ToArray();
            var output = forwardFactors2.Select((value, index) => new { Value = value, Index = index }).Aggregate(new List<int>(), (acc, x) => { acc.Add(x.Value * backwardFactors2[x.Index]); return acc; });
            Assert.Equal(new[] { 150, 100, 60, 30 }, output.ToArray());
        }

        [Theory]
        [InlineData(new[] { 2, 3, 5, 10 }, new[] { 150, 100, 60, 30 })]
        [InlineData(new[] { 12, 8, 5, 3, 2 }, new[] { 240, 360, 576, 960, 1440 })]
        public void Kvällskluring_PureLinq_Theory(int[] input, int[] expected) {
            var forwardProducts = input.Skip(1).Take(input.Length - 2).Aggregate(new List<int> { 1, input.First() }, (list, x) => { list.Add(x * list.Last()); return list; });
            var backwardProducts = input.Reverse().Skip(1).Take(input.Length - 2).Aggregate(new List<int> { 1, input.Last() }, (list, x) => { list.Add(x * list.Last()); return list; }).ToArray().Reverse().ToArray();
            var output = forwardProducts.Select((value, index) => new { Value = value, Index = index }).Aggregate(new List<int>(), (acc, x) => { acc.Add(x.Value * backwardProducts[x.Index]); return acc; });
            Assert.Equal(expected, output.ToArray());
        }



        [Theory]
        [InlineData(new[] { 2, 3, 5, 10 }, new[] { 150, 100, 60, 30 })]
        [InlineData(new[] { 12, 8, 5, 3, 2 }, new[] { 240, 360, 576, 960, 1440 })]
        public void Kvällskluring_PureLinqWithZip_Theory(int[] input, int[] expected) {
            var forwardProducts = input.Skip(1).Take(input.Length - 2).Aggregate(new List<int> { 1, input.First() }, (list, x) => { list.Add(x * list.Last()); return list; });
            var backwardProducts = input.Reverse().Skip(1).Take(input.Length - 2).Aggregate(new List<int> { 1, input.Last() }, (list, x) => { list.Add(x * list.Last()); return list; }).ToArray().Reverse().ToArray();
            var output = forwardProducts.Zip(backwardProducts, (x, y) => x * y);
                               
            Assert.Equal(expected, output.ToArray());
        }
    }
}
