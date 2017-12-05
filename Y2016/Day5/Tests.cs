using Utilities;
using Xunit;

namespace Y2016.Day5 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {

            var hash = Md5.Hash("abc3231929");

            Assert.StartsWith("00000", hash);
        }

        [Fact]
        public void Problem2_Example1() {
            var password = PasswordGenerator.Generate("abc", 1);
            Assert.Equal("1", password);
        }

        [Fact]
        public void Problem2_Example2() {
            var password = PasswordGenerator.Generate("abc", 8);
            Assert.Equal("05ace8e3", password);
        }
    }
}
