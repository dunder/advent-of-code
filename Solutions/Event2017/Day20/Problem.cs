using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solutions.Event2017.Day20
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2017;
        public override Day Day => Day.Day20;

        public override string FirstStar()
        {
            var input = ReadLineInput();
            var result = Particles.ClosestToOrigo(input);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadLineInput();
            var result = Particles.LeftAfterCollisions(input);
            return result.ToString();
        }
    }

    public class Particles
    {
        public static int ClosestToOrigo(IList<string> input)
        {
            var particles = new List<Particle>();
            for (int i = 0; i < input.Count; i++)
            {
                var line = input[i];
                string[] particleData = Regex.Split(line, @"(?<=>), ");
                var position = particleData[0];
                var velocity = particleData[1];
                var acceleration = particleData[2];

                particles.Add(new Particle(i, position, velocity, acceleration));
            }

            var x = particles.Aggregate((p1, p2) => p1.TotalAcceleration < p2.TotalAcceleration ? p1 : p2);
            return x.Id;
        }

        public static int LeftAfterCollisions(IList<string> input)
        {
            var particles = new List<Particle>();
            for (int i = 0; i < input.Count; i++)
            {
                var line = input[i];
                string[] particleData = Regex.Split(line, @"(?<=>), ");
                var position = particleData[0];
                var velocity = particleData[1];
                var acceleration = particleData[2];

                particles.Add(new Particle(i, position, velocity, acceleration));
            }

            for (int i = 1; i < 1000; i++)
            {
                particles = particles.GroupBy(p => p.Position).Where(g => g.Count() == 1).SelectMany(g => g).ToList();

                foreach (var particle in particles)
                {
                    particle.Step();
                }
            }

            return particles.Count;
        }
    }

    public class Particle
    {
        public Particle(int id, string position, string velocity, string acceleration)
        {
            Id = id;
            Position = new Triple(position);
            Velocity = new Triple(velocity);
            Acceleration = new Triple(acceleration);
        }

        public Particle(Triple position, Triple velocity, Triple acceleration)
        {
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
        }

        public int Id { get; }
        public Triple Position { get; set; }
        public Triple Velocity { get; set; }
        public Triple Acceleration { get; set; }

        public int TotalAcceleration => Math.Abs(Acceleration.X * Acceleration.X) +
                                        Math.Abs(Acceleration.Y * Acceleration.Y) +
                                        Math.Abs(Acceleration.Z * Acceleration.Z);

        public bool CollidesWith(Particle other)
        {
            if (other.Id == Id) return false;
            return Position.X == other.Position.X && Position.Y == other.Position.Y && Position.Z == other.Position.Z;
        }

        public Particle Tick()
        {
            var vx = Velocity.X + Acceleration.X;
            var vy = Velocity.Y + Acceleration.Y;
            var vz = Velocity.Z + Acceleration.Z;
            var px = Position.X + Velocity.X;
            var py = Position.Y + Velocity.Y;
            var pz = Position.Z + Velocity.Z;

            return new Particle(new Triple(px, py, pz), new Triple(vx, vy, vz),
                new Triple(Acceleration.X, Acceleration.Y, Acceleration.Z));
        }

        public void Step()
        {
            Velocity.X += Acceleration.X;
            Velocity.Y += Acceleration.Y;
            Velocity.Z += Acceleration.Z;
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
            Position.Z += Velocity.Z;
        }

        public List<int> SteadyDistance()
        {
            var distances = new List<int> {Distance()};
            Particle p = this;
            for (int i = 0; i < 6; i++)
            {
                p = p.Tick();
                distances.Add(p.Distance());
            }

            return distances;
        }

        private int Distance()
        {
            return Math.Abs(Position.X * Position.X) + Math.Abs(Position.Y * Position.Y) +
                   Math.Abs(Position.Z * Position.Z);
        }

        protected bool Equals(Particle other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Particle) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return $"Position= {Position} Velocity = {Velocity} Acceleration = {Acceleration}";
        }
    }

    public class Triple
    {
        private static readonly Regex Number = new Regex(@"-?\d+");

        public Triple(string description)
        {
            var values = Number.Matches(description);
            X = int.Parse(values[0].Value);
            Y = int.Parse(values[1].Value);
            Z = int.Parse(values[2].Value);
        }

        public Triple(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        protected bool Equals(Triple other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Triple) obj);
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

        public override string ToString()
        {
            return $"X={X} Y={Y} Z={Z}";
        }
    }
}