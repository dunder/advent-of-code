using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Facet.Combinatorics;
using Shared.MapGeometry;
using Shared.Tree;

namespace Solutions.Event2016.Day22
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day22;

        public override string FirstStar()
        {
            var discUsageOutput = ReadLineInput();
            var memoryNodeDescriptions = discUsageOutput.Skip(2).ToList();
            var result = CountViablePairs(memoryNodeDescriptions);
            return result.ToString();
        }

        public override string SecondStar()
        {
            var discUsageOutput = ReadLineInput();
            var memoryNodeDescriptions = discUsageOutput.Skip(2).ToList();
            var result = StepsToMoveDataHome(memoryNodeDescriptions);
            return result.ToString();
        }

        private int StepsToMoveDataHome(IList<string> memoryNodeDescriptions)
        {
            var memoryNodes = ParseNodes(memoryNodeDescriptions);

            var maxX = memoryNodes.Max(n => n.Position.X);
            var maxY = memoryNodes.Max(n => n.Position.Y);

            var targetNode = TargetNode(memoryNodes);

            var emptyNode = memoryNodes.Single(n => n.IsEmpty);
            var targetPositionForEmpty = targetNode.Position.Move(Direction.West);

            var nodeLookup = memoryNodes.ToLookup(x => x.Position, x => x);

            bool IsWalkable(Point p)
            {
                var withinMemoryGrid = p.X >= 0 && p.Y >= 0 && p.X <= maxX && p.Y <= maxY;

                return withinMemoryGrid && emptyNode.FitsContentFrom(nodeLookup[p].Single());
            }

            var path = AStar.Search(emptyNode.Position, targetPositionForEmpty, IsWalkable);

            var stepsToTargetPosition = path.Depth;

            // stepsToTargetPosition is the number of steps to reach the position left of the target node
            
            // swap the node (one step more)

            var totalSteps = stepsToTargetPosition + 1;

            // now to move the target memory (G) one step left requires one swap with the empty node (_) and
            // moving the empty node to the left around G with a total of 5 steps per move of G to the left

            //     start     1 step    2 steps   3 steps   4 steps   5 steps (has now moved 1 step left)
            //
            //     . G _     . G .     . G .     . G -     _ G .     G _ .
            //     . . .     . . _     . _ .     _ . .     . . .     . . .
            //     . . .     . . .     . . .     . . .     . . .     . . .

            var distanceFromTargetToAccessNode = targetPositionForEmpty.ManhattanDistance(new Point(0, 0));

            totalSteps = totalSteps + 5 * distanceFromTargetToAccessNode;

            return totalSteps;
        }

        public class MemoryNode
        {
            public MemoryNode(Point position, int size, int used)
            {
                Position = position;
                Size = size;
                Used = used;
            }

            public Point Position { get; }
            public int Size { get; }
            public int Used { get; }
            public bool IsEmpty => Used == 0;
            public bool HasAvailableSpace => Available > 0;
            private int Available => Size - Used;

            public bool FitsContentFrom(MemoryNode other)
            {
                return Available >= other.Used;
            }

            protected bool Equals(MemoryNode other)
            {
                return Position.Equals(other.Position);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((MemoryNode) obj);
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }

            public override string ToString()
            {
                return $"node-x{Position.X}-y{Position.Y} Size: {Size} Used: {Used}";
            }
        }

        public static IList<MemoryNode> ParseNodes(IList<string> nodeDescriptions)
        {
            // should match: /dev/grid/node-x34-y2    92T   72T    20T   78%
            Regex nodeExp = new Regex(@".*node-x(\d+)-y(\d+)\s*(\d+)T\s*(\d+).*");

            var memoryNodes = new List<MemoryNode>();

            foreach (var description in nodeDescriptions)
            {
                var groups = nodeExp.Match(description).Groups;
                var x = int.Parse(groups[1].Value);
                var y = int.Parse(groups[2].Value);
                var size = int.Parse(groups[3].Value);
                var used = int.Parse(groups[4].Value);

                var memoryNode = new MemoryNode(new Point(x, y), size, used);

                memoryNodes.Add(memoryNode);
            }
            return memoryNodes;
        }
        

        public static MemoryNode TargetNode(IList<MemoryNode> memoryNodes)
        {
            return memoryNodes.Where(m => m.Position.Y == 0).OrderBy(m => m.Position.X).Last();
        }

        public static int CountViablePairs(IList<string> nodeDescriptions)
        {
            var memoryNodes = ParseNodes(nodeDescriptions);

            var c = new Combinations<MemoryNode>(memoryNodes, 2);

            var flipped = c.Select(nodes => new List<MemoryNode> {nodes[1], nodes[0]}).ToList();

            var combined = c.Concat(flipped).ToList();

            var notEmpty = combined.Where(nodes => !nodes[0].IsEmpty).ToList();

            var viable = notEmpty.Where(nodes => nodes[1].FitsContentFrom(nodes[0])).ToList();

            return viable.Count;
        }

        public static void PrintInitialMemoryGrid()
        {
            var discUsageOutput = new Problem().ReadLineInput();
            var memoryNodeDescriptions = discUsageOutput.Skip(2).ToList();
            var nodes = ParseNodes(memoryNodeDescriptions);
            var targetNode = TargetNode(nodes);
            PrintMemoryGrid(nodes, targetNode);
        }

        public static void PrintMemoryGrid(IList<MemoryNode> memoryNodes, MemoryNode targetNode)
        {
            var columns = memoryNodes.Max(m => m.Position.X) + 1;
            var rows = memoryNodes.Max(m => m.Position.Y) + 1;
            var printArray = new MemoryNode[columns, rows];
            var empty = memoryNodes.Single(m => m.IsEmpty);

            foreach (var memoryNode in memoryNodes)
            {
                printArray[memoryNode.Position.X, memoryNode.Position.Y] = memoryNode;
            }

            var upperLeftPosition = new Point(0, 0);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    var memoryNode = printArray[x, y];

                    var nodeDescription = "";
                    if (memoryNode.Position == targetNode.Position)
                    {
                        nodeDescription = "G";
                    }
                    else if (memoryNode.IsEmpty)
                    {
                        nodeDescription = "_";
                    }
                    else if (memoryNode.FitsContentFrom(targetNode))
                    {
                        nodeDescription = ".";
                    }
                    else
                    {
                        nodeDescription = "#";
                    }

                    if (memoryNode.Position == upperLeftPosition)
                    {
                        nodeDescription = $"({nodeDescription})";
                    }
                    else
                    {
                        var tail = empty.FitsContentFrom(memoryNode) ? "*" : " ";
                        var prefix = memoryNode.FitsContentFrom(targetNode) ? "+" : " ";
                        nodeDescription = $"{prefix}{nodeDescription}{tail}";
                    }

                    Console.Write(nodeDescription);
                }
                Console.WriteLine();
            }
        }
    }
}