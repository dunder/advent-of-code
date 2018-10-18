using System.Collections.Generic;

namespace Utilities.Tree {
    public interface IGraph<T> {
        IEnumerable<T> GetNeighbours(T vertex);
    }
}