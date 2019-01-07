using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Shared.MapGeometry;
using Xunit;

namespace Solutions.Event2018
{
    public class Day22 : IDay
    {
        public Event Event => Event.Event2018;
        public Day Day => Day.Day22;
        public string Name => "Mode Maze";

        private static readonly int Depth = 7305;
        private static readonly Point Target = new Point(13, 734);
        private static readonly Point Mouth = new Point(0, 0);

        public string FirstStar()
        {
            var result = RiskLevelForSmallestRectangle(Depth, Target);
            return result.ToString();
        }

        public string SecondStar()
        {
            var result = FewestMinutesToTarget(Depth, Target);
            return result.ToString();
        }


        public class GeographicalRegion
        {
            public Point Location { get; }
            public int GeologicalIndex { get; set; }
            public int ErosionLevel { get; set; }
            public GeologicalType GeologicalType => (GeologicalType) (ErosionLevel % 3);

            public GeographicalRegion(Point location)
            {
                Location = location;
            }

            public override string ToString()
            {
                return $"({Location.X},{Location.Y}) Index: {GeologicalIndex} Erosion: {ErosionLevel} Type: {GeologicalType}";
            }

            protected bool Equals(GeographicalRegion other)
            {
                return Location.Equals(other.Location);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((GeographicalRegion) obj);
            }

            public override int GetHashCode()
            {
                return Location.GetHashCode();
            }
        }

        public static int RiskLevelForSmallestRectangle(int depth, Point target)
        {
            var geographicalRegions = CalculateGeology(depth, target);

            var risk = 0;

            for (int y = 0; y <= target.Y; y++)
            {
                for (int x = 0; x <= target.X; x++)
                {
                    var point = new Point(x, y);
                    risk += (int)geographicalRegions[point].GeologicalType;
                }
            }

            return risk;
        }

        public static int FewestMinutesToTarget(int depth, Point targetLocation)
        {
            var geographicalRegions = CalculateGeology(depth, targetLocation);

            var start = new RegionWithEquipment(geographicalRegions[new Point(0, 0)], Equipment.Torch);
            var target = new RegionWithEquipment(geographicalRegions[targetLocation], Equipment.Torch);

            var nodeMap = CreateNodes(geographicalRegions, target);

            var startNode = nodeMap[start];
            var targetNode = nodeMap[target];

            var terminationNode = AStar.Search(startNode, targetNode);

            return terminationNode.MovementCost;
        }

        public enum GeologicalType
        {
            Rocky = 0,
            Wet = 1,
            Narrow = 2
        }

        public enum Equipment
        {
            Neither = 0,
            Torch = 1,
            Climbing = 2
        }

        public static Dictionary<RegionWithEquipment, AStar.Node> CreateNodes(Dictionary<Point, GeographicalRegion> geographicalRegions, RegionWithEquipment target)
        {
            var nodeId = 1;
            var equipmentSet = Enum.GetValues(typeof(Equipment));

            var nodeMap = new Dictionary<RegionWithEquipment, AStar.Node>();

            foreach (var geographicalRegion in geographicalRegions)
            {
                foreach (Equipment equipment in equipmentSet)
                {
                    var regionWithEquipment = new RegionWithEquipment(geographicalRegion.Value, equipment);
                    var node = new AStar.Node(nodeId++, regionWithEquipment.Location.ManhattanDistance(target.Location), regionWithEquipment.ToString());
                    nodeMap.Add(regionWithEquipment, node);
                }
            }

            foreach (var regionNode in nodeMap)
            {
                var regionWithEquipment = regionNode.Key;
                var node = regionNode.Value;
                var neighbors = regionWithEquipment.Neighbors(geographicalRegions);
                node.Neighbors = neighbors
                    .Select(n => new AStar.Neighbor(nodeMap[n], regionWithEquipment.DistanceTo(n)))
                    .ToList();
            }

            return nodeMap;
        }

        public class RegionWithEquipment
        {
            public RegionWithEquipment(GeographicalRegion region, Equipment equipment)
            {
                Region = region;
                Equipment = equipment;
            }

