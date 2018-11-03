using System;
using System.Collections.Generic;

namespace Shared.Extensions {
    public static class EnumFlagsExtensions {

        public static IEnumerable<Enum> GetFlags(this Enum input) {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }
    }
}
