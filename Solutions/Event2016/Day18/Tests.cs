using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Solutions.Event2016.Day18
{
    public class Tests
    {
        [Theory]
        [InlineData('^','^','.')]
        [InlineData('.','^','^')]
        [InlineData('^','.','.')]
        [InlineData('.','.','^')]
        public void IsTrap(char left, char center, char right)
        {
            var isTrap = Problem.IsTrap(left, center, right);

            Assert.True(isTrap);
        }

        [Theory]
        [InlineData('.','^','.')]
        [InlineData('.','.','.')]
        [InlineData('^','.','^')]
        [InlineData('^','^','^')]
        public void IsNotTrap(char left, char center, char right)
        {
            var isTrap = Problem.IsTrap(left, center, right);

            Assert.False(isTrap);
        }

        [Fact]
        public void CreateMap()
        {
            var expectedMap = new List<string>
            {
                "..^^.",
                ".^^^^",
                "^^..^"
            };

            var map = Problem.CreateMap(expectedMap.First(), 3);

            Assert.Equal(expectedMap, map);
        }

        [Fact]
        public void CreateMap2()
        {
            var expectedMap = new List<string>
            {
                ".^^.^.^^^^",
                "^^^...^..^",
                "^.^^.^.^^.",
                "..^^...^^^",
                ".^^^^.^^.^",
                "^^..^.^^..",
                "^^^^..^^^.",
                "^..^^^^.^^",
                ".^^^..^.^^",
                "^^.^^^..^^"
            };

            var map = Problem.CreateMap(expectedMap.First(), 10);

            Assert.Equal(expectedMap, map);
        }

        [Fact]
        public void FirstStar_Example()
        {
            var row = "..^^.";

            var nextRow = Problem.NextRow(row);

            Assert.Equal(".^^^^", nextRow);
        }

        [Fact]
        public void FirstStar_Example2()
        {
            var row = ".^^^^";

            var nextRow = Problem.NextRow(row);

            Assert.Equal("^^..^", nextRow);
        }

        [Fact]
        public void CountSafeTiles()
        {
            var safeTiles = Problem.CountSafeTiles(".^^.^.^^^^", 10);
            Assert.Equal(38, safeTiles);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("1951", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("20002936", actual); // 4.1 s
        }
    }
}
