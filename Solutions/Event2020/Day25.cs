using Xunit;
using Xunit.Abstractions;


namespace Solutions.Event2020
{
    // --- Day 25: Combo Breaker ---

    public class Day25
    {
        private readonly ITestOutputHelper output;


        public Day25(ITestOutputHelper output)
        {
            this.output = output;
        }

        private long EncryptionKey(long publicKey, long loopSize)
        {
            long encryptionKey = 1;
            long subjectNumber = publicKey;

            for (int i = 0; i < loopSize; i++)
            {
                encryptionKey = encryptionKey * subjectNumber % 20201227;
            }

            return encryptionKey;
        }

        private long CalculateLoopSize(long subjectNumber, long publicKey)
        {
            long current = 1;
            long loopSize = 0;
            do
            {
                current = current * subjectNumber % 20201227;
                loopSize++;
            } while (current != publicKey);

            return loopSize;
        }

        public long FirstStar()
        {
            var publicKeyCard = 17773298;
            var publicKeyDoor = 15530095;

            long loopSizeCard = CalculateLoopSize(7, publicKeyCard);
            long encryptionKey1 = EncryptionKey(publicKeyDoor, loopSizeCard);

            return encryptionKey1;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(17980581, FirstStar());
        }

        [Theory]
        [InlineData(5764801, 8)]
        [InlineData(17807724, 11)]
        public void FirstStarLoopSizeTest(int publicKey, int expectedLoopSize)
        {
            var result = CalculateLoopSize(7, publicKey);
            Assert.Equal(expectedLoopSize, result);
        }

        [Theory]
        [InlineData(5764801, 17807724, 14897079)]
        [InlineData(17807724, 5764801, 14897079)]
        public void FirstStarEncryptionKeyTest(int publicKeyCard, int publicKeyDoor, int expectedEncryptionKey)
        {
            var result = CalculateLoopSize(7, publicKeyCard);
            var encryptionKey = EncryptionKey(publicKeyDoor, result);

            Assert.Equal(expectedEncryptionKey, encryptionKey);
        }
    }
}
