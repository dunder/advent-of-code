using Shared.MapGeometry;
using Xunit;

namespace Shared.Tests.MapGeometry 
{
    public class TurnExtensionsTests
    {

        [Fact]
        public void TurnWhenR()
        {
            Assert.Equal(Turn.Right, "R".Turn());
        }
        
        [Fact]
        public void TurnWhenL()
        {
            Assert.Equal(Turn.Left, "L".Turn());
        }
    }
}