            public GeographicalRegion Region { get; set; }
            public Point Location => Region.Location;
            public Equipment Equipment { get; set; }


            public int DistanceTo(RegionWithEquipment other)
            {
                return Equipment == other.Equipment ? 1 : 8;
            }

            public List<RegionWithEquipment> Neighbors(Dictionary<Point, GeographicalRegion> geographicalRegions)
            {
                var adjacent = Region.Location.AdjacentInMainDirections();
                var neighbors = new List<RegionWithEquipment>();

                foreach (var neighborLocation in adjacent.Where(geographicalRegions.ContainsKey))
                {
                    var neighborRegion = geographicalRegions[neighborLocation];
                    foreach (Equipment equipment in Enum.GetValues(typeof(Equipment)))
                    {
                        if (EquipmentValidFor(equipment, Region.GeologicalType) && EquipmentValidFor(equipment, neighborRegion.GeologicalType))
                        {
                            var neighbor = new RegionWithEquipment(neighborRegion, equipment);
                            neighbors.Add(neighbor);
                        }
                    }
                }

                return neighbors;
            }

            public bool EquipmentValidFor(Equipment equipment, GeologicalType geologicalType)
            {
                var rocky = new HashSet<Equipment> { Equipment.Climbing, Equipment.Torch };
                var wet = new HashSet<Equipment> { Equipment.Climbing, Equipment.Neither };
                var narrow = new HashSet<Equipment> { Equipment.Torch, Equipment.Neither };

                switch (geologicalType)
                {
                    case GeologicalType.Rocky:
                        return rocky.Contains(equipment);
                    case GeologicalType.Narrow:
                        return narrow.Contains(equipment);
                    case GeologicalType.Wet:
                        return wet.Contains(equipment);
                }
                throw new ArgumentOutOfRangeException($"Invalid combination of equipment {equipment} and type {geologicalType}");
            }

            public override string ToString()
            {
                return $"({Region.Location.X},{Region.Location.Y}) [{Equipment}]";
            }

            protected bool Equals(RegionWithEquipment other)
            {
                return Equipment == other.Equipment && Region.Location == other.Region.Location;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((RegionWithEquipment) obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Equipment, Region.Location);
            }
        }

        public class AStar
        {
            public class Neighbor
            {
                public Neighbor(Node node, int distanceTo)
                {
                    Node = node;
                    DistanceTo = distanceTo;
                }

                public Node Node { get; set; }
                public int DistanceTo { get; set; }

                public override string ToString()
                {
                    return $"Node: {Node.Id} Distance: {DistanceTo}";
                }
            }

            public class Node
            {
                public Node(int id, int distanceToTarget, string description = "")
                {
                    Id = id;
                    DistanceToTarget = distanceToTarget;
                    Description = description;
                }

                public int Id { get; }
                public int DistanceToTarget { get; }
                public int MovementCost { get; set; }
                public int TotalCost => MovementCost + DistanceToTarget;
                public List<Neighbor> Neighbors { get; set; }
                public Node Parent { get; set; }
                public string Description { get; set; }

                public Node StartNode
                {
                    get
                    {
                        var startNode = this;

                        while (startNode.Parent != null)
                        {
                            startNode = startNode.Parent;
                        }

                        return startNode;
                    }

                }

                public List<Node> Path
                {
                    get
                    {
                        var path = new List<Node> {this};
                        var startNode = this;

                        while (startNode.Parent != null)
                        {
                            startNode = startNode.Parent;
                            path.Add(startNode);
                        }

                        path.Reverse();
                        return path;
                    }
                }

                public int Depth
                {
                    get
                    {
                        var depth = 0;
                        var current = this;
                        while (current.Parent != null)
                        {
                            depth++;
                            current = current.Parent;
                        }
                        return depth;
                    }
                }

                protected bool Equals(Node other)
                {
                    return Id.Equals(other.Id);
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    if (ReferenceEquals(this, obj)) return true;
                    if (obj.GetType() != this.GetType()) return false;
                    return Equals((Node)obj);
                }

                public override int GetHashCode()
                {
                    return Id.GetHashCode();
                }

