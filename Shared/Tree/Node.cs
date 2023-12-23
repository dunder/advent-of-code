using System;
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

        public IList<Node<T>> Nodes
        {
            get
            {
                var path = new List<Node<T>>();
                var node = this;
                path.Add(node);
                while (node.Parent != null)
                {
                    node = node.Parent;
                    path.Add(node);
                }

                path.Reverse();

                return path;
            }
        }

        public IEnumerable<T> PathFromEnd()
        {
            var node = this;

            while (node.Parent != null)
            {
                yield return node.Data;
                node = node.Parent;
            }
        }

        public Node(T data, int depth, Node<T> parent = null)
        {
            Data = data;
            Depth = depth;
            Parent = parent;
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}