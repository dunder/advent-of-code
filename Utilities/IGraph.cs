using System.Collections.Generic;

namespace Utilities {
    public interface IGraph<T> {
        IEnumerable<T> GetNeighbours(T vertex);
    }
}