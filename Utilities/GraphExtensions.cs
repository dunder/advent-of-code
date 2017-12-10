using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities {
    public static class GraphExtensions {

        public static IEnumerable<T> TraverseAll<T>(this T start, Func<T, IEnumerable<T>> neighbourFetcher)  {
            if (!neighbourFetcher(start).Any()) {
                yield return start;
            } else {
                foreach (var neighbour in neighbourFetcher(start)) {
                    foreach (var neighboursNeighbour in neighbour.TraverseAll(neighbourFetcher)) {
                        yield return neighboursNeighbour;
                    }
                }
            }
        }

        public static IEnumerable<T> DepthFirst<T>(this T start, Func<T,IEnumerable<T>> neighbourFetcher) {
            var visited = new HashSet<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count != 0) {
                var current = stack.Pop();

                if (!visited.Add(current)) {
                    continue;
                }

                yield return current;

                var neighbours = neighbourFetcher(current)
                    .Where(n => !visited.Contains(n));

                // If you don't care about the left-to-right order, remove the Reverse
                foreach (var neighbour in neighbours.Reverse()) {
                    stack.Push(neighbour);
                }
            }
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

                // If you don't care about the left-to-right order, remove the Reverse
                foreach (var neighbour in neighbours.Reverse()) {
                    stack.Push(neighbour);
                }
            }
            return (depthFirst, visited);
        }

        public static IEnumerable<T> DepthFirst<T>(this IGraph<T> graph, T start) {
            var visited = new HashSet<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while (stack.Count != 0) {
                var current = stack.Pop();

                if (!visited.Add(current)) {
                    continue;
                }

                yield return current;

                var neighbours = Enumerable.Where<T>(graph.GetNeighbours(current), n => !visited.Contains(n));

                // If you don't care about the left-to-right order, remove the Reverse
                foreach (var neighbour in neighbours.Reverse()) {
                    stack.Push(neighbour);
                }
            }
        }
    }
}