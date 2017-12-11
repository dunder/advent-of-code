using Xunit;

namespace Y2016.Day07 {
    public class Tests {
        [Theory]
        [InlineData("abba[mnop]qrst", true)]
        [InlineData("abcd[bddb]xyyx", false)]
        [InlineData("aaaa[qwer]tyui", false)]
        [InlineData("ioxxoj[asdfgh]zxcvbn", true)]
        public void Problem1_Examples(string candidate, bool isAbba) {

            bool result = Tls.IsAutonomousBridgeBypassingAnnotation(candidate);

            Assert.Equal(isAbba, result);
        }
        [Theory]
        [InlineData("aba[bab]xyz", true)]
        [InlineData("xyx[xyx]xyx", false)]
        [InlineData("aaa[kek]eke", true)]
        [InlineData("zazbz[bzb]cdb", true)]
        public void Problem2_Examples(string candidate, bool isAba) {

            bool result = Tls.IsAreaBroadcastAccessor(candidate);

            Assert.Equal(isAba, result);
        }
    }
}
