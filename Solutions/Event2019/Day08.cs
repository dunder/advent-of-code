using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Xunit;
using static Solutions.InputReader;


namespace Solutions.Event2019
{
    // 
    public class Day08
    {
        public void Parse(IEnumerable<string> input)
        {

        }

        public static int CountZeroes(string input)
        {
            int width = 25;
            int height = 6;

            var fewestZeroes = input.Batch(width * height).OrderBy(layer => layer.Count(c => c == '0')).First().ToList();
            return fewestZeroes.Count(c => c == '1') * fewestZeroes.Count(c => c == '2');
        }
        public int FirstStar()  
        {
            var input = ReadInput();
            return CountZeroes(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return 0;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(-1, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }
    }
}
