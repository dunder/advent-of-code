using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Combinatorics
{
    // https://stackoverflow.com/a/10629938

    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IReadOnlyCollection<T> collection)
        {
            return collection.Permutations(collection.Count());
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IReadOnlyCollection<T> collection, int length)
        {
            if (length == 1)
            {
                return collection.Select(t => new[] { t });
            }

            return collection.Permutations(length - 1).SelectMany(t => collection.Where(o => !t.Contains(o)), (t1, t2) => t1.Concat(new[] { t2 }));
        }

        public static IEnumerable<IEnumerable<T>> PermutationsWithRepetitions<T>(this IReadOnlyCollection<T> collection)
        {
            return collection.PermutationsWithRepetitions(collection.Count);
        }

        public static IEnumerable<IEnumerable<T>> PermutationsWithRepetitions<T>(this IReadOnlyCollection<T> collection, int length)
        {
            if (length == 1)
            {
                return collection.Select(t => new[] { t });

            }
            return collection.PermutationsWithRepetitions(length - 1).SelectMany(t => collection, (t1, t2) => t1.Concat(new[] { t2 }));
        }

        public static IEnumerable<IEnumerable<T>> CombinationsWithRepetitions<T>(this IReadOnlyCollection<T> collection) where T : IComparable
        {
            return collection.CombinationsWithRepetitions(collection.Count);
        }

        public static IEnumerable<IEnumerable<T>> CombinationsWithRepetitions<T>(this IReadOnlyCollection<T> collection, int length) where T : IComparable
        {
            if (length == 1)
            {
                return collection.Select(t => new[] { t });
            }

            return collection.CombinationsWithRepetitions(length - 1).SelectMany(t => collection.Where(o => o.CompareTo(t.Last()) >= 0), (t1, t2) => t1.Concat(new[] { t2 }));
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IReadOnlyCollection<T> collection) where T : IComparable
        {
            return collection.Combinations(collection.Count);
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IReadOnlyCollection<T> collection, int length) where T : IComparable
        {
            if (length == 1)
            {
                return collection.Select(t => new[] { t });
            }

            return Combinations(collection, length - 1).SelectMany(t => collection.Where(o => o.CompareTo(t.Last()) > 0), (t1, t2) => t1.Concat(new[] { t2 }));
        }
    }
}
