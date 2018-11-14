using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Tree
{
    /// <summary>
    /// Ideas from https://stackoverflow.com/a/5806795
    /// </summary>
    public static class GraphExtensions
    {
        public static IEnumerable<T> TraverseAll<T>(this T start, Func<T, IEnumerable<T>> neighborFetcher)
        {
            if (!neighborFetcher(start).Any())
            {
                yield return start;
            }
            else
            {
                foreach (var neighbor in neighborFetcher(start))
                {
                    foreach (var neighborsNeighbor in neighbor.TraverseAll(neighborFetcher))
                    {
                        yield return neighborsNeighbor;
                    }
                }
            }
        }

        public static (Node<T> terminationNode, ISet<T> visited) ShortestPath<T>(this T start,
            Func<T, IEnumerable<T>> neighborFetcher,
            Predicate<T> targetCondition)
        {
            return start.BreadthFirst(neighborFetcher, targetCondition, -1);
        }

        public static (Node<T> terminationNode, ISet<T> visited) BreadthFirst<T>(this T start, Func<T, IEnumerable<T>> neighborFetcher)
        {
            return start.BreadthFirst(neighborFetcher, (n) => false, -1);
        }
        public static (Node<T> terminationNode, ISet<T> visited) BreadthFirst<T>(this T start,
            Func<T, IEnumerable<T>> neighborFetcher,
            int maxDepth)
        {
            return start.BreadthFirst(neighborFetcher, (n) => false, maxDepth);
        }

        public static (Node<T> terminationNode, ISet<T> visited) BreadthFirst<T>(this T start,
            Func<T, IEnumerable<T>> neighborFetcher,
            Predicate<T> targetCondition,
            int maxDepth)
        {
            var visited = new HashSet<T>();

            var queue = new Queue<Node<T>>();
            Node<T> terminationNode = null;

            queue.Enqueue(new Node<T>(start, 0));

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();

                if (!visited.Add(current.Data))
                {
                    continue;
                }

                if (targetCondition(current.Data))
                {
                    terminationNode = current;
                    break;
                }

                if (current.Depth == maxDepth)
                {
                    continue;
                }

                var neighbors = neighborFetcher(current.Data).Where(n => !visited.Contains(n)).ToList();

                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(new Node<T>(neighbor, current.Depth + 1));
                }
            }

            return (terminationNode, visited);
        }
        
        public static (IEnumerable<Node<T>> depthFirst, ISet<T> visited) DepthFirst<T>(this T start,
            Func<T, IEnumerable<T>> neighborFetcher)
        {
            return start.DepthFirst(neighborFetcher, (n) => false);
        }

        public static (IEnumerable<Node<T>> depthFirst, ISet<T> visited) DepthFirst<T>(this T start,
            Func<T, IEnumerable<T>> neighborFetcher,
            Predicate<Node<T>> targetCondition)
        {
            var visited = new HashSet<T>();
            var depthFirst = new List<Node<T>>();
            var stack = new Stack<Node<T>>();

            stack.Push(new Node<T>(start, 0));

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current.Data))
                {
                    continue;
                }

                depthFirst.Add(current);

                if (targetCondition(current))
                {
                    break;
                }

                var neighbors = neighborFetcher(current.Data).Where(n => !visited.Contains(n));

                // left-to-right order
                foreach (var neighbor in neighbors.Reverse())
                {
                    stack.Push(new Node<T>(neighbor, current.Depth + 1));
                }
            }

            return (depthFirst, visited);
        }
    }
}