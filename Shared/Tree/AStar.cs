using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.MapGeometry;

namespace Shared.Tree
{
    public class AStar
    {
        public class Node
        {
            public Node(Point position, int manhattanDistanceToTarget)
            {
                Position = position;
                ManhattanDistanceToTarget = manhattanDistanceToTarget;
            }

            public Point Position { get; }
            public int ManhattanDistanceToTarget { get; }
            public int MovementCost { get; set; }
            public int TotalCost => MovementCost + ManhattanDistanceToTarget;
            public Node Parent { get; set; }

            public Node WalkToNode
            {
                get
                {
                    var walkTo = this;

                    while (walkTo?.Parent?.Parent != null)
                    {
                        walkTo = walkTo.Parent;
                    }

                    return walkTo;
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
                return Position.Equals(other.Position);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Node) obj);
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }

            public override string ToString()
            {
                return $"({Position.X},{Position.Y} G: {MovementCost} H: {ManhattanDistanceToTarget} F: {TotalCost}";
            }
        }

        public static Node Search(Point startingPosition, Point targetPosition, Func<Point, bool> walkable)
        {
            var startNode = new Node(startingPosition, startingPosition.ManhattanDistance(targetPosition));

            var open = new HashSet<Node> { startNode };
            var closed = new HashSet<Node>();

            Node currentNode;

            while (true)
            {
                currentNode = open.OrderBy(n => n.TotalCost).FirstOrDefault();

                if (currentNode == null ||currentNode.Position == targetPosition)
                {
                    break;
                }

                open.Remove(currentNode);
                closed.Add(currentNode);

                var adjacent = currentNode
                    .Position
                    .AdjacentInMainDirections()
                    .Select(p => new Node(p, p.ManhattanDistance(targetPosition)))
                    .ToList();

                if (!adjacent.Any() && !open.Any())
                {
                    break;
                }

                var walkableNonClosed = adjacent.Where(n => walkable(n.Position) && !closed.Contains(n)).ToList();

                foreach (var node in walkableNonClosed)
                {
                    if (open.TryGetValue(node, out var openNode))
                    {
                        if (currentNode.MovementCost < openNode.MovementCost)
                        {
                            open.Remove(openNode);
                            node.MovementCost = currentNode.MovementCost + 1;
                            node.Parent = currentNode;
                            open.Add(node);
                        }
                    }
                    else
                    {
                        node.Parent = currentNode;
                        node.MovementCost = node.MovementCost + 1;
                        open.Add(node);
                    }
                }
            }

            return currentNode;
        }
    }
}
