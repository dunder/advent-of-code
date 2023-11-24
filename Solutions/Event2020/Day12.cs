using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;

namespace Solutions.Event2020
{
    // --- Day 12: Rain Risk ---
    public class Day12
    {
        private readonly ITestOutputHelper output;

        public Day12(ITestOutputHelper output)
        {
            this.output = output;
        }

        private enum Bearing { North, East, South, West }
        
        private enum Direction { Left, Right }

        private record Ship(int x, int y, Bearing bearing)
        {
            public Ship Stear(Direction turn, int degrees)
            {
                var ship = this;

                foreach (var _ in Enumerable.Range(0, degrees / 90))
                {
                    ship = turn == Direction.Left ? ship.TurnLeft() : ship.TurnRight();
                }

                return ship;
            }

            public (int x, int y) Position()
            {
                return (x, y);
            }

            public Ship Forward(int units)
            {
                return Move(bearing, units);
            }

            public Ship Move(Bearing bearing, int units)
            {
                switch (bearing)
                {
                    case Bearing.North:
                        return this with { y = y - units };
                    case Bearing.East:
                        return this with { x = x + units };
                    case Bearing.South:
                        return this with { y = y + units };
                    case Bearing.West:
                        return this with { x = x - units };
                    default:
                        throw new InvalidOperationException("No bearing");
                }
            }

            private Ship TurnLeft()
            {
                switch (bearing)
                {
                    case Bearing.North:
                        return this with { bearing = Bearing.West };
                    case Bearing.East:
                        return this with { bearing = Bearing.North };
                    case Bearing.South:
                        return this with { bearing = Bearing.East };
                    case Bearing.West:
                        return this with { bearing = Bearing.South };
                    default:
                        throw new InvalidOperationException("No bearing");
                }
            }

            private Ship TurnRight()
            {
                switch (bearing)
                {
                    case Bearing.North:
                        return this with { bearing = Bearing.East };
                    case Bearing.East:
                        return this with { bearing = Bearing.South };
                    case Bearing.South:
                        return this with { bearing = Bearing.West };
                    case Bearing.West:
                        return this with { bearing = Bearing.North };
                    default:
                        throw new InvalidOperationException("No bearing");
                }
            }
        }

        private record ShipWithWaypoint(int shipx, int shipy, Bearing bearing, int wpx, int wpy)
        {
            public ShipWithWaypoint RotateWaypoint(Direction turn, int degrees)
            {
                var shipWithWaypoint = this;

                foreach (var _ in Enumerable.Range(0, degrees / 90))
                {
                    shipWithWaypoint = turn == Direction.Left ? 
                        shipWithWaypoint.RotateWaypointLeft() : 
                        shipWithWaypoint.RotateWaypointRight();
                }

                return shipWithWaypoint;
            }

            public (int x, int y) Position()
            {
                return (shipx, shipy);
            }

            public ShipWithWaypoint Forward(int units)
            {
                return this with { shipx = shipx + units * wpx, shipy = shipy + units * wpy };
            }

            public ShipWithWaypoint MoveWaypoint(Bearing bearing, int units)
            {
                switch (bearing)
                {
                    case Bearing.North:
                        return this with { wpy = wpy - units };
                    case Bearing.East:
                        return this with { wpx = wpx + units };
                    case Bearing.South:
                        return this with { wpy = wpy + units };
                    case Bearing.West:
                        return this with { wpx = wpx - units };
                    default:
                        throw new InvalidOperationException("No bearing");
                }
            }

            private ShipWithWaypoint RotateWaypointLeft()
            {
                return this with { wpx = wpy, wpy = -wpx };
            }

            private ShipWithWaypoint RotateWaypointRight()
            {
                return this with { wpx = -wpy, wpy = wpx };
            }
        }

