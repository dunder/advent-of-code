using Xunit;

namespace Solutions.Event2017.Day23 {
    public class Tests {

        /*
        0 set b 57
        1 set c b
        2 jnz a 2
        3 jnz 1 5
        4 mul b 100
        5 sub b -100000
        6 set c b
        7 sub c -17000
        8 set f 1
        9 set d 2
        10 set e 2
        11 set g d
        12 mul g e  
        13 sub g b
        14 jnz g 2   (goto 16)
        15 set f 0
        16 sub e -1
        17 set g e 
        18 sub g b
        19 jnz g -8  (goto 11)
        20 sub d -1
        21 set g d
        22 sub g b
        23 jnz g -13 (goto 10)
        24 jnz f 2   (goto 26)
        25 sub h -1
        26 set g b
        27 sub g c
        28 jnz g 2   (goto 30)
        29 jnz 1 3   (goto end)
        30 sub b -17
        31 jnz 1 -23 (goto 8)
        */

        [Fact(Skip = "Infinite loop, used during development")]
        public void Analyzed() {
            var h = 0;

            for (var b = 105700; b <= 122700; b += 17) {
                var f = 1;
                for (var d = 2; d <= b; d++) {
                    for (var e = 2; e <= b; e++) {
                        var g = d;
                        g *= e;
                        g -= b;
                        if (g == 0) {
                            f = 0;
                        }
                    }
                }

                if (f == 0) {
                    h++;
                }
            }

            Assert.Equal(915, h);
        }

        [Fact(Skip = "Infinite loop, used during development")]
        public void OptimizedInnerExpression() {
            var h = 0;

            for (var b = 105700; b <= 122700; b += 17) {
                var f = 1;
                for (var d = 2; d <= b; d++) {
                    for (var e = 2; e <= b; e++) {
                        if (d * e == b) {
                            f = 0;
                        }
                    }
                }

                if (f == 0) {
                    h++;
                }
            }

            Assert.Equal(915, h);
        }

        // We are counting the number of times f equals 0 which is when
        // d * e == b. If both loops use b as the upper bound there will
        // be a range of values that are known to produce d * e > b. 
        // If we use the fact that e >= 2 then we know that 
        // d * 2 > b for all d > b / 2
        // If we limit the outer loop to 2 <= d < b / 2 then we also know 
        // that the inner loop will produce d * e > b for all e > b / d
        // hence we can set an upper bound on the inner loop to b / d.
        // 
        // Once f is set to 0 it is not updated again until both loops
        // have finished so there is no use in keep looping when a 
        // pair of factors is found so we can add a break statement after
        // f = 0.
        //
        // Analyzing this further we can se that the only possibility for f
        // to have the value of 1 after the inner loops is when there is no
        // combination of d and e that multiplied gives b which actually 
        // means that what the progam does is count all b's that are not
        // prime numbers. See the final solution in the Day23.Coprocessor.Run2 
        // method.
        [Fact]
        public void OptimizedBoundries() {
            var h = 0;

            for (var b = 105700; b <= 122700; b += 17) {
                var f = 1;
                for (var d = 2; d <= b / 2; d++) {
                    for (var e = 2; e <= b / d; e++) {
                        if (d * e == b) {
                            f = 0;
                            break;
                        }
                    }
                }

                if (f == 0) {
                    h++;
                }
            }

            Assert.Equal(915, h);
        }

        [Fact]
        public void FirstStar()
        {
            var actual = new Problem().FirstStar();
            Assert.Equal("3025", actual);
        }

        [Fact]
        public void SecondStar()
        {
            var actual = new Problem().SecondStar();
            Assert.Equal("915", actual);
        }
    }
}
