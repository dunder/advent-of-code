using System.Collections.Generic;

namespace Shared.Tree
{
    public class Node<T>
    {
        public T Data { get; }
        public int Depth { get; }
        public Node<T> Parent { get; }

        public Node<T> Start
        {
            get
            {
                var walkTo = this;
                while (walkTo.Parent != null)
                {
                    walkTo = walkTo.Parent;
                }

                return walkTo;
            }
        }
        public IList<T> Path
        {
            get
            {
                var path = new List<T>();
                var node = this;
                path.Add(node.Data);
                while (node.Parent != null)
                {
                    node = node.Parent;
                    path.Add(node.Data);
                }

                path.Reverse();

                return path;
            }
        }

        public Node(T data, int depth, Node<T> parent = null)
        {
            Data = data;
            Depth = depth;
            Parent = parent;
        }
    }
}