                public override string ToString()
                {
                    return $"{Id} G: {MovementCost} H: {DistanceToTarget} F: {TotalCost} [{Description}]";
                }
            }

            public static Node Search(Node startNode, Node targetNode)
            {
                var open = new HashSet<Node> { startNode };
                var closed = new HashSet<Node>();

                Node currentNode;

                while (true)
                {
                    currentNode = open.OrderBy(n => n.TotalCost).FirstOrDefault();

                    if (currentNode == null || currentNode.Equals(targetNode))
                    {
                        break;
                    }

                    open.Remove(currentNode);
                    closed.Add(currentNode);

                    var neighbors = currentNode.Neighbors;

                    if (!neighbors.Any() && !open.Any())
                    {
                        break;
                    }

                    var nonClosedNeighbors = neighbors.Where(n => !closed.Contains(n.Node)).ToList();

                    foreach (var neighbor in nonClosedNeighbors)
                    {
                        var neighborNode = neighbor.Node;
                        var distance = currentNode.MovementCost + neighbor.DistanceTo;

                        if (!open.TryGetValue(neighborNode, out _))
                        {
                            open.Add(neighborNode);
                        }
                        else if (distance >= neighborNode.MovementCost)
                        {
                            continue;
                        }

                        neighborNode.Parent = currentNode;
                        neighborNode.MovementCost = distance;
                    }
                }

                return currentNode;
            }
        }

        public static Dictionary<Point, GeographicalRegion> CalculateGeology(int depth, Point target)
        {
            var geographicalRegions = new Dictionary<Point, GeographicalRegion>();

            for (int x = 0; x <= target.X*2; x++)
            {
                CreateGeographicalRegion(depth, x, 0, target, geographicalRegions);
            }

            for (int y = 1; y <= target.Y + 20; y++)
            {
                CreateGeographicalRegion(depth, 0, y, target, geographicalRegions);
            }

            for (int y = 1; y <= target.Y + 20; y++)
            {
                for (int x = 1; x <= target.X*2; x++)
                {
                    CreateGeographicalRegion(depth, x, y, target, geographicalRegions);

                }
            }

            return geographicalRegions;
        }

        private static void CreateGeographicalRegion(int depth, int x, int y, Point target, Dictionary<Point, GeographicalRegion> geographicalRegions)
        {
            var point = new Point(x, y);
            var geologicalIndex = GeologicalIndex(point, target, geographicalRegions);
            var region = new GeographicalRegion(point)
            {
                GeologicalIndex = geologicalIndex,
                ErosionLevel = ErosionLevel(geologicalIndex, depth)
            };

            geographicalRegions.Add(point, region);
        }

        public static int ErosionLevel(int geologicalIndex, int depth)
        {
            return (geologicalIndex + depth) % 20183;
        }

        public static int GeologicalIndex(Point point, Point target, Dictionary<Point, GeographicalRegion> geographicalRegions)
        {
            if (point == Mouth) return 0;
            if (point == target) return 0;
            if (point.Y == 0) return point.X * 16807;
            if (point.X == 0) return point.Y * 48271;
            var risk1 = new Point(point.X - 1, point.Y);
            var risk2 = new Point(point.X, point.Y - 1);

            return geographicalRegions[risk1].ErosionLevel*geographicalRegions[risk2].ErosionLevel;
        }

        public static (Point target, Dictionary<Point, GeologicalType> geographicalTypes) ParseCave(IList<string> input)
        {
            var geographyTypes = new Dictionary<Point, GeologicalType>();
            Point target = new Point();
            for (int y = 0; y < input.Count; y++)
            {
                var line = input[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var symbol = line[x];
                    var point = new Point(x, y);

                    switch (symbol)
                    {
                        case '.':
                            geographyTypes.Add(point, GeologicalType.Rocky);
                            break;
                        case 'M':
                            break;
                        case '=':
                            geographyTypes.Add(point, GeologicalType.Wet);
                            break;
                        case '|': 
                            geographyTypes.Add(point, GeologicalType.Narrow);
                            break;
                        case 'T':
                            target = point;
                            break;
                    }
                }
            }

            return (target, geographyTypes);
        }

