using Xunit;

namespace Solutions.Event2016.Day21
{
    public class Tests
    {
        [Fact]
        public void SwapIndex()
        {
            var input = "abcde";

            var output = Problem.SwapIndex(input, 4, 0);

            Assert.Equal("ebcda", output);
        }

        [Fact]
        public void SwapIndexReversed()
        {
            var input = "ebcda";

            var output = Problem.SwapIndex(input, 4, 0);

            Assert.Equal("abcde", output);
        }

        [Fact]
        public void SwapLetter()
        {
            var input = "ebcda";

            var output = Problem.SwapLetter(input, 'd', 'b');

            Assert.Equal("edcba", output);
        }

        [Fact]
        public void SwapLetterReversed()
        {
            var input = "edcba";

            var output = Problem.SwapLetter(input, 'd', 'b');

            Assert.Equal("ebcda", output);
        }

        [Fact]
        public void RotateLeft()
        {
            var input = "abcde";

            var output = Problem.RotateLeft(input, 1);

            Assert.Equal("bcdea", output);
        }

        [Fact]
        public void RotateLeftReversed()
        {
            var input = "bcdea";

            var output = Problem.RotateRight(input, 1);

            Assert.Equal("abcde", output);
        }

        [Fact]
        public void RotateRight()
        {
            var input = "abcde";

            var output = Problem.RotateRight(input, 1);

            Assert.Equal("eabcd", output);
        }

        [Fact]
        public void RotateRightReversed()
        {
            var input = "eabcd";

            var output = Problem.RotateLeft(input, 1);

            Assert.Equal("abcde", output);
        }

        [Fact]
        public void RotateLetter()
        {
            var input = "abdec";

            var output = Problem.RotateLetter(input, 'b');

            Assert.Equal("ecabd", output);
        }

        [Fact]
        public void RotateLetterReversed()
        {
            var input = "ecabd";

            var output = Problem.RotateLetterReversed(input, 'b');

            Assert.Equal("abdec", output);
        }

        [Fact]
        public void ReverseBetween()
        {
            var input = "abcde";

            var output = Problem.ReverseBetween(input, 1, 3);

            Assert.Equal("adcbe", output);
        }

        [Fact]
        public void ReverseBetweenReversed()
        {
            var input = "adcbe";

            var output = Problem.ReverseBetween(input, 1, 3);

            Assert.Equal("abcde", output);
        }

        [Fact]
        public void Move()
        {
            var input = "bdeac";

            var output = Problem.Move(input, 3, 0);

            Assert.Equal("abdec", output);
        }

        [Fact]
        public void MoveReverse()
        {
            var input = "abdec";

            var output = Problem.Move(input, 0, 3);

            Assert.Equal("bdeac", output);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("gfdhebac", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("dhaegfbc", actual);
        }
    }
}
