using System.Collections.Generic;
using System.Linq;

namespace Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Yield<T>(this T element)
        {
            yield return element;
        }

        public static IEnumerable<(T, int)> Sequences<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable.ToList();

            T previous = list[0];

            int counter = 1;
            for (int i = 1; i < list.Count; i++)
            {
                T current = list[i];
                if (!current.Equals(previous))
                {
                    yield return (previous, counter);
                    counter = 0;
                }

                if (i == list.Count - 1)
                {
                    yield return (current, counter + 1);
                }

                counter++;
                previous = current;
            }
        }
    }
}