        [Fact]
        public void FirstStarCalculateGeologyExample()
        {
            var target = new Point(10, 10);

            var geographicalRegions = CalculateGeology(510, target);

            var geographicalRegion1 = geographicalRegions[new Point(0, 0)];

            Assert.Equal(0, geographicalRegion1.GeologicalIndex);
            Assert.Equal(510, geographicalRegion1.ErosionLevel);
            Assert.Equal(GeologicalType.Rocky, geographicalRegion1.GeologicalType);

            var geographicalRegion2 = geographicalRegions[new Point(1, 0)];

            Assert.Equal(16807, geographicalRegion2.GeologicalIndex);
            Assert.Equal(17317, geographicalRegion2.ErosionLevel);
            Assert.Equal(GeologicalType.Wet, geographicalRegion2.GeologicalType);

            var geographicalRegion3 = geographicalRegions[new Point(0, 1)];

            Assert.Equal(48271, geographicalRegion3.GeologicalIndex);
            Assert.Equal(8415, geographicalRegion3.ErosionLevel);
            Assert.Equal(GeologicalType.Rocky, geographicalRegion3.GeologicalType);

            var geographicalRegion4 = geographicalRegions[new Point(1, 1)];

            Assert.Equal(145722555, geographicalRegion4.GeologicalIndex);
            Assert.Equal(1805, geographicalRegion4.ErosionLevel);
            Assert.Equal(GeologicalType.Narrow, geographicalRegion4.GeologicalType);

            var geographicalRegionAtTarget = geographicalRegions[new Point(10, 10)];

            Assert.Equal(0, geographicalRegionAtTarget.GeologicalIndex);
            Assert.Equal(510, geographicalRegionAtTarget.ErosionLevel);
            Assert.Equal(GeologicalType.Rocky, geographicalRegionAtTarget.GeologicalType);

            var totalRiskLevel = RiskLevelForSmallestRectangle(510, target);

            Assert.Equal(114, totalRiskLevel);
        }

        [Fact]
        public void SecondStarExample()
        {
            var lines = new[]
            {
                "X=.|=.|.|=.|=|=.",
                ".|=|=|||..|.=...",
                ".==|....||=..|==",
                "=.|....|.==.|==.",
                "=|..==...=.|==..",
                "=||.=.=||=|=..|=",
                "|.=.===|||..=..|",
                "|..==||=.|==|===",
                ".=..===..=|.|||.",
                ".======|||=|=.|=",
                ".===|=|===T===||",
                "=|||...|==..|=.|",
                "=.=|=.=..=.||==|",
                "||=|=...|==.=|==",
                "|=.=||===.|||===",
                "||.|==.|.|.||=||"
            };

            var (target, _) = ParseCave(lines);

            var minutesToTarget = FewestMinutesToTarget(510, target);

            Assert.Equal(45, minutesToTarget);
        }

        [Fact]
        public void CreateFile()
        {
            var geology = CalculateGeology(Depth, Target);
            var start = new Point(0, 0);
            var maxY = geology.Keys.Max(p => p.Y);
            var maxX = geology.Keys.Max(p => p.X);
            var lines = new List<string>();
            for (int y = 0; y <= maxY; y++)
            {
                var line = new StringBuilder();
                for (int x = 0; x <= maxX; x++)
                {
                    var location = new Point(x, y);
                    var region = geology[location];

                    if (region.Location == start) line.Append("M");
                    else if (region.Location == Target) line.Append("T");
                    else
                    {
                        switch (region.GeologicalType)
                        {
                            case GeologicalType.Rocky:
                                line.Append(".");
                                break;
                            case GeologicalType.Wet:
                                line.Append("=");
                                break;
                            case GeologicalType.Narrow:
                                line.Append("|");
                                break;

                        }
                    }
                }
                lines.Add(line.ToString());
            }

            File.WriteAllLines(@".\cave.txt", lines);
        }

        [Fact]
        public void FirstStarTest()
        {
            var actual = FirstStar();
            Assert.Equal("10204", actual);
        }

        [Fact]
        public void SecondStarTest()
        {
            var actual = SecondStar();
            Assert.Equal("1004", actual);
        }
    }
}