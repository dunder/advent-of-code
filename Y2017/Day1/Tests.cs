using System.Xml.Schema;
using Xunit;
using Xunit.Sdk;

namespace Y2017.Day1 {
    public class Tests {
        [Theory]
        [InlineData("1122", 3)]
        [InlineData("1111", 4)]
        [InlineData("1234", 0)]
        [InlineData("91212129", 9)]
        public void Problem1_Example1(string input, int expectedSum) {
            var result = Captcha.Read(input);
            Assert.Equal(expectedSum, result);
        }

        [Theory]
        [InlineData("1212", 6)]
        [InlineData("1221", 0)]
        [InlineData("123425", 4)]
        [InlineData("123123", 12)]
        [InlineData("12131415", 4)]
        public void Problem2_Example1(string input, int expectedSum) {
            var result = Captcha.ReadHalfway(input);
            Assert.Equal(expectedSum, result);
        }

        public class Captcha {
            public static int Read(string input) {
                int sum = 0;
                for (int i = 0; i < input.Length; i++) {
                    var index = i;
                    var nextIndex = i + 1;
                    if (nextIndex == input.Length) {
                        nextIndex = 0;
                    }
                    char digit1 = input[index];
                    char digit2 = input[nextIndex];
                    if (digit1 == digit2) {
                        sum += int.Parse(new string(new [] {digit1}));
                    }
                }
                return sum;
            }
            public static int ReadHalfway(string input) {
                int sum = 0;
                for (int i = 0; i < input.Length; i++) {
                    var index = i;
                    var nextIndex = i + input.Length / 2;
                    if (nextIndex > input.Length - 1) {
                        nextIndex = nextIndex % input.Length;
                    }
                    char digit1 = input[index];
                    char digit2 = input[nextIndex];
                    if (digit1 == digit2) {
                        sum += int.Parse(new string(new [] {digit1}));
                    }
                }
                return sum;
            }
        }
    }
}
