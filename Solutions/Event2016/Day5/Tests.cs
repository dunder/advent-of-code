using System.Diagnostics;
using Shared.Crypto;
using Solutions.Event2016.Day5;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day05 {
    public class Tests {

        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output) {
            _output = output;
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Problem1_Example1() {

            var hash = Md5.Hash("abc3231929");

            Assert.StartsWith("00000", hash);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Problem1_Example2() {
            var s = new Stopwatch();
            s.Start();
            var password = PasswordGenerator.GenerateParallel("abc", 1);
            s.Stop();
            _output.WriteLine($"Took: {s.Elapsed}");
            Assert.Equal("1", password);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Problem1_Example3() {
            var s = new Stopwatch();
            s.Start();
            var password = PasswordGenerator.GenerateParallel("abc", 8);
            s.Stop();
            _output.WriteLine($"Took: {s.Elapsed}");
            Assert.Equal("18f47a30", password);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Problem2_Example1() {
            var s = new Stopwatch();
            s.Start();
            var password = PasswordGenerator.GenerateNew("abc", 8);
            s.Stop();
            _output.WriteLine($"Took: {s.Elapsed}");
            Assert.Equal("05ace8e3", password);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Day5_Problem1_Solution()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("f97c354d", actual);
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void Day5_Problem2_Solution()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("863dde27", actual);
        }
    }
}
