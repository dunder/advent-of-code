using Xunit;

namespace Y2016.Day04 {
    public class Tests {
        [Fact]
        public void Problem1_Example1() {

            var count = RoomEncryptor.CountCorrectRoomDescriptors(new[] {
                "aaaaa-bbb-z-y-x-123[abxyz]",
                "a-b-c-d-e-f-g-h-987[abcde]",
                "not-a-real-room-404[oarel]",
                "totally-real-room-200[decoy]",
            });

            Assert.Equal(123 + 987 + 404, count);
        }

        [Fact]
        public void Problem2_Example1() {
            var encrypted = "qzmt-zixmtkozy-ivhz-343";

            var decrypted = RoomEncryptor.Decrypt(encrypted);

            Assert.Equal("very-encrypted-name-343", decrypted);
        }
    }
}
