using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Y2016.Day04 {
    public class Problems {
        private readonly ITestOutputHelper _output;

        public Problems(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void Problem1() {

            string[] input = File.ReadAllLines(@".\Day04\input.txt");

            var result = RoomEncryptor.CountCorrectRoomDescriptors(input);

            Assert.Equal(137896, result);
            _output.WriteLine($"Day 4 problem 1: {result}");
        }

        [Fact]
        public void Problem2() {
            string[] input = File.ReadAllLines(@".\Day04\input.txt");

            var result = RoomEncryptor.SectorOf(RoomEncryptor.Decrypt(input));

            Assert.Equal("501", result);
            _output.WriteLine($"Day 4 problem 2: {result}");
        }
    }
}
