using Xunit;

namespace Y2017.Day23 {
    public class Tests {
        [Fact]
        public void Analyzed() {
            var b = 105700;
            var c = 122700;
            var h = 0;

            for (; b <= c; b += 17) {
                var f = 1; // rad 8

                for (var d = 2; d <= (b / 2); d++) {

                    for (var e = 2; e <= b / (d - 1); e++) {
                        f = d * e == b ? 0 : 1;
                    }
                }
                if (f == 0) {
                    h++;
                }
            }

            Assert.Equal(915, h);
        }
    }
}
