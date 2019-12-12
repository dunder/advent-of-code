using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Solutions.InputReader;



namespace Solutions.Event2019
{
    // --- Day 12: The N-Body Problem ---
    public class Day12
    {
        public struct Point : IEquatable<Point>
        {
            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public Point(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public int ManhattanDistanceTo(Point other)
            {
                return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
            }

            public override string ToString()
            {
                return $"({X},{Y},{Z})";
            }

            public bool Equals(Point other)
            {
                return X == other.X && Y == other.Y && Z == other.Z;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Point other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = X;
                    hashCode = (hashCode * 397) ^ Y;
                    hashCode = (hashCode * 397) ^ Z;
                    return hashCode;
                }
            }
        }

        private class Moon
        {
            public string Name { get; }
            public Point Position { get; set; }
            public Point Velocity { get; set; }

            public Moon(string name, Point position)
            {
                Name = name;
                Position = position;
                Velocity = new Point(0,0,0);
            }

            public Moon Clone()
            {
                return new Moon(Name, Position) {Velocity = Velocity};
            }

            public void ApplyGravity(Moon other)
            {
                var x = Velocity.X;
                var y = Velocity.Y;
                var z = Velocity.Z;

                if (other.Position.X < Position.X)
                {
                    x -= 1;

                } 
                else if (other.Position.X > Position.X)
                {
                    x += 1;
                }

                if (other.Position.Y < Position.Y)
                {
                    y -= 1;

                } 
                else if (other.Position.Y > Position.Y)
                {
                    y += 1;
                }

                if (other.Position.Z < Position.Z)
                {
                    z -= 1;

                } 
                else if (other.Position.Z > Position.Z)
                {
                    z += 1;
                }

                Velocity = new Point(x,y,z);
            }

            public void ApplyVelocity()
            {
                this.Position = new Point(Position.X + Velocity.X, Position.Y + Velocity.Y, Position.Z + Velocity.Z);
            }

            public int TotalEnergy => PotentialEnergy * KineticEnergy;
            public int PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);

            public int KineticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);

            public override string ToString()
            {
                return $"{Name} Position: {Position} Velocity: {Velocity} Energy: {PotentialEnergy} * {KineticEnergy} = {TotalEnergy}";
            }
        }

        private static string State(List<Moon> moons)
        {
            var sb = new StringBuilder();
            foreach (var moon in moons)
            {
                sb.Append($"{moon.Position.X}{moon.Position.Y}{moon.Position.Z}{moon.Velocity.X}{moon.Velocity.Y}{moon.Velocity.Z}");
            }

            return sb.ToString();
        }

        private static void Step(List<Moon> moons)
        {
            foreach (var moon in moons)
            {
                foreach (var other in moons.Where(m => !m.Name.Equals(moon.Name)))
                {
                    moon.ApplyGravity(other);
                }
            }

            foreach (var moon in moons)
            {
                moon.ApplyVelocity();
            }
        }

        private static bool SameState(List<Moon> previous, List<Moon> current)
        {
            for (int i = 0; i < previous.Count; i++)
            {
                if (!previous[i].Position.Equals(current[i].Position) || !previous[i].Velocity.Equals(current[i].Velocity))
                {
                    return false;
                }
            }

            return true;
        }
        private static void Run(List<Moon> moons, int steps)
        {
            for (int i = 1; i <= steps; i++)
            {
                Step(moons);
            }
        }

        private static int Run2(List<Moon> moons, int steps)
        {
            var states = new HashSet<string>();
            for (int step = 0; step <= steps; step++)
            {
                Step(moons);
                var state = State(moons);
                if (states.Contains(state))
                {
                    return step;
                }

                states.Add(state);
            }

            return steps;
        }

        public int FirstStar()
        {
            var io = new Moon("Io", new Point(15, -2, -6));
            var europa = new Moon("Europa", new Point(-5, -4, -11));
            var ganymede = new Moon("Ganymede", new Point(0, -6, 0));
            var callisto = new Moon("Callisto", new Point(5, 9, 6));
            var moons = new List<Moon> {io, europa, ganymede, callisto};

            Run(moons, 1000);
            
            return moons.Sum(moon => moon.TotalEnergy);
        }

        public int SecondStar()
        {
            var io = new Moon("Io", new Point(15, -2, -6));
            var europa = new Moon("Europa", new Point(-5, -4, -11));
            var ganymede = new Moon("Ganymede", new Point(0, -6, 0));
            var callisto = new Moon("Callisto", new Point(5, 9, 6));
            var moons = new List<Moon> { io, europa, ganymede, callisto };

            int steps = Run2(moons, int.MaxValue);

            return steps;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(6735, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(-1, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var io = new Moon("Io", new Point(-1, 0, 2));
            var europa = new Moon("Europa",new Point(2, -10, -7));
            var ganymede = new Moon("Ganymede", new Point(4, -8, 8));
            var callisto = new Moon("Callisto", new Point(3, 5, -1));
            var moons = new List<Moon> { io, europa, ganymede, callisto };

            Run(moons, 10);

            Assert.Equal(179, moons.Sum(m => m.TotalEnergy));
        }


        [Fact]
        public void SecondStarExample()
        {
            var io = new Moon("Io", new Point(-1, 0, 2));
            var europa = new Moon("Europa", new Point(2, -10, -7));
            var ganymede = new Moon("Ganymede", new Point(4, -8, 8));
            var callisto = new Moon("Callisto", new Point(3, 5, -1));
            var moons = new List<Moon> { io, europa, ganymede, callisto };

            int steps = Run2(moons, 3000);

            Assert.Equal(2772, steps);
        }
    }
}

