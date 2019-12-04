using System.Collections.Generic;

namespace Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Yield<T>(this T element)
        {
            yield return element;
        }
    }
}