using System.Linq;
using Shared.Combinatorics;
using Xunit;

namespace Shared.Tests.Combinatorics
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void PermuationsTest()
        {
            var values = new[] { 1, 2, 3 };

            var permutations = values.Permutations();

            Assert.Equal(6, permutations.Count());

            Assert.Contains(new []{1, 2, 3}, permutations);
            Assert.Contains(new []{1, 3, 2}, permutations);
            Assert.Contains(new []{2, 1, 3}, permutations);
            Assert.Contains(new []{2, 3, 1}, permutations);
            Assert.Contains(new []{3, 1, 2}, permutations);
            Assert.Contains(new []{3, 2, 1}, permutations);
        }

        [Fact]
        public void PermuationsTestMany()
        {
            var values = new[] {1, 2, 3, 4, 5, 6};

            var permutations = values.Permutations();

            // 6! = 720
            Assert.Equal(720, permutations.Count());
        }        

    }
}
