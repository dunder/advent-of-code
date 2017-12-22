using System.Collections.Generic;
using Xunit;

namespace Y2017.Day20 {
    public class Tests {

        [Fact]
        public static void Problem1_Example1() {
            string[] input = {
                "p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>",
                "p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>"
            };

            var result = Particles.ClosestToOrigo(input);

            Assert.Equal(0, result);
        }

        [Fact]
        public static void Problem2_Example1() {
            string[] input = {
                "p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>",
                "p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>",
                "p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>",
                "p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>"
            };

            var result = Particles.LeftAfterCollisions(input);

            Assert.Equal(1, result);
        }

        [Fact]
        public void Problem2_SelfCOllision() {
            List<Particle> particles = new List<Particle> {
                new Particle(0, "<-6,0,0>", "<0,0,0>", "<0,0,0>"),
                new Particle(1, "<-6,0,0>", "<0,0,0>", "<0,0,0>")
            };

            Assert.False(particles[0].CollidesWith(particles[0]));
            Assert.True(particles[0].CollidesWith(particles[1]));
            Assert.True(particles[1].CollidesWith(particles[0]));
        }

        [Fact]
        public void Problem2_Test1() {
            List<Particle> particles = new List<Particle> {
                new Particle(0, "<-6,1,-245>", "<0,0,0>", "<0,0,0>"),
                new Particle(1, "<-6,1,-245>", "<0,0,0>", "<0,0,0>"),
                new Particle(2, "<-6,1,-245>", "<0,0,0>", "<0,0,0>"),
                new Particle(3, "<-5,0,0>", "<0,0,0>", "<0,0,0>"),
                new Particle(4, "<-6,1,-245>", "<0,0,0>", "<0,0,0>"),
            };

            Assert.True(particles[0].CollidesWith(particles[1]));
            Assert.True(particles[0].CollidesWith(particles[2]));
            Assert.True(particles[0].CollidesWith(particles[4]));
        }
    }
}
