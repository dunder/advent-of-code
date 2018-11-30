using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Solutions.Event2016.Day22
{
    class AStarTwo
    {
        public class Node
        {
            public Point Position { get; set; }
            public int MinCostToStart { get; set; }
            public int MinCostToEnd { get; set; }
            public int ManhattanDistanceToEnd { get; set; }
            public int Cost => MinCostToStart + ManhattanDistanceToEnd;
            public bool Visited { get; set; }

            public IList<Node> Connections
            {
                get
                {
                    return new List<Node>();
                }
            }
        }

        public Node Start { get; set; }
        public Node End { get; set; }
        public int NodeVisits { get; set; }


        public List<Node> GetShortestPathAstar()
        {
            foreach (var node in Map.Nodes)
                node.StraightLineDistanceToEnd = node.StraightLineDistanceTo(End);
            AstarSearch();
            var shortestPath = new List<Node>();
            shortestPath.Add(End);
            // BuildShortestPath(shortestPath, End);
            shortestPath.Reverse();
            return shortestPath;
        }

        private void AstarSearch()
        {
            Start.MinCostToStart = 0;
            var prioQueue = new List<Node>();
            prioQueue.Add(Start);
            do
            {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart + x.ManhattanDistanceToEnd).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);
                NodeVisits++;
                foreach (var cnn in node.Connections.OrderBy(x => x.Cost))
                {
                    var childNode = cnn.ConnectedNode;
                    if (childNode.Visited)
                        continue;
                    if (childNode.MinCostToStart == null ||
                        node.MinCostToStart + cnn.Cost < childNode.MinCostToStart)
                    {
                        childNode.MinCostToStart = node.MinCostToStart + cnn.Cost;
                        childNode.NearestToStart = node;
                        if (!prioQueue.Contains(childNode))
                            prioQueue.Add(childNode);
                    }
                }
                node.Visited = true;
                if (node == End)
                    return;
            } while (prioQueue.Any());
        }
    }
}
