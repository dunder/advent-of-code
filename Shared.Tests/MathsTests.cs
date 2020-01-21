using Xunit;

namespace Shared.Tests
{
    public class MathsTests
    {
        [Fact]
        public void ModNegative()
        {
            var actual = Maths.Mod(-8, 7);
            Assert.Equal(6, actual);
        }
    }
}