        private Ship ApplyInstructions(Ship ship, IList<string> input)
        {
            foreach (var instruction in input)
            {
                char action = instruction[0];
                int units = int.Parse(instruction.Substring(1));

                switch (action)
                {
                    case 'N': 
                        ship = ship.Move(Bearing.North, units); 
                        break;
                    case 'S': 
                        ship = ship.Move(Bearing.South, units); 
                        break;
                    case 'E': 
                        ship = ship.Move(Bearing.East, units); 
                        break;
                    case 'W': 
                        ship = ship.Move(Bearing.West, units); 
                        break;
                    case 'L': 
                        ship = ship.Stear(Direction.Left, units); 
                        break;
                    case 'R': 
                        ship = ship.Stear(Direction.Right, units); 
                        break;
                    case 'F': 
                        ship = ship.Forward(units); 
                        break;
                    default: 
                        throw new InvalidOperationException($"Unknown action: {action}");
                }
            }

            return ship;
        }
        private ShipWithWaypoint ApplyInstructions(ShipWithWaypoint shipWithWaypoint, IList<string> input)
        {
            foreach (var instruction in input)
            {
                char action = instruction[0];
                int units = int.Parse(instruction.Substring(1));

                switch (action)
                {
                    case 'N': 
                        shipWithWaypoint = shipWithWaypoint.MoveWaypoint(Bearing.North, units); 
                        break;
                    case 'S': 
                        shipWithWaypoint = shipWithWaypoint.MoveWaypoint(Bearing.South, units); 
                        break;
                    case 'E': 
                        shipWithWaypoint = shipWithWaypoint.MoveWaypoint(Bearing.East, units); 
                        break;
                    case 'W': 
                        shipWithWaypoint = shipWithWaypoint.MoveWaypoint(Bearing.West, units); 
                        break;
                    case 'L': 
                        shipWithWaypoint = shipWithWaypoint.RotateWaypoint(Direction.Left, units); 
                        break;
                    case 'R': 
                        shipWithWaypoint = shipWithWaypoint.RotateWaypoint(Direction.Right, units); 
                        break;
                    case 'F': 
                        shipWithWaypoint = shipWithWaypoint.Forward(units); 
                        break;
                    default: 
                        throw new InvalidOperationException($"Unknown action: {action}");
                }
            }

            return shipWithWaypoint;
        }

        private int ManhattanDistance((int x, int y) from, (int x, int y) to)
        {
            return Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y);
        }

        private int CalculateManhattanDistanceForInstructions(IList<string> input)
        {
            var departureShip = new Ship(0, 0, Bearing.East);
            var destinationShip = ApplyInstructions(departureShip, input);

            return ManhattanDistance(departureShip.Position(), destinationShip.Position());
        }

        private int CalculateManhattanDistanceForWaypointInstructions(IList<string> input)
        {
            var departureShip = new ShipWithWaypoint(0, 0, Bearing.East, 10, -1);
            var destinationShip = ApplyInstructions(departureShip, input);

            return ManhattanDistance(departureShip.Position(), destinationShip.Position());
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return CalculateManhattanDistanceForInstructions(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();
            return CalculateManhattanDistanceForWaypointInstructions(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(441, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(40014, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>
            {
                "F10",
                "N3",
                "F7",
                "R90",
                "F11"
            };

            Assert.Equal(25, CalculateManhattanDistanceForInstructions(example));
        }

        [Fact]
        public void TurnLeftTests()
        {
            var ship = new Ship(0,0, Bearing.North);

            ship = ship.Stear(Direction.Left, 90);
            
            Assert.Equal(Bearing.West, ship.bearing);

            ship = ship.Stear(Direction.Left, 90);

            Assert.Equal(Bearing.South, ship.bearing);

            ship = ship.Stear(Direction.Left, 90);

            Assert.Equal(Bearing.East, ship.bearing);
            
            ship = ship.Stear(Direction.Left, 90);
            
            Assert.Equal(Bearing.North, ship.bearing); 
            
            ship = ship.Stear(Direction.Left, 360);

            Assert.Equal(Bearing.North, ship.bearing);
        }

        [Fact]
        public void TurnRightTests()
        {
            var ship = new Ship(0,0, Bearing.North);

            ship = ship.Stear(Direction.Right, 90);
            
            Assert.Equal(Bearing.East, ship.bearing);

            ship = ship.Stear(Direction.Right, 90);

            Assert.Equal(Bearing.South, ship.bearing);

            ship = ship.Stear(Direction.Right, 90);

            Assert.Equal(Bearing.West, ship.bearing);
            
            ship = ship.Stear(Direction.Right, 90);
            
            Assert.Equal(Bearing.North, ship.bearing);

            ship = ship.Stear(Direction.Right, 360);

            Assert.Equal(Bearing.North, ship.bearing);
        }

        [Fact]
        public void SecondStarExample()
        {
            var example = new List<string>
            {
                "F10",
                "N3",
                "F7",
                "R90",
                "F11"
            };

            Assert.Equal(286, CalculateManhattanDistanceForWaypointInstructions(example));
        }
    }
}
