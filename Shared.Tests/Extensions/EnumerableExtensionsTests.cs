using Shared.Extensions;
using Xunit;

namespace Shared.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void SequenceStartsWithSingle()
        {
            var sequences = "1234445".Sequences();

            Assert.Collection(sequences,
                x => Assert.Equal(('1', 1), x),
                x => Assert.Equal(('2', 1), x),
                x => Assert.Equal(('3', 1), x),
                x => Assert.Equal(('4', 3), x),
                x => Assert.Equal(('5', 1), x));
        }        
        
        [Fact]
        public void SequenceEndsWithSingle()
        {
            var sequences = "11234445".Sequences();

            Assert.Collection(sequences,
                x => Assert.Equal(('1', 2), x),
                x => Assert.Equal(('2', 1), x),
                x => Assert.Equal(('3', 1), x),
                x => Assert.Equal(('4', 3), x),
                x => Assert.Equal(('5', 1), x));
        }

        [Fact]
        public void SequenceEndsWithMultiple()
        {
            var sequences = "112344455".Sequences();

            Assert.Collection(sequences,
                x => Assert.Equal(('1', 2), x),
                x => Assert.Equal(('2', 1), x),
                x => Assert.Equal(('3', 1), x),
                x => Assert.Equal(('4', 3), x),
                x => Assert.Equal(('5', 2), x));
        }
        
        [Fact]
        public void SequenceInt()
        {
            var sequences = new[] { 1, 1, 2, 3, 4, 4, 4, 5 }.Sequences();

            Assert.Collection(sequences,
                x => Assert.Equal((1, 2), x),
                x => Assert.Equal((2, 1), x),
                x => Assert.Equal((3, 1), x),
                x => Assert.Equal((4, 3), x),
                x => Assert.Equal((5, 1), x));
        }        

        [Fact]
        public void SequenceString()
        {
            var sequences = new[] { "1", "1", "2", "3", "4", "4", "4", "5" }.Sequences();

            Assert.Collection(sequences,
                x => Assert.Equal(("1", 2), x),
                x => Assert.Equal(("2", 1), x),
                x => Assert.Equal(("3", 1), x),
                x => Assert.Equal(("4", 3), x),
                x => Assert.Equal(("5", 1), x));
        }
    }
}
