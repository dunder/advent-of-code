using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.MapGeometry;

namespace Solutions.Event2016.Day22
{
    // THIS SOLUTION BELOW IS NOT COMPLETED
    // 
    // Some code taken from here: https://web.archive.org/web/20170505034417/http://blog.two-cats.com/2014/06/a-star-example/
    public class AStarOne
    {
        public class AStarSearchParameters
        {
            public AStarSearchParameters(Problem.MemoryNode startNode, Problem.MemoryNode targetNode, IList<Problem.MemoryNode> memoryNodes)
            {
                StartPosition = startNode.Position;
                TargetPosition = targetNode.Position;
                Grid = new Dictionary<Point, AStarNode>();
                Width = memoryNodes.Max(n => n.Position.X);
                Height = memoryNodes.Max(n => n.Position.Y);

                foreach (var memoryNode in memoryNodes)
                {
                    Grid.Add(memoryNode.Position, new AStarNode
                    {
                        Position = memoryNode.Position,
                        State = AStareNodeState.Untested,
                        IsWalkable = startNode.FitsContentFrom(memoryNode)
                    });
                }
            }

            private int Width { get; set; }
            private int Height { get; set; }
            public Point StartPosition { get; set; }
            public Point TargetPosition { get; set; }
            //public bool[,] Map { get; set; }
            public Dictionary<Point, AStarNode> Grid { get; set; }

            public bool AStarSearch(AStarNode currentNode)
            {
                currentNode.State = AStareNodeState.Closed;
                List<AStarNode> nextNodes = GetAdjacentWalkableNodes(currentNode);
                nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
                foreach (var nextNode in nextNodes)
                {
                    if (nextNode.Position == this.TargetPosition)
                    {
                        return true;
                    }
                    else
                    {
                        if (AStarSearch(nextNode)) // Note: Recurses back into Search(Node)
                            return true;
                    }
                }
                return false;
            }

            private List<AStarNode> GetAdjacentWalkableNodes(AStarNode fromNode)
            {
                List<AStarNode> walkableNodes = new List<AStarNode>();
                IEnumerable<Point> nextLocations = fromNode.Position.AdjacentInMainDirections();

                foreach (var location in nextLocations)
                {
                    int x = location.X;
                    int y = location.Y;

                    // Stay within the grid's boundaries
                    if (x < 0 || x >= this.Width || y < 0 || y >= this.Height)
                    {
                        continue;
                    }

                    AStarNode node = this.Grid[location];
                    // Ignore non-walkable nodes
                    if (!node.IsWalkable)
                    {
                        continue;
                    }

                    // Ignore already-closed nodes
                    if (node.State == AStareNodeState.Closed)
                    {
                        continue;
                    }

                    // Already-open nodes are only added to the list if their G-value is lower going via this route.
                    if (node.State == AStareNodeState.Open)
                    {
                        float traversalCost = AStarNode.GetTraversalCost(node.Position, node.Parent.Position);
                        float gTemp = fromNode.G + traversalCost;
                        if (gTemp < node.G)
                        {
                            node.Parent = fromNode;
                            walkableNodes.Add(node);
                        }
                    }
                    else
                    {
                        // If it's untested, set the parent and flag it as 'Open' for consideration
                        node.Parent = fromNode;
                        node.State = AStareNodeState.Open;
                        walkableNodes.Add(node);
                    }
                }

                return walkableNodes;
            }
        }

        public enum AStareNodeState
        {
            Untested,
            Open,
            Closed
        }
        public class AStarNode
        {
            public Point Position { get; set; }
            public bool IsWalkable { get; set; }

            public int G { get; set; }
            public int H { get; set; }
            public int F => G + H;

            public AStareNodeState State { get; set; }

            public AStarNode Parent { get; set; }

            public static int GetTraversalCost(Point position, Point parentPosition)
            {
                return Math.Abs(position.X - parentPosition.X) + Math.Abs(position.Y - parentPosition.Y);
            }

            public override string ToString()
            {
                var walkable = IsWalkable ? "." : "#";
                return $"({Position.X},{Position.Y} {walkable} G: {G} H: {H} F: {F} State: {State} Parent: ({Parent.Position.X},{Parent.Position.Y})";
            }
        }


    }
}
