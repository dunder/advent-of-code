using System.Collections.Generic;
using System.Linq;
using Shared.Extensions;
using Xunit;

using static Solutions.InputReader;

namespace Solutions.Event2015
{
    // --- Day 2: I Was Told There Would Be No Math ---
    public class Day02
    {
        public class Present
        {
            public int L { get; set; }
            public int W { get; set; }
            public int H { get; set; }

            public int Slack => Sides.Min();
            public int Wrapping => Area + Slack;

            public int RibbonWrap => SidePerimeters.Min();
            public int RibbonBow => Volume;
            public int Ribbon => RibbonWrap + RibbonBow;

            public int Area => 2*Sides.Sum();
            public int Volume => L * W * H;
            public IEnumerable<int> Sides => new List<int> {L * W, W * H, H * L};
            public IEnumerable<int> SidePerimeters => new List<int> {2*L + 2*W, 2*W + 2*H, 2*H + 2*L};
        }

        public static IEnumerable<Present> Parse(IEnumerable<string> input)
        {
            return input.Select(d =>
            {
                var parts = d.Split('x').Select(int.Parse).ToList();
                return new Present
                {
                    L = parts[0],
                    W = parts[1],
                    H = parts[2]

                };
            });
        }

        public static int FirstStar()
        {
            var input = ReadLineInput();
            var presents = Parse(input);

            return presents.Sum(p => p.Wrapping);
        }

        public static int SecondStar()
        {
            var input = ReadLineInput();
            var presents = Parse(input);

            return presents.Sum(p => p.Ribbon);
        }

        [Fact]
        public void FirstStarTest()
        {
            var result = FirstStar();

            Assert.Equal(1588178, result);
        }
        [Fact]
        public void SecondStarTest()
        {
            var result = SecondStar();

            Assert.Equal(3783758, result);
        }

        [Fact]
        public void ParseTest()
        {
            var result = Parse("2x3x4".Yield()).Single();

            Assert.Equal(2, result.L);
            Assert.Equal(3, result.W);
            Assert.Equal(4, result.H);
        }

        [Theory]
        [InlineData(2,3,4,52)]
        [InlineData(1,1,10,42)]
        public void AreaTest(int w, int h, int l, int expectedArea)
        {
            var present = new Present {W = w, H = h, L = l};

            Assert.Equal(expectedArea, present.Area);
        }        
        
        [Theory]
        [InlineData(2,3,4,6)]
        [InlineData(1,1,10,1)]
        public void SlackTest(int w, int h, int l, int expectedArea)
        {
            var present = new Present {W = w, H = h, L = l};

            Assert.Equal(expectedArea, present.Slack);
        }       
        
        [Theory]
        [InlineData(2,3,4,10)]
        [InlineData(1,1,10,4)]
        public void RibbonWrapTest(int w, int h, int l, int expectedArea)
        {
            var present = new Present {W = w, H = h, L = l};

            Assert.Equal(expectedArea, present.RibbonWrap);
        }   
        
        [Theory]
        [InlineData(2,3,4,24)]
        [InlineData(1,1,10,10)]
        public void RibbonBowTest(int w, int h, int l, int expectedArea)
        {
            var present = new Present {W = w, H = h, L = l};

            Assert.Equal(expectedArea, present.RibbonBow);
        }
    }
}
