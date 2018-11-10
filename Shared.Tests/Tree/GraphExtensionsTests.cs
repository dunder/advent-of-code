using System.Collections.Generic;
using Shared.Tree;
using Xunit;

namespace Shared.Tests.Tree {
    public class GraphExtensionsTests {

        [Fact]
        public void BreadthFirstTraversal()
        {
            //                 a
            //              /  |  \
            //             b   c   d
            //           / |   |   | \
            //          e  f   g   h  i
            //                    / \
            //                   j   k
            //
            var graph = new Dictionary<string, IList<string>>
            {
                {"a", new List<string> {"b", "c", "d"}},
                {"b", new List<string> {"e", "f"}},
                {"c", new List<string> {"g" }},
                {"d", new List<string> {"h", "i"}},
                {"e", new List<string>()},
                {"f", new List<string>()},
                {"g", new List<string>()},
                {"h", new List<string> {"j", "k"}},
                {"i", new List<string>()},
                {"j", new List<string>()},
                {"k", new List<string>()},
            };

            IList<string> Neighbors(string n) => graph[n];

            var path = "a".BreadthFirst(Neighbors);

            Assert.Equal(new [] {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k"}, path);
        }

        [Fact]
        public void DepthFirstTraversal()
        {
            //                 a
            //              /  |  \
            //             b   e   g
            //           / |   |   | \
            //          c  d   f   h  k
            //                    / \
            //                   i   j
            //
            var graph = new Dictionary<string, IList<string>>
            {
                {"a", new List<string> {"b", "e", "g"}},
                {"b", new List<string> {"c", "d"}},
                {"c", new List<string>()},
                {"d", new List<string>()},
                {"e", new List<string> {"f"}},
                {"f", new List<string>()},
                {"g", new List<string> {"h", "k"}},
                {"h", new List<string> {"i", "j"}},
                {"i", new List<string>()},
                {"j", new List<string>()},
                {"k", new List<string>()},
            };

            IList<string> Neighbors(string n) => graph[n];

            var (path, _) = "a".DepthFirst(Neighbors);

            Assert.Equal(new [] {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k"}, path);
        }
    }
}
