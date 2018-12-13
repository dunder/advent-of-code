using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.MapGeometry;
using Xunit;
using static Solutions.InputReader;

namespace Solutions.Event2018
{
    public class Day13 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day13;
        public string Name => "Mine Cart Madness";

        const char CartFacingDown = 'v';
        const char CartFacingUp = '^';
        const char CartFacingLeft = '<';
        const char CartFacingRight = '>';

        public string FirstStar()
        {
            var input = ReadLineInput();
            var result = LocationOfFirstCrash(input);
            return result;
        }

        public string SecondStar()
        {
            var input = ReadLineInput();
            var result = LocationOfLastCart(input);
            return result;
        }

        public class Cart
        {
            private static int _idGenerator;
            private static readonly Turn[] Turns =
            {
                Shared.MapGeometry.Turn.Left,
                Shared.MapGeometry.Turn.None,
                Shared.MapGeometry.Turn.Right
            };

            public Cart()
            {
                Id = _idGenerator++;
            }

            public int Id { get; }
            public Direction Facing { get; set; }
            public Point Position { get; set; }

            private int DirectionSelection { get; set; }

            public void Turn(IDictionary<Point, char> trackParts)
            {
                var track = trackParts[Position];

                switch (track)
                {
                    case '+':
                        var turn = Turns[DirectionSelection++ % 3];
                        Facing = TurnTo(Facing, turn);
                        break;
                    case '/':
                        switch (Facing)
                        {
                            case Direction.North:
                            case Direction.South:
                                Facing = TurnTo(Facing, Shared.MapGeometry.Turn.Right);
                                break;
                            case Direction.East:
                            case Direction.West:
                                Facing = TurnTo(Facing, Shared.MapGeometry.Turn.Left);
                                break;
                        }
                        break;
                    case '\\':
                        switch (Facing)
                        {
                            case Direction.North:
                            case Direction.South:
                                Facing = TurnTo(Facing, Shared.MapGeometry.Turn.Left);
                                break;
                            case Direction.East:
                            case Direction.West:
                                Facing = TurnTo(Facing, Shared.MapGeometry.Turn.Right);
                                break;
                        }
                        break;
                }
            }

            public void MoveOne(IDictionary<Point, char> trackParts)
            {
                Turn(trackParts);
                Position = Position.Move(Facing);
            }

            public override string ToString()
            {
                return $"{Position}, {Facing}";
            }

            protected bool Equals(Cart other)
            {
                return Id == other.Id;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Cart) obj);
            }

            public override int GetHashCode()
            {
                return Id;
            }
        }

        public static Direction TurnTo(Direction direction, Turn turn)
        {
            if (turn == Turn.None) return direction;

            switch (direction)
            {
                case Direction.North:
                    return turn == Turn.Right ? Direction.East : Direction.West;
                case Direction.East:
                    return turn == Turn.Right ? Direction.South : Direction.North;
                case Direction.South:
                    return turn == Turn.Right ? Direction.West : Direction.East;
                case Direction.West:
                    return turn == Turn.Right ? Direction.North : Direction.South;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), $"Unsupported direction: {direction}");
            }
        }

        public static Direction ToDirection(char facing)
        {
            switch (facing)
            {
                case CartFacingUp:
                    return Direction.North;
                case CartFacingRight:
                    return Direction.East;
                case CartFacingDown:
                    return Direction.South;
                case CartFacingLeft:
                    return Direction.West;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static (IList<Cart> cars, Dictionary<Point, char> trackParts) ParseTracks(IList<string> input)
        {
            var trackParts = new Dictionary<Point, char>();

            for (int y = 0; y < input.Count; y++)
            {
                var line = input[y];
                for (int x = 0; x < line.Length; x++)
                {
                    trackParts.Add(new Point(x,y), line[x]);
                }
            }

            var carts = trackParts
                .Where(p => p.Value == CartFacingDown || p.Value == CartFacingUp || p.Value == CartFacingLeft ||
                            p.Value == CartFacingRight)
                .Select(p => new Cart
                {
                    Position = p.Key,
                    Facing = ToDirection(p.Value)
                })
                .ToList();

            foreach (var cart in carts)
            {
                if (cart.Facing == Direction.West || cart.Facing == Direction.East)
                {
                    trackParts[cart.Position] = '-';
                }
                else
                {
                    trackParts[cart.Position] = '|';
                }
            }

            return (carts, trackParts);
        }

        private string LocationOfFirstCrash(IList<string> input)
        {
            var(carts, trackParts) = ParseTracks(input);

            carts = carts
                .OrderBy(p => p.Position.X)
                .ThenBy(p => p.Position.Y)
                .ToList();

            while (true)
            {
                foreach (var cart in carts)
                {
                    cart.MoveOne(trackParts);

                    var positions = new HashSet<Point>();

                    foreach (var otherCart in carts)
                    {
                        if (positions.Contains(otherCart.Position))
                        {
                            return $"{otherCart.Position.X},{otherCart.Position.Y}";
                        }

                        positions.Add(otherCart.Position);
                    }
                }
            }
        }

        public string LocationOfLastCart(IList<string> input)
        {
            var (cars, trackParts) = ParseTracks(input);

            while (true)
            {
                cars = cars
                    .OrderBy(p => p.Position.Y)
                    .ThenBy(p => p.Position.X)
                    .ToList();

                var cartsToRemove = new List<Cart>();

                foreach (var cart in cars)
                {
                    cart.MoveOne(trackParts);

                    var cart1 = cart;
                    var otherCars = cars.Where(c => c.Id != cart1.Id).ToList();
                    var crash = otherCars.SingleOrDefault(o => o.Position == cart.Position);
                    if (crash != null)
                    {
                        cartsToRemove.Add(cart);
                        cartsToRemove.Add(crash);
                    }
                }

                foreach (var cartToRemove in cartsToRemove)
                {
                    cars.Remove(cartToRemove);
                }

                if (cars.Count == 1)
                {
                    var lastCar = cars.Single();
                    return $"{lastCar.Position.X},{lastCar.Position.Y}";
                }
            }
        }

        [Fact]
        public void FirstStarExample()
        {
            var input = new List<string>
            {
                @"/->-\        ",
                @"|   |  /----\",
                @"| /-+--+-\  |",
                @"| | |  | v  |",
                @"\-+-/  \-+--/",
                @"\------/     ",
            };

            var crashLocation = LocationOfFirstCrash(input);

            Assert.Equal("7,3", crashLocation);
        }

        [Fact]
        public void SecondStarExample()
        {
            var input = new List<string>
            {
                @"/>-<\  ",
                @"|   |  ",
                @"| /<+-\",
                @"| | | v",
                @"\>+</ |",
                @"  |   ^",
                @"  \<->/",
            };

            var lastCarPosition = LocationOfLastCart(input);

            Assert.Equal("6,4", lastCarPosition);
        }
        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("38,72", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("68,27", actual);
        }
    }
}