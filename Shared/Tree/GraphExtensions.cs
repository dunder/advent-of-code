using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Tree {
    /// <summary>
    /// Ideas from https://stackoverflow.com/a/5806795
    /// </summary>
    public static class GraphExtensions {

        public static IEnumerable<T> TraverseAll<T>(this T start, Func<T, IEnumerable<T>> neighborFetcher)  {
            if (!neighborFetcher(start).Any()) {
                yield return start;
            } else {
                foreach (var neighbor in neighborFetcher(start)) {
                    foreach (var neighborsNeighbor in neighbor.TraverseAll(neighborFetcher)) {
                        yield return neighborsNeighbor;
                    }
                }
            }
        }

        public static IEnumerable<T> DepthFirst<T>(this T start, Func<T,IEnumerable<T>> neighborFetcher) {
            var visited = new HashSet<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count != 0) {
                var current = stack.Pop();

                if (!visited.Add(current)) {
                    continue;
                }

                yield return current;

                var neighbors = neighborFetcher(current)
                    .Where(n => !visited.Contains(n));

                // left-to-right order
                foreach (var neighbor in neighbors.Reverse()) {
                    stack.Push(neighbor);
                }
            }
        }

        public static (IEnumerable<T> breadthFirst, ISet<T> visited) BreadthFirst<T>(this T start, Func<T,IEnumerable<T>> neighborFetcher, Predicate<T> targetCondition)
        {
            var breadthFirst = new List<T>();
            var visited = new HashSet<T>();

            var queue = new Queue<T>();

            queue.Enqueue(start);

            while (queue.Count != 0) {
                var current = queue.Dequeue();

                if (!visited.Add(current)) {
                    continue;
                }

                breadthFirst.Add(current);

                if (targetCondition(current))
                {
                    break;
                }

                var neighbors = neighborFetcher(current).Where(n => !visited.Contains(n));

                foreach (var neighbor in neighbors) {
                    queue.Enqueue(neighbor);
                }
            }

            return (breadthFirst, visited);
        }

        public static (IEnumerable<T> depthFirst, ISet<T> visited) DepthFirstWithVisited<T>(this T start, Func<T, IEnumerable<T>> neighbourFetcher) {
            var visited = new HashSet<T>();
            var depthFirst = new List<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count != 0) {
                var current = stack.Pop();

                if (!visited.Add(current)) {
                    continue;
                }

                depthFirst.Add(current);

                var neighbours = neighbourFetcher(current)
                    .Where(n => !visited.Contains(n));

                // left-to-right order
                foreach (var neighbour in neighbours.Reverse()) {
                    stack.Push(neighbour);
                }
            }
            return (depthFirst, visited);
        }
    }
}